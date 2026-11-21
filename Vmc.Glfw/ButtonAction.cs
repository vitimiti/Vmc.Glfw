// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the action of a button.
/// </summary>
public enum ButtonAction
{
    /// <summary>
    /// The button is released.
    /// </summary>
    Release,

    /// <summary>
    /// The button is pressed.
    /// </summary>
    Press,

    /// <summary>
    /// The button is repeated.
    /// </summary>
    Repeat,
}
