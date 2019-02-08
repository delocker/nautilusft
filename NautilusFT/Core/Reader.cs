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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NautilusFT
{
    public class Reader
    {
        public bool RenderComplete = false;
        private static readonly int GOMOffset = 0x1432840;

        // GameWorld + 0x30] + 0x18] + 0x28 = "LocalWorld"
        private static readonly int[] LocalGameWorldOffsets = new int[] { 0x30, 0x18, 0x28 };

        private static readonly int[] LootCountOffsets = new int[] { 0x18 };
        private static readonly int[] LootListOffsets = new int[] { 0x58 };
        private static readonly int[] PlayerCountOffsets = new int[] { 0x18 };

        private static readonly int[] PlayerListOffsets = new int[] { 0x70 };

        private readonly List<EntityLootListCache> lootAddressesCache = new List<EntityLootListCache>();

        private readonly List<EntityLootListCache> lootAddressesFresh = new List<EntityLootListCache>();
        private readonly List<EntityPlayerListCache> playerAddressesCache = new List<EntityPlayerListCache>();

        private readonly List<EntityPlayerListCache> playerAddressesFresh = new List<EntityPlayerListCache>();

        private PointerCacheStruct structs;

        private System.Timers.Timer playerTimer;
        private System.Timers.Timer lootTimer;

        public Reader()
        {
            Memory.OpenTargetProcess(Settings.ProcessNameGame);
            structs.GameObjectManagerOffset = GOMOffset;
            structs.GameBaseAddress = Memory.ImageBaseAddress();
            structs.GameObjectManagerPointer = (IntPtr)(structs.GameBaseAddress.ToInt64() + structs.GameObjectManagerOffset);
            Settings.PointersTime = DateTime.Now.AddSeconds(-15);
            HelperOpenGL.PlayerListToRenderPre.Clear();
            HelperOpenGL.LootListToRenderPre.Clear();
            Settings.ListTeams.Clear();

            playerTimer = new System.Timers.Timer
            {
                Interval = 1
            };

            playerTimer.Elapsed += PlayerTimerTick;
            playerTimer.AutoReset = false;
            playerTimer.Enabled = true;
            playerTimer.Stop();
            playerTimer.Start();

            lootTimer = new System.Timers.Timer
            {
                Interval = 1
            };

            lootTimer.Elapsed += LootTimerTick;
            lootTimer.AutoReset = false;
            lootTimer.Enabled = true;
            lootTimer.Stop();
            lootTimer.Start();
        }

        public IntPtr CameraPointer { get => structs.CameraPointer; set => structs.CameraPointer = value; }

        public IntPtr CameraMatrixPointer { get => structs.CameraMatrixPointer; set => structs.CameraMatrixPointer = value; }

        public bool FindLoot { get; set; }

        public IntPtr GameBaseAddress { get => structs.GameBaseAddress; set => structs.GameBaseAddress = value; }

        public IntPtr GameWorldPointer { get => structs.GameWorldPointer; set => structs.GameWorldPointer = value; }

        public bool IterateLootComplete { get; set; } = true;

        public bool IteratePlayersComplete { get; set; } = true;

        public IntPtr LocalGameWorldPointer { get => structs.LocalGameWorldPointer; set => structs.LocalGameWorldPointer = value; }

        public int LootCount { get => structs.LootCount; set => structs.LootCount = value; }

        public IntPtr LootFirstAddressInTable { get => structs.LootFirstAddressInTable; set => structs.LootFirstAddressInTable = value; }

        public IntPtr LootListPointer { get => structs.LootListPointer; set => structs.LootListPointer = value; }

        public int PlayerCount { get => structs.PlayerCount; set => structs.PlayerCount = value; }

        public int PlayerCountBOT { get; set; }

        public int PlayerCountPMC { get; set; }

        public int PlayerCountSCAV { get; set; }

        public IntPtr PlayerListPointer { get => structs.PlayersListPointer; set => structs.PlayersListPointer = value; }

        public IntPtr PlayersFirstAddressInTable { get => structs.PlayersFirstAddressInTable; set => structs.PlayersFirstAddressInTable = value; }

        public bool UpdateBasePointers { get; set; } = true;

        public void CloseHandles()
        {
            try
            {
                // Stop any opened process handles.
                Memory.CloseTargetProcess();
            }
            catch
            {
            }
        }

        // Get All Pointers and variables.
        public void GetPointers(bool force = false)
        {
            // If we do force update, we clean all the cached pointers.
            if (force)
            {
                structs.GameWorldPointer = IntPtr.Zero;
                structs.LocalGameWorldPointer = IntPtr.Zero;
                structs.CameraPointer = IntPtr.Zero;
                structs.CameraMatrixPointer = IntPtr.Zero;
                structs.PlayersListPointer = IntPtr.Zero;
                structs.LootListPointer = IntPtr.Zero;
                structs.PlayerCountAddress = IntPtr.Zero;
                structs.LootCountAddress = IntPtr.Zero;
                structs.PlayersFirstAddressInTable = IntPtr.Zero;
                structs.LootFirstAddressInTable = IntPtr.Zero;
            }

            GetGameWorldPointer();
            GetLocalGameWorldPointer();
            /// GetMainAppPointer(); can hack some stuff not for all
            /// GetCommonUIPointer(); can hack some stuff not for all
            GetCameraPointer();
            GetCameraMatrixPointer();

            if (Settings.DumpObjects)
            {
                Settings.DumpObjects = false;
            }

            GetPlayerListPointer();
            GetLootListPointer();
            GetPlayerCount();
            GetLootCount();
            GetExfilPoints();
        }

        // Return playercount.
        public void GetPlayerCount()
        {
            try
            {
                if (structs.PlayerCountAddress == IntPtr.Zero)
                {
                    structs.PlayerCountAddress = Memory.GetPtr(structs.PlayersListPointer, PlayerCountOffsets);
                }

                structs.PlayerCount = Memory.Read<int>(structs.PlayerCountAddress.ToInt64());
            }
            catch
            {
                structs.PlayerCount = 0;
            }
        }

        private void LootTimerTick(object source, ElapsedEventArgs e)
        {
            // We have open handle, so we can finally do something.
            if (Memory.HasActiveHandle())
            {
                // If we are about to update loot again we flag it here.
                if ((DateTime.Now - Settings.LootInfoTime).Seconds > Settings.LootInfoUpdateRateSec)
                {
                    // If we completed gathering loot info.
                    if (Settings.FindLoot && IterateLootComplete)
                    {
                        // Drop the complete flag so we can update again.
                        IterateLootComplete = false;

                        // Call to gather info on loot.
                        IterateLoot();
                    }

                    // Update last loot update time.
                    Settings.LootInfoTime = DateTime.Now;
                }
            }
            else
            {
                LootCount = 0;
            }

            lootTimer.Start();
        }

        private void BuildFreshPlayerList(IntPtr playersFirstAddressInTable)
        {
            // Clear temporary entities address list list that is used to build cache.
            playerAddressesFresh.Clear();

            // Update playercount just in case.
            GetPlayerCount();

            // 1. Build temporary players address list and populate cache list if missing.

            // We get a big buffer of (long * PlayerCount) size and read all pointers in one go, one rpm call instead xPlayerCount.
            var databuffer = Memory.ReadBytes((long)playersFirstAddressInTable, sizeof(long) * structs.PlayerCount);

            for (int i = 0; i < structs.PlayerCount; i++)
            {
                // Get player address space from pointer.
                var playerObjectAddressPointer = playersFirstAddressInTable + (i * 0x8); // to get rid of this i need to rework GetPtr function...

                // Sort them away.
                var playerObjectAddress = BitConverter.ToInt64(databuffer, i * sizeof(long));
                var playerEntity = new EntityPlayerListCache(playerObjectAddressPointer, playerObjectAddress);

                // Add to temp cache.
                playerAddressesFresh.Add(playerEntity);

                // Do we have that address in cache.
                var indexCache = playerAddressesCache.IndexOf(playerEntity);

                // We Don`t.
                if (indexCache < 0)
                {
                    // Add that team to list.
                    playerAddressesCache.Add(playerEntity);
                }
            }
        }

        private void BuildCachedPlayerList()
        {
            /* Check cached list and remove those who don`t exist in temp anymore
               Get fields filled in cache list if null */

            // Reverse for for safe removal. Foreach is readonly so only for is acceptable.
            for (int i = playerAddressesCache.Count - 1; i >= 0; i--)
            {
                var indexTemp = playerAddressesFresh.IndexOf(playerAddressesCache[i]);

                // We Do
                if (indexTemp >= 0)
                {
                    playerAddressesCache[i].GetPlayerValues();
                }

                // We Don`t
                if (indexTemp < 0)
                {
                    // Remove players from cache
                    playerAddressesCache.RemoveAt(i);
                }
            }
        }

        private void BuildRenderPlayerList()
        {
            if (HelperOpenGL.PlayerListToRenderComplete)
            {
                return;
            }

            // Clear entities list that is used to build list for for rendering.
            HelperOpenGL.PlayerListToRenderPre.Clear();

            for (int i = playerAddressesCache.Count - 1; i >= 0; i--)
            {
                playerAddressesCache[i].BuildRenderList();
            }

            HelperOpenGL.PlayerListToRenderComplete = true;
        }

        private void IterateLoot()
        {
            /* Iterations:
             * 1. Build temporary loot address list and populate cache list if missing.
             * 2. Check cached list and remove those who don`t exist in temp anymore.
             * 3. Get fields filled in cache list if enties are empty (done once for non realtime data like nickname/side/etc).
             * 4. Build entity list to render from cache.
             */

            // Get to the list with players address pointers.
            if (structs.LootFirstAddressInTable == IntPtr.Zero)
            {
                structs.LootFirstAddressInTable = Memory.GetPtr(structs.LootListPointer, new int[] { 0x10, 0x20 });
            }

            BuildFreshLootablesList(structs.LootFirstAddressInTable);
            BuildCachedLootablesList();
            BuildRenderLootablesList();

            if (Settings.FindLootRebuildTable)
            {
                Settings.LootListForListView.Sort();
            }

            Settings.DumpLoot = false;
            IterateLootComplete = true;
        }

        private void BuildFreshLootablesList(IntPtr lootFirstAddressInTable)
        {
            lootAddressesFresh.Clear();

            // Update lootcount just in case.
            GetLootCount();

            // 1. Build temporary players loot list and populate cache list if missing.

            // We get a big buffer of (long * PlayerCount) size and read all pointers in one go, one rpm call instead xPlayerCount.
            var databuffer = Memory.ReadBytes((long)lootFirstAddressInTable, sizeof(long) * structs.LootCount);

            for (int i = 0; i < structs.LootCount; i++)
            {
                // Get loot address space from pointer.
                var lootObjectAdressPointer = lootFirstAddressInTable + (i * 0x8); // to get rid of this i need to rework GetPtr function...

                // Sort them away.
                var lootObjectAddress = BitConverter.ToInt64(databuffer, i * sizeof(long));
                var lootEntity = new EntityLootListCache(lootObjectAdressPointer, lootObjectAddress);

                // Add to temp cache.
                lootAddressesFresh.Add(lootEntity);

                // Do we have that address in cache.
                var indexCache = lootAddressesCache.IndexOf(lootEntity);

                // We Don`t.
                if (indexCache < 0)
                {
                    lootAddressesCache.Add(lootEntity);
                }
            }
        }

        private void BuildCachedLootablesList()
        {
            /* Check cached list and remove those who don`t exist in temp anymore
             * Get fields filled in cache list if null */

            // Reverse for for safe removal. Foreach is readonly so only forr is acceptable.
            for (int i = lootAddressesCache.Count - 1; i >= 0; i--)
            {
                var indexTemp = lootAddressesFresh.IndexOf(lootAddressesCache[i]);

                // We Do
                if (indexTemp >= 0)
                {
                    lootAddressesCache[i].GetLootablesValues();
                }

                // We Don`t
                if (indexTemp < 0)
                {
                    // Remove players from cache.
                    lootAddressesCache.RemoveAt(i);
                }
            }
        }

        private void BuildRenderLootablesList()
        {
            if (HelperOpenGL.LootListToRenderComplete)
            {
                return;
            }

            // Clear entities list that is used to build list for  for rendering.
            HelperOpenGL.LootListToRenderPre.Clear();

            for (int i = lootAddressesCache.Count - 1; i >= 0; i--)
            {
                var indexTemp = lootAddressesFresh.IndexOf(lootAddressesCache[i]);
                lootAddressesCache[i].BuildRenderList();
            }

            HelperOpenGL.LootListToRenderComplete = true;
        }

        private IntPtr FindObject(string objNameToFind, bool findActiveObjectInsteadTagged, int selectWhichToUse = 1)
        {
            var currentFoundCount = 0;

            // For tagged objects.
            var tagged_or_active = 0x8;

            // Max objects limit to look through.
            var limit = 1800;

            // For active objects.
            if (findActiveObjectInsteadTagged)
            {
                tagged_or_active = 0x18;
                limit = 350;
            }

            var output = IntPtr.Zero;

            if (!Memory.HasActiveHandle())
            {
                return output;
            }

            // Get to the addresses with objects ,straight at the pointer.
            long currentAddress = Memory.GetPtr(structs.GameObjectManagerPointer, new int[] { tagged_or_active }).ToInt64();

            for (int curObject = 1; curObject < limit; curObject++)
            {
                // That lands us at the door of 0x8 that we didn`t read and not got inside :D
                currentAddress = Memory.GetPtr((IntPtr)currentAddress, new int[] { 0x8 }).ToInt64();

                // Return this address if we have found one.
                var objectAddress = Memory.GetPtr((IntPtr)currentAddress, new int[] { 0x10 }).ToInt64();

                var objectNameAddress = Memory.GetPtr((IntPtr)objectAddress, new int[] { 0x60, 0x0 }).ToInt64();
                var objectName = Helper.GetStringFromMemory((IntPtr)objectNameAddress, false, objNameToFind.Length + 15);

                if (string.Compare(objectName, objNameToFind, true) == 0)
                {
                    currentFoundCount++;
                    output = (IntPtr)objectAddress;
                    Console.WriteLine(curObject + " : " + objectName + " : " + objectAddress.ToString("X"));
                    DumpGameObject(objectName + " : " + objectAddress.ToString("X"), findActiveObjectInsteadTagged.ToString());

                    if (selectWhichToUse == currentFoundCount)
                    {
                        break;
                    }
                }
            }

            return output;
        }

        private void DumpGameObject(string objectName, string activeOrTagged)
        {
            if (Settings.DumpObjects)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"_dump_Objects_" + activeOrTagged + ".txt", true))
                {
                    file.WriteLine(string.Empty);
                }
            }
        }

        private void GetCameraPointer()
        {
            // We have zero pointer so what we can do to find camera pointer.
            if (structs.CameraPointer == IntPtr.Zero)
            {
                // First try to actually find one. We do this at game loading screen (count down to deploy).
                structs.CameraPointer = FindObject("fps camera", false);

                // If we didn`t find it.
                if (structs.CameraPointer == IntPtr.Zero)
                {
                    // Try to look for cached version.
                    if (Settings.CameraPointerCached != IntPtr.Zero)
                    {
                        structs.CameraPointer = Settings.CameraPointerCached;
                    }
                }

                // If all ok.
                Settings.CameraPointerCached = structs.CameraPointer;
            }
        }

        private void GetCameraMatrixPointer()
        {
            if (structs.CameraMatrixPointer == IntPtr.Zero && structs.CameraPointer != IntPtr.Zero)
            {
                structs.CameraMatrixPointer = Memory.GetPtr(structs.CameraPointer, new int[] { 0x30, 0x18, 0xC0 });
            }
        }

        // Find Active object GameWorld.
        private void GetGameWorldPointer()
        {
            if (structs.GameWorldPointer == IntPtr.Zero)
            {
                structs.GameWorldPointer = FindObject("gameworld", true, 2);
            }
        }

        private void GetMainAppPointer()
        {
            if (structs.MainAppPointer == IntPtr.Zero)
            {
                structs.MainAppPointer = FindObject("Application (Main Client)", true);
            }
        }

        private void GetCommonUIPointer()
        {
            if (structs.CommonUIPointer == IntPtr.Zero)
            {
                structs.CommonUIPointer = FindObject("common ui", true);
            }
        }

        // Get Pointer to Local World.
        private void GetLocalGameWorldPointer()
        {
            if (structs.LocalGameWorldPointer == IntPtr.Zero)
            {
                structs.LocalGameWorldPointer = Memory.GetPtr(structs.GameWorldPointer, LocalGameWorldOffsets);
            }
        }

        // Get Pointer to PlayerList.
        private void GetPlayerListPointer()
        {
            if (structs.PlayersListPointer == IntPtr.Zero)
            {
                structs.PlayersListPointer = Memory.GetPtr(structs.LocalGameWorldPointer, PlayerListOffsets);
            }
        }

        // Get Pointer to LootList.
        private void GetLootListPointer()
        {
            if (structs.LootListPointer == IntPtr.Zero)
            {
                structs.LootListPointer = Memory.GetPtr(structs.LocalGameWorldPointer, LootListOffsets);
            }
        }

        // Return lootcount.
        private void GetLootCount()
        {
            try
            {
                if (structs.LootCountAddress == IntPtr.Zero)
                {
                    structs.LootCountAddress = Memory.GetPtr(structs.LootListPointer, LootCountOffsets);
                }

                structs.LootCount = Memory.Read<int>(structs.LootCountAddress.ToInt64());
            }
            catch
            {
                structs.LootCount = 0;
            }
        }

        private void GetExfilPoints()
        {
            var exfilControllerPointer = Memory.GetPtr(structs.LocalGameWorldPointer, new int[] { 0x18 });
            var exfilPmcPointPointer = Memory.GetPtr(exfilControllerPointer, new int[] { 0x20 });
            var exfilScavPointPointer = Memory.GetPtr(exfilControllerPointer, new int[] { 0x28 });

            var exfilPmcPointCountPtr = Memory.GetPtr(exfilPmcPointPointer, new int[] { 0x18 });
            var exfilScavPointCountPtr = Memory.GetPtr(exfilScavPointPointer, new int[] { 0x18 });
            var exfilPmcPointCount = Memory.Read<long>((long)exfilPmcPointCountPtr);
            var exfilScavPointCount = Memory.Read<long>((long)exfilScavPointCountPtr);

            var exfilPmcPointFirst = Memory.GetPtr(exfilPmcPointPointer, new int[] { 0x20 });
            var exfilScavPointFirst = Memory.GetPtr(exfilScavPointPointer, new int[] { 0x20 });

            var exfilPmcPointList = new List<string>();
            var exfilScavPointList = new List<string>();

            Console.WriteLine("---PMC Exit Points---");
            for (int i = 0; i <= exfilPmcPointCount; i++)
            {
                var exfilPointAddr = exfilPmcPointFirst + (i * 0x8);

                // ExFilPmcPointList.
                var exfilPointNamePtr = Memory.GetPtr(exfilPointAddr, new int[] { 0x30, 0x10, 0x14 });
                var exfilPointName = Helper.GetStringFromMemory(exfilPointNamePtr, true, 32);
                var exfilPointOnlinePtr = Memory.GetPtr(exfilPointAddr, new int[] { 0x78 });
                var exfilPointOnline = Memory.Read<int>((long)exfilPointOnlinePtr);

                var indexCache = exfilPmcPointList.IndexOf(exfilPointName);

                if (exfilPointOnline == 4 && indexCache < 0)
                {
                    // Add that team to list.
                    exfilPmcPointList.Add(exfilPointName);
                    Console.WriteLine(exfilPointName);
                }
            }

            Console.WriteLine("---SCAV Exit Points---");
            for (int i = 0; i <= exfilScavPointCount; i++)
            {
                var exfilPointAddr = exfilScavPointFirst + (i * 0x8);

                // ExFilPmcPointList.
                var exfilPointNamePtr = Memory.GetPtr(exfilPointAddr, new int[] { 0x30, 0x10, 0x14 });
                var exfilPointName = Helper.GetStringFromMemory(exfilPointNamePtr, true, 32);
                var exfilPointOnlinePtr = Memory.GetPtr(exfilPointAddr, new int[] { 0x78 });
                var exfilPointOnline = Memory.Read<int>((long)exfilPointOnlinePtr);

                var indexCache = exfilScavPointList.IndexOf(exfilPointName);

                if (exfilPointOnline == 4 && indexCache < 0)
                {
                    exfilScavPointList.Add(exfilPointName);
                    Console.WriteLine(exfilPointName);
                }
            }
        }

        private void PlayerTimerTick(object source, ElapsedEventArgs e)
        {
            // We have open handle, so we can finally do something.
            if (Memory.HasActiveHandle())
            {
                /* Pointers update
                 * Time when we update all game pointers, find gameworld object etc. 
                 * Quick but still time consuming so we do it every 15 seconds and if conditions apply. */
                if ((DateTime.Now - Settings.PointersTime).Seconds > Settings.PointersUpdateRateSec && PlayerCount == 0)
                {
                    if (UpdateBasePointers)
                    {
                        GetPointers(true);
                    }

                    GetPointers();

                    Console.WriteLine(LocalGameWorldPointer.ToString("X"));
                    Settings.PointersTime = DateTime.Now;
                }

                /* Standard player info update.
                 * If we have players on map, if we completed gathering player info and have an handle to the process. */
                if (((DateTime.Now - Settings.PlayersInfoTime).Milliseconds > Settings.UpdateRate) &&
                    IteratePlayersComplete && PlayerCount != 0)
                {
                    // If we are about to update extras on palyers, we flag it here.
                    if ((DateTime.Now - Settings.PlayersExtraInfoTime).Seconds > Settings.PlayersExtraInfoUpdateRateSec)
                    {
                        Settings.PlayersExtraInfoCanUpdate = true;
                        Settings.PlayersExtraInfoTime = DateTime.Now;
                    }
                    else
                    {
                        Settings.PlayersExtraInfoCanUpdate = false;
                    }

                    // Call to gather info on players.
                    PlayerCountPMC = 0;
                    PlayerCountSCAV = 0;
                    PlayerCountBOT = 0;

                    // Iterate dictionary at LocalGameworld + 60] + 10]  = Dict Pointer ] + 0x20 + i * 0x8.
                    // Drop the complete flag so we can update again on iteration. But if we have overlay on we handle it there.
                    IteratePlayersComplete = false;
                    IteratePlayers();

                    // Update players time.
                    Settings.PlayersInfoTime = DateTime.Now;

                    if (Aggressive.PlayerTargetsDictionaryEquipmentNeedsUpdate)
                    {
                        Aggressive.PlayerTargetsDictionaryEquipmentNeedsUpdate = false;
                        Aggressive.PlayerTargetsDictEquipment.Clear();
                        Aggressive.PlayerTargetsDictProfile.Clear();
                    }
                }
            }
            else
            {
                PlayerCount = 0;
            }

            playerTimer.Start();
        }

        public void IteratePlayers()
        {
            /* Iterations:
             * 1. Build temporary players address list and populate cache list if missing.
             * 2. Check cached list and remove those who don`t exist in temp anymore.
             * 3. Get fields filled in cache list if enties are empty (done once for non realtime data like nickname/side/etc).
             * 4. Build entity list to render from cache.
             */

            // Get to the list with players address pointers.
            if (structs.PlayersFirstAddressInTable == IntPtr.Zero)
            {
                structs.PlayersFirstAddressInTable = Memory.GetPtr(structs.PlayersListPointer, new int[] { 0x10, 0x20 });
            }

            BuildFreshPlayerList(structs.PlayersFirstAddressInTable);
            BuildCachedPlayerList();
            BuildRenderPlayerList();

            IteratePlayersComplete = true;
        }

        internal struct PointerCacheStruct
        {
            // FPS Camera pointer for ESP.
            public IntPtr CameraPointer;

            // FPS Camera pointer for ESP.
            public IntPtr CameraMatrixPointer;

            // FPS Camera pointer for ESP.
            public IntPtr OpticCameraPointer;

            // FPS Camera pointer for ESP.
            public IntPtr OpticCameraMatrixPointer;

            // BaseImage of a game process. Populated on init.
            public IntPtr GameBaseAddress;

            // GameObjectManager need to add signature scanner for it.
            public long GameObjectManagerOffset;

            public IntPtr GameObjectManagerPointer;

            // We look for that object and return its memory address.
            public IntPtr GameWorldPointer;

            // Get Pointer to Local World when we are ingame.
            public IntPtr LocalGameWorldPointer;

            public IntPtr MainAppPointer;
            public IntPtr CommonUIPointer;

            // LootList + 0x18 = "Playercount".
            public int LootCount;

            public IntPtr LootCountAddress;

            public IntPtr LootFirstAddressInTable;

            // LocalWorld + 0x50 = "LootList".
            public IntPtr LootListPointer;

            // Playerlist + 0x18 = "Playercount".
            public int PlayerCount;

            public IntPtr PlayerCountAddress;

            public IntPtr PlayersFirstAddressInTable;

            // LocalWorld + 0x60 = "Playerlist".
            public IntPtr PlayersListPointer;
        }


        public static class AsyncHelper
        {
            private static readonly TaskFactory TaskFactory = new
                TaskFactory(
                            CancellationToken.None,
                            TaskCreationOptions.None,
                            TaskContinuationOptions.None,
                            TaskScheduler.Default);

            public static TResult RunSync<TResult>(Func<Task<TResult>> func)
                => TaskFactory
                    .StartNew(func)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

            public static void RunSync(Func<Task> func)
                => TaskFactory
                    .StartNew(func)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();
        }
    }
}