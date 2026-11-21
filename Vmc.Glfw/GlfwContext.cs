// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.InteropServices;

namespace Vmc.Glfw;

public sealed class GlfwContext : IDisposable
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
            return Unsafe.Glfw.GetPlatform();
        }
    }

    internal GlfwContext()
    {
        Unsafe.Glfw.Init();
        Utilities.CheckError();
    }

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
        return Unsafe.Glfw.PlatformSupported(platform);
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);
}
