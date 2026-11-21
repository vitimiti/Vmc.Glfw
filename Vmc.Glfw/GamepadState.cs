// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the state of a gamepad, including its buttons and axes.
/// </summary>
/// <param name="Buttons">The collection of button actions for the gamepad.</param>
/// <param name="Axes">The collection of axis values for the gamepad.</param>
public sealed record GamepadState(
    IReadOnlyCollection<ButtonAction> Buttons,
    IReadOnlyCollection<float> Axes
)
{
    /// <summary>
    /// Gets the collection of button actions for the gamepad.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the buttons collection does not have exactly 15 elements.</exception>
    public IReadOnlyCollection<ButtonAction> Buttons { get; } =
        Buttons.Count != 15
            ? throw new ArgumentException(
                "Buttons collection must have exactly 15 elements.",
                nameof(Buttons)
            )
            : Buttons;

    /// <summary>
    /// Gets the collection of axis values for the gamepad.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the axes collection does not have exactly 6 elements.</exception>
    public IReadOnlyCollection<float> Axes { get; } =
        Axes.Count != 6
            ? throw new ArgumentException(
                "Axes collection must have exactly 6 elements.",
                nameof(Axes)
            )
            : Axes;
}
