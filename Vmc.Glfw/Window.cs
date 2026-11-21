// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SysUnsafe = System.Runtime.CompilerServices.Unsafe;

namespace Vmc.Glfw;

public delegate void WindowPositionCallback(Window window, Point position);
public delegate void WindowSizeCallback(Window window, Size size);
public delegate void WindowCloseCallback(Window window);
public delegate void WindowRefreshCallback(Window window);
public delegate void WindowFocusCallback(Window window, bool focused);
public delegate void WindowIconifyCallback(Window window, bool iconified);
public delegate void WindowMaximizeCallback(Window window, bool maximized);
public delegate void FramebufferSizeCallback(Window window, Size size);
public delegate void WindowContentScaleCallback(Window window, Vector2 scale);
public delegate void KeyCallback(
    Window window,
    Key key,
    int scancode,
    KeyAction action,
    KeyModifiers mods
);
public delegate void CharCallback(Window window, uint codepoint);
public delegate void CharModsCallback(Window window, uint codepoint, KeyModifiers mods);
public delegate void MouseButtonCallback(
    Window window,
    MouseButton button,
    ButtonAction action,
    KeyModifiers mods
);
public delegate void CursorPositionCallback(Window window, PointF position);
public delegate void CursorEnterCallback(Window window, bool entered);
public delegate void ScrollCallback(Window window, Vector2 offset);
public delegate void DropCallback(Window window, string[] paths);

public sealed unsafe class Window : IDisposable
{
    private static readonly CallbacksWrapper Callbacks = new();

    private readonly bool _isOwned;

    private GCHandle _gcHandle;
    private bool _disposed;

    internal Unsafe.Glfw.Window* Handle { get; }

    public bool ShouldClose
    {
        get
        {
            ThrowIfDisposed();
            var result = Unsafe.Glfw.WindowShouldClose(Handle);
            Utilities.CheckError();
            return result;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowShouldClose(Handle, value);
            Utilities.CheckError();
        }
    }

    public string Title
    {
        get
        {
            ThrowIfDisposed();
            var titlePtr = Unsafe.Glfw.GetWindowTitle(Handle);
            Utilities.CheckError();
            return Marshal.PtrToStringUTF8((nint)titlePtr) ?? string.Empty;
        }
        set
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(value);
            var titlePtr = (byte*)Marshal.StringToCoTaskMemUTF8(value);
            try
            {
                Unsafe.Glfw.SetWindowTitle(Handle, titlePtr);
                Utilities.CheckError();
            }
            finally
            {
                Marshal.FreeCoTaskMem((nint)titlePtr);
            }
        }
    }

    public Point Position
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetWindowPos(Handle, out var xpos, out var ypos);
            Utilities.CheckError();
            return new Point(xpos, ypos);
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowPos(Handle, value.X, value.Y);
            Utilities.CheckError();
        }
    }

    public Size Size
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetWindowSize(Handle, out var width, out var height);
            Utilities.CheckError();
            return new Size(width, height);
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowSize(Handle, value.Width, value.Height);
            Utilities.CheckError();
        }
    }

    public Size FramebufferSize
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetFramebufferSize(Handle, out var width, out var height);
            Utilities.CheckError();
            return new Size(width, height);
        }
    }

    public (int Left, int Top, int Right, int Bottom) FrameSize
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetWindowFrameSize(
                Handle,
                out var left,
                out var top,
                out var right,
                out var bottom
            );
            Utilities.CheckError();
            return (left, top, right, bottom);
        }
    }

    public Vector2 ContentScale
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetWindowContentScale(Handle, out var xscale, out var yscale);
            Utilities.CheckError();
            return new Vector2(xscale, yscale);
        }
    }

    public float Opacity
    {
        get
        {
            ThrowIfDisposed();
            var opacity = Unsafe.Glfw.GetWindowOpacity(Handle);
            Utilities.CheckError();
            return opacity;
        }
        set
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 1F);
            Unsafe.Glfw.SetWindowOpacity(Handle, value);
            Utilities.CheckError();
        }
    }

    public Monitor? Monitor
    {
        get
        {
            ThrowIfDisposed();
            var monitorHandle = Unsafe.Glfw.GetWindowMonitor(Handle);
            Utilities.CheckError();
            return monitorHandle is not null ? new Monitor(monitorHandle, isOwned: false) : null;
        }
    }

    public bool Focused
    {
        get
        {
            ThrowIfDisposed();
            var focused = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Focused);
            Utilities.CheckError();
            return focused != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.Focused, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool Iconified
    {
        get
        {
            ThrowIfDisposed();
            var iconified = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Iconified);
            Utilities.CheckError();
            return iconified != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.Iconified, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool Resizable
    {
        get
        {
            ThrowIfDisposed();
            var resizable = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Resizable);
            Utilities.CheckError();
            return resizable != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.Resizable, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool Visible
    {
        get
        {
            ThrowIfDisposed();
            var visible = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Visible);
            Utilities.CheckError();
            return visible != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.Visible, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool Decorated
    {
        get
        {
            ThrowIfDisposed();
            var decorated = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Decorated);
            Utilities.CheckError();
            return decorated != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.Decorated, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool AutoIconify
    {
        get
        {
            ThrowIfDisposed();
            var autoIconify = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.AutoIconify);
            Utilities.CheckError();
            return autoIconify != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.AutoIconify, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool Floating
    {
        get
        {
            ThrowIfDisposed();
            var floating = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Floating);
            Utilities.CheckError();
            return floating != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.Floating, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool Maximized
    {
        get
        {
            ThrowIfDisposed();
            var maximized = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Maximized);
            Utilities.CheckError();
            return maximized != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.Maximized, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool TransparentFramebuffer
    {
        get
        {
            ThrowIfDisposed();
            var transparentFramebuffer = Unsafe.Glfw.GetWindowAttrib(
                Handle,
                Unsafe.Glfw.TransparentFramebuffer
            );
            Utilities.CheckError();
            return transparentFramebuffer != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.TransparentFramebuffer, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool Hovered
    {
        get
        {
            ThrowIfDisposed();
            var hovered = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.Hovered);
            Utilities.CheckError();
            return hovered != 0;
        }
    }

    public bool FocusOnShow
    {
        get
        {
            ThrowIfDisposed();
            var focusOnShow = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.FocusOnShow);
            Utilities.CheckError();
            return focusOnShow != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.FocusOnShow, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool HasMousePassthrough
    {
        get
        {
            ThrowIfDisposed();
            var hasMousePassthrough = Unsafe.Glfw.GetWindowAttrib(
                Handle,
                Unsafe.Glfw.MousePassthrough
            );
            Utilities.CheckError();
            return hasMousePassthrough != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.MousePassthrough, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public CursorInputMode CursorInputMode
    {
        get
        {
            ThrowIfDisposed();
            var mode = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.CursorConst);
            Utilities.CheckError();
            return (CursorInputMode)mode;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.CursorConst, (int)value);
            Utilities.CheckError();
        }
    }

    public bool HasStickyKeys
    {
        get
        {
            ThrowIfDisposed();
            var hasStickyKeys = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.StickyKeys);
            Utilities.CheckError();
            return hasStickyKeys != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.StickyKeys, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool HasStickyMouseButtons
    {
        get
        {
            ThrowIfDisposed();
            var hasStickyMouseButtons = Unsafe.Glfw.GetWindowAttrib(
                Handle,
                Unsafe.Glfw.StickyMouseButtons
            );
            Utilities.CheckError();
            return hasStickyMouseButtons != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.StickyMouseButtons, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool LocksKeyMods
    {
        get
        {
            ThrowIfDisposed();
            var locksKeyMods = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.LockKeyMods);
            Utilities.CheckError();
            return locksKeyMods != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.LockKeyMods, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public bool HasRawMouseMotion
    {
        get
        {
            ThrowIfDisposed();
            var hasRawMouseMotion = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.RawMouseMotion);
            Utilities.CheckError();
            return hasRawMouseMotion != 0;
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetWindowAttrib(Handle, Unsafe.Glfw.RawMouseMotion, value ? 1 : 0);
            Utilities.CheckError();
        }
    }

    public PointF CursorPosition
    {
        get
        {
            ThrowIfDisposed();
            double xpos;
            double ypos;
            Unsafe.Glfw.GetCursorPos(Handle, &xpos, &ypos);
            Utilities.CheckError();
            return new PointF((float)xpos, (float)ypos);
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetCursorPos(Handle, value.X, value.Y);
            Utilities.CheckError();
        }
    }

    public string ClipboardString
    {
        get
        {
            ThrowIfDisposed();
            var text = Unsafe.Glfw.GetClipboardString(Handle);
            Utilities.CheckError();
            return Marshal.PtrToStringUTF8((nint)text) ?? string.Empty;
        }
        set
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(value);
            var textPtr = Marshal.StringToCoTaskMemUTF8(value);
            try
            {
                Unsafe.Glfw.SetClipboardString(Handle, (byte*)textPtr);
                Utilities.CheckError();
            }
            finally
            {
                Marshal.FreeCoTaskMem(textPtr);
            }
        }
    }

    internal Window(Unsafe.Glfw.Window* handle, bool isOwned = true)
    {
        Handle = handle;
        _isOwned = isOwned;
        _gcHandle = GCHandle.Alloc(Callbacks);
        Unsafe.Glfw.SetWindowUserPointer(Handle, (void*)GCHandle.ToIntPtr(_gcHandle));
        Utilities.CheckError();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowPositionCallbackHandler(Unsafe.Glfw.Window* handle, int x, int y)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.PositionCallback?.Invoke(unownedWindow, new Point(x, y));
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowSizeCallbackHandler(Unsafe.Glfw.Window* handle, int width, int height)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.SizeCallback?.Invoke(unownedWindow, new Size(width, height));
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowCloseCallbackHandler(Unsafe.Glfw.Window* handle)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.CloseCallback?.Invoke(unownedWindow);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowRefreshCallbackHandler(Unsafe.Glfw.Window* handle)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.RefreshCallback?.Invoke(unownedWindow);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowFocusCallbackHandler(Unsafe.Glfw.Window* handle, int focused)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.FocusCallback?.Invoke(unownedWindow, focused != 0);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowIconifyCallbackHandler(Unsafe.Glfw.Window* handle, int iconified)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.IconifyCallback?.Invoke(unownedWindow, iconified != 0);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowMaximizeCallbackHandler(Unsafe.Glfw.Window* handle, int maximized)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.MaximizeCallback?.Invoke(unownedWindow, maximized != 0);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void FramebufferSizeCallbackHandler(
        Unsafe.Glfw.Window* handle,
        int width,
        int height
    )
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.FramebufferSizeCallback?.Invoke(unownedWindow, new Size(width, height));
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void WindowContentScaleCallbackHandler(
        Unsafe.Glfw.Window* handle,
        float xscale,
        float yscale
    )
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.ContentScaleCallback?.Invoke(unownedWindow, new Vector2(xscale, yscale));
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void KeyCallbackHandler(
        Unsafe.Glfw.Window* handle,
        Key key,
        int scancode,
        KeyAction action,
        KeyModifiers mods
    )
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.KeyCallback?.Invoke(unownedWindow, key, scancode, action, mods);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void CharCallbackHandler(Unsafe.Glfw.Window* handle, uint codepoint)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.CharCallback?.Invoke(unownedWindow, codepoint);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void CharModsCallbackHandler(
        Unsafe.Glfw.Window* handle,
        uint codepoint,
        KeyModifiers mods
    )
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.CharModsCallback?.Invoke(unownedWindow, codepoint, mods);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void MouseButtonCallbackHandler(
        Unsafe.Glfw.Window* handle,
        MouseButton button,
        ButtonAction action,
        KeyModifiers mods
    )
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.MouseButtonCallback?.Invoke(unownedWindow, button, action, mods);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void CursorPositionCallbackHandler(
        Unsafe.Glfw.Window* handle,
        double xpos,
        double ypos
    )
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.CursorPositionCallback?.Invoke(
            unownedWindow,
            new PointF((float)xpos, (float)ypos)
        );
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void CursorEnterCallbackHandler(Unsafe.Glfw.Window* handle, int entered)
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.CursorEnterCallback?.Invoke(unownedWindow, entered != 0);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void ScrollCallbackHandler(
        Unsafe.Glfw.Window* handle,
        double xoffset,
        double yoffset
    )
    {
        if (handle is null)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        callbacksWrapper?.ScrollCallback?.Invoke(
            unownedWindow,
            new Vector2((float)xoffset, (float)yoffset)
        );
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void DropCallbackHandler(Unsafe.Glfw.Window* handle, int pathCount, byte** paths)
    {
        if (handle is null || paths is null || pathCount == 0)
        {
            return;
        }

        var windowPtr = Unsafe.Glfw.GetWindowUserPointer(handle);
        var unownedWindow = new Window((Unsafe.Glfw.Window*)windowPtr, isOwned: false);

        var callbacksWrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)windowPtr).Target;
        if (callbacksWrapper?.DropCallback is not null)
        {
            var pathArray = new string[pathCount];
            for (var i = 0; i < pathCount; i++)
            {
                pathArray[i] = Marshal.PtrToStringUTF8((nint)paths[i]) ?? string.Empty;
            }
            callbacksWrapper.DropCallback.Invoke(unownedWindow, pathArray);
        }
    }

    public void Dispose()
    {
        if (_disposed || !_isOwned)
        {
            return;
        }

        _disposed = true;

        try
        {
            Unsafe.Glfw.SetWindowUserPointer(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowPosCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowSizeCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowCloseCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowRefreshCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowFocusCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowIconifyCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowMaximizeCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetFramebufferSizeCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetWindowContentScaleCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetKeyCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetCharCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetCharModsCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetMouseButtonCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetCursorPosCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetCursorEnterCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetScrollCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetDropCallback(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.DestroyWindow(Handle);
            Utilities.CheckError();
        }
        finally
        {
            if (_gcHandle.IsAllocated)
            {
                _gcHandle.Free();
            }
        }
    }

    public void SetIcon(IReadOnlyCollection<Image> images)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(images);
        var imageCount = images.Count;
        var imageArray = new Unsafe.Glfw.Image[imageCount];
        for (var i = 0; i < imageCount; i++)
        {
            var image = images.ElementAt(i);
            var pixelsSpan = image.Pixels.ToArray().AsSpan();
            imageArray[i] = new Unsafe.Glfw.Image
            {
                Width = image.Size.Width,
                Height = image.Size.Height,
                Pixels = (byte*)SysUnsafe.AsPointer(ref pixelsSpan),
            };
        }

        var imagesSpan = imageArray.AsSpan();
        Unsafe.Glfw.SetWindowIcon(
            Handle,
            imageCount,
            (Unsafe.Glfw.Image*)SysUnsafe.AsPointer(ref imagesSpan)
        );
        Utilities.CheckError();
    }

    public void SetSizeLimits(Size minSize, Size maxSize)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetWindowSizeLimits(
            Handle,
            minSize.Width,
            minSize.Height,
            maxSize.Width,
            maxSize.Height
        );
        Utilities.CheckError();
    }

    public void SetAspectRatio(int numer, int denom)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetWindowAspectRatio(Handle, numer, denom);
        Utilities.CheckError();
    }

    public void Iconify()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.IconifyWindow(Handle);
        Utilities.CheckError();
    }

    public void Restore()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.RestoreWindow(Handle);
        Utilities.CheckError();
    }

    public void Maximize()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.MaximizeWindow(Handle);
        Utilities.CheckError();
    }

    public void Show()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.ShowWindow(Handle);
        Utilities.CheckError();
    }

    public void Hide()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.HideWindow(Handle);
        Utilities.CheckError();
    }

    public void Focus()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.FocusWindow(Handle);
        Utilities.CheckError();
    }

    public void RequestAttention()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.RequestWindowAttention(Handle);
        Utilities.CheckError();
    }

    public void SetMonitor(Monitor? monitor, Point position, Size size, int refreshRate)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetWindowMonitor(
            Handle,
            monitor is null ? null : monitor.Handle,
            position.X,
            position.Y,
            size.Width,
            size.Height,
            refreshRate
        );

        Utilities.CheckError();
    }

    public void SetPositionCallback(WindowPositionCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.PositionCallback = callback;
        Unsafe.Glfw.SetWindowPosCallback(
            Handle,
            callback is null ? null : &WindowPositionCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetSizeCallback(WindowSizeCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.SizeCallback = callback;
        Unsafe.Glfw.SetWindowSizeCallback(
            Handle,
            callback is null ? null : &WindowSizeCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetCloseCallback(WindowCloseCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.CloseCallback = callback;
        Unsafe.Glfw.SetWindowCloseCallback(
            Handle,
            callback is null ? null : &WindowCloseCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetRefreshCallback(WindowRefreshCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.RefreshCallback = callback;
        Unsafe.Glfw.SetWindowRefreshCallback(
            Handle,
            callback is null ? null : &WindowRefreshCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetFocusCallback(WindowFocusCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.FocusCallback = callback;
        Unsafe.Glfw.SetWindowFocusCallback(
            Handle,
            callback is null ? null : &WindowFocusCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetIconifyCallback(WindowIconifyCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.IconifyCallback = callback;
        Unsafe.Glfw.SetWindowIconifyCallback(
            Handle,
            callback is null ? null : &WindowIconifyCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetMaximizeCallback(WindowMaximizeCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.MaximizeCallback = callback;
        Unsafe.Glfw.SetWindowMaximizeCallback(
            Handle,
            callback is null ? null : &WindowMaximizeCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetFramebufferSizeCallback(FramebufferSizeCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.FramebufferSizeCallback = callback;
        Unsafe.Glfw.SetFramebufferSizeCallback(
            Handle,
            callback is null ? null : &FramebufferSizeCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetContentScaleCallback(WindowContentScaleCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.ContentScaleCallback = callback;
        Unsafe.Glfw.SetWindowContentScaleCallback(
            Handle,
            callback is null ? null : &WindowContentScaleCallbackHandler
        );
        Utilities.CheckError();
    }

    public KeyAction GetKey(Key key)
    {
        ThrowIfDisposed();
        var action = Unsafe.Glfw.GetKey(Handle, key);
        Utilities.CheckError();
        return action;
    }

    public ButtonAction GetMouseButton(MouseButton button)
    {
        ThrowIfDisposed();
        var action = Unsafe.Glfw.GetMouseButton(Handle, button);
        Utilities.CheckError();
        return action;
    }

    public void SetCursor(Cursor? cursor)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetCursor(Handle, cursor is null ? null : cursor.Handle);
        Utilities.CheckError();
    }

    public void SetKeyCallback(KeyCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.KeyCallback = callback;
        Unsafe.Glfw.SetKeyCallback(Handle, callback is null ? null : &KeyCallbackHandler);
        Utilities.CheckError();
    }

    public void SetCharCallback(CharCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.CharCallback = callback;
        Unsafe.Glfw.SetCharCallback(Handle, callback is null ? null : &CharCallbackHandler);
        Utilities.CheckError();
    }

    public void SetCharModsCallback(CharModsCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.CharModsCallback = callback;
        Unsafe.Glfw.SetCharModsCallback(Handle, callback is null ? null : &CharModsCallbackHandler);
        Utilities.CheckError();
    }

    public void SetMouseButtonCallback(MouseButtonCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.MouseButtonCallback = callback;
        Unsafe.Glfw.SetMouseButtonCallback(
            Handle,
            callback is null ? null : &MouseButtonCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetCursorPositionCallback(CursorPositionCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.CursorPositionCallback = callback;
        Unsafe.Glfw.SetCursorPosCallback(
            Handle,
            callback is null ? null : &CursorPositionCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetCursorEnterCallback(CursorEnterCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.CursorEnterCallback = callback;
        Unsafe.Glfw.SetCursorEnterCallback(
            Handle,
            callback is null ? null : &CursorEnterCallbackHandler
        );
        Utilities.CheckError();
    }

    public void SetScrollCallback(ScrollCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.ScrollCallback = callback;
        Unsafe.Glfw.SetScrollCallback(Handle, callback is null ? null : &ScrollCallbackHandler);
        Utilities.CheckError();
    }

    private sealed class CallbacksWrapper
    {
        public WindowPositionCallback? PositionCallback { get; set; }
        public WindowSizeCallback? SizeCallback { get; set; }
        public WindowCloseCallback? CloseCallback { get; set; }
        public WindowRefreshCallback? RefreshCallback { get; set; }
        public WindowFocusCallback? FocusCallback { get; set; }
        public WindowIconifyCallback? IconifyCallback { get; set; }
        public WindowMaximizeCallback? MaximizeCallback { get; set; }
        public FramebufferSizeCallback? FramebufferSizeCallback { get; set; }
        public WindowContentScaleCallback? ContentScaleCallback { get; set; }
        public KeyCallback? KeyCallback { get; set; }
        public CharCallback? CharCallback { get; set; }
        public CharModsCallback? CharModsCallback { get; set; }
        public MouseButtonCallback? MouseButtonCallback { get; set; }
        public CursorPositionCallback? CursorPositionCallback { get; set; }
        public CursorEnterCallback? CursorEnterCallback { get; set; }
        public ScrollCallback? ScrollCallback { get; set; }
        public DropCallback? DropCallback { get; set; }
    }
}
