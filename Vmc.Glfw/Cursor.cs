// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents a cursor.
/// </summary>
/// <remarks>
/// This class implements <see cref="IDisposable"/>.
/// </remarks>
public sealed unsafe class Cursor : IDisposable
{
    private readonly bool _isOwned;

    private bool _disposed;

    /// <summary>
    /// Gets the handle to the underlying unsafe cursor.
    /// </summary>
    internal Unsafe.Glfw.Cursor* Handle { get; }

    /// <summary>
    /// Creates a new cursor instance.
    /// </summary>
    /// <param name="handle">The handle to the underlying unsafe cursor.</param>
    /// <param name="isOwned">Indicates whether the cursor is owned and should be disposed.</param>
    internal Cursor(Unsafe.Glfw.Cursor* handle, bool isOwned = true)
    {
        Handle = handle;
        _isOwned = isOwned;
    }

    /// <summary>
    /// Disposes the cursor if it is owned.
    /// </summary>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void Dispose()
    {
        if (_disposed || !_isOwned)
        {
            return;
        }

        _disposed = true;

        Unsafe.Glfw.DestroyCursor(Handle);
        Utilities.CheckError();
    }
}
