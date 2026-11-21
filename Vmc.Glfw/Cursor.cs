// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

public sealed unsafe class Cursor : IDisposable
{
    private readonly bool _isOwned;

    private bool _disposed;

    internal Unsafe.Glfw.Cursor* Handle { get; }

    internal Cursor(Unsafe.Glfw.Cursor* handle, bool isOwned = true)
    {
        Handle = handle;
        _isOwned = isOwned;
    }

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
