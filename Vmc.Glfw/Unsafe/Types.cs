// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw.Unsafe;

internal static partial class Glfw
{
    public readonly record struct VkInstance(nint Handle)
    {
        public static VkInstance Null => new(0);

        public bool IsNull => Handle == 0;

        public static implicit operator nint(VkInstance instance) => instance.Handle;

        public static implicit operator VkInstance(nint handle) => new(handle);
    }
}
