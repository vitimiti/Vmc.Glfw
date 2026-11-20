// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

public enum Platform
{
    Any = 0x0006_0000,
    Win32,
    Cocoa,
    Wayland,
    X11,
    Null,
}
