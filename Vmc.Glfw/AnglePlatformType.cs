// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// The type of platform used by ANGLE.
/// </summary>
public enum AnglePlatformType
{
    /// <summary>
    /// No platform is used.
    /// </summary>
    None = 0x0003_7001,

    /// <summary>
    /// Use the OpenGL platform.
    /// </summary>
    OpenGL,

    /// <summary>
    /// Use the OpenGLES platform.
    /// </summary>
    OpenGLES,

    /// <summary>
    /// Use the Direct3D 9 platform.
    /// </summary>
    D3D9,

    /// <summary>
    /// Use the Direct3D 11 platform.
    /// </summary>
    D3D11,

    /// <summary>
    /// Use the Vulkan platform.
    /// </summary>
    Vulkan,

    /// <summary>
    /// Use the Metal platform.
    /// </summary>
    Metal,
}
