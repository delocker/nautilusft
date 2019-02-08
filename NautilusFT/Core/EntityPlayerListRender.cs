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
using System.Drawing;

namespace NautilusFT
{
    public sealed class EntityPlayerListRender : IEquatable<EntityPlayerListRender>
    {
        private PlayerStruct structs;

        public EntityPlayerListRender(
            string nickname,
            Color drawcolor, 
            int size, 
            float mapLocX, 
            float mapLocY,
            float mapLocZ,
            float mapLocXview,
            string weapon,
            string side,
            int health,
            string pose,
            bool cheater)
        {
            Nickname = nickname;
            Drawcolor = drawcolor;
            Size = size;
            MapLocX = mapLocX;
            MapLocY = mapLocY;
            MapLocZ = mapLocZ;

            MapLocXview = mapLocXview;
            Weapon = weapon;
            Side = side;
            Health = health;
            Pose = pose;
            Cheater = cheater;
            structs = new PlayerStruct
            {
            };
        }

        public string Nickname { get; set; }

        public string Pose { get; set; }

        public Color Drawcolor { get; set; }

        public int Size { get; set; }

        public float MapLocX { get; set; }

        public float MapLocY { get; set; }

        public float MapLocZ { get; set; }

        public float MapLocXview { get; set; }

        public int Health { get; set; }

        public string Weapon { get; set; }

        public string Side { get; set; }

        public bool Cheater { get; set; }


        /*
         If your class properly implements IEquatable<T>, then IndexOf() will use your Equals() method to test for equality.
         Otherwise, IndexOf() will use reference equality.
        */

        public bool Equals(EntityPlayerListRender other)
        {
            if (Nickname == other.Nickname)
            {
                return true;
            }

            return false;
        }

        // todo move to struct for better organizing
        internal struct PlayerStruct
        {
        }
    }
}