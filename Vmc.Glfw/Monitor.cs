// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SysUnsafe = System.Runtime.CompilerServices.Unsafe;

namespace Vmc.Glfw;

/// <summary>
/// Represents a callback for monitor connection events.
/// </summary>
/// <param name="monitor">The monitor that triggered the event.</param>
/// <param name="eventType">The type of connection event.</param>
public delegate void MonitorCallback(Monitor monitor, ConnectionEventType eventType);

/// <summary>
/// Represents a monitor.
/// </summary>
/// <remarks>
/// This class implements <see cref="IDisposable"/>.
/// </remarks>
public sealed unsafe class Monitor : IDisposable
{
    private static readonly CallbacksWrapper Callbacks = new();

    private readonly bool _isOwned;

    private GCHandle _gcHandle;
    private bool _disposed;

    /// <summary>
    /// Gets the handle to the underlying GLFW monitor.
    /// </summary>
    internal Unsafe.Glfw.Monitor* Handle { get; }

    /// <summary>
    /// Gets the position of the monitor.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public Point Position
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetMonitorPos(Handle, out var x, out var y);
            Utilities.CheckError();
            return new Point(x, y);
        }
    }

    /// <summary>
    /// Gets the work area of the monitor.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public Rectangle WorkArea
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetMonitorWorkarea(
                Handle,
                out var x,
                out var y,
                out var width,
                out var height
            );
            Utilities.CheckError();
            return new Rectangle(x, y, width, height);
        }
    }

    /// <summary>
    /// Gets the physical size of the monitor.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public Size PhysicalSize
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetMonitorPhysicalSize(Handle, out var width, out var height);
            Utilities.CheckError();
            return new Size(width, height);
        }
    }

    /// <summary>
    /// Gets the content scale of the monitor.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public (float X, float Y) ContentScale
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetMonitorContentScale(Handle, out var x, out var y);
            Utilities.CheckError();
            return (x, y);
        }
    }

    /// <summary>
    /// Gets the name of the monitor.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public string Name
    {
        get
        {
            ThrowIfDisposed();
            var name = Unsafe.Glfw.GetMonitorName(Handle);
            Utilities.CheckError();
            return Marshal.PtrToStringUTF8((nint)name) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets or sets the gamma ramp of the monitor.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public GammaRamp GammaRamp
    {
        get
        {
            ThrowIfDisposed();
            var ramp = Unsafe.Glfw.GetGammaRamp(Handle);
            Utilities.CheckError();
            var rampCount = ramp->Size;
            return new GammaRamp(
                new Span<ushort>(ramp->Red, rampCount).ToArray(),
                new Span<ushort>(ramp->Green, rampCount).ToArray(),
                new Span<ushort>(ramp->Blue, rampCount).ToArray()
            );
        }
        set
        {
            ThrowIfDisposed();
            var rampCount = value.Red.Count;
            var redSpan = value.Red.ToArray().AsSpan();
            var greenSpan = value.Green.ToArray().AsSpan();
            var blueSpan = value.Blue.ToArray().AsSpan();
            var ramp = new Unsafe.Glfw.GammaRamp
            {
                Red = (ushort*)SysUnsafe.AsPointer(ref redSpan),
                Green = (ushort*)SysUnsafe.AsPointer(ref greenSpan),
                Blue = (ushort*)SysUnsafe.AsPointer(ref blueSpan),
                Size = rampCount,
            };

            Unsafe.Glfw.SetGammaRamp(Handle, ramp);
            Utilities.CheckError();
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Monitor"/> class.
    /// </summary>
    /// <param name="handle">The handle to the underlying GLFW monitor.</param>
    /// <param name="isOwned">Indicates whether the monitor is owned by this instance.</param>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    internal Monitor(Unsafe.Glfw.Monitor* handle, bool isOwned = true)
    {
        Handle = handle;
        _isOwned = isOwned;
        _gcHandle = GCHandle.Alloc(Callbacks);
        Unsafe.Glfw.SetMonitorUserPointer(handle, (void*)GCHandle.ToIntPtr(_gcHandle));
        Utilities.CheckError();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void MonitorCallbackHandler(
        Unsafe.Glfw.Monitor* monitor,
        ConnectionEventType eventType
    )
    {
        if (monitor is null)
        {
            return;
        }

        var userPtr = Unsafe.Glfw.GetMonitorUserPointer(monitor);
        var wrapper = (CallbacksWrapper?)GCHandle.FromIntPtr((nint)userPtr).Target;
        if (wrapper is null)
        {
            return;
        }

        var unownedMonitor = new Monitor(monitor, isOwned: false);
        wrapper.Callback?.Invoke(unownedMonitor, eventType);
    }

    /// <summary>
    /// Disposes the monitor and releases any associated resources.
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
            Unsafe.Glfw.SetMonitorUserPointer(Handle, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetMonitorCallback(null);
            Utilities.CheckError();

            Unsafe.Glfw.SetMonitorCallback(null);
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
    /// Gets the available video modes for the monitor.
    /// </summary>
    /// <returns>A read-only collection of video modes, or <see langword="null"/> if no modes are available.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public ReadOnlyCollection<VideoMode>? GetVideoModes()
    {
        ThrowIfDisposed();
        var modes = Unsafe.Glfw.GetVideoModes(Handle, out var count);
        Utilities.CheckError();
        if (modes == null)
        {
            return null;
        }

        if (count == 0)
        {
            return [];
        }

        var videoModes = new VideoMode[count];
        for (var i = 0; i < count; i++)
        {
            var mode = modes[i];
            videoModes[i] = new VideoMode(
                new Size(mode.Width, mode.Height),
                (mode.RedBits, mode.GreenBits, mode.BlueBits),
                mode.RefreshRate
            );
        }

        return new ReadOnlyCollection<VideoMode>(videoModes);
    }

    /// <summary>
    /// Gets the current video mode of the monitor.
    /// </summary>
    /// <returns>The current video mode.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public VideoMode GetCurrentVideoMode()
    {
        ThrowIfDisposed();
        var mode = Unsafe.Glfw.GetVideoMode(Handle);
        Utilities.CheckError();
        return new VideoMode(
            new Size(mode->Width, mode->Height),
            (mode->RedBits, mode->GreenBits, mode->BlueBits),
            mode->RefreshRate
        );
    }

    /// <summary>
    /// Sets the gamma value for the monitor.
    /// </summary>
    /// <param name="gamma">The gamma value to set.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetGamma(float gamma)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetGamma(Handle, gamma);
        Utilities.CheckError();
    }

    /// <summary>
    /// Sets the global monitor callback.
    /// </summary>
    /// <param name="callback">The callback to set, or <see langword="null"/> to remove the callback.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the monitor has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetCallback(MonitorCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.Callback = callback;
        Unsafe.Glfw.SetMonitorCallback(callback is null ? null : &MonitorCallbackHandler);
        Utilities.CheckError();
    }

    private sealed class CallbacksWrapper
    {
        public MonitorCallback? Callback { get; set; }
    }
}
