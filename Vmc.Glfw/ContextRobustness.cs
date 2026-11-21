// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw;

/// <summary>
/// Represents the context robustness strategy.
/// </summary>
public enum ContextRobustness
{
    /// <summary>
    /// No robustness strategy is used.
    /// </summary>
    None,

    /// <summary>
    /// The context will not be notified of resets.
    /// </summary>
    NoResetNotification = 0x0003_1001,

    /// <summary>
    /// The context will be lost on reset.
    /// </summary>
    LoseContextOnReset,
}
