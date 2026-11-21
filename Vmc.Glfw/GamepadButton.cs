// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the buttons of a gamepad.
/// </summary>
public enum GamepadButton
{
    /// <summary>
    /// The A button.
    /// </summary>
    A,

    /// <summary>
    /// The B button.
    /// </summary>
    B,

    /// <summary>
    /// The X button.
    /// </summary>
    X,

    /// <summary>
    /// The Y button.
    /// </summary>
    Y,

    /// <summary>
    /// The left bumper button.
    /// </summary>
    LeftBumper,

    /// <summary>
    /// The right bumper button.
    /// </summary>
    RightBumper,

    /// <summary>
    /// The back button.
    /// </summary>
    Back,

    /// <summary>
    /// The start button.
    /// </summary>
    Start,

    /// <summary>
    /// The guide button.
    /// </summary>
    Guide,

    /// <summary>
    /// The left thumb button.
    /// </summary>
    LeftThumb,

    /// <summary>
    /// The right thumb button.
    /// </summary>
    RightThumb,

    /// <summary>
    /// The D-pad up button.
    /// </summary>
    DPadUp,

    /// <summary>
    /// The D-pad right button.
    /// </summary>
    DPadRight,

    /// <summary>
    /// The D-pad down button.
    /// </summary>
    DPadDown,

    /// <summary>
    /// The D-pad left button.
    /// </summary>
    DPadLeft,

    /// <summary>
    /// The last button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="DPadLeft"/>.
    /// </remarks>
    Last = DPadLeft,

    /// <summary>
    /// The cross button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="A"/>.
    /// </remarks>
    Cross = A,

    /// <summary>
    /// The circle button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="B"/>.
    /// </remarks>
    Circle = B,

    /// <summary>
    /// The square button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="X"/>.
    /// </remarks>
    Square = X,

    /// <summary>
    /// The triangle button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="Y"/>.
    /// </remarks>
    Triangle = Y,
}
