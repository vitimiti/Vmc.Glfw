// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Vmc.Glfw.Builders;
using SysUnsafe = System.Runtime.CompilerServices.Unsafe;

namespace Vmc.Glfw;

public sealed class NativeContext : IDisposable
{
    private bool _disposed;

    public static Version Version
    {
        get
        {
            Unsafe.Glfw.GetVersion(out var major, out var minor, out var rev);
            return new Version(major, minor, rev);
        }
    }

    public static unsafe string VersionString
    {
        get
        {
            return Marshal.PtrToStringUTF8((nint)Unsafe.Glfw.GetVersionString()) ?? string.Empty;
        }
    }

    public Platform Platform
    {
        get
        {
            ThrowIfDisposed();
            var platform = Unsafe.Glfw.GetPlatform();
            Utilities.CheckError();
            return platform;
        }
    }

    public bool RawMouseMotionSupported
    {
        get
        {
            ThrowIfDisposed();
            var rawMouseMotionSupported = Unsafe.Glfw.RawMouseMotionSupported();
            Utilities.CheckError();
            return rawMouseMotionSupported;
        }
    }

    public TimeSpan Time
    {
        get
        {
            ThrowIfDisposed();
            var time = Unsafe.Glfw.GetTime();
            Utilities.CheckError();
            return TimeSpan.FromSeconds(time);
        }
        set
        {
            ThrowIfDisposed();
            Unsafe.Glfw.SetTime(value.TotalSeconds);
            Utilities.CheckError();
        }
    }

    public ulong TimerValue
    {
        get
        {
            ThrowIfDisposed();
            var timerValue = Unsafe.Glfw.GetTimerValue();
            Utilities.CheckError();
            return timerValue;
        }
    }

    public ulong TimerFrequency
    {
        get
        {
            ThrowIfDisposed();
            var timerFrequency = Unsafe.Glfw.GetTimerFrequency();
            Utilities.CheckError();
            return timerFrequency;
        }
    }

    public unsafe Window? CurrentContext
    {
        get
        {
            ThrowIfDisposed();
            var window = Unsafe.Glfw.GetCurrentContext();
            Utilities.CheckError();
            return window is null ? null : new Window(window, isOwned: false);
        }
    }

    internal NativeContext()
    {
        Unsafe.Glfw.Init();
        Utilities.CheckError();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Unsafe.Glfw.Terminate();
        Utilities.CheckError();
    }

    public bool IsPlatformSupported(Platform platform)
    {
        ThrowIfDisposed();
        var supported = Unsafe.Glfw.PlatformSupported(platform);
        Utilities.CheckError();
        return supported;
    }

    public unsafe ReadOnlyCollection<Monitor>? GetMonitors()
    {
        ThrowIfDisposed();
        var monitors = Unsafe.Glfw.GetMonitors(out var count);
        Utilities.CheckError();
        if (monitors is null)
        {
            return null;
        }
        if (count == 0)
        {
            return [];
        }

        var monitorList = new Monitor[count];
        for (var i = 0; i < count; i++)
        {
            if (monitors[i] is null)
            {
                continue;
            }

            monitorList[i] = new Monitor(monitors[i]);
        }

        return new ReadOnlyCollection<Monitor>(monitorList);
    }

    public unsafe Monitor? GetPrimaryMonitor()
    {
        ThrowIfDisposed();
        var monitor = Unsafe.Glfw.GetPrimaryMonitor();
        Utilities.CheckError();
        return monitor is null ? null : new Monitor(monitor);
    }

    public WindowBuilder CreateWindowBuilder()
    {
        ThrowIfDisposed();
        return new WindowBuilder();
    }

    public void PollEvents()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.PollEvents();
        Utilities.CheckError();
    }

    public void WaitEvents()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.WaitEvents();
        Utilities.CheckError();
    }

    public void WaitEventsTimeout(TimeSpan timeout)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.WaitEventsTimeout(timeout.TotalSeconds);
        Utilities.CheckError();
    }

    public void PostEmptyEvent()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.PostEmptyEvent();
        Utilities.CheckError();
    }

    public unsafe string? GetKeyName(Key key, int scancode)
    {
        ThrowIfDisposed();
        var keyName = Unsafe.Glfw.GetKeyName(key, scancode);
        Utilities.CheckError();
        return keyName is null ? null : Marshal.PtrToStringAnsi((nint)keyName);
    }

    public int GetKeyScancode(Key key)
    {
        ThrowIfDisposed();
        var scancode = Unsafe.Glfw.GetKeyScancode(key);
        Utilities.CheckError();
        return scancode;
    }

    public unsafe Cursor CreateCursor(Image image, Point hotspot)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(image);
        var pixelsSpan = image.Pixels.ToArray().AsSpan();
        var actualImage = new Unsafe.Glfw.Image
        {
            Width = image.Size.Width,
            Height = image.Size.Height,
            Pixels = (byte*)SysUnsafe.AsPointer(ref pixelsSpan),
        };

        var cursor = Unsafe.Glfw.CreateCursor(in actualImage, hotspot.X, hotspot.Y);
        Utilities.CheckError();
        return new Cursor(cursor);
    }

    public unsafe Cursor CreateStandardCursor(CursorShape shape)
    {
        ThrowIfDisposed();
        var cursor = Unsafe.Glfw.CreateStandardCursor(shape);
        Utilities.CheckError();
        return new Cursor(cursor);
    }

    public Joystick GetJoystick(JoystickID id)
    {
        ThrowIfDisposed();
        return new Joystick(id);
    }

    public unsafe bool IsExtensionSupported(string extension)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(extension);
        var extensionPtr = Marshal.StringToCoTaskMemUTF8(extension);
        try
        {
            var isSupported = Unsafe.Glfw.ExtensionSupported((byte*)extensionPtr);
            Utilities.CheckError();
            return isSupported;
        }
        finally
        {
            Marshal.FreeCoTaskMem(extensionPtr);
        }
    }

    public unsafe TDelegate GetProcAddress<TDelegate>(string procname)
        where TDelegate : Delegate
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(procname);
        var procnamePtr = Marshal.StringToCoTaskMemUTF8(procname);
        try
        {
            var address = Unsafe.Glfw.GetProcAddress((byte*)procnamePtr);
            Utilities.CheckError();
            return Marshal.GetDelegateForFunctionPointer<TDelegate>((nint)address);
        }
        finally
        {
            Marshal.FreeCoTaskMem(procnamePtr);
        }
    }
}
