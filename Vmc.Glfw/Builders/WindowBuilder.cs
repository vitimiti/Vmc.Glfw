// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL w not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Drawing;
using System.Runtime.InteropServices;

namespace Vmc.Glfw.Builders;

/// <summary>
/// Builder class for creating and configuring a Window.
/// </summary>
public sealed class WindowBuilder
{
    private Window? _window;
    private unsafe Unsafe.Glfw.Window* _handle;

    internal WindowBuilder() { }

    /// <summary>
    /// Sets whether the window should be focused when created.
    /// </summary>
    /// <param name="b"><see langword="true"/> to focus the window, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Focused(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Focused, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should be iconified when created.
    /// </summary>
    /// <param name="b"><see langword="true"/> to iconify the window, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Iconified(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Iconified, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should be resizable.
    /// </summary>
    /// <param name="b"><see langword="true"/> to make the window resizable, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Resizable(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Resizable, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should be visible when created.
    /// </summary>
    /// <param name="b"><see langword="true"/> to make the window visible, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Visible(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Visible, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should have decorations.
    /// </summary>
    /// <param name="b"><see langword="true"/> to add decorations, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Decorated(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Decorated, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should automatically iconify.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable auto iconify, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder AutoIconifies(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AutoIconify, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should float above other windows.
    /// </summary>
    /// <param name="b"><see langword="true"/> to make the window floating, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Floating(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Floating, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should be maximized when created.
    /// </summary>
    /// <param name="b"><see langword="true"/> to maximize the window, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Maximized(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Maximized, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the cursor should be centered in the window.
    /// </summary>
    /// <param name="b"><see langword="true"/> to center the cursor, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder CentersCursor(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.CenterCursor, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should have a transparent frameBuffer.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable transparent frameBuffer, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder HasTransparentFrameBuffer(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.TransparentFrameBuffer, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should gain focus when shown.
    /// </summary>
    /// <param name="b"><see langword="true"/> to focus on show, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder FocusesOnShow(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.FocusOnShow, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should allow mouse passthrough.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable mouse passthrough, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithMousePassthrough(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.MousePassthrough, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the position of the window.
    /// </summary>
    /// <param name="p">The position to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithPosition(Point p)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionX, p.X);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionY, p.Y);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the window to have any position.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithAnyPosition()
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionX, Unsafe.Glfw.AnyPosition);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.PositionY, Unsafe.Glfw.AnyPosition);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should scale to the monitor.
    /// </summary>
    /// <param name="b"><see langword="true"/> to scale to the monitor, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder ScalesToMonitor(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ScaleToMonitor, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window's frameBuffer should scale.
    /// </summary>
    /// <param name="b"><see langword="true"/> to scale the frameBuffer, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder ScalesFrameBuffer(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ScaleFrameBuffer, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the window should use a Retina frameBuffer on Cocoa.
    /// </summary>
    /// <param name="b"><see langword="true"/> to use a Retina frameBuffer, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithCocoaRetinaFrameBuffer(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.CocoaRetinaFrameBuffer, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the Cocoa frame name for the window.
    /// </summary>
    /// <param name="s">The frame name to set.</param>
    /// <returns>The current builder instance.</returns>
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

    /// <summary>
    /// Sets whether the window should use Cocoa graphics switching.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable Cocoa graphics switching, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithCocoaGraphicsSwitching(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.CocoaGraphicsSwitching, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the X11 class name for the window.
    /// </summary>
    /// <param name="s">The class name to set.</param>
    /// <returns>The current builder instance.</returns>
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

    /// <summary>
    /// Sets the X11 instance name for the window.
    /// </summary>
    /// <param name="s">The instance name to set.</param>
    /// <returns>The current builder instance.</returns>
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

    /// <summary>
    /// Sets whether the Win32 keyboard menu is enabled for the window.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable the Win32 keyboard menu, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithWin32KeyboardMenu(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Win32KeyboardMenu, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the Win32 window should show the default.
    /// </summary>
    /// <param name="b"><see langword="true"/> to show the default, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder Win32ShowsDefault(bool b)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Win32ShowDefault, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the Wayland app ID for the window.
    /// </summary>
    /// <param name="s">The app ID to set.</param>
    /// <returns>The current builder instance.</returns>
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

    /// <summary>
    /// Sets the frameBuffer bits for the window.
    /// </summary>
    /// <param name="color">The color specifying the frameBuffer bits.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithFrameBufferBits(Color color)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.RedBits, color.R);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.GreenBits, color.G);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.BlueBits, color.B);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AlphaBits, color.A);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the depth bits for the window.
    /// </summary>
    /// <param name="bits">The number of depth bits to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithDepthBits(int bits)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.DepthBits, bits);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the stencil bits for the window.
    /// </summary>
    /// <param name="bits">The number of stencil bits to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithStencilBits(int bits)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.StencilBits, bits);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the accumulation buffer bits for the window.
    /// </summary>
    /// <param name="color">The color specifying the accumulation buffer bits.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithAccumulationBits(Color color)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumRedBits, color.R);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumGreenBits, color.G);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumBlueBits, color.B);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AccumAlphaBits, color.A);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the number of auxiliary buffers for the window.
    /// </summary>
    /// <param name="count">The number of auxiliary buffers to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithAuxiliaryBuffers(int count)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.AuxBuffers, count);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Enables or disables stereo rendering for the window.
    /// </summary>
    /// <param name="enabled">True to enable stereo rendering, false to disable.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithStereo(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Stereo, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the number of samples for multisampling.
    /// </summary>
    /// <param name="count">The number of samples to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithSamples(int count)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.Samples, count);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Enables or disables sRGB capability for the window.
    /// </summary>
    /// <param name="enabled">True to enable sRGB capability, false to disable.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder IsSrgbCapable(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.SrgbCapable, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Enables or disables double buffering for the window.
    /// </summary>
    /// <param name="enabled">True to enable double buffering, false to disable.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithDoubleBuffer(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.DoubleBuffer, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the client API for the window.
    /// </summary>
    /// <param name="api">The client API to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithClientApi(ClientApi api)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ClientApi, (int)api);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the context version for the window.
    /// </summary>
    /// <param name="version">The context version to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithContextVersion(Version version)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextVersionMajor, version.Major);
        Utilities.CheckError();
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextVersionMinor, version.Minor);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the context robustness for the window.
    /// </summary>
    /// <param name="robustness">The context robustness to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithContextRobustness(ContextRobustness robustness)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextRobustness, (int)robustness);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Enables or disables OpenGL forward compatibility for the window.
    /// </summary>
    /// <param name="enabled">True to enable OpenGL forward compatibility, false to disable.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder IsOpenGLForwardCompatible(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.OpenGLForwardCompat, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Enables or disables context debug for the window.
    /// </summary>
    /// <param name="enabled">True to enable context debug, false to disable.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithContextDebug(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextDebug, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the OpenGL profile for the window.
    /// </summary>
    /// <param name="profile">The OpenGL profile to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithOpenGLProfile(OpenGLProfile profile)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.OpenGLProfile, (int)profile);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the context release behavior for the window.
    /// </summary>
    /// <param name="behavior">The context release behavior to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithContextReleaseBehavior(ContextReleaseBehavior behavior)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextReleaseBehavior, (int)behavior);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Enables or disables context no error for the window.
    /// </summary>
    /// <param name="enabled">True to enable context no error, false to disable.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithContextNoError(bool enabled)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextNoError, enabled ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the context creation API for the window.
    /// </summary>
    /// <param name="api">The context creation API to set.</param>
    /// <returns>The current builder instance.</returns>
    public WindowBuilder WithContextCreationApi(ContextCreationApi api)
    {
        Unsafe.Glfw.WindowHint(Unsafe.Glfw.ContextCreationApi, (int)api);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Builds the window with the specified size, title, monitor, and shared window.
    /// </summary>
    /// <param name="size">The size of the window.</param>
    /// <param name="title">The title of the window.</param>
    /// <param name="monitor">The monitor to use for full screen mode, or <see langword="null"/> for windowed mode.</param>
    /// <param name="share">The window to share resources with, or <see langword="null"/> if not sharing.</param>
    /// <returns>The created window instance.</returns>
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
