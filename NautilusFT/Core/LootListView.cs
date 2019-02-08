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

namespace NautilusFT
{
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
    public sealed class LootListView : IEquatable<LootListView>, IComparable<LootListView>
#pragma warning restore S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
    {
        public LootListView(string name, string status)
        {
            Name = name;
            Status = status;
        }

        public string Name { get; set; }

        public string Status { get; set; }

        public bool Equals(LootListView other)
        {
            if (Name == other.Name)
            {
                return true;
            }

            return false;
        }

        public int CompareTo(LootListView other)
        {
            return Name.CompareTo(other.Name);
        }

        internal static List<LootListView> GetLootLists()
        {
            return Settings.LootListForListView;
        }
    }
}