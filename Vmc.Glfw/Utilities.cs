// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.InteropServices;

namespace Vmc.Glfw;

internal static class Utilities
{
    public static unsafe void CheckError()
    {
        var code = Unsafe.Glfw.GetError(out var description);
        if (code is not ErrorCode.None)
        {
            throw new GlfwException(code, Marshal.PtrToStringUTF8((nint)description));
        }
    }
}
