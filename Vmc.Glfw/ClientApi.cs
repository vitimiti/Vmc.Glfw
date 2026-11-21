// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the client API type.
/// </summary>
public enum ClientApi
{
    /// <summary>
    /// No client API is used.
    /// </summary>
    None,

    /// <summary>
    /// Use the OpenGL client API.
    /// </summary>
    OpenGL = 0x0003_0001,

    /// <summary>
    /// Use the OpenGLES client API.
    /// </summary>
    OpenGLES,
}
