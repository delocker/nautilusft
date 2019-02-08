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
    public static class Aggressive
    {

        public static Data data = new Data();
        public static bool MoveDo;
        public static int MoveDirection;
        public static bool MoveKeepDoing;
        public static float MoveStep = 0.5f;

        public static bool PlayerTargetsDictionaryEquipmentNeedsUpdate;
        public static bool PlayerTargetsListEquipmentViewNeedsUpdate;

        public static long EquipmentValueToMap;
        public static long ProfileValueToMap;

        public static bool EquipmentWriteDo;

        public static bool ScaleDown;
        public static bool ScaleDo;

        public static bool NoRecoil;
        public static bool NoRecoilDo;

        public static bool MemberCategoryDoDefault;
        public static bool MemberCategoryDoDev;
        public static bool MemberCategoryDoMod;
        public static bool MemberCategoryDo;

        public static bool FakeMeDo;
        public static string FakeName = "shaliuno";

        public enum Movements
        {
            Forward,
            Backward,
            Left,
            Right,
            Up,
            Down,
        }

        public enum MemberCategoryID
        {
            Default = 0,
            Developer = 1,
            UniqueId = 2,
            System = 16,
        }

        public static Dictionary<long, string> PlayerTargetsDictEquipment { get; set; } = new Dictionary<long, string>();
        public static Dictionary<long, string> PlayerTargetsDictProfile { get; set; } = new Dictionary<long, string>();
        public static DateTime MoveDateTime { get; set; } = DateTime.Now;

        public static void MoveMe()
        {
            if ((DateTime.Now - MoveDateTime).Milliseconds > 1 && MoveDo)
            {
                MoveDateTime = DateTime.Now;
                var databuffer = Memory.ReadBytes((long)data.myPositionAndViewPointer, sizeof(float) * 200);
                var positionX = BitConverter.ToSingle(databuffer, 0);
                var positionY = BitConverter.ToSingle(databuffer, 4);
                var positionZ = BitConverter.ToSingle(databuffer, 8);
                var directionX = BitConverter.ToSingle(databuffer, 0x1F4);

                // Get degress from game for player.
                var degressFOVcenter = directionX;
                float newX, newZ;

                var movestep = MoveStep;

                switch (MoveDirection)
                {
                    case (int)Movements.Forward:
                        if (degressFOVcenter < 0)
                        {
                            degressFOVcenter = 360 + degressFOVcenter;
                        }

                        newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 0), positionX + newX);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 2), positionZ + newZ);
                        break;

                    case (int)Movements.Backward:
                        if (degressFOVcenter < 0)
                        {
                            degressFOVcenter = 360 + degressFOVcenter;
                        }

                        newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 0), positionX - newX);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 2), positionZ - newZ);
                        break;

                    case (int)Movements.Left:
                        degressFOVcenter = degressFOVcenter - 90;
                        if (degressFOVcenter < 0)
                        {
                            degressFOVcenter = 360 + degressFOVcenter;
                        }

                        newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 0), positionX + newX);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 2), positionZ + newZ);
                        break;

                    case (int)Movements.Right:
                        degressFOVcenter = degressFOVcenter - 90;
                        if (degressFOVcenter < 0)
                        {
                            degressFOVcenter = 360 + degressFOVcenter;
                        }

                        newX = (float)(Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        newZ = (float)(Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * movestep);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 0), positionX - newX);
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 2), positionZ - newZ);
                        break;

                    case (int)Aggressive.Movements.Up:
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 1), positionY + 1.5f);
                        break;

                    case (int)Aggressive.Movements.Down:
                        Memory.Write((long)data.myPositionAndViewPointer + (sizeof(float) * 1), positionY - 1.5f);
                        break;

                    default:
                        break;
                }

                if (!MoveKeepDoing)
                {
                    MoveDo = false;
                }
            }
        }

        public struct Data
        {
            public IntPtr myPositionAndViewPointer;
        }
    }
}