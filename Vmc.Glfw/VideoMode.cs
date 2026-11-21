// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Drawing;

namespace Vmc.Glfw;

public sealed record VideoMode(
    Size Size,
    (int Red, int Green, int Blue) BitsPerPixel,
    int RefreshRate
);
