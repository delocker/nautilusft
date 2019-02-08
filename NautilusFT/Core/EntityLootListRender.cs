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
    public sealed class EntityLootListRender : IEquatable<EntityLootListRender>
    {
        public EntityLootListRender(string lootname, Color drawcolor, int size, float mapLocX, float mapLocY, float mapLocZ)
        {
            Lootname = lootname;
            Drawcolor = drawcolor;
            Size = size;
            MapLocX = mapLocX;
            MapLocY = mapLocY;
            MapLocZ = mapLocZ;
        }

        public string Lootname { get; set; }

        public Color Drawcolor { get; set; }

        public int Size { get; set; }

        public float MapLocX { get; set; }

        public float MapLocY { get; set; }

        public float MapLocZ { get; set; }

        /* If your class properly implements IEquatable<T>, then IndexOf() will use your Equals() method to test for equality.
         Otherwise, IndexOf() will use reference equality. */

        public bool Equals(EntityLootListRender other)
        {
            if (Lootname == other.Lootname)
            {
                return true;
            }

            return false;
        }
    }
}