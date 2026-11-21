// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.InteropServices;

namespace Vmc.Glfw;

/// <summary>
/// Represents an exception that occurs in the native context.
/// </summary>
/// <param name="errorCode">The error code associated with the exception.</param>
/// <param name="message">The error message.</param>
public class NativeContextException(ErrorCode errorCode, string? message)
    : ExternalException(message, (int)errorCode)
{
    /// <summary>
    /// Gets the native error code associated with the exception.
    /// </summary>
    public ErrorCode NativeErrorCode => (ErrorCode)HResult;
}
