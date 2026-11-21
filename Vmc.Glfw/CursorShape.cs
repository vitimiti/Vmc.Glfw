// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the shape of a standard cursor.
/// </summary>
public enum CursorShape
{
    /// <summary>
    /// The arrow cursor.
    /// </summary>
    Arrow = 0x0003_6001,

    /// <summary>
    /// The I-beam cursor.
    /// </summary>
    IBeam,

    /// <summary>
    /// The crosshair cursor.
    /// </summary>
    Crosshair,

    /// <summary>
    /// The pointing hand cursor.
    /// </summary>
    PointingHand,

    /// <summary>
    /// The horizontal resize cursor.
    /// </summary>
    ResizeEW,

    /// <summary>
    /// The vertical resize cursor.
    /// </summary>
    ResizeNS,

    /// <summary>
    /// The diagonal resize cursor (NW-SE).
    /// </summary>
    ResizeNWSE,

    /// <summary>
    /// The diagonal resize cursor (NE-SW).
    /// </summary>
    ResizeNESW,

    /// <summary>
    /// The move cursor.
    /// </summary>
    ResizeAll,

    /// <summary>
    /// The not allowed cursor.
    /// </summary>
    NotAllowed,

    /// <summary>
    /// The horizontal resize cursor.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="ResizeEW"/>.
    /// </remarks>
    HResize = ResizeEW,

    /// <summary>
    /// The vertical resize cursor.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="ResizeNS"/>.
    /// </remarks>
    VResize = ResizeNS,

    /// <summary>
    /// The pointing hand cursor.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="PointingHand"/>.
    /// </remarks>
    Hand = PointingHand,
}
