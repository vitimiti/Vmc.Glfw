// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the context creation API type.
/// </summary>
public enum ContextCreationApi
{
    /// <summary>
    /// Use the native context creation API.
    /// </summary>
    Native = 0x0003_6001,

    /// <summary>
    /// Use the EGL context creation API.
    /// </summary>
    Egl,

    /// <summary>
    /// Use the OSMesa context creation API.
    /// </summary>
    OsMesa,
}
