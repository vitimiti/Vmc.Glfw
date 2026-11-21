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

/// <summary>
/// Represents a window in the GLFW context.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="position">The new position of the window.</param>
public delegate void WindowPositionCallback(Window window, Point position);

/// <summary>
/// Represents a callback for when the window is resized.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="size">The new size of the window.</param>
public delegate void WindowSizeCallback(Window window, Size size);

/// <summary>
/// Represents a callback for when the window is closed.
/// </summary>
/// <param name="window">The window instance.</param>
public delegate void WindowCloseCallback(Window window);

/// <summary>
/// Represents a callback for when the window is refreshed.
/// </summary>
/// <param name="window">The window instance.</param>
public delegate void WindowRefreshCallback(Window window);

/// <summary>
/// Represents a callback for when the window gains or loses focus.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="focused">Indicates whether the window is focused.</param>
public delegate void WindowFocusCallback(Window window, bool focused);

/// <summary>
/// Represents a callback for when the window is iconified.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="iconified">Indicates whether the window is iconified.</param>
public delegate void WindowIconifyCallback(Window window, bool iconified);

/// <summary>
/// Represents a callback for when the window is maximized.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="maximized">Indicates whether the window is maximized.</param>
public delegate void WindowMaximizeCallback(Window window, bool maximized);

/// <summary>
/// Represents a callback for when the framebuffer size changes.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="size">The new size of the framebuffer.</param>
public delegate void FramebufferSizeCallback(Window window, Size size);

/// <summary>
/// Represents a callback for when the window content scale changes.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="scale">The new content scale of the window.</param>
public delegate void WindowContentScaleCallback(Window window, Vector2 scale);

/// <summary>
/// Represents a callback for when a key is pressed, repeated, or released.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="key">The key that was pressed or released.</param>
/// <param name="scancode">The platform-specific scancode of the key.</param>
/// <param name="action">The action (press, release, repeat).</param>
/// <param name="mods">The modifier keys active during the event.</param>
public delegate void KeyCallback(
    Window window,
    Key key,
    int scancode,
    KeyAction action,
    KeyModifiers mods
);

/// <summary>
/// Represents a callback for when a character is input.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="codepoint">The Unicode codepoint of the character.</param>
public delegate void CharCallback(Window window, uint codepoint);

/// <summary>
/// Represents a callback for when a character is input with modifier keys.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="codepoint">The Unicode codepoint of the character.</param>
/// <param name="mods">The modifier keys active during the event.</param>
public delegate void CharModsCallback(Window window, uint codepoint, KeyModifiers mods);

/// <summary>
/// Represents a callback for when a mouse button is pressed or released.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="button">The mouse button that was pressed or released.</param>
/// <param name="action">The action (press or release).</param>
/// <param name="mods">The modifier keys active during the event.</param>
public delegate void MouseButtonCallback(
    Window window,
    MouseButton button,
    ButtonAction action,
    KeyModifiers mods
);

/// <summary>
/// Represents a callback for when the cursor position changes.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="position">The new cursor position.</param>
public delegate void CursorPositionCallback(Window window, PointF position);

/// <summary>
/// Represents a callback for when the cursor enters or leaves the window.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="entered">Indicates whether the cursor entered (true) or left (false) the window.</param>
public delegate void CursorEnterCallback(Window window, bool entered);

/// <summary>
/// Represents a callback for when the scroll wheel is used.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="offset">The scroll offset.</param>
public delegate void ScrollCallback(Window window, Vector2 offset);

/// <summary>
/// Represents a callback for when files are dropped onto the window.
/// </summary>
/// <param name="window">The window instance.</param>
/// <param name="paths">The paths of the dropped files.</param>
public delegate void DropCallback(Window window, string[] paths);

/// <summary>
/// Represents a window instance.
/// </summary>
/// <remarks>
/// This class implements <see cref="IDisposable"/>.
/// </remarks>
public sealed unsafe class Window : IDisposable
{
    private static readonly CallbacksWrapper Callbacks = new();

    private readonly bool _isOwned;

    private GCHandle _gcHandle;
    private bool _disposed;

    /// <summary>
    /// Gets the native handle of the window.
    /// </summary>
    internal Unsafe.Glfw.Window* Handle { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the window should close.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets the title of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets the position of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets the size of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the size of the framebuffer.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the size of the window frame.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the content scale of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets the opacity of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the monitor that the window is currently on, if any.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window is focused.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window is iconified (minimized).
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window is resizable.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window is visible.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window is decorated.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window auto-iconifies.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window is floating.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window is maximized.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window has a transparent framebuffer.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets whether the window is hovered.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window should take focus when shown.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window has mouse passthrough enabled.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets the cursor input mode for the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window has sticky keys enabled.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window has sticky mouse buttons enabled.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window locks key mods.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets whether the window has raw mouse motion enabled.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets whether the window has double buffering enabled.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public bool HasDoubleBuffer
    {
        get
        {
            ThrowIfDisposed();
            var hasDoubleBuffer = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.DoubleBuffer);
            Utilities.CheckError();
            return hasDoubleBuffer != 0;
        }
    }

    /// <summary>
    /// Gets the client API used by the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public ClientApi ClientApi
    {
        get
        {
            ThrowIfDisposed();
            var clientApi = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ClientApi);
            Utilities.CheckError();
            return (ClientApi)clientApi;
        }
    }

    /// <summary>
    /// Gets the context version of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public Version ContextVersion
    {
        get
        {
            ThrowIfDisposed();
            var major = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextVersionMajor);
            Utilities.CheckError();
            var minor = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextVersionMinor);
            Utilities.CheckError();
            var rev = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextRevision);
            Utilities.CheckError();
            return new Version(major, minor, rev);
        }
    }

    /// <summary>
    /// Gets the context robustness of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public ContextRobustness ContextRobustness
    {
        get
        {
            ThrowIfDisposed();
            var robustness = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextRobustness);
            Utilities.CheckError();
            return (ContextRobustness)robustness;
        }
    }

    /// <summary>
    /// Gets whether the OpenGL context is forward compatible.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public bool IsOpenGLForwardCompatible
    {
        get
        {
            ThrowIfDisposed();
            var forwardCompat = Unsafe.Glfw.GetWindowAttrib(
                Handle,
                Unsafe.Glfw.OpenGLForwardCompat
            );
            Utilities.CheckError();
            return forwardCompat != 0;
        }
    }

    /// <summary>
    /// Gets whether the OpenGL context is a debug context.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public bool IsContextDebug
    {
        get
        {
            ThrowIfDisposed();
            var contextDebug = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextDebug);
            Utilities.CheckError();
            return contextDebug != 0;
        }
    }

    /// <summary>
    /// Gets the OpenGL profile of the context.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public OpenGLProfile OpenGLProfile
    {
        get
        {
            ThrowIfDisposed();
            var profile = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.OpenGLProfile);
            Utilities.CheckError();
            return (OpenGLProfile)profile;
        }
    }

    /// <summary>
    /// Gets the context release behavior of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public ContextReleaseBehavior ContextReleaseBehavior
    {
        get
        {
            ThrowIfDisposed();
            var behavior = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextReleaseBehavior);
            Utilities.CheckError();
            return (ContextReleaseBehavior)behavior;
        }
    }

    /// <summary>
    /// Gets whether the context has the no-error flag set.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public bool IsContextNoError
    {
        get
        {
            ThrowIfDisposed();
            var noError = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextNoError);
            Utilities.CheckError();
            return noError != 0;
        }
    }

    /// <summary>
    /// Gets the context creation API of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public ContextCreationApi ContextCreationApi
    {
        get
        {
            ThrowIfDisposed();
            var api = Unsafe.Glfw.GetWindowAttrib(Handle, Unsafe.Glfw.ContextCreationApi);
            Utilities.CheckError();
            return (ContextCreationApi)api;
        }
    }

    /// <summary>
    /// Gets or sets the cursor position of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets the clipboard string of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Creates a new instance of the Window class with the specified handle and ownership flag.
    /// </summary>
    /// <param name="handle">The native GLFW window handle.</param>
    /// <param name="isOwned">Indicates whether the window instance owns the native handle.</param>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Disposes the window and releases its resources.
    /// </summary>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the icon of the window.
    /// </summary>
    /// <param name="images">A collection of images to use as the window icon.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the size limits of the window.
    /// </summary>
    /// <param name="minSize">The minimum size of the window.</param>
    /// <param name="maxSize">The maximum size of the window.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the aspect ratio of the window.
    /// </summary>
    /// <param name="numer">The numerator of the aspect ratio.</param>
    /// <param name="denom">The denominator of the aspect ratio.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetAspectRatio(int numer, int denom)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetWindowAspectRatio(Handle, numer, denom);
        Utilities.CheckError();
    }

    /// <summary>
    /// Iconifies the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void Iconify()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.IconifyWindow(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Restores the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void Restore()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.RestoreWindow(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Maximizes the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void Maximize()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.MaximizeWindow(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Shows the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void Show()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.ShowWindow(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Hides the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void Hide()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.HideWindow(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Focuses the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void Focus()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.FocusWindow(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Requests attention for the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void RequestAttention()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.RequestWindowAttention(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Sets the monitor, position, size, and refresh rate of the window.
    /// </summary>
    /// <param name="monitor">The monitor to set, or <see langword="null"/> for windowed mode.</param>
    /// <param name="position">The position of the window.</param>
    /// <param name="size">The size of the window.</param>
    /// <param name="refreshRate">The refresh rate of the window.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the position callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the size callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the close callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the refresh callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the focus callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the iconify callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the maximize callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the framebuffer size callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the content scale callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the action of the specified key.
    /// </summary>
    /// <param name="key">The key to query.</param>
    /// <returns>The action of the key.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public KeyAction GetKey(Key key)
    {
        ThrowIfDisposed();
        var action = Unsafe.Glfw.GetKey(Handle, key);
        Utilities.CheckError();
        return action;
    }

    /// <summary>
    /// Gets the action of the specified mouse button.
    /// </summary>
    /// <param name="button">The mouse button to query.</param>
    /// <returns>The action of the mouse button.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public ButtonAction GetMouseButton(MouseButton button)
    {
        ThrowIfDisposed();
        var action = Unsafe.Glfw.GetMouseButton(Handle, button);
        Utilities.CheckError();
        return action;
    }

    /// <summary>
    /// Sets the cursor for the window.
    /// </summary>
    /// <param name="cursor">The cursor to set, or <see langword="null"/> to remove the cursor.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetCursor(Cursor? cursor)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetCursor(Handle, cursor is null ? null : cursor.Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Sets the key callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetKeyCallback(KeyCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.KeyCallback = callback;
        Unsafe.Glfw.SetKeyCallback(Handle, callback is null ? null : &KeyCallbackHandler);
        Utilities.CheckError();
    }

    /// <summary>
    /// Sets the character callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetCharCallback(CharCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.CharCallback = callback;
        Unsafe.Glfw.SetCharCallback(Handle, callback is null ? null : &CharCallbackHandler);
        Utilities.CheckError();
    }

    /// <summary>
    /// Sets the character with modifiers callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetCharModsCallback(CharModsCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.CharModsCallback = callback;
        Unsafe.Glfw.SetCharModsCallback(Handle, callback is null ? null : &CharModsCallbackHandler);
        Utilities.CheckError();
    }

    /// <summary>
    /// Sets the mouse button callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the cursor position callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the cursor enter callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Sets the scroll callback for the window.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetScrollCallback(ScrollCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.ScrollCallback = callback;
        Unsafe.Glfw.SetScrollCallback(Handle, callback is null ? null : &ScrollCallbackHandler);
        Utilities.CheckError();
    }

    /// <summary>
    /// Makes the OpenGL or OpenGL ES context of the window current on the calling thread.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void MakeContextCurrent()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.MakeContextCurrent(Handle);
        Utilities.CheckError();
    }

    /// <summary>
    /// Swaps the front and back buffers of the window.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the window has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SwapBuffers()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SwapBuffers(Handle);
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
