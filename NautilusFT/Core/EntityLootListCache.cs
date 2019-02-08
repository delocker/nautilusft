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
    internal struct LootablesStruct
    {
        public string LootName;
        public string LootNameOriginal;
        public float PosX;
        public float PosY;
        public float PosZ;
        public bool BlackListed;
        public IntPtr lootPositionPointer;
    }

    public sealed class EntityLootListCache : IEquatable<EntityLootListCache>
    {
        private static readonly int[] LootNameOffsets = new int[] { 0x10, 0x30, 0x60, 0x0 };
        private static readonly int[] LootPosition = new int[] { 0x10, 0x30, 0x30, 0x08, 0x38, 0x90 }; // first XYZ

        private readonly IntPtr baseAddrPointer;
        private readonly long baseAddr;
        private LootablesStruct structs;

        public EntityLootListCache(IntPtr addrPointer, long addr)
        {
            baseAddrPointer = addrPointer;
            baseAddr = addr;
            structs = new LootablesStruct();
        }

        public bool Equals(EntityLootListCache other)
        {
            if (baseAddr == other.baseAddr)
            {
                return true;
            }

            return false;
        }

        public void GetLootablesValues()
        {
            GetLootablesName();
            GetLootablesPosition();
        }

        public void BuildRenderList()
        {
            // If item is blacklisted we just don`t draw it.
            if (structs.BlackListed)
            {
                return;
            }

            // Check if we have that loot name if the list we need to look for.
            var indexLC = Settings.LootListToLookFor.IndexOf(structs.LootName);

            // We do.
            if (indexLC >= 0)
            {
                // If we have name already we prepare render list.
                var entityLoot = new EntityLootListRender(
                    structs.LootName,
                    Color.Fuchsia,
                    10, // hardcoded? good?
                    structs.PosX,
                    structs.PosY,
                    structs.PosZ);

                HelperOpenGL.LootListToRenderPre.Add(entityLoot);
            }

            var lootForSelector = new LootListView(structs.LootName, "-");

            if (!Settings.LootListForListView.Contains(lootForSelector))
            {
                if (Settings.LootListToLookFor.Contains(structs.LootName))
                {
                    // We have that item in loot list selected as to look for.
                    lootForSelector.Status = "+";

                    // Add with + sign.
                    Settings.LootListForListView.Add(lootForSelector);
                }
                else
                {
                    // Add with - sign.
                    Settings.LootListForListView.Add(lootForSelector);
                }

                Settings.FindLootRebuildTable = true;
            }
        }


        private void GetLootablesName()
        {
            if (structs.BlackListed)
            {
                return;
            }

            if (structs.LootName == null)
            {
                var lootNamePointer = Memory.GetPtr(baseAddrPointer, LootNameOffsets);
                var lootNameOriginal = Helper.GetStringFromMemory(lootNamePointer, false, 64);

                // Regex replace to represent readable stuff.
                var lootnameReadable = Helper.RegexTextReplace(lootNameOriginal);

                // Loot Blacklist check - don`t draw it is blacklisted in xml file.
                var indexBlackList = Settings.ListLootBlackList.IndexOf(lootNameOriginal);
                var indexBlackList2 = Settings.ListLootBlackList.IndexOf(lootnameReadable);

                structs.LootName = lootnameReadable;
                structs.LootNameOriginal = lootNameOriginal;

                if (indexBlackList >= 0 || indexBlackList2 >= 0)
                {
                    // We have that already in blacklist and return.
                    structs.BlackListed = true;
                }
                else
                {
                    // Dump loot to xml when dump button clicked. Add to Xml manually.
                    if (Settings.DumpLoot)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"_dump_Loot.txt", true))
                        {
                            file.WriteLine(lootnameReadable);
                        }
                    }
                }
            }
        }

        private void GetLootablesPosition()
        {
            if (structs.BlackListed)
            {
                return;
            }

            if (structs.lootPositionPointer == IntPtr.Zero)
            {
                structs.lootPositionPointer = Memory.GetPtr(baseAddrPointer, LootPosition);
            }

            // If pos is null, i.e. it was never assigned a value, therefore we read it only once, as the loot doesn`t move.
            if (structs.PosX == 0 && structs.PosY == 0 && structs.PosZ == 0)
            {
                // Read three XYZ positions in one RPM call.
                var databuffer = Memory.ReadBytes((long)structs.lootPositionPointer, sizeof(float) * 3);

                // Sort them away.
                structs.PosX = BitConverter.ToSingle(databuffer, 0); /* buffer[0] = float 1 */
                structs.PosY = BitConverter.ToSingle(databuffer, 4); /* buffer[4] = float 2 */
                structs.PosZ = BitConverter.ToSingle(databuffer, 8); /* buffer[8] = float 3 */
            }
        }

    }
}