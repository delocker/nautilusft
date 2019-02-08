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

using OpenTK;
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace NautilusFT
{
    public enum BodyParts
    {
        Head,
        Chest,
        Stomach,
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }

    public enum BodyPartsMaxValues
    {
        Head = 35,
        Chest = 80,
        Stomach = 70,
        LeftArm = 60,
        RightArm = 60,
        LeftLeg = 65,
        RightLeg = 65
    }

    public enum Pose
    {
        Stand,
        Crouch,
        Prone
    }

    public enum EMemberCategory
    {
        Default = 0,
        Developer = 1,
        UniqueId = 2,
        Trader = 4,
        Group = 8,
        System = 16,
        ChatModerator = 32,
        ChatModeratorWithPermanentBan = 64,
        Emissar = 98, // my guess
        UnitTest = 128
        ///    3 ? another cocksucker?
    }

    public enum EquipmentSlot
    {
        FirstPrimaryWeapon,
        SecondPrimaryWeapon,
        Holster,
        Scabbard,
        Backpack,
        SecuredContainer,
        TacticalVest,
        ArmorVest,
        Pockets,
        Eyewear,
        FaceCover,
        Headwear,
        Earpiece,
        Dogtag,
        ArmBand
    }

    public struct PlayerStruct
    {
        public string AccountID;
        public string GroupID;
        public int HealthPercent;
        public bool LocalPlayer;
        public string NickName;
        public string NickNameOriginal;
        public Color PlayerColor;
        public IntPtr[] playerHealthPointer;
        public IntPtr playerPositionAndViewPointer;
        public IntPtr playerPositionAndViewPointerReserve;

        public IntPtr[] playerWeaponsSlotsPointer;
        public float PosX;
        public float PosY;
        public float PosZ;
        public string Side;
        public int TeamIdx;
        public float ViewX;
        public string Weapons;
        public bool DumpComplete;
        public string Pose;
        public int MemberCategory;
        public bool Cheater;
        public DateTime ResetTime;

        public IntPtr EquipmentPointer;
        public long EquipmentValue;
        public IntPtr ProfilePointer;
        public long ProfileValue;
        public IntPtr MemberCategoryPointer;
        public IntPtr NickNamePointer;
        public IntPtr NickNamePointerLength;

        public byte[] NickNameValue;

        public IntPtr NoRecoilPointer;
        public int NoRecoilOriginalValue;
        public IntPtr NoDamagePointer;
        public int NoDamageOriginalValue;
    }

    public sealed class EntityPlayerListCache : IEquatable<EntityPlayerListCache>
    {
        public PlayerStruct structs;

        private static readonly int PlayerProfile = 0x450;
        private static readonly int[] PlayerNicknameOffsets = new int[] { PlayerProfile, 0x28, 0x10, 0x14 };
        private static readonly int[] PlayerNicknameLengthOffsets = new int[] { PlayerProfile, 0x28, 0x10, 0x10 };

        // Int32, Side 1 = USEC, 2 = BEAR, 4 = SCAV
        private static readonly int[] PlayerSideOffsets = new int[] { PlayerProfile, 0x28, 0x48 };
        private static readonly int[] PlayerMemberCategoryOffsets = new int[] { PlayerProfile, 0x28, 0x58 };

        private static readonly int[] PlayerAccountIDOffsets = new int[] { PlayerProfile, 0x18, 0x14 };

        // yhis is serialized string, unique enough to differ teams. Could be broken.
        private static readonly int[] PlayerGroupOffsets = new int[] { PlayerProfile, 0x28, 0x18, 0x14 };

        // private static readonly int[] PlayerPositionOffsets = new int[] { 0x40, 0x60 }; // first XYZ
        private static readonly int[] PlayerPositionOffsets = new int[] { 0x40, 0x0 }; // 0x60 first XYZ
        private static readonly int[] PlayerPositionOffsetsReserve = new int[] { 0x10, 0x30, 0x30, 0x08, 0x38, 0x90 }; // first XYZ
        /* * 0x10, 0x30, 0x30 = ?
        * 
        * Player + 0x8] = Transform
        * Transform + 0x38] = Visuals
        * Visuals + 0x90 = Vec3 Position
        * 
        * 
        * */

        // (GameObject)->GetTransform()->Transform::Internal_get_pos()

        // Weapon Anim / No Recoil
        private static readonly int[] PlayerNoRecoilOffsets = new int[] { 0x78, 0xc0 }; // first XYZ

        // (GameObject)->GetTransform()->Transform::Internal_get_pos()

        /*BONES? player + 0xb0] + 0x28] + 0x28] + 0x10] + 0x20] + 0x10] + 0x38] + 0x90] */
        //// private static readonly int[] PlayerViewOffset = new int[] { 0x58, 0x1EC }; // first XYZ
        private static readonly int[] PlayerEquipmentOffsets = new int[] { PlayerProfile, 0x48, 0x10 };
        ////private static readonly int[] PlayerEquipmentExtendedOffsets = new int[] { PlayerProfile, 0x48, 0x10, 0x68, 0x0 }; //right at first entry

        private static readonly int[] PlayerWeaponFirstPrimarySlotOffsets = new int[] { PlayerProfile, 0x48, 0x30, 0x10, 0x28, 0x30 };
        private static readonly int[] PlayerWeaponSecondPrimarySlotOffsets = new int[] { PlayerProfile, 0x48, 0x30, 0x10, 0x28, 0x38 };
        private static readonly int[] PlayerWeaponNameOffsets = new int[] { 0x20, 0x20, 0x58, 0x14 };

        // BodyCurrent , BodyMaximumHP + 0x4
        private static readonly int[] PlayerHealthControllerOffsets = new int[] { 0x458, 0x10, 0x28 };

        ////private static readonly int[] PlayerBodyHydrationOffsets = new int[] { PlayerProfile, 0x40, 0x10, 0x20 };

        private readonly IntPtr baseAddrPointer;
        private readonly IntPtr baseAddr;

        public EntityPlayerListCache(IntPtr addrPointer, long addr)
        {
            baseAddrPointer = addrPointer;
            baseAddr = (IntPtr)addr;
            structs = new PlayerStruct
            {
                playerWeaponsSlotsPointer = new IntPtr[2],
                playerHealthPointer = new IntPtr[7],
                MemberCategory = -1,
                ResetTime = DateTime.Now,
            };
        }

        public bool Equals(EntityPlayerListCache other)
        {
            if (baseAddr == other.baseAddr && baseAddrPointer == other.baseAddrPointer)
            {
                return true;
            }

            return false;
        }

        public void GetPlayerValues()
        {
            // Cached
            GetPlayerAccountID();
            GetPlayerSide();
            GetPlayerGroupID();
            GetPlayerNickname();
            GetPlayerMemberCategory();

            GetPlayerColor();

            // Realtime
            GetPlayerPositionAndViewAngle();

            // Realtime Delayed
            GetPlayerHealthPecent();
            GetPlayerWeapons();

            // Aggressive
            GetAggressivePointers();
            AgressiveThings();

            // Other
            DumpToFile();
        }

        public void BuildRenderList()
        {
            var entityPlayer = new EntityPlayerListRender(
                structs.NickName,
                structs.PlayerColor,
                10,
                structs.PosX,
                structs.PosY,
                structs.PosZ,
                structs.ViewX,
                structs.Weapons,
                structs.Side,
                structs.HealthPercent,
                structs.Pose,
                structs.Cheater);

            if (structs.LocalPlayer)
            {
                HelperOpenGL.PlayerListToRenderPre.Insert(0, entityPlayer);
            }
            else
            {
                HelperOpenGL.PlayerListToRenderPre.Add(entityPlayer);
            }
        }

        private DateTime ResetTime()
        {
            return DateTime.Now.AddSeconds(Helper.GetRandomNumber(1, 2));
        }

        private void GetPlayerAccountID()
        {
            if (structs.AccountID == null)
            {
                var playerAccountIDPointer = Memory.GetPtr(baseAddrPointer, PlayerAccountIDOffsets);
                var playerAccountID = Helper.GetStringFromMemory(playerAccountIDPointer, true);
                structs.AccountID = playerAccountID;
            }
        }

        private void GetPlayerMemberCategory()
        {
            if (structs.MemberCategory == -1)
            {
                var playerMemberCategoryPointer = Memory.GetPtr(baseAddrPointer, PlayerMemberCategoryOffsets);
                structs.MemberCategoryPointer = playerMemberCategoryPointer;
                var playerMemberCategory = Memory.Read<int>((long)playerMemberCategoryPointer);
                structs.MemberCategory = playerMemberCategory;
            }
        }

        private void GetPlayerSide()
        {
            if (structs.Side == null)
            {
                var playerSidePointer = Memory.GetPtr(baseAddrPointer, PlayerSideOffsets);
                int side = Memory.Read<int>((long)playerSidePointer);

                switch (side)
                {
                    case 1:
                        structs.Side = "USEC";
                        break;

                    case 2:
                        structs.Side = "BEAR";
                        break;

                    case 4:
                        if (structs.AccountID != "0")
                        {
                            structs.Side = "SCAV";
                        }
                        else
                        {
                            structs.Side = "UNKN";
                        }

                        break;

                    default:
                        structs.Side = "UNKN";
                        break;
                }
            }
        }

        private void GetPlayerGroupID()
        {
            if (structs.GroupID == null)
            {
                var playerGroupPointer = Memory.GetPtr(baseAddrPointer, PlayerGroupOffsets);

                // Read groupID if it is empty then it will be "n/a"
                structs.GroupID = Helper.GetStringFromMemory(playerGroupPointer, true);

                // If there is no groupID
                if (structs.GroupID == "n/a")
                {
                    structs.TeamIdx = -1;
                    return;
                }

                // Check if we have group in TeamsList
                var indexColor = Settings.ListTeams.IndexOf(structs.GroupID);

                // We Do
                if (indexColor >= 0)
                {
                    structs.TeamIdx = indexColor;
                    return;
                }

                // We Don`t
                if (indexColor < 0)
                {
                    // Add that team to list
                    Settings.ListTeams.Add(structs.GroupID);
                    structs.TeamIdx = Settings.ListTeams.Count - 1;
                    return;
                }
            }
        }

        private void GetPlayerColor()
        {
            if (structs.PlayerColor == Color.Empty)
            {
                Color drawcolor;
                if (structs.AccountID == Settings.AccID)
                {
                    // 100% ME
                    drawcolor = Settings.ColorPlayerMyself;
                }
                else if (Settings.ListFriends.IndexOf(structs.AccountID) >= 0)
                {
                    // 100% FRIEND
                    drawcolor = Settings.ColorPlayerFRIEND;
                }
                else if (Settings.ListCheaters.ContainsKey(structs.AccountID))
                {
                    // 100% CHEATER
                    drawcolor = Settings.ColorPlayerCHEATERorADMIN;
                    structs.Cheater = true;
                    string oldnames;
                    Settings.ListCheaters.TryGetValue(structs.AccountID, out oldnames);
                    Console.WriteLine("CHEATER" + " : " + structs.NickName + " (" + oldnames + ")");
                }
                else if (structs.MemberCategory != 0 && structs.MemberCategory != 2)
                {
                    Console.WriteLine(Enum.GetName(typeof(EMemberCategory), structs.MemberCategory) + " : " + structs.NickName + " : " + structs.MemberCategory);
                    drawcolor = Settings.ColorPlayerCHEATERorADMIN;
                }
                else if (structs.TeamIdx >= 0)
                {
                    // Colorizing player by team color.
                    drawcolor = (Settings.ListTeamColors.Count - 1) >= structs.TeamIdx ? Settings.ListTeamColors[structs.TeamIdx] : Settings.ColorPlayerOther;
                }
                else if (structs.Side == "BEAR" || structs.Side == "USEC")
                {
                    // 100% HUMAN
                    drawcolor = Settings.ColorPlayerOther;
                }
                else if (structs.AccountID != "0")
                {
                    // SCAV
                    drawcolor = Settings.ColorPlayerOther;
                }
                else
                {
                    // BOT
                    drawcolor = Settings.ColorPlayerBOT;
                }

                structs.PlayerColor = drawcolor;
            }
        }

        private void GetPlayerNickname()
        {
            if (structs.NickName == null)
            {
                var playerNicknamePointer = Memory.GetPtr(baseAddrPointer, PlayerNicknameOffsets);

                structs.NickNamePointerLength = Memory.GetPtr(baseAddrPointer, PlayerNicknameLengthOffsets);
                structs.NickNamePointer = playerNicknamePointer;
                structs.NickNameValue = Memory.ReadBytes(playerNicknamePointer.ToInt64(), 32);
                var playerNickname = Helper.GetStringFromMemory(playerNicknamePointer, true);
                structs.NickNameOriginal = playerNickname;

                if (structs.AccountID == Settings.AccID)
                {
                    // 100% ME
                    structs.NickName = " ";
                    structs.LocalPlayer = true;
                    Settings.GroupID = structs.GroupID;
                }
                else if (Settings.ListFriends.IndexOf(structs.AccountID) >= 0)
                {
                    // 100% FRIEND
                    structs.NickName = playerNickname;
                }
                else if (structs.TeamIdx >= 0)
                {
                    structs.NickName = playerNickname;
                }
                else if (structs.Side == "BEAR" || structs.Side == "USEC")
                {
                    // 100% HUMAN
                    structs.NickName = playerNickname;
                }
                else if (structs.AccountID != "0")
                {
                    // SCAV
                    structs.NickName = playerNickname;
                }
                else
                {
                    // BOT
                    structs.NickName = playerNickname;
                }
            }
        }

        private void GetPlayerPositionAndViewAngle()
        {
            if (structs.playerPositionAndViewPointer == IntPtr.Zero)
            {
                structs.playerPositionAndViewPointer = Memory.GetPtr(baseAddrPointer, PlayerPositionOffsets);
            }

            // Workaround because when player count changes (players dies/joins), some cached data becomes void. Better re-read.
            if (DateTime.Now > structs.ResetTime)
            {
                structs.playerPositionAndViewPointerReserve = Memory.GetPtr(baseAddrPointer, PlayerPositionOffsetsReserve);
                structs.ResetTime = ResetTime();
            }

            // Read three XYZ positions in one RPM call. As they are quite 'far' away from each other, we have to read bytes inbetween to have one RPM call. No big deal.
            var databuffer = Memory.ReadBytes((long)structs.playerPositionAndViewPointer, sizeof(float) * 200);

            // Used for viewAngle (azimuth)
            structs.ViewX = BitConverter.ToSingle(databuffer, 0x1F4); // 1F4

            var isStanding = BitConverter.ToSingle(databuffer, 0x1C0); // 1 = Stand 0 = Sit // 1C8
            var isProne = BitConverter.ToInt32(databuffer, 0x258); // 1 = Prone , 0 = Not

            if (isProne == 1)
            {
                structs.Pose = "Prone";
            }
            else
            {
                if (isStanding == 0)
                {
                    structs.Pose = "Crouch";
                }
                else
                {
                    structs.Pose = "Standing";
                }
            }

            // Sort them away.
            if (structs.LocalPlayer)
            {
                structs.PosX = BitConverter.ToSingle(databuffer, 0x60);
                structs.PosY = BitConverter.ToSingle(databuffer, 0x64);
                structs.PosZ = BitConverter.ToSingle(databuffer, 0x68);
                Aggressive.data.myPositionAndViewPointer = structs.playerPositionAndViewPointerReserve;
            }

            // Transform
            if (!structs.LocalPlayer)
            {
                var databuffer2 = Memory.ReadBytes((long)structs.playerPositionAndViewPointerReserve, sizeof(float) * 3);
                structs.PosX = BitConverter.ToSingle(databuffer2, 0);
                structs.PosY = BitConverter.ToSingle(databuffer2, 4);
                structs.PosZ = BitConverter.ToSingle(databuffer2, 8);
            }
        }

        private void GetPlayerHealthPecent()
        {
            if (!Settings.PlayersExtraInfoCanUpdate)
            {
                if (structs.HealthPercent == 0)
                {
                    structs.HealthPercent = 100;
                }

                return;
            }

            var playerHealthController = Memory.GetPtr(baseAddrPointer, PlayerHealthControllerOffsets);

            // Go through all body pointers and get bodypart pointers filled in
            for (int i = 0; i < structs.playerHealthPointer.Length; i++)
            {
                if (structs.playerHealthPointer[i] == IntPtr.Zero)
                {
                    structs.playerHealthPointer[i] = Memory.GetPtr(playerHealthController, new int[] { 0x20 + (0x8 * i), 0x10, 0x10 });
                }
            }

            // HP 435 / 100 = 4.35 , currHP / 4.35 = percentage of 100%.
            var playerBodyHeadValue = Memory.Read<float>((long)structs.playerHealthPointer[(int)BodyParts.Head]);
            var playerBodyChestValue = Memory.Read<float>((long)structs.playerHealthPointer[(int)BodyParts.Chest]);
            var playerBodyStomachValue = Memory.Read<float>((long)structs.playerHealthPointer[(int)BodyParts.Stomach]);
            var playerBodyLeftArmValue = Memory.Read<float>((long)structs.playerHealthPointer[(int)BodyParts.LeftArm]);
            var playerBodyRightArmValue = Memory.Read<float>((long)structs.playerHealthPointer[(int)BodyParts.RightArm]);
            var playerBodyLeftLegValue = Memory.Read<float>((long)structs.playerHealthPointer[(int)BodyParts.LeftLeg]);
            var playerBodyRightLegValue = Memory.Read<float>((long)structs.playerHealthPointer[(int)BodyParts.RightLeg]);

            structs.HealthPercent = (int)((playerBodyHeadValue + playerBodyChestValue +
               playerBodyStomachValue + playerBodyLeftArmValue +
              playerBodyRightArmValue + playerBodyLeftLegValue + playerBodyRightLegValue) / 4.35f);
        }

        private void GetPlayerWeapons()
        {
            if (!Settings.PlayersExtraInfoCanUpdate)
            {
                if (structs.Weapons == null)
                {
                    structs.Weapons = string.Empty;
                }

                return;
            }

            if (structs.playerWeaponsSlotsPointer[0] == IntPtr.Zero)
            {
                structs.playerWeaponsSlotsPointer[0] = Memory.GetPtr(baseAddrPointer, PlayerWeaponFirstPrimarySlotOffsets);
            }

            if (structs.playerWeaponsSlotsPointer[1] == IntPtr.Zero)
            {
                structs.playerWeaponsSlotsPointer[1] = Memory.GetPtr(baseAddrPointer, PlayerWeaponSecondPrimarySlotOffsets);
            }

            var primaryWeaponPointer = Memory.GetPtr(structs.playerWeaponsSlotsPointer[0], PlayerWeaponNameOffsets);
            var secondaryWeaponPointer = Memory.GetPtr(structs.playerWeaponsSlotsPointer[1], PlayerWeaponNameOffsets);

            var weaponOne = Helper.GetStringFromMemory(primaryWeaponPointer, true, 64);
            var weaponTwo = Helper.GetStringFromMemory(secondaryWeaponPointer, true, 64);
            structs.Weapons = weaponOne + "\n" + weaponTwo;
            Regex weaponRegex = new Regex("(n.a)|weapon_|(izhmash_|izhmeh_|toz_|tochmash_|lobaev_|molot_)|_[0-9]+([0-9]|[a-zA-Z])+");
            structs.Weapons = weaponRegex.Replace(structs.Weapons, string.Empty);
        }

        private void GetAggressivePointers()
        {
            if (structs.EquipmentPointer == IntPtr.Zero)
            {
                structs.EquipmentPointer = Memory.GetPtr(baseAddrPointer, PlayerEquipmentOffsets);
            }

            if (structs.EquipmentValue == 0)
            {
                structs.EquipmentValue = Memory.Read<long>((long)structs.EquipmentPointer);
            }

            if (structs.ProfilePointer == IntPtr.Zero)
            {
                structs.ProfilePointer = Memory.GetPtr(baseAddrPointer, new int[] { PlayerProfile });
            }

            if (structs.ProfileValue == 0)
            {
                structs.ProfileValue = Memory.Read<long>((long)structs.ProfilePointer);
            }

            if (structs.LocalPlayer)
            {
                if (structs.NoRecoilPointer == IntPtr.Zero)
                {
                    structs.NoRecoilPointer = Memory.GetPtr(baseAddrPointer, PlayerNoRecoilOffsets);
                }

                if (structs.NoRecoilOriginalValue == 0)
                {
                    structs.NoRecoilOriginalValue = Memory.Read<int>((long)structs.NoRecoilPointer);
                }
            }
        }

        private void AgressiveThings()
        {
            if (!Aggressive.PlayerTargetsDictEquipment.ContainsKey(structs.EquipmentValue))
            {
                if (structs.LocalPlayer)
                {
                    Aggressive.PlayerTargetsDictEquipment.Add(structs.EquipmentValue, "-YOU-");
                }
                else
                {
                    var distance = (int)Math.Round(Helper.GetDistance(new Vector3(structs.PosX, structs.PosY, structs.PosZ), new Vector3(Helper.myPosXingame, Helper.myPosYingame, Helper.myPosZingame)));
                    if (distance < 1000)
                    {
                        Aggressive.PlayerTargetsDictEquipment.Add(structs.EquipmentValue, structs.NickNameOriginal);
                    }
                }

                if (!Aggressive.PlayerTargetsDictProfile.ContainsKey(structs.ProfileValue))
                {
                    if (structs.LocalPlayer)
                    {
                        Aggressive.PlayerTargetsDictProfile.Add(structs.ProfileValue, "-YOU-");
                    }
                    else
                    {
                        var distance = (int)Math.Round(Helper.GetDistance(new Vector3(structs.PosX, structs.PosY, structs.PosZ), new Vector3(Helper.myPosXingame, Helper.myPosYingame, Helper.myPosZingame)));
                        if (distance < 1000)
                        {
                            Aggressive.PlayerTargetsDictProfile.Add(structs.ProfileValue, structs.NickNameOriginal);
                        }
                    }

                    Aggressive.PlayerTargetsListEquipmentViewNeedsUpdate = true;
                }
            }

            if (structs.LocalPlayer)
            {
                if (Aggressive.EquipmentWriteDo)
                {
                    Aggressive.EquipmentWriteDo = false;
                    Memory.Write((long)structs.EquipmentPointer, Aggressive.EquipmentValueToMap);
                    Memory.Write((long)structs.ProfilePointer, Aggressive.ProfileValueToMap);
                }

                if (Aggressive.FakeMeDo)
                {
                    var nickNamePointer = Memory.GetPtr(structs.ProfilePointer, new int[] { 0x28, 0x10, 0x14 });
                    var nickNamePointerLength = Memory.GetPtr(structs.ProfilePointer, new int[] { 0x28, 0x10, 0x10 });

                    byte[] bytes = Encoding.Unicode.GetBytes(Aggressive.FakeName);
                    Memory.WriteBytes((long)nickNamePointer, bytes.Length, bytes);
                    Memory.Write((long)nickNamePointerLength, Aggressive.FakeName.Length);

                    Aggressive.FakeMeDo = false;
                }

                if (Aggressive.MemberCategoryDo)
                {
                    Aggressive.MemberCategoryDo = false;

                    var memberCategoryPointer = Memory.GetPtr(structs.ProfilePointer, new int[] { 0x28, 0x58 });

                    if (Aggressive.MemberCategoryDoMod)
                    {
                        Aggressive.MemberCategoryDoMod = false;
                        Memory.Write((long)memberCategoryPointer, (int)Aggressive.MemberCategoryID.System);
                    }

                    if (Aggressive.MemberCategoryDoDev)
                    {
                        Aggressive.MemberCategoryDoDev = false;
                        Memory.Write((long)memberCategoryPointer, (int)Aggressive.MemberCategoryID.Developer);
                    }

                    if (Aggressive.MemberCategoryDoDefault)
                    {
                        Aggressive.MemberCategoryDoDefault = false;
                        Memory.Write((long)memberCategoryPointer, (int)Aggressive.MemberCategoryID.UniqueId);
                    }
                }

                if (Aggressive.NoRecoilDo)
                {
                    if (Aggressive.NoRecoil)
                    {
                        Memory.Write<int>((long)structs.NoRecoilPointer, 4);
                    }
                    else
                    {
                        Memory.Write<int>((long)structs.NoRecoilPointer, structs.NoRecoilOriginalValue);
                    }

                    Aggressive.NoRecoilDo = false;
                }
            }
        }


        private void DumpToFile()
        {
            // Dump players to text file. Excluding bots and those who were dumped already.
            if (!structs.DumpComplete && structs.AccountID != "0" && Settings.DumpPlayers)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"_dump_Players.txt", true, System.Text.Encoding.Unicode))
                {
                    file.Write(structs.AccountID);
                    file.Write("\t" + structs.NickName);
                    file.WriteLine();
                }

                structs.DumpComplete = true;
            }
        }
    }
}
