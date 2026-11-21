// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the modifier keys on a keyboard.
/// </summary>
[Flags]
public enum KeyModifiers
{
    /// <summary>
    /// No modifier key is pressed.
    /// </summary>
    None = 0,

    /// <summary>
    /// The shift key is pressed.
    /// </summary>
    Shift = 1 << 0,

    /// <summary>
    /// The control key is pressed.
    /// </summary>
    Control = 1 << 1,

    /// <summary>
    /// The alt key is pressed.
    /// </summary>
    Alt = 1 << 2,

    /// <summary>
    /// The super key is pressed.
    /// </summary>
    Super = 1 << 3,

    /// <summary>
    /// The caps lock key is active.
    /// </summary>
    CapsLock = 1 << 4,

    /// <summary>
    /// The num lock key is active.
    /// </summary>
    NumLock = 1 << 5,
}
