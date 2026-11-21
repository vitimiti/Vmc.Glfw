// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

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
}
