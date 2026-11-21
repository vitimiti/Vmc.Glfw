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

/// <summary>
/// Represents the native context for GLFW.
/// </summary>
public sealed class NativeContext : IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Gets the version of the GLFW library.
    /// </summary>
    public static Version Version
    {
        get
        {
            Unsafe.Glfw.GetVersion(out var major, out var minor, out var rev);
            return new Version(major, minor, rev);
        }
    }

    /// <summary>
    /// Gets the version string of the GLFW library.
    /// </summary>
    public static unsafe string VersionString
    {
        get
        {
            return Marshal.PtrToStringUTF8((nint)Unsafe.Glfw.GetVersionString()) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the platform of the GLFW library.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets a value indicating whether raw mouse motion is supported.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets or sets the current time of the GLFW library.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the current value of the GLFW timer.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the frequency of the GLFW timer.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the current GLFW context.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Creates a new instance of the native context.
    /// </summary>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    internal NativeContext()
    {
        Unsafe.Glfw.Init();
        Utilities.CheckError();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    /// <summary>
    /// Disposes the native context and releases all associated resources.
    /// </summary>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Determines whether the specified platform is supported by GLFW.
    /// </summary>
    /// <param name="platform">The platform to check.</param>
    /// <returns><see langword="true"/> if the platform is supported; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public bool IsPlatformSupported(Platform platform)
    {
        ThrowIfDisposed();
        var supported = Unsafe.Glfw.PlatformSupported(platform);
        Utilities.CheckError();
        return supported;
    }

    /// <summary>
    /// Gets the list of available monitors.
    /// </summary>
    /// <returns>A read-only collection of monitors, or <see langword="null"/> if no monitors are available.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the primary monitor.
    /// </summary>
    /// <returns>The primary monitor, or <see langword="null"/> if no primary monitor is available.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public unsafe Monitor? GetPrimaryMonitor()
    {
        ThrowIfDisposed();
        var monitor = Unsafe.Glfw.GetPrimaryMonitor();
        Utilities.CheckError();
        return monitor is null ? null : new Monitor(monitor);
    }

    /// <summary>
    /// Creates a new window builder.
    /// </summary>
    /// <returns>A new instance of <see cref="WindowBuilder"/>.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public WindowBuilder CreateWindowBuilder()
    {
        ThrowIfDisposed();
        return new WindowBuilder();
    }

    /// <summary>
    /// Processes all pending events.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void PollEvents()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.PollEvents();
        Utilities.CheckError();
    }

    /// <summary>
    /// Waits for events to be processed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void WaitEvents()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.WaitEvents();
        Utilities.CheckError();
    }

    /// <summary>
    /// Waits for events to be processed with a specified timeout.
    /// </summary>
    /// <param name="timeout">The maximum time to wait for events.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void WaitEventsTimeout(TimeSpan timeout)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.WaitEventsTimeout(timeout.TotalSeconds);
        Utilities.CheckError();
    }

    /// <summary>
    /// Posts an empty event to the event queue.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void PostEmptyEvent()
    {
        ThrowIfDisposed();
        Unsafe.Glfw.PostEmptyEvent();
        Utilities.CheckError();
    }

    /// <summary>
    /// Gets the name of the specified key.
    /// </summary>
    /// <param name="key">The key to get the name for.</param>
    /// <param name="scancode">The scancode of the key.</param>
    /// <returns>The name of the key, or <see langword="null"/> if the key has no name.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public unsafe string? GetKeyName(Key key, int scancode)
    {
        ThrowIfDisposed();
        var keyName = Unsafe.Glfw.GetKeyName(key, scancode);
        Utilities.CheckError();
        return keyName is null ? null : Marshal.PtrToStringAnsi((nint)keyName);
    }

    /// <summary>
    /// Gets the scancode of the specified key.
    /// </summary>
    /// <param name="key">The key to get the scancode for.</param>
    /// <returns>The scancode of the key.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public int GetKeyScancode(Key key)
    {
        ThrowIfDisposed();
        var scancode = Unsafe.Glfw.GetKeyScancode(key);
        Utilities.CheckError();
        return scancode;
    }

    /// <summary>
    /// Creates a new cursor with the specified image and hotspot.
    /// </summary>
    /// <param name="image">The image for the cursor.</param>
    /// <param name="hotspot">The hotspot of the cursor.</param>
    /// <returns>A new instance of <see cref="Cursor"/>.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Creates a standard cursor with the specified shape.
    /// </summary>
    /// <param name="shape">The shape of the cursor.</param>
    /// <returns>A new instance of <see cref="Cursor"/>.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public unsafe Cursor CreateStandardCursor(CursorShape shape)
    {
        ThrowIfDisposed();
        var cursor = Unsafe.Glfw.CreateStandardCursor(shape);
        Utilities.CheckError();
        return new Cursor(cursor);
    }

    /// <summary>
    /// Gets the joystick with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the joystick.</param>
    /// <returns>A new instance of <see cref="Joystick"/>.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public Joystick GetJoystick(JoystickID id)
    {
        ThrowIfDisposed();
        return new Joystick(id);
    }

    /// <summary>
    /// Sets the swap interval for the current context.
    /// </summary>
    /// <param name="interval">The swap interval.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetSwapInterval(int interval)
    {
        ThrowIfDisposed();
        Unsafe.Glfw.SwapInterval(interval);
        Utilities.CheckError();
    }

    /// <summary>
    /// Checks if the specified extension is supported.
    /// </summary>
    /// <param name="extension">The name of the extension.</param>
    /// <returns><see langword="true"/> if the extension is supported; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the function pointer for the specified OpenGL or Vulkan function.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="procname">The name of the function.</param>
    /// <returns>A delegate representing the function.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the context has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public unsafe TDelegate GetProcess<TDelegate>(string procname)
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
