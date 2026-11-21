// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents an error code returned by the GLFW library.
/// </summary>
public enum ErrorCode
{
    /// <summary>
    /// No error has occurred.
    /// </summary>
    None,

    /// <summary>
    /// The GLFW library is not initialized.
    /// </summary>
    NotInitialized = 0x0001_0001,

    /// <summary>
    /// No context is current for the calling thread.
    /// </summary>
    NoCurrentContext,

    /// <summary>
    /// An invalid enum value was passed to a GLFW function.
    /// </summary>
    InvalidEnum,

    /// <summary>
    /// An invalid value was passed to a GLFW function.
    /// </summary>
    InvalidValue,

    /// <summary>
    /// A memory allocation failed.
    /// </summary>
    OutOfMemory,

    /// <summary>
    /// The requested API is unavailable.
    /// </summary>
    ApiUnavailable,

    /// <summary>
    /// The requested API version is unavailable.
    /// </summary>
    VersionUnavailable,

    /// <summary>
    /// A platform-specific error occurred.
    /// </summary>
    PlatformError,

    /// <summary>
    /// The requested format is unavailable.
    /// </summary>
    FormatUnavailable,

    /// <summary>
    /// No window context is available.
    /// </summary>
    NoWindowContext,

    /// <summary>
    /// The requested cursor is unavailable.
    /// </summary>
    CursorUnavailable,

    /// <summary>
    /// The requested feature is unavailable.
    /// </summary>
    FeatureUnavailable,

    /// <summary>
    /// The requested feature is unimplemented.
    /// </summary>
    FeatureUnimplemented,

    /// <summary>
    /// The platform is unavailable.
    /// </summary>
    PlatformUnavailable,
}
