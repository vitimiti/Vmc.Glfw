// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the platform.
/// </summary>
public enum Platform
{
    /// <summary>
    /// Represents any platform.
    /// </summary>
    Any = 0x0006_0000,

    /// <summary>
    /// Represents the Win32 platform.
    /// </summary>
    Win32,

    /// <summary>
    /// Represents the Cocoa platform.
    /// </summary>
    Cocoa,

    /// <summary>
    /// Represents the Wayland platform.
    /// </summary>
    Wayland,

    /// <summary>
    /// Represents the X11 platform.
    /// </summary>
    X11,

    /// <summary>
    /// Represents the Null platform.
    /// </summary>
    Null,
}
