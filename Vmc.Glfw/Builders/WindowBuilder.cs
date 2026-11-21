// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL w not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Drawing;
using System.Runtime.InteropServices;

namespace Vmc.Glfw.Builders;

public sealed class WindowBuilder
{
    private Window? _window;
    private unsafe Unsafe.Glfw.Window* _handle;

    internal WindowBuilder() { }

    public WindowBuilder Focused(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Focused, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder Iconified(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Iconified, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder Resizable(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Resizable, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder Visible(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Visible, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder Decorated(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Decorated, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder AutoIconifies(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AutoIconify, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder Floating(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Floating, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder Maximized(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Maximized, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder CentersCursor(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.CenterCursor, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder HasTransparentFramebuffer(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.TransparentFramebuffer, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder FocusesOnShow(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.FocusOnShow, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithMousePassthrough(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.MousePassthrough, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithPosition(Point p)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionX, p.X);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionY, p.Y);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithAnyPosition()
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionX, Unsafe.Glfw.AnyPosition);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionY, Unsafe.Glfw.AnyPosition);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder ScalesToMonitor(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ScaleToMonitor, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder ScalesFramebuffer(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ScaleFramebuffer, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithCocoaRetinaFramebuffer(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.CocoaRetinaFramebuffer, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public unsafe WindowBuilder WithCocoaFrameName(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        var namePtr = (byte*)Marshal.StringToCoTaskMemUTF8(s);
        try
        {
            Unsafe.Glfw.WindowHintString(Unsafe.Glfw.CocoaFrameName, namePtr);
            Utilities.CheckError();
            return this;
        }
        finally
        {
            Marshal.FreeCoTaskMem((nint)namePtr);
        }
    }

    public WindowBuilder WithCocoaGraphicsSwitching(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.CocoaGraphicsSwitching, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public unsafe WindowBuilder WithX11ClassName(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        var namePtr = (byte*)Marshal.StringToCoTaskMemUTF8(s);
        try
        {
            Unsafe.Glfw.WindowHintString(Unsafe.Glfw.X11ClassName, namePtr);
            Utilities.CheckError();
            return this;
        }
        finally
        {
            Marshal.FreeCoTaskMem((nint)namePtr);
        }
    }

    public unsafe WindowBuilder WithX11InstanceName(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        var namePtr = (byte*)Marshal.StringToCoTaskMemUTF8(s);
        try
        {
            Unsafe.Glfw.WindowHintString(Unsafe.Glfw.X11InstanceName, namePtr);
            Utilities.CheckError();
            return this;
        }
        finally
        {
            Marshal.FreeCoTaskMem((nint)namePtr);
        }
    }

    public WindowBuilder WithWin32KeyboardMenu(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Win32KeyboardMenu, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder Win32ShowsDefault(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Win32ShowDefault, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public unsafe WindowBuilder WithWaylandAppID(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        var sPtr = (byte*)Marshal.StringToCoTaskMemUTF8(s);
        try
        {
            Unsafe.Glfw.WindowHintString(Unsafe.Glfw.WaylandAppID, sPtr);
            Utilities.CheckError();
            return this;
        }
        finally
        {
            Marshal.FreeCoTaskMem((nint)sPtr);
        }
    }

    public WindowBuilder WithFramebufferBits(Color c)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.RedBits, c.R);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.GreenBits, c.G);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.BlueBits, c.B);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AlphaBits, c.A);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithDepthBits(int bits)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.DepthBits, bits);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithStencilBits(int bits)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.StencilBits, bits);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithAccumulationBits(Color c)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumRedBits, c.R);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumGreenBits, c.G);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumBlueBits, c.B);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumAlphaBits, c.A);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithAuxiliaryBuffers(int count)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AuxBuffers, count);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithStereo(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Stereo, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithSamples(int count)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Samples, count);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder IsSrgbCapable(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.SrgbCapable, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithDoubleBuffer(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.DoubleBuffer, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithClientApi(ClientApi api)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ClientApi, (int)api);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithContextVersion(Version version)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextVersionMajor, version.Major);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextVersionMinor, version.Minor);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithContextRobustness(ContextRobustness robustness)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextRobustness, (int)robustness);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder IsOpenGLForwardCompatible(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.OpenGLForwardCompat, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithContextDebug(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextDebug, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithOpenGLProfile(OpenGLProfile profile)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.OpenGLProfile, (int)profile);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithContextReleaseBehavior(ContextReleaseBehavior behavior)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextReleaseBehavior, (int)behavior);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithContextNoError(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextNoError, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    public WindowBuilder WithContextCreationApi(ContextCreationApi api)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextCreationApi, (int)api);
        Utilities.CheckError();
        return this;
    }

    public unsafe Window Build(
        Size size,
        string title,
        Monitor? monitor = null,
        Window? share = null
    )
    {
        ArgumentNullException.ThrowIfNull(title);
        var titlePtr = (byte*)Marshal.StringToCoTaskMemUTF8(title);
        try
        {
            _handle = Unsafe.Glfw.CreateWindow(
                size.Width,
                size.Height,
                titlePtr,
                monitor is null ? null : monitor.Handle,
                share is null ? null : share.Handle
            );

            Utilities.CheckError();
            return _window ??= new Window(_handle);
        }
        finally
        {
            Marshal.FreeCoTaskMem((nint)titlePtr);
        }
    }
}
