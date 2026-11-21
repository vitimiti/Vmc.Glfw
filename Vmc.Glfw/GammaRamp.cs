// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents a gamma ramp, which defines the mapping of input color values to output color values for red, green, and blue channels.
/// </summary>
/// <param name="Red">The collection of red channel values.</param>
/// <param name="Green">The collection of green channel values.</param>
/// <param name="Blue">The collection of blue channel values.</param>
public sealed record GammaRamp(
    IReadOnlyCollection<ushort> Red,
    IReadOnlyCollection<ushort> Green,
    IReadOnlyCollection<ushort> Blue
)
{
    /// <summary>
    /// Gets the collection of red channel values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the color collections do not have the same length.</exception>
    public IReadOnlyCollection<ushort> Red { get; } =
        Red.Count != Green.Count || Red.Count != Blue.Count
            ? throw new ArgumentException("All color collections must have the same length.")
            : Red;

    /// <summary>
    /// Gets the collection of green channel values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the color collections do not have the same length.</exception>
    public IReadOnlyCollection<ushort> Green { get; } =
        Green.Count != Red.Count || Green.Count != Blue.Count
            ? throw new ArgumentException("All color collections must have the same length.")
            : Green;

    /// <summary>
    /// Gets the collection of blue channel values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the color collections do not have the same length.</exception>
    public IReadOnlyCollection<ushort> Blue { get; } =
        Blue.Count != Red.Count || Blue.Count != Green.Count
            ? throw new ArgumentException("All color collections must have the same length.")
            : Blue;
}
