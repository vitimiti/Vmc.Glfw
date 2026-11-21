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

public delegate void MonitorCallback(Monitor monitor, ConnectionEventType eventType);

public sealed unsafe class Monitor : IDisposable
{
    private static readonly CallbacksWrapper Callbacks = new();

    private readonly bool _isOwned;

    private GCHandle _gcHandle;
    private bool _disposed;

    internal Unsafe.Glfw.Monitor* Handle { get; }

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
            var rampCount = value.Red.Length;
            var redSpan = value.Red.AsSpan();
            var greenSpan = value.Green.AsSpan();
            var blueSpan = value.Blue.AsSpan();
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
        }
        finally
        {
            if (_gcHandle.IsAllocated)
            {
                _gcHandle.Free();
            }
        }
    }

    public static void SetCallback(MonitorCallback callback)
    {
        Callbacks.Callback = callback;
        Unsafe.Glfw.SetMonitorCallback(&MonitorCallbackHandler);
        Utilities.CheckError();
    }

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

    public void SetGamma(float gamma)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SetGamma(Handle, gamma);
        Utilities.CheckError();
    }

    private sealed class CallbacksWrapper
    {
        public MonitorCallback? Callback { get; set; }
    }
}
