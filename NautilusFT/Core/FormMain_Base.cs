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

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NautilusFT
{
    internal enum IconPositionTexture
    {
        player = 1,
        player_dead,
        npc,
        npc_dead,
        vehicle,
        vehicle_broken,
        loot,
        animal,
        helicrash,
        tentbox,
        parachute,
        money,
        house,
        statictech,
        pin,
        unknown
    }

    internal enum FontSizes
    {
        name = 13,
        misc = 10,
    }

    public partial class FormMain : Form
    {

        private bool dotDo = false;

        // External Files.
        private XDocument doc;

        private System.Timers.Timer renderTimer;
        private System.Timers.Timer aggressiveTimer;

        private bool mouseFormButtonDown;

        private int mouseDeltaLastX;
        private int mouseDeltaLastY;
        private int openglControlMapMouseDragOffsetTempX;
        private int openglControlMapMouseDragOffsetTempY;

        private int openglControlMapDragOffsetX;
        private int openglControlMapDragOffsetY;
        private float myCoordForMapCenterX, myCoordForMapCenterY;

        private int windowPosX;
        private int windowPosY;
        private int windowSizeW;
        private int windowsSizeH;
        private string windowState;

        private Reader reader;
        private Overlay overlayFormNew;

        private float mapMarkerX, mapMarkerY, mapMarkerXpre, mapMarkerYpre;
        private bool mapMarkerParse = false;
        private bool mapMarkerDraw = false;
        private bool updatePointerLabels = true;

        public FormMain()
        {
            InitializeComponent();
            InitializeListView();

            if (!Settings.Standalone)
            {
                Helper.ClientReader = new ReaderServiceClient();

                ((BasicHttpBinding)Helper.ClientReader.Endpoint.Binding).MaxReceivedMessageSize = int.MaxValue;

                // Use TimeSpan constructor to specify:
                // ... Days, hours, minutes, seconds, milliseconds.
                // ... The TimeSpan returned has those values.
                ((BasicHttpBinding)Helper.ClientReader.Endpoint.Binding).OpenTimeout = new TimeSpan(0, 0, 0, 0, 500);
                ((BasicHttpBinding)Helper.ClientReader.Endpoint.Binding).CloseTimeout = new TimeSpan(0, 0, 0, 0, 500);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsSave();
        }

        private void BaseLoad(object sender, EventArgs e)
        {
            Icon = Properties.Resources.Nautilus;
            Text = Helper.GetRandomStringStrong(Helper.GetRandomNumber(1, 100));

            // Event handlers for OpelGL Controls.
            openglControlMap.Paint += new PaintEventHandler(GlControl_Map_RenderDo);
            openglControlMap.MouseDown += new MouseEventHandler(GlControl_Map_MouseDown);
            openglControlMap.MouseMove += new MouseEventHandler(GlControl_Map_MouseMove);
            openglControlMap.MouseUp += new MouseEventHandler(GlControl_Map_MouseUp);
            openglControlMap.MouseWheel += new MouseEventHandler(GlControl_Map_MouseWheel);
            openglControlMap.MouseDoubleClick += new MouseEventHandler(GlControlMapMouseDoubleClick);

            SettingsLoad();
        }

        private void DumpPlayers(object sender, EventArgs e)
        {
            Settings.DumpPlayers = true;
        }

        private void DumpLoot(object sender, EventArgs e)
        {
            Settings.DumpLoot = true;
            DeFocusElement();
        }

        private void StartButtonClick()
        {
            if (buttonStartStop.Text == "Start")
            {
                try
                {
                    if (!Settings.DemoMode)
                    {
                        reader = new Reader();
                    }

                    buttonStartStop.Text = "Stop";

                    renderTimer = new System.Timers.Timer
                    {
                        Interval = 1
                    };

                    renderTimer.Elapsed += RenderTimerTick;
                    renderTimer.AutoReset = false;
                    renderTimer.Enabled = true;
                    renderTimer.Stop();
                    renderTimer.Start();

                    aggressiveTimer = new System.Timers.Timer
                    {
                        Interval = 1
                    };

                    aggressiveTimer.Elapsed += AggressiveTimerTick;
                    aggressiveTimer.AutoReset = false;
                    aggressiveTimer.Enabled = true;
                    aggressiveTimer.Stop();
                    aggressiveTimer.Start();

                    openglControlMapCanUse = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong when pressing Start button: \n\n" + ex);
                }
            }
            else if (buttonStartStop.Text == "Stop")
            {
                try
                {
                    if (!Settings.DemoMode)
                    {
                        aggressiveTimer.Stop();
                        reader.CloseHandles();
                        reader = null;
                    }

                    buttonStartStop.Text = "Start";

                    openglControlMapCanUse = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong when pressing Start button: \n\n" + ex);
                }
            }
        }

        private void RenderOverlay()
        {
            if (overlayFormNew != null && overlayFormNew.overlayLoaded)
            {
                if (!Settings.DemoMode && overlayFormNew.CameraMatrixPointer != reader.CameraMatrixPointer)
                {
                    overlayFormNew.CameraMatrixPointer = reader.CameraMatrixPointer;
                }

                if (overlayFormNew.renderOverlayComplete)
                {
                    overlayFormNew.renderOverlayComplete = false;
                    overlayFormNew.DirectXThread();
                }
            }
        }

        private void RenderTimerTick(object source, ElapsedEventArgs e)
        {
            if (Settings.DemoMode)
            {
                // Force repaint event.
                if (renderMapComplete)
                {
                    renderMapComplete = false;

                    HelperOpenGL.GlControl_Refresh(openglControlMap);
                    RenderOverlay();
                }
            }

            ThreadHelper.SetText(this, textBoxAddressBase, reader.GameBaseAddress.ToString("X"));
            ThreadHelper.SetText(this, textBoxAddressGameWorld, reader.GameWorldPointer.ToString("X"));
            ThreadHelper.SetText(this, textBoxAddressLocalGameWorld, reader.LocalGameWorldPointer.ToString("X"));
            ThreadHelper.SetText(this, textBoxFPSCamera, reader.CameraPointer.ToString("X"));
            ThreadHelper.SetText(this, textBoxPID, Settings.GamePIDCurrent.ToString());

            if (Settings.MapCenterMap && !mouseFormButtonDown)
            {
                myCoordForMapCenterX = (int)Math.Round(Helper.myPosXingame) * Settings.MapZoomLevelFromZoomLevels * openglInvertMap;
                myCoordForMapCenterY = (int)Math.Round(Helper.myPosZingame) * Settings.MapZoomLevelFromZoomLevels * openglInvertMap;

                openglControlMapDragOffsetX = (int)Math.Round(myCoordForMapCenterX * -1);
                openglControlMapDragOffsetY = (int)Math.Round(myCoordForMapCenterY * -1);
                openglControlMapMouseDragOffsetTempX = openglControlMapDragOffsetX;
                openglControlMapMouseDragOffsetTempY = openglControlMapDragOffsetY;
            }

            // Aggressive Mode
            if (Aggressive.PlayerTargetsListEquipmentViewNeedsUpdate)
            {
                listViewEquipment.Invoke(new Action(() =>
                {
                    listViewEquipment.Items.Clear();

                    for (int i = Aggressive.PlayerTargetsDictEquipment.Count - 1; i >= 0; i--)
                    {
                        string[] listEntry = new string[3];
                        listEntry[0] = Aggressive.PlayerTargetsDictEquipment.ElementAt(i).Value;
                        listEntry[1] = Aggressive.PlayerTargetsDictEquipment.ElementAt(i).Key.ToString();
                        listEntry[2] = Aggressive.PlayerTargetsDictProfile.ElementAt(i).Key.ToString();
                        ListViewItem listItem = new ListViewItem(listEntry);
                        listViewEquipment.Items.Add(listItem);
                    }
                }));

                Aggressive.PlayerTargetsListEquipmentViewNeedsUpdate = false;
            }

            // Force repaint event.
            if (HelperOpenGL.PlayerListToRenderComplete)
            {
                HelperOpenGL.PlayerListToRenderFinal.Clear();
                HelperOpenGL.PlayerListToRenderFinal.AddRange(HelperOpenGL.PlayerListToRenderPre);
                HelperOpenGL.PlayerListToRenderComplete = false;
                if (HelperOpenGL.LootListToRenderComplete)
                {
                    HelperOpenGL.LootListToRenderFinal.Clear();
                    HelperOpenGL.LootListToRenderFinal.AddRange(HelperOpenGL.LootListToRenderPre);
                    HelperOpenGL.LootListToRenderComplete = false;
                }

                HelperOpenGL.GlControl_Refresh(openglControlMap);
                RenderOverlay();
            }

            if (buttonStartStop.Text == "Stop")
            {
                renderTimer.Start();
            }
        }

        private void AggressiveTimerTick(object source, ElapsedEventArgs e)
        {
            if (((DateTime.Now - Settings.PlayersInfoTime).Milliseconds > Settings.UpdateRate) && reader.PlayerCount != 0)
            {
                Aggressive.MoveMe();
            }

            if (buttonStartStop.Text == "Stop")
            {
                aggressiveTimer.Start();
            }
        }

        // When form is resized. It checks for even numbers, necessary for openGL Control, otherwise it can blur a little.
        private void BaseResizeEnd(object sender, EventArgs e)
        {
            var h = Size.Height;
            var w = Size.Width;

            while (Helper.IsOdd(h))
            {
                h -= 1;
            }

            while (Helper.IsOdd(w))
            {
                w -= 1;
            }

            Width = w;
            Height = h;
        }

        private void SettingsLoad()
        {
            /* SEED FOR ENCRYPTION IS , DATETIMENOW, IP ADDRESS, FREESPCE DISK C AND RANDOM NUMBERS
             * http://social.msdn.microsoft.com/Forums/vstudio/en-US/b981bdc2-8c3c-41b9-8965-b90cd3028cd6/c-30-get-loggedin-username-computername-and-ip-address?forum=csharpgeneral
             * http://msdn.microsoft.com/ru-ru/library/system.datetime.now%28v=vs.110%29.aspx
             *  migrating above to something alese as StringCipher 
             *  ////.Select(x => StringCipher.Decrypt(x.Value, Settings.Cipher).ToString()).ToList(); */

            doc = XDocument.Load(@"dataset.xml");

            //// Load Lists.
            Settings.ListTextReplace = doc.Root.Elements("TextReplace").Elements("data").ToDictionary(r => r.Attribute("old").Value, r => r.Attribute("new").Value);
            Settings.ListLootBlackList = doc.Root.Elements("LootBlackList").Elements("data").Attributes("value").Select(x => x.Value.ToString()).ToList();
            Settings.ListFriends = doc.Root.Elements("Friends").Elements("data").Attributes("value").Select(x => x.Value.ToString()).ToList();
            Settings.ListCheaters = doc.Root.Elements("Cheaters").Elements("data").ToDictionary(r => r.Attribute("accountid").Value, r => r.Attribute("name").Value);
            Settings.LootListToLookFor = doc.Root.Elements("LootItems").Elements("data").Attributes("value").Select(x => x.Value.ToString()).ToList();

            //// Main Tab - Settings.
            checkBoxCenterMap.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("CenterMap").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxDemoMode.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("DemoMode").Select(x => x.Value).SingleOrDefault()) ?? "false");
            Settings.MapCenterMap = checkBoxCenterMap.Checked;
            Settings.DemoMode = checkBoxDemoMode.Checked;

            //// Main Tab - Draw.
            checkBoxShowPlayers.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("Draw").Elements("ShowPlayers").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxShowPlayerNames.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("Draw").Elements("ShowPlayerNames").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxShowPlayerWeapons.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("Draw").Elements("ShowPlayerWeapons").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxShowPlayerSide.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("Draw").Elements("ShowPlayerSide").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxShowPlayerFOV.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("Draw").Elements("ShowPlayerFOV").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxShowPlayerHealth.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("Draw").Elements("ShowPlayerHealth").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxShowPlayerDistance.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("Draw").Elements("ShowPlayerDistance").Select(x => x.Value).SingleOrDefault()) ?? "true");

            Settings.MapShowPlayers = checkBoxShowPlayers.Checked;
            Settings.MapShowPlayerName = checkBoxShowPlayerNames.Checked;
            Settings.MapShowPlayerWeapons = checkBoxShowPlayerWeapons.Checked;
            Settings.MapShowPlayerSide = checkBoxShowPlayerSide.Checked;
            Settings.MapShowPlayerFOV = checkBoxShowPlayerFOV.Checked;
            Settings.MapShowPlayerHealth = checkBoxShowPlayerHealth.Checked;
            Settings.MapShowPlayerDistance = checkBoxShowPlayerDistance.Checked;
            Settings.FindLoot = checkBoxFindLoot.Checked;

            //// Main Tab - OSD.

            checkBoxOSDShowStats.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("OSD").Elements("ShowStats").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxOSDAzimuth.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("OSD").Elements("Azimuth").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxOSDDateTime.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("OSD").Elements("DateTime").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxOSDPlayerCount.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("OSD").Elements("PlayerCount").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxOSDFPS.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("OSD").Elements("FPS").Select(x => x.Value).SingleOrDefault()) ?? "true");
            checkBoxOSDReadCalls.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("OSD").Elements("ReadCalls").Select(x => x.Value).SingleOrDefault()) ?? "true");

            Settings.MapOSDShowStats = checkBoxOSDShowStats.Checked;
            Settings.MapOSDAzimuth = checkBoxOSDAzimuth.Checked;
            Settings.MapOSDDateTime = checkBoxOSDDateTime.Checked;
            Settings.MapOSDPlayerCount = checkBoxOSDPlayerCount.Checked;
            Settings.MapOSDFPS = checkBoxOSDFPS.Checked;
            Settings.MapOSDReadCalls = checkBoxOSDReadCalls.Checked;

            //// Other Tab.

            comboBoxRefreshRateMap.SelectedIndex = Convert.ToInt32(Convert.ToString(doc.Root.Elements("Settings").Elements("RefreshRateMap").Select(x => x.Value).SingleOrDefault()) ?? "1");
            comboBoxMap.SelectedIndex = Convert.ToInt32(Convert.ToString(doc.Root.Elements("Settings").Elements("Map").Select(x => x.Value).SingleOrDefault()) ?? "2");
            Settings.UpdateRate = Convert.ToInt32(comboBoxRefreshRateMap.SelectedItem);

            checkboxTransparent.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("WindowTransparent").Select(x => x.Value).SingleOrDefault()) ?? "false");
            checkBoxNoBorder.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("WindowNoBorder").Select(x => x.Value).SingleOrDefault()) ?? "false");
            checkBoxOnTop.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("WindowOnTop").Select(x => x.Value).SingleOrDefault()) ?? "false");
            checkBoxFullScreen.Checked = Convert.ToBoolean(Convert.ToString(doc.Root.Elements("Settings").Elements("WindowFullScreen").Select(x => x.Value).SingleOrDefault()) ?? "false");

            var mainWindowWidth = Convert.ToInt32(Convert.ToString(doc.Root.Elements("Settings").Elements("MainWindowWidth").Select(x => x.Value).SingleOrDefault()) ?? "50");
            var mainWindowHeight = Convert.ToInt32(Convert.ToString(doc.Root.Elements("Settings").Elements("MainWindowHeight").Select(x => x.Value).SingleOrDefault()) ?? "50");
            var mainWindowLocationX = Convert.ToInt32(Convert.ToString(doc.Root.Elements("Settings").Elements("MainWindowLocationX").Select(x => x.Value).SingleOrDefault()) ?? "1020");
            var mainWindowLocationY = Convert.ToInt32(Convert.ToString(doc.Root.Elements("Settings").Elements("MainWindowLocationY").Select(x => x.Value).SingleOrDefault()) ?? "699");

            Width = mainWindowWidth;
            Height = mainWindowHeight;
            Location = new Point(mainWindowLocationX, mainWindowLocationY);

            // Account ID
            Settings.AccID = doc.Root.Elements("Settings").Elements("AccountID").Select(x => x.Value).SingleOrDefault() ?? "0";

            /* Get PID and cached value from saved file, that will help us to check should we use that cache or not 
             * if PID matches current one or we need to update. */

            Settings.GamePIDCached = Convert.ToInt32(Convert.ToString(doc.Root.Elements("Settings").Elements("gamePIDCached").Select(x => x.Value).SingleOrDefault()) ?? "0");

            // TODO
            Settings.GameWorldPointerCached = (IntPtr)Convert.ToInt64(Convert.ToString(doc.Root.Elements("Settings").Elements("GameWorldPointerCached").Select(x => x.Value).SingleOrDefault()) ?? "0");
            Settings.CameraPointerCached = (IntPtr)Convert.ToInt64(Convert.ToString(doc.Root.Elements("Settings").Elements("CameraPointerCached").Select(x => x.Value).SingleOrDefault()) ?? "0");
            Settings.MapSizeIcon = Settings.MapSizesPlayerIcon[Settings.MapZoomLevelFromZoomLevels];
        }

        private void SettingsSave()
        {
            // Prepate XML Document.
            System.IO.File.Copy(@"dataset.xml", @"dataset.xml.bak", true);

            doc =
            new XDocument(
                new XElement(
                    "DataSet",
            //// Main Tab - Settings.
            new XElement(
                "Settings",
                new XElement("CenterMap", checkBoxCenterMap.Checked.ToString()),
                new XElement("DemoMode", checkBoxDemoMode.Checked.ToString()),
            //// Main Tab - Draw.
                new XElement(
                    "Draw",
                    new XElement("ShowPlayers", checkBoxShowPlayers.Checked.ToString()),
                    new XElement("ShowPlayerNames", checkBoxShowPlayerNames.Checked.ToString()),
                    new XElement("ShowPlayerWeapons", checkBoxShowPlayerWeapons.Checked.ToString()),
                    new XElement("ShowPlayerSide", checkBoxShowPlayerSide.Checked.ToString()),
                    new XElement("ShowPlayerFOV", checkBoxShowPlayerFOV.Checked.ToString()),
                    new XElement("ShowPlayerHealth", checkBoxShowPlayerHealth.Checked.ToString()),
                    new XElement("ShowDistance", checkBoxShowPlayerDistance.Checked.ToString())),
            //// Main Tab - OSD.
                new XElement(
                    "OSD",
                    new XElement("ShowStats", checkBoxOSDShowStats.Checked.ToString()),
                    new XElement("Azimuth", checkBoxOSDAzimuth.Checked.ToString()),
                    new XElement("DateTime", checkBoxOSDDateTime.Checked.ToString()),
                    new XElement("PlayerCount", checkBoxOSDPlayerCount.Checked.ToString()),
                    new XElement("FPS", checkBoxOSDFPS.Checked.ToString()),
                    new XElement("ReadCalls", checkBoxOSDReadCalls.Checked.ToString())),
            //// Other.
                new XElement("FindLoot", checkBoxFindLoot.Checked.ToString()),
                new XElement("ShowGroups", olvShowGroupsCheck.Checked.ToString()),
                new XElement("RefreshRateMap", Convert.ToInt32(comboBoxRefreshRateMap.SelectedIndex).ToString()),
                new XElement("Map", Convert.ToInt32(comboBoxMap.SelectedIndex).ToString()),
                new XElement("WindowTransparent", checkboxTransparent.Checked.ToString()),
                new XElement("WindowNoBorder", checkBoxNoBorder.Checked.ToString()),
                new XElement("WindowOnTop", checkBoxOnTop.Checked.ToString()),
                new XElement("WindowFullScreen", checkBoxFullScreen.Checked.ToString()),
                new XElement("MainWindowWidth", Size.Width.ToString()),
                new XElement("MainWindowHeight", Size.Height.ToString()),
                new XElement("MainWindowLocationX", Location.X.ToString()),
                new XElement("MainWindowLocationY", Location.Y.ToString()),
                new XElement("GameWorldPointerCached", Settings.GameWorldPointerCached.ToString()),
                new XElement("CameraPointerCached", Settings.CameraPointerCached.ToString()),
                new XElement("AccountID", Settings.AccID),
            new XElement("gamePIDCached", Settings.GamePIDCurrent.ToString())),
            new XElement(
                "TextReplace",
                Settings.ListTextReplace.Select(
                d => new XElement(
                "data",
                new XAttribute("old", d.Key),
                new XAttribute("new", d.Value)))),
            new XElement(
                "LootBlackList",
                Settings.ListLootBlackList.Select(x => new XElement("data", new XAttribute("value", x)))),
            new XElement(
                "Friends",
                Settings.ListFriends.Select(x => new XElement("data", new XAttribute("value", x)))),
            new XElement(
                "Cheaters",
                Settings.ListCheaters.Select(x => new XElement("data", new XAttribute("accountid", x.Key), new XAttribute("name", x.Value)))),
            new XElement(
                "LootItems",
                Settings.LootListToLookFor.Select(x => new XElement("data", new XAttribute("value", x))))));

            doc.Save(@"dataset.xml");
        }

        // Used for beauty, no button stays focused on click.
        private void DeFocusElement() => Focus();

        private void GlControl_Map_MouseDown(object sender, MouseEventArgs e)
        {
            mouseFormButtonDown = true;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                // Remember current mouse delta.
                mouseDeltaLastX = e.X;
                mouseDeltaLastY = e.Y;
            }
        }

        private void GlControl_Map_MouseUp(object sender, MouseEventArgs e)
        {
            mouseFormButtonDown = false;

            // Remember offset so it doesn`t jump back on next mouse click/move.
            openglControlMapMouseDragOffsetTempX = openglControlMapDragOffsetX;
            openglControlMapMouseDragOffsetTempY = openglControlMapDragOffsetY;
        }

        private void GlControl_Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseFormButtonDown)
            {
                // Get different between Last delta and current one.
                var mouseDeltaNewX = e.X - mouseDeltaLastX;
                var mouseDeltaNewY = e.Y - mouseDeltaLastY;

                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                {
                    // Adjust the map.
                    openglControlMapDragOffsetX = openglControlMapMouseDragOffsetTempX + mouseDeltaNewX;
                    openglControlMapDragOffsetY = openglControlMapMouseDragOffsetTempY - mouseDeltaNewY;
                }

                // Adjust the form position.
                if (e.Button == MouseButtons.Middle)
                {
                    ActiveForm.Location = new Point(ActiveForm.Location.X + mouseDeltaNewX, ActiveForm.Location.Y + mouseDeltaNewY);
                }
            }
        }

        private void GlControl_Map_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                Settings.MapZoomLevelFromZoomLevels = Math.Min(Settings.MapZoomLevelsMinMaxRange[1], Settings.MapZoomLevelFromZoomLevels + 1);
                Settings.MapSizeIcon = Settings.MapSizesPlayerIcon[Settings.MapZoomLevelFromZoomLevels];
            }
            else
            {
                Settings.MapZoomLevelFromZoomLevels = Math.Max(Settings.MapZoomLevelsMinMaxRange[0], Settings.MapZoomLevelFromZoomLevels - 1);
                Settings.MapSizeIcon = Settings.MapSizesPlayerIcon[Settings.MapZoomLevelFromZoomLevels];
            }

            // Get scale coefficient between zooms to alter our map offset.
            float lastZoom = Settings.MapZoomLevelFromZoomLevels;
            float coeff = Settings.MapZoomLevelFromZoomLevels / lastZoom;

            // Zoom Handling.
            if (!Settings.MapCenterMap)
            {
                openglControlMapDragOffsetX = (int)Math.Round(openglControlMapDragOffsetX * coeff);
                openglControlMapDragOffsetY = (int)Math.Round(openglControlMapDragOffsetY * coeff);
                openglControlMapMouseDragOffsetTempX = openglControlMapDragOffsetX;
                openglControlMapMouseDragOffsetTempY = openglControlMapDragOffsetY;
            }
        }

        private void GlControlMapMouseDoubleClick(object sender, MouseEventArgs e)
        {
            /// // no reason to use that for now, had some ideas to gather info around marker or select players to kill... but im lazy
            /// return;
            /// if (e.Button == MouseButtons.Right)
            /// {
            ///     mapMarkerParse = true;
            ///     mapMarkerDraw = true;
            /// 
            ///     mapMarkerXpre = (e.X - (openglControlMap.Width / 2)) - openglControlMapDragOffsetX;
            ///     mapMarkerYpre = -(e.Y - (openglControlMap.Height / 2)) - openglControlMapDragOffsetY;
            /// }
            /// 
            /// mapMarkerXpre = (e.X - (openglControlMap.Width / 2)) - openglControlMapDragOffsetX;
            /// mapMarkerYpre = -(e.Y - (openglControlMap.Height / 2)) - openglControlMapDragOffsetY;
        }

        // What happens when we click start (depends on text on button).
        private void Button_StartClick(object sender, EventArgs e)
        {
            StartButtonClick();
        }

        private void ButtonOverlay_Click(object sender, EventArgs e)
        {
            if (overlayFormNew == null)
            {
                overlayFormNew = new Overlay();
                buttonOverlay.ForeColor = Color.Fuchsia;
                overlayFormNew.Show();
            }
            else
            {
                ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                buttonOverlay.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                overlayFormNew.Close();
                overlayFormNew = null;
            }
        }

        private void Button_ShowHide_Click(object sender, EventArgs e)
        {
            {
                if (buttonShowHide.Text.Equals("Hide"))
                {
                    panelSettings.Visible = false;
                    buttonShowHide.Text = "Show";
                    openglControlMap.Location = new Point(0, openglControlMap.Location.Y);
                    openglControlMap.Size = new Size(openglControlMap.Width + panelSettings.Width, openglControlMap.Size.Height);

                    return;
                }

                // We are cloing form and app here.
                if (buttonShowHide.Text.Equals("Show"))
                {
                    panelSettings.Visible = true;
                    buttonShowHide.Text = "Hide";
                    openglControlMap.Location = new Point(panelSettings.Width, openglControlMap.Location.Y);
                    openglControlMap.Size = new Size(openglControlMap.Width - panelSettings.Width, openglControlMap.Size.Height);
                    return;
                }
            }
        }

        private void Button_Close_Click(object sender, EventArgs e) => Close();

        private void Button_Screenshot_Click(object sender, EventArgs e)
        {
            Settings.Screenshot = !Settings.Screenshot;
            if (Settings.Screenshot)
            {
                buttonScreenshot.BackColor = Color.LightGreen;
            }

            if (!Settings.Screenshot)
            {
                buttonScreenshot.BackColor = Color.LightCoral;
            }

            DeFocusElement();
        }

        private void CheckBox_CenterMap_CheckedChanged(object sender, EventArgs e) => Settings.MapCenterMap = checkBoxCenterMap.Checked;
        private void CheckBox_ShowPlayers_CheckedChanged(object sender, EventArgs e) => Settings.MapShowPlayers = checkBoxShowPlayers.Checked;
        private void CheckBox_ShowPlayerName_CheckedChanged(object sender, EventArgs e) => Settings.MapShowPlayerName = checkBoxShowPlayerNames.Checked;
        private void CheckBox_ShowPlayerWeapons_CheckedChanged(object sender, EventArgs e) => Settings.MapShowPlayerWeapons = checkBoxShowPlayerWeapons.Checked;
        private void CheckBox_ShowPlayerSide_CheckedChanged(object sender, EventArgs e) => Settings.MapShowPlayerSide = checkBoxShowPlayerSide.Checked;
        private void CheckBox_ShowPlayerHealth_CheckedChanged(object sender, EventArgs e) => Settings.MapShowPlayerHealth = checkBoxShowPlayerHealth.Checked;
        private void CheckBox_ShowPlayerFOV_CheckedChanged(object sender, EventArgs e) => Settings.MapShowPlayerFOV = checkBoxShowPlayerFOV.Checked;
        private void CheckBox_ShowPlayerDistance_CheckedChanged(object sender, EventArgs e) => Settings.MapShowPlayerDistance = checkBoxShowPlayerDistance.Checked;
        private void CheckBox_FindLoot_CheckedChanged(object sender, EventArgs e) => Settings.FindLoot = checkBoxFindLoot.Checked;
        private void Checkbox_Transparent_CheckedChanged(object sender, EventArgs e) => Opacity = checkboxTransparent.Checked ? (float)5 / 10 : 1;

        private void CheckBox_NoBorder_CheckedChanged(object sender, EventArgs e)
        {
            var windowssize_x_noborder_tmp = Width;
            var windowssize_y_noborder_tmp = Height;

            FormBorderStyle = checkBoxNoBorder.Checked ? System.Windows.Forms.FormBorderStyle.None : System.Windows.Forms.FormBorderStyle.Sizable;

            windowssize_x_noborder_tmp = windowssize_x_noborder_tmp - Width;
            windowssize_y_noborder_tmp = windowssize_y_noborder_tmp - Height;

            Width = Width + windowssize_x_noborder_tmp;
            Height = Height + windowssize_y_noborder_tmp;
        }

        private void CheckBox_OnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnTop.Checked)
            {
                TopMost = true;
                BringToFront();
            }
            else
            {
                TopMost = false;
            }
        }

        private void CheckBox_FullScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFullScreen.Checked)
            {
                if (!checkBoxNoBorder.Checked)
                {
                    FormBorderStyle = FormBorderStyle.None;
                }

                windowSizeW = Size.Width;
                windowsSizeH = Size.Height;
                windowPosX = Location.X;
                windowPosY = Location.Y;

                windowState = WindowState.ToString();
                WindowState = FormWindowState.Normal;

                Location = new Point(Screen.FromControl(this).Bounds.Left, Screen.FromControl(this).Bounds.Top);

                Width = Screen.FromControl(this).Bounds.Width;
                Height = Screen.FromControl(this).Bounds.Height;
            }
            else
            {
                if (!checkBoxNoBorder.Checked)
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                }

                Location = new Point(windowPosX, windowPosY);
                Width = windowSizeW;
                Height = windowsSizeH;
            }
        }

        private void CheckBox_DemoMode_CheckedChanged(object sender, EventArgs e) => Settings.DemoMode = checkBoxDemoMode.Checked;
        private void ComboBox_RefreshRateMap_SelectedIndexChanged(object sender, EventArgs e) => Settings.UpdateRate = Convert.ToInt32(comboBoxRefreshRateMap.SelectedItem);
        private void ComboBox_Map_SelectedIndexChanged(object sender, EventArgs e) => Settings.CurrentMapSwitchTo = comboBoxMap.SelectedItem.ToString();
        private void CheckBoxOSDReadCalls_CheckedChanged(object sender, EventArgs e) => Settings.MapOSDReadCalls = checkBoxOSDReadCalls.Checked;
        private void CheckBoxOSDAzimuth_CheckedChanged(object sender, EventArgs e) => Settings.MapOSDAzimuth = checkBoxOSDAzimuth.Checked;
        private void CheckBoxOSDDateTime_CheckedChanged(object sender, EventArgs e) => Settings.MapOSDDateTime = checkBoxOSDDateTime.Checked;
        private void CheckBoxOSDPlayerCount_CheckedChanged(object sender, EventArgs e) => Settings.MapOSDPlayerCount = checkBoxOSDPlayerCount.Checked;
        private void CheckBoxOSDShowStats_CheckedChanged(object sender, EventArgs e) => Settings.MapOSDShowStats = checkBoxOSDShowStats.Checked;

        private void ModListClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStripModListView.Show(Cursor.Position);
            }
        }

        private void BlacklistLootItem(object sender, EventArgs e)
        {
            // TODO
            /// if (olvLootList.SelectedIndices.Count > 0)
            /// {
            ///     for (int i = 0; i < olvLootList.SelectedIndices.Count; i++)
            ///     {
            ///         var selectedIndex = olvLootList.SelectedIndices[i];
            ///         var selectedSubItem = olvLootList.GetSubItem(selectedIndex, columnEnabled.DisplayIndex);
            ///         var idx = olvLootList.FindIndex(x => x.UID.Contains(modListView.GetSubItem(selectedIndex, columnUID.DisplayIndex).Text));
            ///     }
            /// 
            /// }
        }

        private void ButtonEquipmentUpdate_Click(object sender, EventArgs e)
        {
            Aggressive.PlayerTargetsDictionaryEquipmentNeedsUpdate = true;
        }

        private void ListViewEquipmentMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 1)
            {
                try
                {
                    Aggressive.EquipmentValueToMap = Convert.ToInt64(listViewEquipment.SelectedItems[0].SubItems[1].Text);
                    Aggressive.ProfileValueToMap = Convert.ToInt64(listViewEquipment.SelectedItems[0].SubItems[2].Text);

                    Aggressive.EquipmentWriteDo = true;

                    labelEquipment.BackColor = Color.LimeGreen;
                    labelEquipment.Text = listViewEquipment.SelectedItems[0].SubItems[0].Text;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void ButtonEquipmentRevert_Click(object sender, EventArgs e)
        {
        }

        private void ButtonMoveForward_Click(object sender, EventArgs e)
        {
            if (!Aggressive.MoveDo)
            {
                Aggressive.MoveDo = true;
                Aggressive.MoveDirection = (int)Aggressive.Movements.Forward;
            }
        }

        private void ButtonMoveLeft_Click(object sender, EventArgs e)
        {
            if (!Aggressive.MoveDo)
            {
                Aggressive.MoveDo = true;
                Aggressive.MoveDirection = (int)Aggressive.Movements.Left;
            }
        }

        private void ButtonMoveRight_Click(object sender, EventArgs e)
        {
            if (!Aggressive.MoveDo)
            {
                Aggressive.MoveDo = true;
                Aggressive.MoveDirection = (int)Aggressive.Movements.Right;
            }
        }

        private void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            if (!Aggressive.MoveDo)
            {
                Aggressive.MoveDo = true;
                Aggressive.MoveDirection = (int)Aggressive.Movements.Down;
            }
        }

        private void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            if (!Aggressive.MoveDo)
            {
                Aggressive.MoveDo = true;
                Aggressive.MoveDirection = (int)Aggressive.Movements.Up;
            }
        }

        private void ButtonMoveBackward_Click(object sender, EventArgs e)
        {
            if (!Aggressive.MoveDo)
            {
                Aggressive.MoveDo = true;
                Aggressive.MoveDirection = (int)Aggressive.Movements.Backward;
            }
        }

        private void Button_Dot(object sender, EventArgs e)
        {
            if (dotDo)
            {
                ButtonDotDraw.Text = "Stop";
                dotDo = false;
            }
            else
            {
                ButtonDotDraw.Text = "DOT";
                dotDo = true;
            }
        }

        private void ButtonNoRecoil_Click(object sender, EventArgs e)
        {
            if (Aggressive.NoRecoil)
            {
                Aggressive.NoRecoil = false;
                Aggressive.NoRecoilDo = true;
                buttonNoRecoil.BackColor = Color.LightCoral;
            }
            else
            {
                Aggressive.NoRecoil = true;
                Aggressive.NoRecoilDo = true;
                buttonNoRecoil.BackColor = Color.LightGreen;
            }
        }

        private void ButtonTravel_Click(object sender, EventArgs e)
        {
            if (Aggressive.MoveKeepDoing)
            {
                Aggressive.MoveKeepDoing = false;
                buttonTravel.BackColor = Color.LightCoral;
            }
            else
            {
                Aggressive.MoveKeepDoing = true;
                buttonTravel.BackColor = Color.LightGreen;
            }
        }
        
        private void ButtonBecomeMod_Click(object sender, EventArgs e)
        {
            Aggressive.MemberCategoryDoMod = true;
            Aggressive.MemberCategoryDo = true;
        }

        private void ButtonBecomeDev_Click(object sender, EventArgs e)
        {
            Aggressive.MemberCategoryDoDev = true;
            Aggressive.MemberCategoryDo = true;
        }

        private void ButtonBecomeDef_Click(object sender, EventArgs e)
        {
            Aggressive.MemberCategoryDoDefault = true;
            Aggressive.MemberCategoryDo = true;
        }

        private void ButtonFakeMe(object sender, EventArgs e)
        {
            Aggressive.FakeMeDo = true;
        }

        private void CheckBoxFPS_CheckedChanged(object sender, EventArgs e) => Settings.MapOSDFPS = checkBoxOSDFPS.Checked;

        private void RadioButtonMirror_CheckedChanged(object sender, EventArgs e) => Settings.OverlayMirror = true;

        private void RadioButtonESP_CheckedChanged(object sender, EventArgs e) => Settings.OverlayMirror = false;
    }
}