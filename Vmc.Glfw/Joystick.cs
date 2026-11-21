// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vmc.Glfw;

public delegate void JoystickCallback(JoystickID jid, ConnectionEventType eventType);

public sealed unsafe class Joystick : IDisposable
{
    private static readonly CallbacksHandler Callbacks = new();

    private readonly bool _isOwned;

    private GCHandle _gcHandle;
    private bool _disposed;

    public JoystickID Id { get; }

    public bool IsPresent
    {
        get
        {
            ThrowIfDisposed();
            return Unsafe.Glfw.JoystickPresent(Id);
        }
    }

    public IReadOnlyCollection<float> Axes
    {
        get
        {
            ThrowIfDisposed();
            var axes = Unsafe.Glfw.GetJoystickAxes(Id, out var count);
            Utilities.CheckError();
            if (axes is null || count == 0)
            {
                return [];
            }

            var result = new Span<float>(axes, count);
            return result.ToArray();
        }
    }

    public IReadOnlyCollection<ButtonAction> Buttons
    {
        get
        {
            ThrowIfDisposed();
            var buttons = Unsafe.Glfw.GetJoystickButtons(Id, out var count);
            Utilities.CheckError();
            if (buttons is null || count == 0)
            {
                return [];
            }

            var result = new Span<ButtonAction>(buttons, count);
            return result.ToArray();
        }
    }

    public IReadOnlyCollection<HatStates> Hats
    {
        get
        {
            ThrowIfDisposed();
            var hats = Unsafe.Glfw.GetJoystickHats(Id, out var count);
            Utilities.CheckError();
            if (hats is null || count == 0)
            {
                return [];
            }

            var result = new Span<HatStates>(hats, count);
            return result.ToArray();
        }
    }

    public string? Name
    {
        get
        {
            ThrowIfDisposed();
            var name = Unsafe.Glfw.GetJoystickName(Id);
            Utilities.CheckError();
            return name is null ? null : Marshal.PtrToStringUTF8((nint)name);
        }
    }

    public string? Guid
    {
        get
        {
            ThrowIfDisposed();
            var guid = Unsafe.Glfw.GetJoystickGuid(Id);
            Utilities.CheckError();
            return guid is null ? null : Marshal.PtrToStringUTF8((nint)guid);
        }
    }

    public bool IsGamepad
    {
        get
        {
            ThrowIfDisposed();
            return Unsafe.Glfw.JoystickIsGamepad(Id);
        }
    }

    public string? GamepadName
    {
        get
        {
            ThrowIfDisposed();
            var name = Unsafe.Glfw.GetGamepadName(Id);
            Utilities.CheckError();
            return name is null ? null : Marshal.PtrToStringUTF8((nint)name);
        }
    }

    public GamepadState GamepadState
    {
        get
        {
            ThrowIfDisposed();
            Unsafe.Glfw.GetGamepadState(Id, out var state);
            Utilities.CheckError();

            var buttons = new Span<ButtonAction>(state.Buttons, 15).ToArray();
            var axes = new Span<float>(state.Axes, 6).ToArray();
            return new GamepadState(buttons, axes);
        }
    }

    internal Joystick(JoystickID id, bool isOwned = true)
    {
        Id = id;
        _isOwned = isOwned;
        _gcHandle = GCHandle.Alloc(Callbacks);
        Unsafe.Glfw.SetJoystickUserPointer(Id, (void*)GCHandle.ToIntPtr(_gcHandle));
        Utilities.CheckError();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void JoystickCallbackHandler(JoystickID jid, ConnectionEventType eventType)
    {
        var userPtr = Unsafe.Glfw.GetJoystickUserPointer(jid);
        var wrapper = (CallbacksHandler?)GCHandle.FromIntPtr((nint)userPtr).Target;
        wrapper?.Callback?.Invoke(jid, eventType);
    }

    public void Dispose()
    {
        if (_disposed || !_isOwned)
        {
            return;
        }

        _disposed = true;

        try
        {
            Unsafe.Glfw.SetJoystickUserPointer(Id, null);
            Utilities.CheckError();

            Unsafe.Glfw.SetJoystickCallback(null);
            Utilities.CheckError();
        }
        finally
        {
            if (_gcHandle.IsAllocated)
            {
                _gcHandle.Free();
            }
        }
    }

    public void SetCallback(JoystickCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.Callback = callback;
        Unsafe.Glfw.SetJoystickCallback(callback is null ? null : &JoystickCallbackHandler);
        Utilities.CheckError();
    }

    public void UpdateGamepadMappings(string mappings)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(mappings);
        var mappingsPtr = Marshal.StringToCoTaskMemUTF8(mappings);
        try
        {
            Unsafe.Glfw.UpdateGamepadMappings((byte*)mappingsPtr);
            Utilities.CheckError();
        }
        finally
        {
            Marshal.FreeCoTaskMem(mappingsPtr);
        }
    }

    private sealed class CallbacksHandler
    {
        public JoystickCallback? Callback { get; set; }
    }
}
