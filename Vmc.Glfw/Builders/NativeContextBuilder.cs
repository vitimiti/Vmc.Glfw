// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vmc.Glfw.Builders;

/// <summary>
/// Builder class for creating and configuring a NativeContext.
/// </summary>
public sealed class NativeContextBuilder
{
    private NativeContext? _context;

    /// <summary>
    /// Sets whether joystick hats should be exposed as buttons.
    /// </summary>
    /// <param name="b"><see langword="true"/> to expose joystick hats as buttons, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public NativeContextBuilder WithJoystickHatsExposedAsButtons(bool b)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.JoystickHatButtons, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the ANGLE platform type.
    /// </summary>
    /// <param name="type">The ANGLE platform type to set.</param>
    /// <returns>The current builder instance.</returns>
    public NativeContextBuilder WithAnglePlatformType(AnglePlatformType type)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.AnglePlatformType, (int)type);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets the platform for the native context.
    /// </summary>
    /// <param name="platform">The platform to set.</param>
    /// <returns>The current builder instance.</returns>
    public NativeContextBuilder WithPlatform(Platform platform)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.Platform, (int)platform);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the Cocoa chdir resources option is enabled.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable, <see langword="false"/> to disable.</param>
    /// <returns>The current builder instance.</returns>
    public NativeContextBuilder WithCocoaChDirResources(bool b)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.CocoaChDirResources, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the Cocoa menubar option is enabled.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable, <see langword="false"/> to disable.</param>
    /// <returns>The current builder instance.</returns>
    public NativeContextBuilder WithCocoaMenubar(bool b)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.CocoaMenubar, b ? 1 : 0);
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether the Wayland libdecor option is enabled.
    /// </summary>
    /// <param name="b"><see langword="true"/> to enable, <see langword="false"/> to disable.</param>
    /// <returns>The current builder instance.</returns>
    public NativeContextBuilder WithWaylandLibDecor(bool b)
    {
        Unsafe.Glfw.InitHint(
            Unsafe.Glfw.WaylandLibDecor,
            b ? Unsafe.Glfw.WaylandPreferLibDecor : Unsafe.Glfw.WaylandDisableLibDecor
        );
        Utilities.CheckError();
        return this;
    }

    /// <summary>
    /// Sets whether to use .NET allocators for memory management.
    /// </summary>
    /// <param name="b"><see langword="true"/> to use .NET allocators, <see langword="false"/> otherwise.</param>
    /// <returns>The current builder instance.</returns>
    public unsafe NativeContextBuilder WithDotnetAllocators(bool b)
    {
        if (b)
        {
            Unsafe.Glfw.Allocator allocator = new()
            {
                Allocate = &AllocateFn,
                Reallocate = &ReallocateFn,
                Deallocate = &DeallocateFn,
                User = null,
            };
            Unsafe.Glfw.InitAllocator(in allocator);
            Utilities.CheckError();
        }
        return this;
    }

    /// <summary>
    /// Builds and returns the configured NativeContext instance.
    /// </summary>
    /// <returns>The configured NativeContext instance.</returns>
    public NativeContext Build() => _context ??= new NativeContext();

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void* AllocateFn(nuint size, void* _) => NativeMemory.Alloc(size);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void* ReallocateFn(void* block, nuint size, void* _) =>
        NativeMemory.Realloc(block, size);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void DeallocateFn(void* block, void* _) => NativeMemory.Free(block);
}
