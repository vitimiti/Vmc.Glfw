// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

public sealed record GammaRamp(ushort[] Red, ushort[] Green, ushort[] Blue)
{
    public ushort[] Red { get; } =
        Red.Length != Green.Length || Red.Length != Blue.Length
            ? throw new ArgumentException("All color arrays must have the same length.")
            : Red;

    public ushort[] Green { get; } =
        Green.Length != Red.Length || Green.Length != Blue.Length
            ? throw new ArgumentException("All color arrays must have the same length.")
            : Green;

    public ushort[] Blue { get; } =
        Blue.Length != Red.Length || Blue.Length != Green.Length
            ? throw new ArgumentException("All color arrays must have the same length.")
            : Blue;
}
