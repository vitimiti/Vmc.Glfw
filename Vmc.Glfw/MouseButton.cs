// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents mouse buttons.
/// </summary>
public enum MouseButton
{
    /// <summary>
    /// The first mouse button.
    /// </summary>
    Button1,

    /// <summary>
    /// The second mouse button.
    /// </summary>
    Button2,

    /// <summary>
    /// The third mouse button.
    /// </summary>
    Button3,

    /// <summary>
    /// The fourth mouse button.
    /// </summary>
    Button4,

    /// <summary>
    /// The fifth mouse button.
    /// </summary>
    Button5,

    /// <summary>
    /// The sixth mouse button.
    /// </summary>
    Button6,

    /// <summary>
    /// The seventh mouse button.
    /// </summary>
    Button7,

    /// <summary>
    /// The eighth mouse button.
    /// </summary>
    Button8,

    /// <summary>
    /// The last mouse button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="Button8"/>.
    /// </remarks>
    Last = Button8,

    /// <summary>
    /// The left mouse button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="Button1"/>.
    /// </remarks>
    Left = Button1,

    /// <summary>
    /// The right mouse button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="Button2"/>.
    /// </remarks>
    Right = Button2,

    /// <summary>
    /// The middle mouse button.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="Button3"/>.
    /// </remarks>
    Middle = Button3,
}
