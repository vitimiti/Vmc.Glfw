// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.InteropServices;

namespace Vmc.Glfw.Unsafe;

internal static class Glfw
{
    private const string DllName = "GLFW";

    static Glfw() =>
        NativeLibrary.SetDllImportResolver(
            typeof(Glfw).Assembly,
            (name, assembly, path) =>
                NativeLibrary.Load(
                    name switch
                    {
                        DllName when OperatingSystem.IsWindows() => "glfw3.dll",
                        DllName when OperatingSystem.IsLinux() => "libglfw.so.3",
                        DllName when OperatingSystem.IsMacOS() => "libglfw.3.dylib",
                        _ => name,
                    }
                )
        );
}
