// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the OpenGL profile.
/// </summary>
public enum OpenGLProfile
{
    /// <summary>
    /// Represents any OpenGL profile.
    /// </summary>
    Any,

    /// <summary>
    /// Represents the core OpenGL profile.
    /// </summary>
    Core = 0x0003_2001,

    /// <summary>
    /// Represents the compatibility OpenGL profile.
    /// </summary>
    Compat,
}
