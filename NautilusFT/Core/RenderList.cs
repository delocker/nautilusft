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

using System.Drawing;

namespace NautilusFT
{
    public class RenderItem
    {


        public RenderStruct structs;

        public RenderItem()
        {
            structs = new RenderStruct
            {
            };
        }

        public struct RenderStruct
        {
            public string Text;
            public bool TextOutline;
            public float MapPosX;
            public float MapPosZ;
            public float MapPosXend;
            public float MapPosZend;
            public Color DrawColor;
            public int Size;
            public float Rotation;
            public float RotationGPS;
        }
    }
}