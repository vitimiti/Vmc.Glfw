// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.InteropServices;

namespace Vmc.Glfw;

/// <summary>
/// Provides utility methods for GLFW.
/// </summary>
internal static class Utilities
{
    /// <summary>
    /// Checks for GLFW errors and throws a NativeContextException if an error occurs.
    /// </summary>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public static unsafe void CheckError()
    {
        var code = Unsafe.Glfw.GetError(out var description);
        if (code is not ErrorCode.None)
        {
            throw new NativeContextException(code, Marshal.PtrToStringUTF8((nint)description));
        }
    }
}
