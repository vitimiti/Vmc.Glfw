// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the keys on a keyboard.
/// </summary>
public enum Key
{
    /// <summary>
    /// An unknown key.
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// The space key.
    /// </summary>
    Space = 32,

    /// <summary>
    /// The apostrophe key.
    /// </summary>
    Apostrophe = 39,

    /// <summary>
    /// The comma key.
    /// </summary>
    Comma = 44,

    /// <summary>
    /// The minus key.
    /// </summary>
    /// <summary>
    /// The minus key.
    /// </summary>
    Minus,

    /// <summary>
    /// The period key.
    /// </summary>
    Period,

    /// <summary>
    /// The slash key.
    /// </summary>
    Slash,

    /// <summary>
    /// The 0 key.
    /// </summary>
    Number0,

    /// <summary>
    /// The 1 key.
    /// </summary>
    Number1,

    /// <summary>
    /// The 2 key.
    /// </summary>
    Number2,

    /// <summary>
    /// The 3 key.
    /// </summary>
    Number3,

    /// <summary>
    /// The 4 key.
    /// </summary>
    Number4,

    /// <summary>
    /// The 5 key.
    /// </summary>
    Number5,

    /// <summary>
    /// The 6 key.
    /// </summary>
    Number6,

    /// <summary>
    /// The 7 key.
    /// </summary>
    Number7,

    /// <summary>
    /// The 8 key.
    /// </summary>
    Number8,

    /// <summary>
    /// The 9 key.
    /// </summary>
    Number9,

    /// <summary>
    /// The semicolon key.
    /// </summary>
    Semicolon = 59,

    /// <summary>
    /// The equal key.
    /// </summary>
    Equal = 61,

    /// <summary>
    /// The A key.
    /// </summary>
    A = 65,

    /// <summary>
    /// The B key.
    /// </summary>
    B,

    /// <summary>
    /// The C key.
    /// </summary>
    C,

    /// <summary>
    /// The D key.
    /// </summary>
    D,

    /// <summary>
    /// The E key.
    /// </summary>
    E,

    /// <summary>
    /// The F key.
    /// </summary>
    F,

    /// <summary>
    /// The G key.
    /// </summary>
    G,

    /// <summary>
    /// The H key.
    /// </summary>
    H,

    /// <summary>
    /// The I key.
    /// </summary>
    I,

    /// <summary>
    /// The J key.
    /// </summary>
    J,

    /// <summary>
    /// The K key.
    /// </summary>
    K,

    /// <summary>
    /// The L key.
    /// </summary>
    L,

    /// <summary>
    /// The M key.
    /// </summary>
    M,

    /// <summary>
    /// The N key.
    /// </summary>
    N,

    /// <summary>
    /// The O key.
    /// </summary>
    O,

    /// <summary>
    /// The P key.
    /// </summary>
    P,

    /// <summary>
    /// The Q key.
    /// </summary>
    Q,

    /// <summary>
    /// The R key.
    /// </summary>
    R,

    /// <summary>
    /// The S key.
    /// </summary>
    S,

    /// <summary>
    /// The T key.
    /// </summary>
    T,

    /// <summary>
    /// The U key.
    /// </summary>
    U,

    /// <summary>
    /// The V key.
    /// </summary>
    V,

    /// <summary>
    /// The W key.
    /// </summary>
    W,

    /// <summary>
    /// The X key.
    /// </summary>
    X,

    /// <summary>
    /// The Y key.
    /// </summary>
    Y,

    /// <summary>
    /// The Z key.
    /// </summary>
    Z,

    /// <summary>
    /// The left bracket key.
    /// </summary>
    LeftBracket,

    /// <summary>
    /// The backslash key.
    /// </summary>
    Backslash,

    /// <summary>
    /// The right bracket key.
    /// </summary>
    RightBracket,

    /// <summary>
    /// The grave accent key.
    /// </summary>
    GraveAccent = 96,

    /// <summary>
    /// The first world key.
    /// </summary>
    World1 = 161,

    /// <summary>
    /// The second world key.
    /// </summary>
    World2,

    /// <summary>
    /// The escape key.
    /// </summary>
    Escape = 256,

    /// <summary>
    /// The enter key.
    /// </summary>
    Enter,

    /// <summary>
    /// The tab key.
    /// </summary>
    Tab,

    /// <summary>
    /// The backspace key.
    /// </summary>
    Backspace,

    /// <summary>
    /// The insert key.
    /// </summary>
    Insert,

    /// <summary>
    /// The delete key.
    /// </summary>
    Delete,

    /// <summary>
    /// The right arrow key.
    /// </summary>
    Right,

    /// <summary>
    /// The left arrow key.
    /// </summary>
    Left,

    /// <summary>
    /// The down arrow key.
    /// </summary>
    Down,

    /// <summary>
    /// The up arrow key.
    /// </summary>
    Up,

    /// <summary>
    /// The page up key.
    /// </summary>
    PageUp,

    /// <summary>
    /// The page down key.
    /// </summary>
    PageDown,

    /// <summary>
    /// The home key.
    /// </summary>
    Home,

    /// <summary>
    /// The end key.
    /// </summary>
    End,

    /// <summary>
    /// The caps lock key.
    /// </summary>
    CapsLock = 280,

    /// <summary>
    /// The scroll lock key.
    /// </summary>
    ScrollLock,

    /// <summary>
    /// The num lock key.
    /// </summary>
    NumLock,

    /// <summary>
    /// The print screen key.
    /// </summary>
    PrintScreen,

    /// <summary>
    /// The pause key.
    /// </summary>
    Pause,

    /// <summary>
    /// The F1 key.
    /// </summary>
    F1 = 290,

    /// <summary>
    /// The F2 key.
    /// </summary>
    F2,

    /// <summary>
    /// The F3 key.
    /// </summary>
    F3,

    /// <summary>
    /// The F4 key.
    /// </summary>
    F4,

    /// <summary>
    /// The F5 key.
    /// </summary>
    F5,

    /// <summary>
    /// The F6 key.
    /// </summary>
    F6,

    /// <summary>
    /// The F7 key.
    /// </summary>
    F7,

    /// <summary>
    /// The F8 key.
    /// </summary>
    F8,

    /// <summary>
    /// The F9 key.
    /// </summary>
    F9,

    /// <summary>
    /// The F10 key.
    /// </summary>
    F10,

    /// <summary>
    /// The F11 key.
    /// </summary>
    F11,

    /// <summary>
    /// The F12 key.
    /// </summary>
    F12,

    /// <summary>
    /// The F13 key.
    /// </summary>
    F13,

    /// <summary>
    /// The F14 key.
    /// </summary>
    F14,

    /// <summary>
    /// The F15 key.
    /// </summary>
    F15,

    /// <summary>
    /// The F16 key.
    /// </summary>
    F16,

    /// <summary>
    /// The F17 key.
    /// </summary>
    F17,

    /// <summary>
    /// The F18 key.
    /// </summary>
    F18,

    /// <summary>
    /// The F19 key.
    /// </summary>
    F19,

    /// <summary>
    /// The F20 key.
    /// </summary>
    F20,

    /// <summary>
    /// The F21 key.
    /// </summary>
    F21,

    /// <summary>
    /// The F22 key.
    /// </summary>
    F22,

    /// <summary>
    /// The F23 key.
    /// </summary>
    F23,

    /// <summary>
    /// The F24 key.
    /// </summary>
    F24,

    /// <summary>
    /// The F25 key.
    /// </summary>
    F25,

    /// <summary>
    /// The keypad 0 key.
    /// </summary>
    Keypad0 = 320,

    /// <summary>
    /// The keypad 1 key.
    /// </summary>
    Keypad1,

    /// <summary>
    /// The keypad 2 key.
    /// </summary>
    Keypad2,

    /// <summary>
    /// The keypad 3 key.
    /// </summary>
    Keypad3,

    /// <summary>
    /// The keypad 4 key.
    /// </summary>
    Keypad4,

    /// <summary>
    /// The keypad 5 key.
    /// </summary>
    Keypad5,

    /// <summary>
    /// The keypad 6 key.
    /// </summary>
    Keypad6,

    /// <summary>
    /// The keypad 7 key.
    /// </summary>
    Keypad7,

    /// <summary>
    /// The keypad 8 key.
    /// </summary>
    Keypad8,

    /// <summary>
    /// The keypad 9 key.
    /// </summary>
    Keypad9,

    /// <summary>
    /// The keypad decimal key.
    /// </summary>
    KeypadDecimal,

    /// <summary>
    /// The keypad divide key.
    /// </summary>
    KeypadDivide,

    /// <summary>
    /// The keypad multiply key.
    /// </summary>
    KeypadMultiply,

    /// <summary>
    /// The keypad subtract key.
    /// </summary>
    KeypadSubtract,

    /// <summary>
    /// The keypad add key.
    /// </summary>
    KeypadAdd,

    /// <summary>
    /// The keypad enter key.
    /// </summary>
    KeypadEnter,

    /// <summary>
    /// The keypad equal key.
    /// </summary>
    KeypadEqual,

    /// <summary>
    /// The left shift key.
    /// </summary>
    LeftShift = 340,

    /// <summary>
    /// The left control key.
    /// </summary>
    LeftControl,

    /// <summary>
    /// The left alt key.
    /// </summary>
    LeftAlt,

    /// <summary>
    /// The left super key.
    /// </summary>
    LeftSuper,

    /// <summary>
    /// The right shift key.
    /// </summary>
    RightShift,

    /// <summary>
    /// The right control key.
    /// </summary>
    RightControl,

    /// <summary>
    /// The right alt key.
    /// </summary>
    RightAlt,

    /// <summary>
    /// The right super key.
    /// </summary>
    RightSuper,

    /// <summary>
    /// The menu key.
    /// </summary>
    Menu,

    /// <summary>
    /// The last key.
    /// </summary>
    /// <remarks>
    /// This is an alias for <see cref="Menu"/>.
    /// </remarks>
    Last = Menu,
}
