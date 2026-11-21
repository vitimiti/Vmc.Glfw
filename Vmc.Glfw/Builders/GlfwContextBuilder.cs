// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vmc.Glfw.Builders;

public sealed class GlfwContextBuilder
{
    private GlfwContext? _context;

    public GlfwContextBuilder WithJoystickHatsExposedAsButtons(bool b)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.JoystickHatButtons, b ? 1 : 0);
        CheckError();
        return this;
    }

    public GlfwContextBuilder WithCocoaChDirResources(bool b)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.CocoaChDirResources, b ? 1 : 0);
        CheckError();
        return this;
    }

    public GlfwContextBuilder WithCocoaMenubar(bool b)
    {
        Unsafe.Glfw.InitHint(Unsafe.Glfw.CocoaMenubar, b ? 1 : 0);
        CheckError();
        return this;
    }

    public GlfwContextBuilder WithWaylandLibDecor(bool b)
    {
        Unsafe.Glfw.InitHint(
            Unsafe.Glfw.WaylandLibDecor,
            b ? Unsafe.Glfw.WaylandPreferLibDecor : Unsafe.Glfw.WaylandDisableLibDecor
        );
        CheckError();
        return this;
    }

    public unsafe GlfwContextBuilder WithDotnetAllocators(bool b)
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
            CheckError();
        }
        return this;
    }

    public GlfwContext Build() => _context ??= new GlfwContext();

    private static unsafe void CheckError()
    {
        var code = Unsafe.Glfw.GetError(out var description);
        if (code is not ErrorCode.None)
        {
            throw new GlfwException(code, Marshal.PtrToStringUTF8((nint)description));
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void* AllocateFn(nuint size, void* _) => NativeMemory.Alloc(size);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void* ReallocateFn(void* block, nuint size, void* _) =>
        NativeMemory.Realloc(block, size);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void DeallocateFn(void* block, void* _) => NativeMemory.Free(block);
}
