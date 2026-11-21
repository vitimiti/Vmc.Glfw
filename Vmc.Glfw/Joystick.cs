// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vmc.Glfw;

public delegate void JoystickCallback(JoystickID jid, ConnectionEventType eventType);

/// <summary>
/// Represents a joystick device and provides access to its state and properties.
/// </summary>
/// <remarks>
/// This class implements <see cref="IDisposable"/>.
/// </remarks>
public sealed unsafe class Joystick : IDisposable
{
    private static readonly CallbacksHandler Callbacks = new();

    private readonly bool _isOwned;

    private GCHandle _gcHandle;
    private bool _disposed;

    /// <summary>
    /// Gets the identifier of the joystick.
    /// </summary>
    public JoystickID Id { get; }

    /// <summary>
    /// Gets a value indicating whether the joystick is currently connected.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public bool IsPresent
    {
        get
        {
            ThrowIfDisposed();
            var result = Unsafe.Glfw.JoystickPresent(Id);
            Utilities.CheckError();
            return result;
        }
    }

    /// <summary>
    /// Gets the collection of joystick axes values.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the collection of joystick button states.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the collection of joystick hat switch states.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the name of the joystick.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the GUID of the joystick.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets a value indicating whether the joystick is recognized as a gamepad.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public bool IsGamepad
    {
        get
        {
            ThrowIfDisposed();
            return Unsafe.Glfw.JoystickIsGamepad(Id);
        }
    }

    /// <summary>
    /// Gets the name of the gamepad if the joystick is recognized as a gamepad.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Gets the current state of the gamepad if the joystick is recognized as a gamepad.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Creates a new instance of the <see cref="Joystick"/> class.
    /// </summary>
    /// <param name="id">The identifier of the joystick.</param>
    /// <param name="isOwned">Indicates whether the joystick instance owns the underlying resources.</param>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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

    /// <summary>
    /// Disposes the joystick instance and releases any associated resources.
    /// </summary>
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

    /// <summary>
    /// Sets the callback to be invoked when the joystick connection state changes.
    /// </summary>
    /// <param name="callback">The callback to set.</param>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
    public void SetCallback(JoystickCallback? callback)
    {
        ThrowIfDisposed();
        Callbacks.Callback = callback;
        Unsafe.Glfw.SetJoystickCallback(callback is null ? null : &JoystickCallbackHandler);
        Utilities.CheckError();
    }

    /// <summary>
    /// Updates the gamepad mappings with the specified mappings string.
    /// </summary>
    /// <param name="mappings">The gamepad mappings string.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mappings"/> is null.</exception>
    /// <exception cref="ObjectDisposedException">Thrown if the joystick instance has been disposed.</exception>
    /// <exception cref="NativeContextException">Thrown when a GLFW error occurs.</exception>
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
