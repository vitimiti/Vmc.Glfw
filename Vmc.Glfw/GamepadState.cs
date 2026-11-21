// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

public sealed record GamepadState(
    IReadOnlyCollection<ButtonAction> Buttons,
    IReadOnlyCollection<float> Axes
)
{
    public IReadOnlyCollection<ButtonAction> Buttons { get; } =
        Buttons.Count != 15
            ? throw new ArgumentException(
                "Buttons collection must have exactly 15 elements.",
                nameof(Buttons)
            )
            : Buttons;

    public IReadOnlyCollection<float> Axes { get; } =
        Axes.Count != 6
            ? throw new ArgumentException(
                "Axes collection must have exactly 6 elements.",
                nameof(Axes)
            )
            : Axes;
}
