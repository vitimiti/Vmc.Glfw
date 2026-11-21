// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the axes of a gamepad.
/// </summary>
public enum GamepadAxis
{
    /// <summary>
    /// The left stick X-axis.
    /// </summary>
    LeftX,

    /// <summary>
    /// The left stick Y-axis.
    /// </summary>
    LeftY,

    /// <summary>
    /// The right stick X-axis.
    /// </summary>
    RightX,

    /// <summary>
    /// The right stick Y-axis.
    /// </summary>
    RightY,

    /// <summary>
    /// The left trigger axis.
    /// </summary>
    LeftTrigger,

    /// <summary>
    /// The right trigger axis.
    /// </summary>
    RightTrigger,

    /// <summary>
    /// The last axis.
    /// </summary>
    Last = RightTrigger,
}
