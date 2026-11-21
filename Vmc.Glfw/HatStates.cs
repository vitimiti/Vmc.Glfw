// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the possible states of a gamepad hat switch.
/// </summary>
[Flags]
public enum HatStates
{
    /// <summary>
    /// No hat switch input.
    /// </summary>
    None = 0,

    /// <summary>
    /// Hat switch is centered.
    /// </summary>
    Centered = None,

    /// <summary>
    /// Hat switch is in the up position.
    /// </summary>
    Up = 1 << 0,

    /// <summary>
    /// Hat switch is in the right position.
    /// </summary>
    Right = 1 << 1,

    /// <summary>
    /// Hat switch is in the down position.
    /// </summary>
    Down = 1 << 2,

    /// <summary>
    /// Hat switch is in the left position.
    /// </summary>
    Left = 1 << 3,

    /// <summary>
    /// Hat switch is in the right-up diagonal position.
    /// </summary>
    RightUp = Right | Up,

    /// <summary>
    /// Hat switch is in the right-down diagonal position.
    /// </summary>
    RightDown = Right | Down,

    /// <summary>
    /// Hat switch is in the left-up diagonal position.
    /// </summary>
    LeftUp = Left | Up,

    /// <summary>
    /// Hat switch is in the left-down diagonal position.
    /// </summary>
    LeftDown = Left | Down,
}
