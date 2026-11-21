// Copyright Victor Matia 2026
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Vmc.Glfw.Unsafe;

internal static partial class Glfw
{
    public const int Focused = 0x0002_0001;
    public const int Iconified = 0x0002_0002;
    public const int Resizable = 0x0002_0003;
    public const int Visible = 0x0002_0004;
    public const int Decorated = 0x0002_0005;
    public const int AutoIconify = 0x0002_0006;
    public const int Floating = 0x0002_0007;
    public const int Maximized = 0x0002_0008;
    public const int CenterCursor = 0x0002_0009;
    public const int TransparentFrameBuffer = 0x0002_000A;
    public const int Hovered = 0x0002_000B;
    public const int FocusOnShow = 0x0002_000C;
    public const int MousePassthrough = 0x0002_000D;
    public const int PositionX = 0x0002_000E;
    public const int PositionY = 0x0002_000F;
    public const int RedBits = 0x0002_1001;
    public const int GreenBits = 0x0002_1002;
    public const int BlueBits = 0x0002_1003;
    public const int AlphaBits = 0x0002_1004;
    public const int DepthBits = 0x0002_1005;
    public const int StencilBits = 0x0002_1006;
    public const int AccumRedBits = 0x0002_1007;
    public const int AccumGreenBits = 0x0002_1008;
    public const int AccumBlueBits = 0x0002_1009;
    public const int AccumAlphaBits = 0x0002_100A;
    public const int AuxBuffers = 0x0002_100B;
    public const int Stereo = 0x0002_100C;
    public const int Samples = 0x0002_100D;
    public const int SrgbCapable = 0x0002_100E;
    public const int RefreshRate = 0x0002_100F;
    public const int DoubleBuffer = 0x0002_1010;
    public const int ClientApi = 0x0002_2001;
    public const int ContextVersionMajor = 0x0002_2002;
    public const int ContextVersionMinor = 0x0002_2003;
    public const int ContextRevision = 0x0002_2004;
    public const int ContextRobustness = 0x0002_2005;
    public const int OpenGLForwardCompat = 0x0002_2006;
    public const int ContextDebug = 0x0002_2007;
    public const int OpenGLProfile = 0x0002_2008;
    public const int ContextReleaseBehavior = 0x0002_2009;
    public const int ContextNoError = 0x0002_200A;
    public const int ContextCreationApi = 0x0002_200B;
    public const int ScaleToMonitor = 0x0002_200C;
    public const int ScaleFrameBuffer = 0x0002_200D;
    public const int CocoaRetinaFrameBuffer = 0x0002_3001;
    public const int CocoaFrameName = 0x0002_3002;
    public const int CocoaGraphicsSwitching = 0x0002_3003;
    public const int X11ClassName = 0x0002_4001;
    public const int X11InstanceName = 0x0002_4002;
    public const int Win32KeyboardMenu = 0x0002_5001;
    public const int Win32ShowDefault = 0x0002_5002;
    public const int WaylandAppID = 0x0002_6001;
    public const int CursorConst = 0x0003_3001;
    public const int StickyKeys = 0x0003_3002;
    public const int StickyMouseButtons = 0x0003_3003;
    public const int LockKeyMods = 0x0003_3004;
    public const int RawMouseMotion = 0x0003_3005;
    public const int WaylandPreferLibDecor = 0x0003_8001;
    public const int WaylandDisableLibDecor = 0x0003_8002;
    public const int AnyPosition = unchecked((int)0x8000_0000);
    public const int JoystickHatButtons = 0x0005_0001;
    public const int AnglePlatformType = 0x0005_0002;
    public const int Platform = 0x0005_0003;
    public const int CocoaChDirResources = 0x0005_1001;
    public const int CocoaMenubar = 0x0005_1002;
    public const int WaylandLibDecor = 0x0005_3001;
}
