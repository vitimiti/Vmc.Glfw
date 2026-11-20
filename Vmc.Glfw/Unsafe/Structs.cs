// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.InteropServices;

namespace Vmc.Glfw.Unsafe;

internal static unsafe partial class Glfw
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VidMode
    {
        public int Width;
        public int Height;
        public int RedBits;
        public int GreenBits;
        public int BlueBits;
        public int RefreshRate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GammaRamp
    {
        public ushort* Red;
        public ushort* Green;
        public ushort* Blue;
        public int Size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Image
    {
        public int Width;
        public int Height;
        public byte* Pixels;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GamepadState
    {
        public fixed byte Buttons[15];
        public fixed float Axes[6];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Allocator
    {
        public delegate* unmanaged[Cdecl]<nint, void*, void*> Allocate;
        public delegate* unmanaged[Cdecl]<void*, nint, void*, void*> Reallocate;
        public delegate* unmanaged[Cdecl]<void*, void*, void> Deallocate;
        public void* User;
    }
}
