// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Drawing;

namespace Vmc.Glfw;

/// <summary>
/// Represents an image with a specified size and pixel data.
/// </summary>
/// <param name="Size">The size of the image.</param>
/// <param name="Pixels">The collection of pixel data.</param>
public record Image(Size Size, IReadOnlyCollection<byte> Pixels);
