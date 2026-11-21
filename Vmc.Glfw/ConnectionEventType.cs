// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the type of connection event.
/// </summary>
public enum ConnectionEventType
{
    /// <summary>
    /// The connection was established.
    /// </summary>
    Connected = 0x0004_0001,

    /// <summary>
    /// The connection was lost.
    /// </summary>
    Disconnected,
}
