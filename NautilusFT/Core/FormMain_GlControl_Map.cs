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
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NautilusFT
{
    public partial class FormMain
    {
        private bool openglControlMapCanUse = false;

        // Fix for shoreline map because it is upside down. Poor devs.
        private int openglInvertMap = 1;
        private bool renderMapComplete = true;

        #region GL_BaseFunctions

        public void GlControl_Map_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);

            HelperOpenGL.GlControl_LoadTextures();
            HelperOpenGL.GlControl_GenerateFonts();
        }

        private void GlControl_Map_Resize(object sender, EventArgs e)
        {
            HelperOpenGL.GlControl_Refresh(openglControlMap);
        }

        private void GlControl_Map_RenderDo(object sender, PaintEventArgs e)
        {
            if (!openglControlMapCanUse)
            {
                return;
            }

            if (Settings.CurrentMap != Settings.CurrentMapSwitchTo)
            {
                Settings.CurrentMap = Settings.CurrentMapSwitchTo;
                HelperOpenGL.GlControl_LoadMapTexture(Settings.CurrentMap);
            }

            HelperOpenGL.GlControl_MakeCurrent(openglControlMap);
            HelperOpenGL.GlControl_SetupViewport(openglControlMap.Width, openglControlMap.Height);
            HelperOpenGL.GlControl_DrawStart();
            HelperOpenGL.GlControl_Draw_Map(openglControlMapDragOffsetX, openglControlMapDragOffsetY);

            // Render real or test objects
            if (!Settings.DemoMode)
            {
                GlControl_Map_PrepareObjects();
            }
            else
            {
                GlControl_Map_PrepareTestObjects(openglControlMap);
            }

            GlControl_Map_RenderObjects();
            HelperOpenGL.GlControl_Draw_End(openglControlMap);
            renderMapComplete = true;
        }

        #endregion GL_BaseFunctions

        // Main Draw Cycle it includes the logic that need to be split later on.
        private void GlControl_Map_PrepareObjects()
        {
            /* Here comes all logic that is required before rendering objects
             * we do all calculations here and add everything else and then add it to render list */

            /* X.Y.
            * Left Top ( - | + )
            * Left Bottom ( - | - )
            * Right Top ( + | + )
            * Right Bottom ( + | - )
            */

            // Fix for shoreline, dont know how to make this more beautiful but it`s OK.
            switch (Settings.CurrentMap)
            {
                case "Shoreline":
                    openglInvertMap = -1;
                    break;

                case "Interchange":
                    openglInvertMap = -1;
                    break;

                default:
                    openglInvertMap = 1;
                    break;
            }

            // Fix to keep up with network.
            if (HelperOpenGL.PlayerListToRenderFinal.Count == 0)
            {
                return;
            }

            Helper.myPosXingame = HelperOpenGL.PlayerListToRenderFinal[0].MapLocX;
            Helper.myPosYingame = HelperOpenGL.PlayerListToRenderFinal[0].MapLocY;
            Helper.myPosZingame = HelperOpenGL.PlayerListToRenderFinal[0].MapLocZ;

            var distance = 0f;

            // On Screen Display
            var osdText = new StringBuilder();

            // Render item declare.
            RenderItem renderItem;

            /* The order it goes to openGL
             * From least important to most, includes text. */

            // Check iteration readcomplete open close bools. It must check if true then iterate then close.
            if (Settings.FindLoot)
            {
                GlControl_Map_PrepareObjects_Loot();
            }

            // Check iteration readcomplete open close bools. It must check if true then iterate then close.
            if (mapMarkerDraw)
            {
                renderItem = new RenderItem();
                renderItem.structs.MapPosX = mapMarkerX;
                renderItem.structs.MapPosZ = mapMarkerY;
                renderItem.structs.Text = "loot";
                renderItem.structs.Rotation = 45;
                renderItem.structs.DrawColor = Color.Yellow;
                HelperOpenGL.OpenglMapIcons.Add(renderItem);

                renderItem = new RenderItem();
                renderItem.structs.MapPosX = mapMarkerX;
                renderItem.structs.MapPosZ = mapMarkerY;
                renderItem.structs.Text = "circlefill";
                renderItem.structs.Size = 32 * Settings.MapSizeIcon;
                renderItem.structs.DrawColor = Color.FromArgb(64, 255, 255, 0);
                HelperOpenGL.OpenglMapGeometry.Add(renderItem);

                ////glControl_Map_Draw_Circle((float)Settings.Data.map_MapMarkerScanSize * map_ratio, mapMarkerX, mapMarkerY, Color.FromArgb(64, 255, 255, 0));
                ////
                ////int distance = (int)Math.Sqrt(Math.Pow((my_player_coord_real_Y - reversalmapMarkerLocY), 2) + Math.Pow((my_player_coord_real_X - reversalmapMarkerLocX), 2));
                ////glControl_Map_DrawLines(3, my_player_coord_formap_X, my_player_coord_formap_Y, mapMarkerX, mapMarkerY, Color.FromArgb(64, Color.Red));
                ////string distanceString = distance + "m";
                ////glControl_Map_DrawText(distanceString, Color.White, 10, mapMarkerX - 20, mapMarkerY + 30);
            }

            // Map Maker TEMP
            for (int i = 0; i < HelperOpenGL.MapMakerDots.Count; i++)
            {
                var entityPosXingame = HelperOpenGL.MapMakerDots[i].MapLocX;
                var entityPosYingame = HelperOpenGL.MapMakerDots[i].MapLocY;
                var entityPosZingame = HelperOpenGL.MapMakerDots[i].MapLocZ;
                var entityPosXmap = (entityPosXingame * Settings.MapZoomLevelFromZoomLevels * openglInvertMap) + openglControlMapDragOffsetX;
                var entityPosZmap = (entityPosZingame * Settings.MapZoomLevelFromZoomLevels * openglInvertMap) + openglControlMapDragOffsetY;
                var entityPosYmap = entityPosYingame;

                renderItem = new RenderItem();
                renderItem.structs.Text = "point";
                renderItem.structs.MapPosX = entityPosXmap;
                renderItem.structs.MapPosZ = entityPosZmap;
                renderItem.structs.DrawColor = HelperOpenGL.MapMakerDots[i].Drawcolor;
                renderItem.structs.Size = HelperOpenGL.MapMakerDots[i].Size;
                HelperOpenGL.OpenglMapGeometry.Add(renderItem);
            }

            // Players
            for (int i = 0; i < HelperOpenGL.PlayerListToRenderFinal.Count; i++)
            {
                // Default FOV.
                var lengthFOV = Settings.MapFOVlineOther * Settings.MapZoomLevelFromZoomLevels;

                var entityPosXingame = HelperOpenGL.PlayerListToRenderFinal[i].MapLocX;
                var entityPosYingame = HelperOpenGL.PlayerListToRenderFinal[i].MapLocY;
                var entityPosZingame = HelperOpenGL.PlayerListToRenderFinal[i].MapLocZ;

                // Don`t draw who at 0.0.0
                if (entityPosXingame == 0 || entityPosZingame == 0)
                {
                    continue;
                }

                var entityPosXmap = (entityPosXingame * Settings.MapZoomLevelFromZoomLevels * openglInvertMap) + openglControlMapDragOffsetX;
                var entityPosZmap = (entityPosZingame * Settings.MapZoomLevelFromZoomLevels * openglInvertMap) + openglControlMapDragOffsetY;
                var entityPosYmap = entityPosYingame;

                // Override for OurPlayer.
                if (i == 0)
                {
                    // dot do map maker
                    if (dotDo)
                    {
                        var entityPlayer = new EntityPlayerListRender(
                          "point",
                          Color.Black,
                          1,
                          HelperOpenGL.PlayerListToRenderFinal[i].MapLocX,
                          HelperOpenGL.PlayerListToRenderFinal[i].MapLocY,
                          HelperOpenGL.PlayerListToRenderFinal[i].MapLocZ,
                          0,
                          string.Empty,
                          string.Empty,
                          0,
                          string.Empty,
                          false);

                        HelperOpenGL.MapMakerDots.Add(entityPlayer);
                    }

                    // Inner circle
                    renderItem = new RenderItem();
                    renderItem.structs.Text = "circle";
                    renderItem.structs.MapPosX = entityPosXmap;
                    renderItem.structs.MapPosZ = entityPosZmap;
                    renderItem.structs.DrawColor = Color.FromArgb(255, Color.Red);
                    renderItem.structs.Size = 100 * Settings.MapZoomLevelFromZoomLevels;
                    HelperOpenGL.OpenglMapGeometry.Add(renderItem);

                    // Medium circle
                    renderItem = new RenderItem();
                    renderItem.structs.Text = "circle";
                    renderItem.structs.MapPosX = entityPosXmap;
                    renderItem.structs.MapPosZ = entityPosZmap;
                    renderItem.structs.DrawColor = Color.FromArgb(255, Color.Green);
                    renderItem.structs.Size = 200 * Settings.MapZoomLevelFromZoomLevels;
                    HelperOpenGL.OpenglMapGeometry.Add(renderItem);

                    // Outer circle
                    renderItem = new RenderItem();
                    renderItem.structs.Text = "circle";
                    renderItem.structs.MapPosX = entityPosXmap;
                    renderItem.structs.MapPosZ = entityPosZmap;
                    renderItem.structs.DrawColor = Color.FromArgb(255, Color.Red);
                    renderItem.structs.Size = 300 * Settings.MapZoomLevelFromZoomLevels;
                    HelperOpenGL.OpenglMapGeometry.Add(renderItem);

                    lengthFOV = Settings.MapFOVlineMyself * Settings.MapZoomLevelFromZoomLevels;
                    textBoxMyPosition.Text = entityPosXingame.ToString() + " | " + entityPosYingame.ToString() + " | " + entityPosZingame.ToString();
                }

                /* Classic Way. Leaving for history.
                 * distance = (int)Math.Sqrt(Math.Pow((
                 * ReaderCore.Entities[0].MapLocZ - ReaderCore.Entities[i].MapLocZ), 2) +
                 * Math.Pow((ReaderCore.Entities[0].MapLocX - ReaderCore.Entities[i].MapLocX), 2));
                 */

                Vector3 vectorMe = new Vector3(Helper.myPosXingame, Helper.myPosZingame, 0);
                Vector3 vectorTarget = new Vector3(entityPosXingame, entityPosZingame, 0);
                Vector3 vectorFOV;

                // Get degress from game for player.
                var degressFOVcenter = HelperOpenGL.PlayerListToRenderFinal[i].MapLocXview;

                // FOV Calculator
                if (openglInvertMap == -1)
                {
                    degressFOVcenter += 180;
                }

                // Correction for negative value.
                if (degressFOVcenter < 0)
                {
                    degressFOVcenter = 360 + degressFOVcenter;
                }

                float degreesFOVleft = degressFOVcenter - 22;
                float degreesFOVright = degressFOVcenter + 22;

                if (degreesFOVleft < 0)
                {
                    degreesFOVleft = 360 + degreesFOVleft;
                }

                if (degreesFOVright < 0)
                {
                    degreesFOVright = 360 + degreesFOVright;
                }

                // Azimuth for OSD.
                if (Settings.MapOSDShowStats && Settings.MapOSDAzimuth && i == 0)
                {
                    // If i is I am :D
                    osdText.Append("AZT: " + ((int)degressFOVcenter).ToString()).AppendLine().AppendLine();
                }

                // Draw FOV Line.
                // Direction for FOV and angle calculator.
                var fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
                var fov_line_Z = (float)(entityPosZmap + (Math.Sin(((degressFOVcenter * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
                vectorFOV = new Vector3(fov_line_X, fov_line_Z, 0);

                if (Settings.MapShowPlayerFOV)
                {
                    renderItem = new RenderItem();
                    renderItem.structs.Text = "linestripple";
                    renderItem.structs.MapPosX = entityPosXmap;
                    renderItem.structs.MapPosZ = entityPosZmap;
                    renderItem.structs.MapPosXend = fov_line_X;
                    renderItem.structs.MapPosZend = fov_line_Z;
                    renderItem.structs.DrawColor = HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor;
                    renderItem.structs.Size = 2;
                    HelperOpenGL.OpenglMapGeometry.Add(renderItem);

                    renderItem = new RenderItem();
                    renderItem.structs.Text = "linestripple_invert";
                    renderItem.structs.MapPosX = entityPosXmap;
                    renderItem.structs.MapPosZ = entityPosZmap;
                    renderItem.structs.MapPosXend = fov_line_X;
                    renderItem.structs.MapPosZend = fov_line_Z;
                    renderItem.structs.DrawColor = System.Windows.Forms.ControlPaint.Dark(HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor);
                    renderItem.structs.Size = 2;
                    HelperOpenGL.OpenglMapGeometry.Add(renderItem);
                }

                distance = (int)Math.Round(Helper.GetDistance(vectorTarget, vectorMe));

                if (i != 0 && Settings.MapShowPlayerDistance)
                {
                    renderItem = new RenderItem();
                    renderItem.structs.Text = distance + "m";
                    renderItem.structs.MapPosX = entityPosXmap + 20;
                    renderItem.structs.MapPosZ = entityPosZmap + 15;
                    renderItem.structs.DrawColor = Settings.ColorText;
                    renderItem.structs.Size = (int)FontSizes.misc;
                    renderItem.structs.TextOutline = true;
                    HelperOpenGL.OpenglMapText.Add(renderItem);
                }

                // Draw Player Icon.
                if (Settings.MapShowPlayers)
                {
                    renderItem = new RenderItem();
                    renderItem.structs.Text = "player";
                    renderItem.structs.MapPosX = entityPosXmap;
                    renderItem.structs.MapPosZ = entityPosZmap;
                    renderItem.structs.DrawColor = HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor;
                    renderItem.structs.Rotation = degressFOVcenter * -1;
                    HelperOpenGL.OpenglMapIcons.Add(renderItem);
                }

                // Draw Player Nickname.
                if (i != 0 && Settings.MapShowPlayerName)
                {
                    switch (HelperOpenGL.PlayerListToRenderFinal[i].Nickname)
                    {
                        case "BOT":
                        case "SCAV":
                            break;

                        default:
                            renderItem = new RenderItem();
                            renderItem.structs.Text = HelperOpenGL.PlayerListToRenderFinal[i].Nickname;
                            renderItem.structs.MapPosX = entityPosXmap + 20 + Settings.MapZoomLevelFromZoomLevels;
                            renderItem.structs.MapPosZ = entityPosZmap;
                            renderItem.structs.DrawColor = HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor;
                            renderItem.structs.TextOutline = true;
                            renderItem.structs.Size = (int)FontSizes.misc;
                            HelperOpenGL.OpenglMapText.Add(renderItem);
                            break;
                    }
                }

                // Draw Weapon.
                if (i != 0 && Settings.MapShowPlayerWeapons)
                {
                    renderItem = new RenderItem();
                    renderItem.structs.Text = HelperOpenGL.PlayerListToRenderFinal[i].Weapon;
                    renderItem.structs.MapPosX = entityPosXmap + 20 + Settings.MapZoomLevelFromZoomLevels;
                    renderItem.structs.MapPosZ = entityPosZmap - 15;
                    renderItem.structs.DrawColor = Settings.ColorText;
                    renderItem.structs.TextOutline = true;
                    renderItem.structs.Size = (int)FontSizes.misc;
                    HelperOpenGL.OpenglMapText.Add(renderItem);
                }

                // PlayerSide.
                if (i != 0 && Settings.MapShowPlayerSide)
                {
                    renderItem = new RenderItem();
                    renderItem.structs.Text = "point";
                    renderItem.structs.MapPosX = entityPosXmap - 7 - Settings.MapZoomLevelFromZoomLevels;
                    renderItem.structs.MapPosZ = entityPosZmap + 7 + Settings.MapZoomLevelFromZoomLevels;
                    renderItem.structs.Size = 5;

                    switch (HelperOpenGL.PlayerListToRenderFinal[i].Side)
                    {
                        case "BEAR":
                            renderItem.structs.DrawColor = Settings.ColorPlayerSideBEAR;
                            HelperOpenGL.OpenglMapGeometry.Add(renderItem);
                            break;

                        case "USEC":
                            renderItem.structs.DrawColor = Settings.ColorPlayerSideUSEC;
                            HelperOpenGL.OpenglMapGeometry.Add(renderItem);
                            break;

                        case "SCAV":
                            renderItem.structs.DrawColor = Settings.ColorPlayerSideSCAV;
                            HelperOpenGL.OpenglMapGeometry.Add(renderItem);
                            break;

                        default:
                            break;
                    }
                }

                // Player Health.
                if (i != 0 && Settings.MapShowPlayerHealth)
                {
                    renderItem = new RenderItem();
                    renderItem.structs.Text = HelperOpenGL.PlayerListToRenderFinal[i].Health.ToString() + "%";
                    renderItem.structs.TextOutline = true;

                    renderItem.structs.MapPosX = entityPosXmap + 20;
                    renderItem.structs.MapPosZ = entityPosZmap - 20;
                    renderItem.structs.DrawColor = Settings.ColorText;
                    renderItem.structs.Size = (int)FontSizes.misc;
                    HelperOpenGL.OpenglMapText.Add(renderItem);
                }

                // Cheater
                if (HelperOpenGL.PlayerListToRenderFinal[i].Cheater)
                {
                    renderItem = new RenderItem();
                    renderItem.structs.Text = "circlefill";
                    renderItem.structs.MapPosX = entityPosXmap;
                    renderItem.structs.MapPosZ = entityPosZmap;
                    renderItem.structs.DrawColor = Color.FromArgb(128, Color.Red);
                    renderItem.structs.Size = Settings.MapSizeIcon + 30;
                }

                // Counters for OSD.
                switch (HelperOpenGL.PlayerListToRenderFinal[i].Side)
                {
                    case "BEAR":
                    case "USEC":
                        reader.PlayerCountPMC++;
                        break;

                    default:
                        break;
                }

                // Counters for OSD.
                switch (HelperOpenGL.PlayerListToRenderFinal[i].Nickname)
                {
                    case "BOT":
                        reader.PlayerCountBOT++;
                        break;

                    case "SCAV":
                        reader.PlayerCountSCAV++;
                        break;

                    default:
                        break;
                }
            }

            Settings.FramesPerSecondCounter++;

            //// OSD.

            if (Settings.MapOSDShowStats && Settings.MapOSDPlayerCount)
            {
                osdText.Append(
                "PMC: " + reader.PlayerCountPMC.ToString()).AppendLine().Append(
                "SCAV: " + reader.PlayerCountSCAV.ToString()).AppendLine().Append(
                "BOT: " + reader.PlayerCountBOT.ToString()).AppendLine().Append(
                "TEAMS: " + Settings.ListTeams.Count).AppendLine().AppendLine();
            }

            if (Settings.MapOSDShowStats)
            {
                osdText.Append(
                "ZOOM: " + Settings.MapZoomLevelFromZoomLevels.ToString()).AppendLine();
            }

            if (Settings.MapOSDShowStats && Settings.MapOSDFPS)
            {
                osdText.Append(
                "FPS: " + Settings.FramerPerSecondOSD).AppendLine();
            }

            if (Settings.MapOSDShowStats && Settings.MapOSDReadCalls)
            {
                osdText.Append(
                "RPM/TICK: " + Memory.RPMCount).AppendLine();
            }

            if (Settings.MapOSDShowStats && Settings.MapOSDDateTime)
            {
                osdText.Append(DateTime.Now.ToString("HH:mm:ss fff"));
            }

            Memory.RPMCount = 0;

            if ((DateTime.Now - Settings.FramesPerSecondTime).Seconds > 1)
            {
                Settings.FramerPerSecondOSD = Settings.FramesPerSecondCounter;
                Settings.FramesPerSecondTime = DateTime.Now;
                Settings.FramesPerSecondCounter = 0;
            }

            renderItem = new RenderItem();
            renderItem.structs.Text = osdText.ToString();
            renderItem.structs.TextOutline = true;

            renderItem.structs.MapPosX = -(openglControlMap.Width / 2) + 10;
            renderItem.structs.MapPosZ = (openglControlMap.Height / 2) - 10;
            renderItem.structs.DrawColor = Settings.ColorOSD;
            renderItem.structs.Size = (int)FontSizes.misc;
            HelperOpenGL.OpenglMapText.Add(renderItem);
        }

        private void GlControl_Map_PrepareObjects_Loot()
        {
            if (findLootListClear)
            {
                if (findLootListClearLootItems)
                {
                    Settings.LootListToLookFor.Clear();
                }

                Settings.LootListForListView.Clear();
                findLootListClear = !findLootListClear;
            }

            RenderItem renderItem;
            for (int i = 0; i < HelperOpenGL.LootListToRenderFinal.Count; i++)
            {
                var lootPosXoriginal = HelperOpenGL.LootListToRenderFinal[i].MapLocX * openglInvertMap;
                var lootPosYoriginal = HelperOpenGL.LootListToRenderFinal[i].MapLocY * openglInvertMap;
                var lootPosZoriginal = HelperOpenGL.LootListToRenderFinal[i].MapLocZ * openglInvertMap;

                var lootPosXmap = (lootPosXoriginal * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetX;
                var lootPosZmap = (lootPosZoriginal * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetY;
                var lootPosYmap = lootPosYoriginal;

                renderItem = new RenderItem();
                renderItem.structs.MapPosX = lootPosXmap;
                renderItem.structs.MapPosZ = lootPosZmap;

                switch (HelperOpenGL.LootListToRenderFinal[i].Lootname)
                {
                    case "lootable":
                    case "box_Weapon_110x45":
                    case "box_Weapon_64":
                    case "box_GunSafe":
                    case "box_WeaponBox":
                        renderItem.structs.Text = "tentbox";
                        renderItem.structs.DrawColor = Settings.ColorMilitaryCrate;
                        HelperOpenGL.OpenglMapIcons.Add(renderItem);
                        break;

                    case "box_Weapon_140":
                        renderItem.structs.Text = "tentbox";
                        renderItem.structs.DrawColor = Settings.ColorMilitaryCrateLong;
                        HelperOpenGL.OpenglMapIcons.Add(renderItem);
                        break;

                    case "box_Ammo":
                        renderItem.structs.Text = "tentbox";
                        renderItem.structs.DrawColor = Settings.ColorMilitaryCrateAmmo;
                        HelperOpenGL.OpenglMapIcons.Add(renderItem);
                        break;

                    case "box_Grenades":
                        renderItem.structs.Text = "tentbox";
                        renderItem.structs.DrawColor = Settings.ColorMilitaryCrateGrenades;
                        HelperOpenGL.OpenglMapIcons.Add(renderItem);
                        break;

                    case "Corpse":
                        renderItem.structs.Text = "npc_dead";
                        renderItem.structs.DrawColor = Settings.ColorCorpse;
                        HelperOpenGL.OpenglMapIcons.Add(renderItem);
                        break;

                    default:
                        renderItem.structs.Text = "loot";
                        renderItem.structs.DrawColor = Settings.ColorLoot;
                        HelperOpenGL.OpenglMapIcons.Add(renderItem);

                        renderItem = new RenderItem();
                        renderItem.structs.MapPosX = lootPosXmap + 10 + Settings.MapZoomLevelFromZoomLevels;
                        renderItem.structs.MapPosZ = lootPosZmap + 8;
                        renderItem.structs.Text = HelperOpenGL.LootListToRenderFinal[i].Lootname;
                        renderItem.structs.DrawColor = System.Windows.Forms.ControlPaint.Light(Settings.ColorLoot);
                        renderItem.structs.Size = (int)FontSizes.misc;
                        renderItem.structs.TextOutline = true;
                        HelperOpenGL.OpenglMapText.Add(renderItem);
                        break;
                }
            }

            if (Settings.FindLootRebuildTable)
            {
                Settings.FindLootRebuildTable = false;

                olvLootList.BuildList();
                Olv_ParseEntries();
            }
        }

        private void GlControl_Map_RenderObjects()
        {
            // Draw all the geometry.
            HelperOpenGL.OpenglMapText.ForEach(u =>
            {
                HelperOpenGL.GlControl_DrawText(u.structs.Text, u.structs.DrawColor, u.structs.Size, u.structs.MapPosX, u.structs.MapPosZ, u.structs.TextOutline);
            });

            HelperOpenGL.OpenglMapIcons.ForEach(u =>
            {
                HelperOpenGL.GlControl_DrawIcons(u.structs.Text, u.structs.MapPosX, u.structs.MapPosZ, u.structs.DrawColor, u.structs.Rotation);
            });

            HelperOpenGL.OpenglMapGeometry.ForEach(u =>
            {
                switch (u.structs.Text)
                {
                    case "line":
                        HelperOpenGL.GlControl_DrawLines(u.structs.Size, u.structs.MapPosX, u.structs.MapPosZ, u.structs.MapPosXend, u.structs.MapPosZend, u.structs.DrawColor);
                        break;

                    case "point":
                        HelperOpenGL.GlControl_DrawPoint(u.structs.Size, u.structs.MapPosX, u.structs.MapPosZ, u.structs.DrawColor);
                        break;

                    case "linestripple":
                        HelperOpenGL.GlControl_DrawLinesStripped(u.structs.Size, u.structs.MapPosX, u.structs.MapPosZ, u.structs.MapPosXend, u.structs.MapPosZend, u.structs.DrawColor);
                        break;

                    case "linestripple_invert":
                        HelperOpenGL.GlControl_DrawLinesStripped(u.structs.Size, u.structs.MapPosX, u.structs.MapPosZ, u.structs.MapPosXend, u.structs.MapPosZend, u.structs.DrawColor, true);
                        break;

                    case "circle":
                        HelperOpenGL.GlControl_DrawCircle(u.structs.Size, u.structs.MapPosX, u.structs.MapPosZ, u.structs.DrawColor);
                        break;

                    case "circlefill":
                        HelperOpenGL.GlControl_DrawCircleFill(u.structs.Size, u.structs.MapPosX, u.structs.MapPosZ, u.structs.DrawColor);
                        break;

                    default:
                        break;
                }
            });

            // And clear as we dont need it anymore.
            HelperOpenGL.OpenglMapGeometry.Clear();
            HelperOpenGL.OpenglMapIcons.Clear();
            HelperOpenGL.OpenglMapText.Clear();
        }

        private void GlControl_Map_PrepareTestObjects(GLControl control)
        {
            //// TODO REWORK FOR NEW CLASS FUCK ME IM LAZY
            ////int of = 0;
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("point", -(control.Width / 2) - of, (control.Height / 2) - of, 0, 0, Color.Red, 15, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("point", -(control.Width / 2) - of, -(control.Height / 2) - of, 0, 0, Color.Green, 15, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("point", (control.Width / 2) - of, -(control.Height / 2) - of, 0, 0, Color.Aqua, 15, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("point", (control.Width / 2) - of, (control.Height / 2) - of, 0, 0, Color.Orange, 15, 0, 0));
            ////
            ////var str = "x " + (-(control.Width / 2) - of) + "\ny " + ((control.Height / 2) - of);
            ////HelperOpenGL.OpenglMapText.Add(new RenderList((str), -(control.Width / 2) - of + 50, (control.Height / 2) - of - 50, 0, 0, Color.Red, (int)FontSizes.misc, 0, 0));
            ////
            ////str = "x " + (-(control.Width / 2) - of) + "\ny " + (-(control.Height / 2) - of);
            ////HelperOpenGL.OpenglMapText.Add(new RenderList((str), -(control.Width / 2) - of + 50, -(control.Height / 2) - of + 50, 0, 0, Color.Green, (int)FontSizes.misc, 0, 0));
            ////
            ////str = "x " + ((control.Width / 2) - of) + "\ny " + (-(control.Height / 2) - of);
            ////HelperOpenGL.OpenglMapText.Add(new RenderList((str), (control.Width / 2) - of - 50, -(control.Height / 2) - of + 50, 0, 0, Color.Aqua, (int)FontSizes.misc, 0, 0));
            ////
            ////str = "x " + ((control.Width / 2) - of) + "\ny " + ((control.Height / 2) - of);
            ////HelperOpenGL.OpenglMapText.Add(new RenderList((str), (control.Width / 2) - of - 50, (control.Height / 2) - of - 50, 0, 0, Color.Orange, (int)FontSizes.misc, 0, 0));
            ////
            ////
            /////* X.Y.
            ////* Left Top ( - | + )
            ////* Left Bottom ( - | - )
            ////* Right Top ( + | + )
            ////* Right Bottom ( + | - )
            ////*/
            ////
            ////var currentRoW = 1;
            ////var currentColumn = 1;
            ////var stepWidth = 100;
            ////var stepHeight = 100;
            ////
            ////var canvasW = -openglControlMap.Width / 2;
            ////var canvasH = openglControlMap.Height / 2;
            ////int entityPosXmap;
            ////int entityPosZmap;
            ////var degressFOV = (float)(DateTime.Now.Millisecond * 0.36);
            ////var lengthFOV = 100;
            ////float fov_line_X;
            ////float fov_line_Y;
            ////
            ////// ROW 1,1
            ////currentRoW = 1;
            ////currentColumn = 1;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////fov_line_Y = (float)(entityPosZmap + (Math.Sin(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("player", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorPlayerMyself, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Player", entityPosXmap + 30, entityPosZmap - 30, 0, 0, Settings.ColorPlayerMyself, (int)FontSizes.name, 0, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("weapon1\nweapon2", entityPosXmap + 30, entityPosZmap - 30 - 15, 0, 0, Settings.ColorText, (int)FontSizes.misc, 0, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList(100 + "m", entityPosXmap + 30, entityPosZmap - 30 + 15, 0, 0, Settings.ColorText, (int)FontSizes.misc, 0, 0));
            ////
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("line", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Settings.ColorPlayerMyself, 2, 0, 0));
            ////
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("circle", entityPosXmap, entityPosZmap, 0, 0, Color.FromArgb(64, Color.Red), 20 * Settings.MapZoomLevelFromZoomLevels, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("circle", entityPosXmap, entityPosZmap, 0, 0, Color.FromArgb(64, Color.Red), 30 * Settings.MapZoomLevelFromZoomLevels, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("circle", entityPosXmap, entityPosZmap, 0, 0, Color.FromArgb(64, Color.Green), 40 * Settings.MapZoomLevelFromZoomLevels, 0, 0));
            //
            ////// ROW 2,1
            ////currentRoW = 2;
            ////currentColumn = 1;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////fov_line_Y = (float)(entityPosZmap + (Math.Sin(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("player", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorPlayerFRIEND, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Friend", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorPlayerFRIEND, (int)FontSizes.name, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("line", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Settings.ColorPlayerFRIEND, 2, 0, 0));
            ////
            ////// ROW 3,1
            ////currentRoW = 3;
            ////currentColumn = 1;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////fov_line_Y = (float)(entityPosZmap + (Math.Sin(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("player", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorPlayerUSEC, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("USEC", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorPlayerUSEC, (int)FontSizes.name, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("line", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Settings.ColorPlayerUSEC, 2, 0, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("weapon1\nweapon2", entityPosXmap + 10, entityPosZmap - 16, 0, 0, Settings.ColorGenericText, (int)FontSizes.misc, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("circlefill", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Color.FromArgb(128, Color.Red), Settings.MapSizeIcon + 2, 0, 0));
            ////
            ////// ROW 4,1
            ////currentRoW = 4;
            ////currentColumn = 1;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////fov_line_Y = (float)(entityPosZmap + (Math.Sin(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("player", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorPlayerBEAR, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("BEAR", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorPlayerBEAR, (int)FontSizes.name, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("line", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Settings.ColorPlayerBEAR, 2, 0, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("weapon1\nweapon2", entityPosXmap + 10, entityPosZmap - 16, 0, 0, Settings.ColorGenericText, (int)FontSizes.misc, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("circlefill", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Color.FromArgb(128, Color.Green), Settings.MapSizeIcon + 2, 0, 0));
            ////
            ////// ROW 5,1
            ////currentRoW = 5;
            ////currentColumn = 1;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////fov_line_Y = (float)(entityPosZmap + (Math.Sin(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("player", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorPlayerSCAV, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("SCAV", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorPlayerSCAV, (int)FontSizes.name, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("line", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Settings.ColorPlayerSCAV, 2, 0, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("weapon1\nweapon2", entityPosXmap + 10, entityPosZmap - 16, 0, 0, Settings.ColorGenericText, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 6,1
            ////currentRoW = 6;
            ////currentColumn = 1;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////fov_line_X = (float)(entityPosXmap + (Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////fov_line_Y = (float)(entityPosZmap + (Math.Sin(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * lengthFOV));
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("player", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorPlayerBOT, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("BOT", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorPlayerBOT, (int)FontSizes.name, 0, 0));
            ////HelperOpenGL.OpenglMapGeometry.Add(new RenderList("line", entityPosXmap, entityPosZmap, fov_line_X, fov_line_Y, Settings.ColorPlayerBOT, 2, 0, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("weapon1\nweapon2", entityPosXmap + 10, entityPosZmap - 16, 0, 0, Settings.ColorGenericText, (int)FontSizes.misc, 0, 0));
            ////
            ////degressFOV = 0;
            ////
            ////// ROW 1,2
            ////currentRoW = 1;
            ////currentColumn = 2;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("tentbox", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorMilitaryCrate, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Military Crate\n110", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorMilitaryCrate, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 2,2
            ////currentRoW = 2;
            ////currentColumn = 2;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("tentbox", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorMilitaryCrateLong, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Military Crate\n110x45", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorMilitaryCrateLong, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 3,2
            ////currentRoW = 3;
            ////currentColumn = 2;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("tentbox", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorMilitaryCrateLong, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Military Crate\n140", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorMilitaryCrateLong, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 4,2
            ////currentRoW = 4;
            ////currentColumn = 2;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("tentbox", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorMilitaryCrate, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Military Crate\n64", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorMilitaryCrate, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 5,2
            ////currentRoW = 5;
            ////currentColumn = 2;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("tentbox", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorMilitaryCrateAmmo, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Military Crate\nAmmo", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorMilitaryCrateAmmo, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 6,2
            ////currentRoW = 6;
            ////currentColumn = 2;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("tentbox", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorMilitaryCrateGrenades, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Military Crate\nGrenades", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorMilitaryCrateGrenades, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 1,3
            ////currentRoW = 1;
            ////currentColumn = 3;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("money", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorMoneySafe, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Safe", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorMoneySafe, (int)FontSizes.misc, 0, 0));
            ////
            ////// ROW 1,3
            ////currentRoW = 1;
            ////currentColumn = 4;
            ////entityPosXmap = canvasW + (currentRoW * stepWidth);
            ////entityPosZmap = canvasH - (currentColumn * stepHeight);
            ////HelperOpenGL.OpenglMapIcons.Add(new RenderList("npc_dead", entityPosXmap, entityPosZmap, 0, 0, Settings.ColorGeneric, 0, degressFOV * -1, 0));
            ////HelperOpenGL.OpenglMapText.Add(new RenderList("Dead", entityPosXmap + 10, entityPosZmap - 5, 0, 0, Settings.ColorGenericText, (int)FontSizes.misc, 0, 0));
        }
    }
}