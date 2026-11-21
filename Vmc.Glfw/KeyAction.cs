// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the action performed on a key.
/// </summary>
public enum KeyAction
{
    /// <summary>
    /// The key was released.
    /// </summary>
    Release,

    /// <summary>
    /// The key was pressed.
    /// </summary>
    Press,

    /// <summary>
    /// The key is being held down and repeating.
    /// </summary>
    Repeat,
}
