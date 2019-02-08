#region License
// ====================================================
// NautilusFT Project by shaliuno.
// 
// This program comes with ABSOLUTELY NO WARRANTY; This is free software,
// and you are welcome to redistribute it under certain conditions; See
// file LICENSE, which is part of this source code package, for details.
// 
// Let your braincells grow and neural connections never fade.
// Live long and prosper. (c) Spock
// ====================================================
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;

namespace NautilusFT
{
    public static class Settings
    {
        // For OpenGL to render Russian Letters and English.
        public static readonly string Charset = @"йцукенгшщзхъэждлорпавыфячсмитьбюЙЦУКЕНГШЩЗХЪЭЖДЛОРПАВЫФЯЧСМИТЬБЮqwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890!@#$%^&*()_+-=[];:'\|/?.><,`~" + '"';
        public static readonly string Cipher = "tm2hFNSdjgqHU2Ag";

        #region colors
        //// http://www.flounder.com/csharp_color_table.htm
        //// https://i.pinimg.com/originals/3b/35/3d/3b353d124b5b71e66011f6cd40003e7b.png

        public static readonly Color ColorCorpse = Color.FromArgb(0, 0, 0);
        public static readonly Color ColorText = Color.FromArgb(255, 255, 255);
        public static readonly Color ColorPlayerBOT = Color.FromArgb(255, 128, 0);
        public static readonly Color ColorPlayerMyself = Color.FromArgb(128, 255, 0);
        public static readonly Color ColorPlayerOther = Color.DodgerBlue;
        public static readonly Color ColorPlayerFRIEND = Color.FromArgb(0, 255, 255);
        public static readonly Color ColorPlayerCHEATERorADMIN = Color.FromArgb(255, 48, 48);

        public static readonly Color ColorLoot = Color.FromArgb(255, 0, 160);
        public static readonly Color ColorMilitaryCrate = Color.FromArgb(0, 204, 0);
        public static readonly Color ColorMilitaryCrateAmmo = Color.FromArgb(255, 160, 0);
        public static readonly Color ColorMilitaryCrateGrenades = Color.FromArgb(255, 196, 0);
        public static readonly Color ColorMilitaryCrateLong = Color.FromArgb(128, 255, 0);

        public static readonly Color ColorPlayerSideSCAV = Color.FromArgb(153, 51, 255);
        public static readonly Color ColorPlayerSideBEAR = Color.FromArgb(255, 0, 96);
        public static readonly Color ColorPlayerSideUSEC = Color.FromArgb(0, 96, 255);
        public static readonly Color ColorOSD = Color.Yellow;

        #endregion

        // Console Debug switch. Right now used as Fingerprint disabler and if we need to write to file.
        public static readonly bool ConsoleDebug = true;

        public static readonly bool FingerPrint = false;

        /* This is dimensions of our GL canvas.
         * The map is to fit this size. I.e. map of 8000x8000 will fit 1000px and will be scaled with zoom level 1.
         * So to make it fit 100% its size our zoom will have to at 8.
         * */

        public static readonly int MapCanvasSize = 1000;

        public static readonly int MapDeadBodyIntervalSec = 15;

        public static readonly int MapFOVlineMyself = 100;

        public static readonly int MapFOVlineOther = 200;

        public static readonly int MapTransparencyIcon = 255;

        public static readonly int PlayersExtraInfoUpdateRateSec = 20;
        public static readonly int LootInfoUpdateRateSec = 10;
        public static readonly int PointersUpdateRateSec = 15;

        // Process Name to look for.
        public static readonly string ProcessNameScreen = "escapefromtarkov"; // "escapefromtarkov";
        public static readonly string ProcessNameGame = "escapefromtarkov"; // "escapefromtarkov";

        // Demo mode without attaching. For OpenGL testing and stff.
        public static bool DemoMode = true;

        public static bool Test = false;
        public static bool Standalone = false;

        public static string AccID;
        public static string GroupID = string.Empty;

        #region getset

        public static string CurrentMap { get; set; } = "---";
        public static string CurrentMapSwitchTo { get; set; } = "---";
        public static int UpdateRate { get; set; } = 25;
        public static List<string> ListFriends { get; set; } = new List<string>();
        public static Dictionary<string, string> ListCheaters { get; set; } = new Dictionary<string, string>();

        public static List<string> ListLootBlackList { get; set; } = new List<string>();
        public static List<string> LootListToLookFor { get; set; } = new List<string>();
        public static List<Color> ListTeamColors { get; set; } = new List<Color>(
            new Color[]
            {
               Color.MediumOrchid,
               Color.FromArgb(255, 96, 255), // pinkish
               Color.FromArgb(96, 196, 255), // blue light
               Color.Gold,

               Color.MediumOrchid,
               Color.FromArgb(255, 96, 255), // pinkish
               Color.FromArgb(96, 196, 255), // blue light
               Color.Gold,

               Color.MediumOrchid,
               Color.FromArgb(255, 96, 255), // pinkish
               Color.FromArgb(96, 196, 255), // blue light
               Color.Gold,
            });

        public static List<string> ListTeams { get; set; } = new List<string>();
        public static Dictionary<string, string> ListTextReplace { get; set; } = new Dictionary<string, string>();
        public static List<LootListView> LootListForListView { get; set; } = new List<LootListView>();
        public static bool MapCenterMap { get; set; }

        public static bool MapShowPlayerDistance { get; set; }

        public static bool FindLoot { get; set; }

        public static bool MapShowPlayerFOV { get; set; }

        public static bool MapShowPlayerHealth { get; set; }

        public static bool MapShowPlayerName { get; set; }

        public static bool MapShowPlayers { get; set; }

        public static bool MapShowPlayerSide { get; set; }

        public static bool MapShowPlayerWeapons { get; set; }

        public static int MapSizeIcon { get; set; } = 4;
        public static int MapZoomLevelFromZoomLevels { get; set; } = 4;
        public static bool PlayersExtraInfoCanUpdate { get; set; } = false;
        public static DateTime PlayersInfoTime { get; set; } = DateTime.Now;
        public static DateTime PlayersExtraInfoTime { get; set; } = DateTime.Now;
        public static DateTime LootInfoTime { get; set; } = DateTime.Now;
        public static DateTime PointersTime { get; set; } = DateTime.Now;
        public static DateTime ScreenShotTime { get; set; } = DateTime.Now;
        public static int[] MapSizesPlayerIcon { get; } = { 0, 5, 6, 6, 7, 7, 8, 8, 9, 10, 10 };
        public static int[] MapZoomLevelsMinMaxRange { get; } = { 1, 10 };
        public static DateTime FramesPerSecondTime { get; set; } = DateTime.Now;
        public static int FramesPerSecondCounter { get; set; }
        public static int FramerPerSecondOSD { get; set; }
        public static bool DumpPlayers { get; set; } = true;
        public static bool DumpObjects { get; set; } = true;
        public static bool DumpLoot { get; set; } = true;
        public static bool Screenshot { get; set; }
        public static bool FindLootRebuildTable { get; set; }
        public static int GamePIDCached { get; set; } = -1;
        public static IntPtr GameWorldPointerCached { get; set; } = IntPtr.Zero;
        public static IntPtr CameraPointerCached { get; set; } = IntPtr.Zero;
        public static int GamePIDCurrent { get; set; } = -1;
        public static bool MapOSDShowStats { get; set; }
        public static bool MapOSDAzimuth { get; set; }
        public static bool MapOSDDateTime { get; set; }
        public static bool MapOSDPlayerCount { get; set; }
        public static bool MapOSDFPS { get; set; }
        public static bool MapOSDReadCalls { get; set; }
        public static bool OverlayMirror { get; set; }
        #endregion
    }
}