#region License
// ====================================================
// NautilusFT Project by shaliuno.
// This program comes with ABSOLUTELY NO WARRANTY; This is free software,
// and you are welcome to redistribute it under certain conditions; See
// file LICENSE, which is part of this source code package, for details.
// ====================================================
#endregion

namespace NautilusFT
{
    public partial class FormMain
    {
        public System.Windows.Forms.Label labelSearch;

        public OpenTK.GLControl openglControlMap;

        private System.Windows.Forms.Button buttonClose;

        private System.Windows.Forms.Button buttonLootListClear;

        private System.Windows.Forms.Button buttonLootListClearAll;

        private System.Windows.Forms.Button buttonShowHide;

        private System.Windows.Forms.Button buttonStartStop;

        private System.Windows.Forms.CheckBox checkBoxFindLoot;

        private System.Windows.Forms.CheckBox checkBoxShowPlayers;

        private System.Windows.Forms.CheckBox checkBoxCenterMap;

        private System.Windows.Forms.CheckBox checkBoxShowPlayerDistance;

        private System.Windows.Forms.CheckBox checkBoxShowPlayerFOV;

        private System.Windows.Forms.CheckBox checkBoxShowPlayerNames;

        private System.Windows.Forms.CheckBox checkBoxShowPlayerSide;

        private System.Windows.Forms.CheckBox checkBoxShowPlayerWeapons;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private BrightIdeasSoftware.OLVColumn columnName;
        private BrightIdeasSoftware.ObjectListView olvLootList;
        private System.Windows.Forms.CheckBox olvShowGroupsCheck;
        private System.Windows.Forms.Panel panelSettings;
        private BrightIdeasSoftware.OLVColumn columnEnabled;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageLoot;
        private System.Windows.Forms.TabPage tabPageOther;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.CheckBox checkBoxDemoMode;
        private System.Windows.Forms.CheckBox checkBoxShowPlayerHealth;
        private System.Windows.Forms.GroupBox groupBoxOSD;
        private System.Windows.Forms.CheckBox checkBoxOSDFPS;
        private System.Windows.Forms.CheckBox checkBoxOSDReadCalls;
        private System.Windows.Forms.CheckBox checkBoxOSDShowStats;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.GroupBox groupBoxDraw;
        private System.Windows.Forms.CheckBox checkBoxOSDAzimuth;
        private System.Windows.Forms.CheckBox checkBoxOSDDateTime;
        private System.Windows.Forms.CheckBox checkBoxOSDPlayerCount;

        #region Windows Form Designer generated code

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.buttonShowHide = new System.Windows.Forms.Button();
            this.openglControlMap = new OpenTK.GLControl();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBoxOSD = new System.Windows.Forms.GroupBox();
            this.checkBoxOSDFPS = new System.Windows.Forms.CheckBox();
            this.checkBoxOSDReadCalls = new System.Windows.Forms.CheckBox();
            this.checkBoxOSDPlayerCount = new System.Windows.Forms.CheckBox();
            this.checkBoxOSDDateTime = new System.Windows.Forms.CheckBox();
            this.checkBoxOSDAzimuth = new System.Windows.Forms.CheckBox();
            this.checkBoxOSDShowStats = new System.Windows.Forms.CheckBox();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxDemoMode = new System.Windows.Forms.CheckBox();
            this.checkBoxCenterMap = new System.Windows.Forms.CheckBox();
            this.groupBoxDraw = new System.Windows.Forms.GroupBox();
            this.checkBoxShowPlayers = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPlayerWeapons = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPlayerSide = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPlayerDistance = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPlayerNames = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPlayerHealth = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPlayerFOV = new System.Windows.Forms.CheckBox();
            this.tabPageLoot = new System.Windows.Forms.TabPage();
            this.olvShowGroupsCheck = new System.Windows.Forms.CheckBox();
            this.olvLootList = new BrightIdeasSoftware.ObjectListView();
            this.columnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnOriginalName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnEnabled = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.checkBoxFindLoot = new System.Windows.Forms.CheckBox();
            this.buttonLootListClearAll = new System.Windows.Forms.Button();
            this.buttonLootListClear = new System.Windows.Forms.Button();
            this.labelSearch = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.tabPageOther = new System.Windows.Forms.TabPage();
            this.ButtonDotDraw = new System.Windows.Forms.Button();
            this.groupBoxDebug = new System.Windows.Forms.GroupBox();
            this.labelAddressBase = new System.Windows.Forms.Label();
            this.textBoxAddressGameWorld = new System.Windows.Forms.TextBox();
            this.labelAddressLocalGameWorld = new System.Windows.Forms.Label();
            this.labelAddressGameworld = new System.Windows.Forms.Label();
            this.textBoxAddressBase = new System.Windows.Forms.TextBox();
            this.labelFPSCamera = new System.Windows.Forms.Label();
            this.labelPID = new System.Windows.Forms.Label();
            this.textBoxPID = new System.Windows.Forms.TextBox();
            this.textBoxAddressLocalGameWorld = new System.Windows.Forms.TextBox();
            this.textBoxFPSCamera = new System.Windows.Forms.TextBox();
            this.buttonScreenshot = new System.Windows.Forms.Button();
            this.labelMap = new System.Windows.Forms.Label();
            this.labelRefreshRate = new System.Windows.Forms.Label();
            this.buttonDumpLoot = new System.Windows.Forms.Button();
            this.buttonDumpPlayers = new System.Windows.Forms.Button();
            this.comboBoxMap = new System.Windows.Forms.ComboBox();
            this.checkBoxFullScreen = new System.Windows.Forms.CheckBox();
            this.checkBoxOnTop = new System.Windows.Forms.CheckBox();
            this.checkBoxNoBorder = new System.Windows.Forms.CheckBox();
            this.checkboxTransparent = new System.Windows.Forms.CheckBox();
            this.comboBoxRefreshRateMap = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonBecomeDef = new System.Windows.Forms.Button();
            this.buttonBecomeDev = new System.Windows.Forms.Button();
            this.buttonBecomeMod = new System.Windows.Forms.Button();
            this.labelMyPosition = new System.Windows.Forms.Label();
            this.textBoxMyPosition = new System.Windows.Forms.TextBox();
            this.labelEquipment = new System.Windows.Forms.Label();
            this.buttonEquipmentUpdate = new System.Windows.Forms.Button();
            this.listViewEquipment = new System.Windows.Forms.ListView();
            this.columnEqName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEqID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProfileID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonNoRecoil = new System.Windows.Forms.Button();
            this.buttonOverlay = new System.Windows.Forms.Button();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.contextMenuStripModListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.blackListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonMoveZplus = new System.Windows.Forms.Button();
            this.buttonMoveZminus = new System.Windows.Forms.Button();
            this.buttonYminus = new System.Windows.Forms.Button();
            this.buttonMoveForward = new System.Windows.Forms.Button();
            this.buttonMoveLeft = new System.Windows.Forms.Button();
            this.buttonMoveRight = new System.Windows.Forms.Button();
            this.buttonTravel = new System.Windows.Forms.Button();
            this.tabControlSettings.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupBoxOSD.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            this.groupBoxDraw.SuspendLayout();
            this.tabPageLoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvLootList)).BeginInit();
            this.tabPageOther.SuspendLayout();
            this.groupBoxDebug.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panelSettings.SuspendLayout();
            this.contextMenuStripModListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonClose.ForeColor = System.Drawing.Color.Red;
            this.buttonClose.Location = new System.Drawing.Point(113, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(54, 23);
            this.buttonClose.TabIndex = 93;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.Button_Close_Click);
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStartStop.ForeColor = System.Drawing.Color.Green;
            this.buttonStartStop.Location = new System.Drawing.Point(58, 3);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(54, 23);
            this.buttonStartStop.TabIndex = 92;
            this.buttonStartStop.Text = "Start";
            this.buttonStartStop.UseVisualStyleBackColor = true;
            this.buttonStartStop.Click += new System.EventHandler(this.Button_StartClick);
            // 
            // buttonShowHide
            // 
            this.buttonShowHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShowHide.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonShowHide.Location = new System.Drawing.Point(3, 3);
            this.buttonShowHide.Name = "buttonShowHide";
            this.buttonShowHide.Size = new System.Drawing.Size(54, 23);
            this.buttonShowHide.TabIndex = 91;
            this.buttonShowHide.Text = "Hide";
            this.buttonShowHide.UseVisualStyleBackColor = true;
            this.buttonShowHide.Click += new System.EventHandler(this.Button_ShowHide_Click);
            // 
            // openglControlMap
            // 
            this.openglControlMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openglControlMap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.openglControlMap.BackColor = System.Drawing.Color.Turquoise;
            this.openglControlMap.Location = new System.Drawing.Point(344, 0);
            this.openglControlMap.Margin = new System.Windows.Forms.Padding(0);
            this.openglControlMap.Name = "openglControlMap";
            this.openglControlMap.Size = new System.Drawing.Size(660, 660);
            this.openglControlMap.TabIndex = 30;
            this.openglControlMap.VSync = false;
            this.openglControlMap.Load += new System.EventHandler(this.GlControl_Map_Load);
            this.openglControlMap.Resize += new System.EventHandler(this.GlControl_Map_Resize);
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControlSettings.Controls.Add(this.tabPageSettings);
            this.tabControlSettings.Controls.Add(this.tabPageLoot);
            this.tabControlSettings.Controls.Add(this.tabPageOther);
            this.tabControlSettings.Controls.Add(this.tabPage1);
            this.tabControlSettings.Location = new System.Drawing.Point(1, 27);
            this.tabControlSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlSettings.Multiline = true;
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.Padding = new System.Drawing.Point(0, 0);
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(344, 639);
            this.tabControlSettings.TabIndex = 1;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPageSettings.Controls.Add(this.groupBoxOSD);
            this.tabPageSettings.Controls.Add(this.groupBoxSettings);
            this.tabPageSettings.Controls.Add(this.groupBoxDraw);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Size = new System.Drawing.Size(336, 613);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Main";
            // 
            // groupBoxOSD
            // 
            this.groupBoxOSD.Controls.Add(this.checkBoxOSDFPS);
            this.groupBoxOSD.Controls.Add(this.checkBoxOSDReadCalls);
            this.groupBoxOSD.Controls.Add(this.checkBoxOSDPlayerCount);
            this.groupBoxOSD.Controls.Add(this.checkBoxOSDDateTime);
            this.groupBoxOSD.Controls.Add(this.checkBoxOSDAzimuth);
            this.groupBoxOSD.Controls.Add(this.checkBoxOSDShowStats);
            this.groupBoxOSD.Location = new System.Drawing.Point(7, 169);
            this.groupBoxOSD.Name = "groupBoxOSD";
            this.groupBoxOSD.Size = new System.Drawing.Size(155, 160);
            this.groupBoxOSD.TabIndex = 44;
            this.groupBoxOSD.TabStop = false;
            this.groupBoxOSD.Text = "OSD";
            // 
            // checkBoxOSDFPS
            // 
            this.checkBoxOSDFPS.AutoSize = true;
            this.checkBoxOSDFPS.Location = new System.Drawing.Point(6, 99);
            this.checkBoxOSDFPS.Name = "checkBoxOSDFPS";
            this.checkBoxOSDFPS.Size = new System.Drawing.Size(46, 17);
            this.checkBoxOSDFPS.TabIndex = 0;
            this.checkBoxOSDFPS.Text = "FPS";
            this.checkBoxOSDFPS.UseVisualStyleBackColor = true;
            this.checkBoxOSDFPS.CheckedChanged += new System.EventHandler(this.CheckBoxFPS_CheckedChanged);
            // 
            // checkBoxOSDReadCalls
            // 
            this.checkBoxOSDReadCalls.AutoSize = true;
            this.checkBoxOSDReadCalls.Location = new System.Drawing.Point(6, 115);
            this.checkBoxOSDReadCalls.Name = "checkBoxOSDReadCalls";
            this.checkBoxOSDReadCalls.Size = new System.Drawing.Size(77, 17);
            this.checkBoxOSDReadCalls.TabIndex = 0;
            this.checkBoxOSDReadCalls.Text = "Read Calls";
            this.checkBoxOSDReadCalls.UseVisualStyleBackColor = true;
            this.checkBoxOSDReadCalls.CheckedChanged += new System.EventHandler(this.CheckBoxOSDReadCalls_CheckedChanged);
            // 
            // checkBoxOSDPlayerCount
            // 
            this.checkBoxOSDPlayerCount.AutoSize = true;
            this.checkBoxOSDPlayerCount.Location = new System.Drawing.Point(6, 83);
            this.checkBoxOSDPlayerCount.Name = "checkBoxOSDPlayerCount";
            this.checkBoxOSDPlayerCount.Size = new System.Drawing.Size(86, 17);
            this.checkBoxOSDPlayerCount.TabIndex = 4;
            this.checkBoxOSDPlayerCount.Text = "Player Count";
            this.checkBoxOSDPlayerCount.UseVisualStyleBackColor = true;
            this.checkBoxOSDPlayerCount.CheckedChanged += new System.EventHandler(this.CheckBoxOSDPlayerCount_CheckedChanged);
            // 
            // checkBoxOSDDateTime
            // 
            this.checkBoxOSDDateTime.AutoSize = true;
            this.checkBoxOSDDateTime.Location = new System.Drawing.Point(6, 67);
            this.checkBoxOSDDateTime.Name = "checkBoxOSDDateTime";
            this.checkBoxOSDDateTime.Size = new System.Drawing.Size(72, 17);
            this.checkBoxOSDDateTime.TabIndex = 3;
            this.checkBoxOSDDateTime.Text = "DateTime";
            this.checkBoxOSDDateTime.UseVisualStyleBackColor = true;
            this.checkBoxOSDDateTime.CheckedChanged += new System.EventHandler(this.CheckBoxOSDDateTime_CheckedChanged);
            // 
            // checkBoxOSDAzimuth
            // 
            this.checkBoxOSDAzimuth.AutoSize = true;
            this.checkBoxOSDAzimuth.Location = new System.Drawing.Point(6, 51);
            this.checkBoxOSDAzimuth.Name = "checkBoxOSDAzimuth";
            this.checkBoxOSDAzimuth.Size = new System.Drawing.Size(63, 17);
            this.checkBoxOSDAzimuth.TabIndex = 2;
            this.checkBoxOSDAzimuth.Text = "Azimuth";
            this.checkBoxOSDAzimuth.UseVisualStyleBackColor = true;
            this.checkBoxOSDAzimuth.CheckedChanged += new System.EventHandler(this.CheckBoxOSDAzimuth_CheckedChanged);
            // 
            // checkBoxOSDShowStats
            // 
            this.checkBoxOSDShowStats.AutoSize = true;
            this.checkBoxOSDShowStats.Checked = true;
            this.checkBoxOSDShowStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOSDShowStats.Location = new System.Drawing.Point(6, 19);
            this.checkBoxOSDShowStats.Name = "checkBoxOSDShowStats";
            this.checkBoxOSDShowStats.Size = new System.Drawing.Size(80, 17);
            this.checkBoxOSDShowStats.TabIndex = 0;
            this.checkBoxOSDShowStats.Text = "Show Stats";
            this.checkBoxOSDShowStats.UseVisualStyleBackColor = true;
            this.checkBoxOSDShowStats.CheckedChanged += new System.EventHandler(this.CheckBoxOSDShowStats_CheckedChanged);
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.checkBoxDemoMode);
            this.groupBoxSettings.Controls.Add(this.checkBoxCenterMap);
            this.groupBoxSettings.Location = new System.Drawing.Point(174, 3);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(155, 160);
            this.groupBoxSettings.TabIndex = 43;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Settings";
            // 
            // checkBoxDemoMode
            // 
            this.checkBoxDemoMode.AutoSize = true;
            this.checkBoxDemoMode.Location = new System.Drawing.Point(6, 51);
            this.checkBoxDemoMode.Name = "checkBoxDemoMode";
            this.checkBoxDemoMode.Size = new System.Drawing.Size(81, 17);
            this.checkBoxDemoMode.TabIndex = 0;
            this.checkBoxDemoMode.Text = "DemoMode";
            this.checkBoxDemoMode.UseVisualStyleBackColor = true;
            this.checkBoxDemoMode.CheckedChanged += new System.EventHandler(this.CheckBox_DemoMode_CheckedChanged);
            // 
            // checkBoxCenterMap
            // 
            this.checkBoxCenterMap.AutoSize = true;
            this.checkBoxCenterMap.Checked = true;
            this.checkBoxCenterMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCenterMap.Location = new System.Drawing.Point(6, 19);
            this.checkBoxCenterMap.Name = "checkBoxCenterMap";
            this.checkBoxCenterMap.Size = new System.Drawing.Size(81, 17);
            this.checkBoxCenterMap.TabIndex = 0;
            this.checkBoxCenterMap.Text = "Center Map";
            this.checkBoxCenterMap.UseVisualStyleBackColor = true;
            this.checkBoxCenterMap.CheckedChanged += new System.EventHandler(this.CheckBox_CenterMap_CheckedChanged);
            // 
            // groupBoxDraw
            // 
            this.groupBoxDraw.Controls.Add(this.checkBoxShowPlayers);
            this.groupBoxDraw.Controls.Add(this.checkBoxShowPlayerWeapons);
            this.groupBoxDraw.Controls.Add(this.checkBoxShowPlayerSide);
            this.groupBoxDraw.Controls.Add(this.checkBoxShowPlayerDistance);
            this.groupBoxDraw.Controls.Add(this.checkBoxShowPlayerNames);
            this.groupBoxDraw.Controls.Add(this.checkBoxShowPlayerHealth);
            this.groupBoxDraw.Controls.Add(this.checkBoxShowPlayerFOV);
            this.groupBoxDraw.Location = new System.Drawing.Point(7, 3);
            this.groupBoxDraw.Name = "groupBoxDraw";
            this.groupBoxDraw.Size = new System.Drawing.Size(155, 160);
            this.groupBoxDraw.TabIndex = 42;
            this.groupBoxDraw.TabStop = false;
            this.groupBoxDraw.Text = "Draw";
            // 
            // checkBoxShowPlayers
            // 
            this.checkBoxShowPlayers.AutoSize = true;
            this.checkBoxShowPlayers.Checked = true;
            this.checkBoxShowPlayers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxShowPlayers.Location = new System.Drawing.Point(6, 19);
            this.checkBoxShowPlayers.Name = "checkBoxShowPlayers";
            this.checkBoxShowPlayers.Size = new System.Drawing.Size(67, 17);
            this.checkBoxShowPlayers.TabIndex = 2;
            this.checkBoxShowPlayers.Text = "Players";
            this.checkBoxShowPlayers.UseVisualStyleBackColor = true;
            this.checkBoxShowPlayers.CheckedChanged += new System.EventHandler(this.CheckBox_ShowPlayers_CheckedChanged);
            // 
            // checkBoxShowPlayerWeapons
            // 
            this.checkBoxShowPlayerWeapons.AutoSize = true;
            this.checkBoxShowPlayerWeapons.Checked = true;
            this.checkBoxShowPlayerWeapons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPlayerWeapons.Location = new System.Drawing.Point(6, 51);
            this.checkBoxShowPlayerWeapons.Name = "checkBoxShowPlayerWeapons";
            this.checkBoxShowPlayerWeapons.Size = new System.Drawing.Size(84, 17);
            this.checkBoxShowPlayerWeapons.TabIndex = 2;
            this.checkBoxShowPlayerWeapons.Text = "--- Weapons";
            this.checkBoxShowPlayerWeapons.UseVisualStyleBackColor = true;
            this.checkBoxShowPlayerWeapons.CheckedChanged += new System.EventHandler(this.CheckBox_ShowPlayerWeapons_CheckedChanged);
            // 
            // checkBoxShowPlayerSide
            // 
            this.checkBoxShowPlayerSide.AutoSize = true;
            this.checkBoxShowPlayerSide.Checked = true;
            this.checkBoxShowPlayerSide.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPlayerSide.Location = new System.Drawing.Point(6, 67);
            this.checkBoxShowPlayerSide.Name = "checkBoxShowPlayerSide";
            this.checkBoxShowPlayerSide.Size = new System.Drawing.Size(59, 17);
            this.checkBoxShowPlayerSide.TabIndex = 2;
            this.checkBoxShowPlayerSide.Text = "--- Side";
            this.checkBoxShowPlayerSide.UseVisualStyleBackColor = true;
            this.checkBoxShowPlayerSide.CheckedChanged += new System.EventHandler(this.CheckBox_ShowPlayerSide_CheckedChanged);
            // 
            // checkBoxShowPlayerDistance
            // 
            this.checkBoxShowPlayerDistance.AutoSize = true;
            this.checkBoxShowPlayerDistance.Checked = true;
            this.checkBoxShowPlayerDistance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPlayerDistance.Location = new System.Drawing.Point(6, 115);
            this.checkBoxShowPlayerDistance.Name = "checkBoxShowPlayerDistance";
            this.checkBoxShowPlayerDistance.Size = new System.Drawing.Size(80, 17);
            this.checkBoxShowPlayerDistance.TabIndex = 40;
            this.checkBoxShowPlayerDistance.Text = "--- Distance";
            this.checkBoxShowPlayerDistance.UseVisualStyleBackColor = true;
            this.checkBoxShowPlayerDistance.CheckedChanged += new System.EventHandler(this.CheckBox_ShowPlayerDistance_CheckedChanged);
            // 
            // checkBoxShowPlayerNames
            // 
            this.checkBoxShowPlayerNames.AutoSize = true;
            this.checkBoxShowPlayerNames.Checked = true;
            this.checkBoxShowPlayerNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPlayerNames.Location = new System.Drawing.Point(6, 35);
            this.checkBoxShowPlayerNames.Name = "checkBoxShowPlayerNames";
            this.checkBoxShowPlayerNames.Size = new System.Drawing.Size(71, 17);
            this.checkBoxShowPlayerNames.TabIndex = 2;
            this.checkBoxShowPlayerNames.Text = "--- Names";
            this.checkBoxShowPlayerNames.UseVisualStyleBackColor = true;
            this.checkBoxShowPlayerNames.CheckedChanged += new System.EventHandler(this.CheckBox_ShowPlayerName_CheckedChanged);
            // 
            // checkBoxShowPlayerHealth
            // 
            this.checkBoxShowPlayerHealth.AutoSize = true;
            this.checkBoxShowPlayerHealth.Checked = true;
            this.checkBoxShowPlayerHealth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPlayerHealth.Location = new System.Drawing.Point(6, 99);
            this.checkBoxShowPlayerHealth.Name = "checkBoxShowPlayerHealth";
            this.checkBoxShowPlayerHealth.Size = new System.Drawing.Size(69, 17);
            this.checkBoxShowPlayerHealth.TabIndex = 39;
            this.checkBoxShowPlayerHealth.Text = "--- Health";
            this.checkBoxShowPlayerHealth.UseVisualStyleBackColor = true;
            this.checkBoxShowPlayerHealth.CheckedChanged += new System.EventHandler(this.CheckBox_ShowPlayerHealth_CheckedChanged);
            // 
            // checkBoxShowPlayerFOV
            // 
            this.checkBoxShowPlayerFOV.AutoSize = true;
            this.checkBoxShowPlayerFOV.Checked = true;
            this.checkBoxShowPlayerFOV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPlayerFOV.Location = new System.Drawing.Point(6, 83);
            this.checkBoxShowPlayerFOV.Name = "checkBoxShowPlayerFOV";
            this.checkBoxShowPlayerFOV.Size = new System.Drawing.Size(59, 17);
            this.checkBoxShowPlayerFOV.TabIndex = 39;
            this.checkBoxShowPlayerFOV.Text = "--- FOV";
            this.checkBoxShowPlayerFOV.UseVisualStyleBackColor = true;
            this.checkBoxShowPlayerFOV.CheckedChanged += new System.EventHandler(this.CheckBox_ShowPlayerFOV_CheckedChanged);
            // 
            // tabPageLoot
            // 
            this.tabPageLoot.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageLoot.Controls.Add(this.olvShowGroupsCheck);
            this.tabPageLoot.Controls.Add(this.olvLootList);
            this.tabPageLoot.Controls.Add(this.checkBoxFindLoot);
            this.tabPageLoot.Controls.Add(this.buttonLootListClearAll);
            this.tabPageLoot.Controls.Add(this.buttonLootListClear);
            this.tabPageLoot.Controls.Add(this.labelSearch);
            this.tabPageLoot.Controls.Add(this.textBoxSearch);
            this.tabPageLoot.Location = new System.Drawing.Point(4, 22);
            this.tabPageLoot.Name = "tabPageLoot";
            this.tabPageLoot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLoot.Size = new System.Drawing.Size(336, 613);
            this.tabPageLoot.TabIndex = 5;
            this.tabPageLoot.Text = "Loot";
            // 
            // olvShowGroupsCheck
            // 
            this.olvShowGroupsCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olvShowGroupsCheck.AutoSize = true;
            this.olvShowGroupsCheck.Enabled = false;
            this.olvShowGroupsCheck.Location = new System.Drawing.Point(4, 19);
            this.olvShowGroupsCheck.Name = "olvShowGroupsCheck";
            this.olvShowGroupsCheck.Size = new System.Drawing.Size(90, 17);
            this.olvShowGroupsCheck.TabIndex = 38;
            this.olvShowGroupsCheck.Text = "Show Groups";
            this.olvShowGroupsCheck.UseVisualStyleBackColor = true;
            this.olvShowGroupsCheck.CheckedChanged += new System.EventHandler(this.Olv_ShowGroupsCheck_CheckedChanged);
            // 
            // olvLootList
            // 
            this.olvLootList.AllColumns.Add(this.columnName);
            this.olvLootList.AllColumns.Add(this.columnOriginalName);
            this.olvLootList.AllColumns.Add(this.columnEnabled);
            this.olvLootList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olvLootList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnOriginalName,
            this.columnEnabled});
            this.olvLootList.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvLootList.FullRowSelect = true;
            this.olvLootList.Location = new System.Drawing.Point(2, 40);
            this.olvLootList.Name = "olvLootList";
            this.olvLootList.SelectColumnsOnRightClick = false;
            this.olvLootList.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.olvLootList.SelectedColumnTint = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.olvLootList.ShowGroups = false;
            this.olvLootList.Size = new System.Drawing.Size(332, 569);
            this.olvLootList.TabIndex = 39;
            this.olvLootList.UseCellFormatEvents = true;
            this.olvLootList.UseCompatibleStateImageBehavior = false;
            this.olvLootList.UseFiltering = true;
            this.olvLootList.UseHyperlinks = true;
            this.olvLootList.View = System.Windows.Forms.View.Details;
            this.olvLootList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ModListClick);
            this.olvLootList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Olv_LootList_MouseDoubleClick);
            // 
            // columnName
            // 
            this.columnName.AspectName = "Name";
            this.columnName.CellPadding = null;
            this.columnName.FillsFreeSpace = true;
            this.columnName.MaximumWidth = 285;
            this.columnName.MinimumWidth = 285;
            this.columnName.Text = "Name";
            this.columnName.Width = 285;
            // 
            // columnOriginalName
            // 
            this.columnOriginalName.CellPadding = null;
            this.columnOriginalName.Text = "";
            this.columnOriginalName.Width = 0;
            // 
            // columnEnabled
            // 
            this.columnEnabled.AspectName = "Status";
            this.columnEnabled.CellPadding = null;
            this.columnEnabled.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnEnabled.MaximumWidth = 30;
            this.columnEnabled.MinimumWidth = 30;
            this.columnEnabled.Text = "±";
            this.columnEnabled.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnEnabled.Width = 30;
            // 
            // checkBoxFindLoot
            // 
            this.checkBoxFindLoot.AutoSize = true;
            this.checkBoxFindLoot.Location = new System.Drawing.Point(4, 3);
            this.checkBoxFindLoot.Name = "checkBoxFindLoot";
            this.checkBoxFindLoot.Size = new System.Drawing.Size(70, 17);
            this.checkBoxFindLoot.TabIndex = 37;
            this.checkBoxFindLoot.Text = "Find Loot";
            this.checkBoxFindLoot.UseVisualStyleBackColor = true;
            this.checkBoxFindLoot.CheckedChanged += new System.EventHandler(this.CheckBox_FindLoot_CheckedChanged);
            // 
            // buttonLootListClearAll
            // 
            this.buttonLootListClearAll.Location = new System.Drawing.Point(146, 3);
            this.buttonLootListClearAll.Name = "buttonLootListClearAll";
            this.buttonLootListClearAll.Size = new System.Drawing.Size(45, 23);
            this.buttonLootListClearAll.TabIndex = 40;
            this.buttonLootListClearAll.Text = "...All";
            this.buttonLootListClearAll.UseVisualStyleBackColor = true;
            this.buttonLootListClearAll.Click += new System.EventHandler(this.Button_ClearAll_Click);
            // 
            // buttonLootListClear
            // 
            this.buttonLootListClear.Location = new System.Drawing.Point(100, 3);
            this.buttonLootListClear.Name = "buttonLootListClear";
            this.buttonLootListClear.Size = new System.Drawing.Size(45, 23);
            this.buttonLootListClear.TabIndex = 40;
            this.buttonLootListClear.Text = "Clear";
            this.buttonLootListClear.UseVisualStyleBackColor = true;
            this.buttonLootListClear.Click += new System.EventHandler(this.Button_Clear_Click);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(207, 20);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 41;
            this.labelSearch.Text = "Search";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(251, 17);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(81, 20);
            this.textBoxSearch.TabIndex = 42;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.TextBox_Search_TextChanged);
            // 
            // tabPageOther
            // 
            this.tabPageOther.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageOther.Controls.Add(this.ButtonDotDraw);
            this.tabPageOther.Controls.Add(this.groupBoxDebug);
            this.tabPageOther.Controls.Add(this.buttonScreenshot);
            this.tabPageOther.Controls.Add(this.labelMap);
            this.tabPageOther.Controls.Add(this.labelRefreshRate);
            this.tabPageOther.Controls.Add(this.buttonDumpLoot);
            this.tabPageOther.Controls.Add(this.buttonDumpPlayers);
            this.tabPageOther.Controls.Add(this.comboBoxMap);
            this.tabPageOther.Controls.Add(this.checkBoxFullScreen);
            this.tabPageOther.Controls.Add(this.checkBoxOnTop);
            this.tabPageOther.Controls.Add(this.checkBoxNoBorder);
            this.tabPageOther.Controls.Add(this.checkboxTransparent);
            this.tabPageOther.Controls.Add(this.comboBoxRefreshRateMap);
            this.tabPageOther.Location = new System.Drawing.Point(4, 22);
            this.tabPageOther.Name = "tabPageOther";
            this.tabPageOther.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOther.Size = new System.Drawing.Size(336, 613);
            this.tabPageOther.TabIndex = 1;
            this.tabPageOther.Text = "Other";
            // 
            // ButtonDotDraw
            // 
            this.ButtonDotDraw.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonDotDraw.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ButtonDotDraw.Location = new System.Drawing.Point(4, 129);
            this.ButtonDotDraw.Name = "ButtonDotDraw";
            this.ButtonDotDraw.Size = new System.Drawing.Size(119, 23);
            this.ButtonDotDraw.TabIndex = 95;
            this.ButtonDotDraw.Text = "DrawDots";
            this.ButtonDotDraw.UseVisualStyleBackColor = true;
            this.ButtonDotDraw.Click += new System.EventHandler(this.Button_Dot);
            // 
            // groupBoxDebug
            // 
            this.groupBoxDebug.Controls.Add(this.labelAddressBase);
            this.groupBoxDebug.Controls.Add(this.textBoxAddressGameWorld);
            this.groupBoxDebug.Controls.Add(this.labelAddressLocalGameWorld);
            this.groupBoxDebug.Controls.Add(this.labelAddressGameworld);
            this.groupBoxDebug.Controls.Add(this.textBoxAddressBase);
            this.groupBoxDebug.Controls.Add(this.labelFPSCamera);
            this.groupBoxDebug.Controls.Add(this.labelPID);
            this.groupBoxDebug.Controls.Add(this.textBoxPID);
            this.groupBoxDebug.Controls.Add(this.textBoxAddressLocalGameWorld);
            this.groupBoxDebug.Controls.Add(this.textBoxFPSCamera);
            this.groupBoxDebug.Location = new System.Drawing.Point(7, 184);
            this.groupBoxDebug.Name = "groupBoxDebug";
            this.groupBoxDebug.Size = new System.Drawing.Size(322, 244);
            this.groupBoxDebug.TabIndex = 94;
            this.groupBoxDebug.TabStop = false;
            this.groupBoxDebug.Text = "Debug";
            // 
            // labelAddressBase
            // 
            this.labelAddressBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelAddressBase.Location = new System.Drawing.Point(6, 16);
            this.labelAddressBase.Name = "labelAddressBase";
            this.labelAddressBase.Size = new System.Drawing.Size(100, 20);
            this.labelAddressBase.TabIndex = 2;
            this.labelAddressBase.Text = "Base";
            this.labelAddressBase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxAddressGameWorld
            // 
            this.textBoxAddressGameWorld.Location = new System.Drawing.Point(6, 88);
            this.textBoxAddressGameWorld.Name = "textBoxAddressGameWorld";
            this.textBoxAddressGameWorld.ReadOnly = true;
            this.textBoxAddressGameWorld.Size = new System.Drawing.Size(100, 20);
            this.textBoxAddressGameWorld.TabIndex = 3;
            // 
            // labelAddressLocalGameWorld
            // 
            this.labelAddressLocalGameWorld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelAddressLocalGameWorld.Location = new System.Drawing.Point(6, 114);
            this.labelAddressLocalGameWorld.Name = "labelAddressLocalGameWorld";
            this.labelAddressLocalGameWorld.Size = new System.Drawing.Size(100, 20);
            this.labelAddressLocalGameWorld.TabIndex = 0;
            this.labelAddressLocalGameWorld.Text = "Local Game World";
            this.labelAddressLocalGameWorld.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAddressGameworld
            // 
            this.labelAddressGameworld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelAddressGameworld.Location = new System.Drawing.Point(6, 65);
            this.labelAddressGameworld.Name = "labelAddressGameworld";
            this.labelAddressGameworld.Size = new System.Drawing.Size(100, 20);
            this.labelAddressGameworld.TabIndex = 2;
            this.labelAddressGameworld.Text = "GameWorld";
            this.labelAddressGameworld.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxAddressBase
            // 
            this.textBoxAddressBase.Location = new System.Drawing.Point(6, 39);
            this.textBoxAddressBase.Name = "textBoxAddressBase";
            this.textBoxAddressBase.ReadOnly = true;
            this.textBoxAddressBase.Size = new System.Drawing.Size(100, 20);
            this.textBoxAddressBase.TabIndex = 3;
            // 
            // labelFPSCamera
            // 
            this.labelFPSCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFPSCamera.Location = new System.Drawing.Point(6, 163);
            this.labelFPSCamera.Name = "labelFPSCamera";
            this.labelFPSCamera.Size = new System.Drawing.Size(100, 20);
            this.labelFPSCamera.TabIndex = 0;
            this.labelFPSCamera.Text = "FPS Camera";
            this.labelFPSCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPID
            // 
            this.labelPID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPID.Location = new System.Drawing.Point(216, 65);
            this.labelPID.Name = "labelPID";
            this.labelPID.Size = new System.Drawing.Size(100, 20);
            this.labelPID.TabIndex = 0;
            this.labelPID.Text = "PID";
            this.labelPID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxPID
            // 
            this.textBoxPID.Location = new System.Drawing.Point(216, 88);
            this.textBoxPID.Name = "textBoxPID";
            this.textBoxPID.ReadOnly = true;
            this.textBoxPID.Size = new System.Drawing.Size(100, 20);
            this.textBoxPID.TabIndex = 1;
            // 
            // textBoxAddressLocalGameWorld
            // 
            this.textBoxAddressLocalGameWorld.Location = new System.Drawing.Point(6, 137);
            this.textBoxAddressLocalGameWorld.Name = "textBoxAddressLocalGameWorld";
            this.textBoxAddressLocalGameWorld.ReadOnly = true;
            this.textBoxAddressLocalGameWorld.Size = new System.Drawing.Size(100, 20);
            this.textBoxAddressLocalGameWorld.TabIndex = 1;
            // 
            // textBoxFPSCamera
            // 
            this.textBoxFPSCamera.Location = new System.Drawing.Point(6, 186);
            this.textBoxFPSCamera.Name = "textBoxFPSCamera";
            this.textBoxFPSCamera.ReadOnly = true;
            this.textBoxFPSCamera.Size = new System.Drawing.Size(100, 20);
            this.textBoxFPSCamera.TabIndex = 1;
            // 
            // buttonScreenshot
            // 
            this.buttonScreenshot.BackColor = System.Drawing.Color.LightCoral;
            this.buttonScreenshot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonScreenshot.Location = new System.Drawing.Point(4, 100);
            this.buttonScreenshot.Name = "buttonScreenshot";
            this.buttonScreenshot.Size = new System.Drawing.Size(119, 23);
            this.buttonScreenshot.TabIndex = 93;
            this.buttonScreenshot.Text = "Screenshot";
            this.buttonScreenshot.UseVisualStyleBackColor = false;
            this.buttonScreenshot.Click += new System.EventHandler(this.Button_Screenshot_Click);
            // 
            // labelMap
            // 
            this.labelMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMap.Location = new System.Drawing.Point(4, 28);
            this.labelMap.Name = "labelMap";
            this.labelMap.Size = new System.Drawing.Size(48, 21);
            this.labelMap.TabIndex = 90;
            this.labelMap.Text = "Map";
            this.labelMap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRefreshRate
            // 
            this.labelRefreshRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRefreshRate.Location = new System.Drawing.Point(4, 4);
            this.labelRefreshRate.Name = "labelRefreshRate";
            this.labelRefreshRate.Size = new System.Drawing.Size(100, 21);
            this.labelRefreshRate.TabIndex = 89;
            this.labelRefreshRate.Text = "Refresh Rate";
            this.labelRefreshRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDumpLoot
            // 
            this.buttonDumpLoot.Enabled = false;
            this.buttonDumpLoot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDumpLoot.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonDumpLoot.Location = new System.Drawing.Point(214, 32);
            this.buttonDumpLoot.Name = "buttonDumpLoot";
            this.buttonDumpLoot.Size = new System.Drawing.Size(119, 23);
            this.buttonDumpLoot.TabIndex = 88;
            this.buttonDumpLoot.Text = "DumpLoot";
            this.buttonDumpLoot.UseVisualStyleBackColor = true;
            this.buttonDumpLoot.Click += new System.EventHandler(this.DumpLoot);
            // 
            // buttonDumpPlayers
            // 
            this.buttonDumpPlayers.Enabled = false;
            this.buttonDumpPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDumpPlayers.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonDumpPlayers.Location = new System.Drawing.Point(214, 3);
            this.buttonDumpPlayers.Name = "buttonDumpPlayers";
            this.buttonDumpPlayers.Size = new System.Drawing.Size(119, 23);
            this.buttonDumpPlayers.TabIndex = 88;
            this.buttonDumpPlayers.Text = "DumpPlayers";
            this.buttonDumpPlayers.UseVisualStyleBackColor = true;
            this.buttonDumpPlayers.Click += new System.EventHandler(this.DumpPlayers);
            // 
            // comboBoxMap
            // 
            this.comboBoxMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMap.FormattingEnabled = true;
            this.comboBoxMap.Items.AddRange(new object[] {
            "---",
            "Customs",
            "Shoreline",
            "Forest",
            "Factory",
            "Interchange"});
            this.comboBoxMap.Location = new System.Drawing.Point(55, 28);
            this.comboBoxMap.Name = "comboBoxMap";
            this.comboBoxMap.Size = new System.Drawing.Size(118, 21);
            this.comboBoxMap.TabIndex = 8;
            this.comboBoxMap.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Map_SelectedIndexChanged);
            // 
            // checkBoxFullScreen
            // 
            this.checkBoxFullScreen.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxFullScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxFullScreen.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.checkBoxFullScreen.Location = new System.Drawing.Point(89, 51);
            this.checkBoxFullScreen.Name = "checkBoxFullScreen";
            this.checkBoxFullScreen.Size = new System.Drawing.Size(85, 23);
            this.checkBoxFullScreen.TabIndex = 85;
            this.checkBoxFullScreen.Text = "Full Screen";
            this.checkBoxFullScreen.UseVisualStyleBackColor = true;
            this.checkBoxFullScreen.CheckedChanged += new System.EventHandler(this.CheckBox_FullScreen_CheckedChanged);
            // 
            // checkBoxOnTop
            // 
            this.checkBoxOnTop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxOnTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxOnTop.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxOnTop.Location = new System.Drawing.Point(89, 75);
            this.checkBoxOnTop.Name = "checkBoxOnTop";
            this.checkBoxOnTop.Size = new System.Drawing.Size(85, 23);
            this.checkBoxOnTop.TabIndex = 84;
            this.checkBoxOnTop.Text = "OnTop";
            this.checkBoxOnTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxOnTop.UseVisualStyleBackColor = true;
            this.checkBoxOnTop.CheckedChanged += new System.EventHandler(this.CheckBox_OnTop_CheckedChanged);
            // 
            // checkBoxNoBorder
            // 
            this.checkBoxNoBorder.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxNoBorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxNoBorder.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxNoBorder.Location = new System.Drawing.Point(3, 75);
            this.checkBoxNoBorder.Name = "checkBoxNoBorder";
            this.checkBoxNoBorder.Size = new System.Drawing.Size(85, 23);
            this.checkBoxNoBorder.TabIndex = 83;
            this.checkBoxNoBorder.Text = "No Border";
            this.checkBoxNoBorder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNoBorder.UseVisualStyleBackColor = true;
            this.checkBoxNoBorder.CheckedChanged += new System.EventHandler(this.CheckBox_NoBorder_CheckedChanged);
            // 
            // checkboxTransparent
            // 
            this.checkboxTransparent.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkboxTransparent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkboxTransparent.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkboxTransparent.Location = new System.Drawing.Point(3, 51);
            this.checkboxTransparent.Name = "checkboxTransparent";
            this.checkboxTransparent.Size = new System.Drawing.Size(85, 23);
            this.checkboxTransparent.TabIndex = 80;
            this.checkboxTransparent.Text = "Transparent";
            this.checkboxTransparent.UseVisualStyleBackColor = true;
            this.checkboxTransparent.CheckedChanged += new System.EventHandler(this.Checkbox_Transparent_CheckedChanged);
            // 
            // comboBoxRefreshRateMap
            // 
            this.comboBoxRefreshRateMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRefreshRateMap.FormattingEnabled = true;
            this.comboBoxRefreshRateMap.Items.AddRange(new object[] {
            "25",
            "50",
            "100",
            "200"});
            this.comboBoxRefreshRateMap.Location = new System.Drawing.Point(107, 4);
            this.comboBoxRefreshRateMap.Name = "comboBoxRefreshRateMap";
            this.comboBoxRefreshRateMap.Size = new System.Drawing.Size(66, 21);
            this.comboBoxRefreshRateMap.TabIndex = 79;
            this.comboBoxRefreshRateMap.SelectedIndexChanged += new System.EventHandler(this.ComboBox_RefreshRateMap_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.buttonBecomeDef);
            this.tabPage1.Controls.Add(this.buttonBecomeDev);
            this.tabPage1.Controls.Add(this.buttonBecomeMod);
            this.tabPage1.Controls.Add(this.labelMyPosition);
            this.tabPage1.Controls.Add(this.textBoxMyPosition);
            this.tabPage1.Controls.Add(this.labelEquipment);
            this.tabPage1.Controls.Add(this.buttonEquipmentUpdate);
            this.tabPage1.Controls.Add(this.listViewEquipment);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(336, 613);
            this.tabPage1.TabIndex = 6;
            this.tabPage1.Text = "Agressive";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.LightCoral;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(108, 476);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 48);
            this.button3.TabIndex = 104;
            this.button3.Text = "FakeMe";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.ButtonFakeMe);
            // 
            // buttonBecomeDef
            // 
            this.buttonBecomeDef.BackColor = System.Drawing.Color.LightCoral;
            this.buttonBecomeDef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBecomeDef.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBecomeDef.Location = new System.Drawing.Point(209, 422);
            this.buttonBecomeDef.Name = "buttonBecomeDef";
            this.buttonBecomeDef.Size = new System.Drawing.Size(100, 48);
            this.buttonBecomeDef.TabIndex = 104;
            this.buttonBecomeDef.Text = "Become Def";
            this.buttonBecomeDef.UseVisualStyleBackColor = false;
            this.buttonBecomeDef.Click += new System.EventHandler(this.ButtonBecomeDef_Click);
            // 
            // buttonBecomeDev
            // 
            this.buttonBecomeDev.BackColor = System.Drawing.Color.LightCoral;
            this.buttonBecomeDev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBecomeDev.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBecomeDev.Location = new System.Drawing.Point(108, 422);
            this.buttonBecomeDev.Name = "buttonBecomeDev";
            this.buttonBecomeDev.Size = new System.Drawing.Size(100, 48);
            this.buttonBecomeDev.TabIndex = 104;
            this.buttonBecomeDev.Text = "Become Dev";
            this.buttonBecomeDev.UseVisualStyleBackColor = false;
            this.buttonBecomeDev.Click += new System.EventHandler(this.ButtonBecomeDev_Click);
            // 
            // buttonBecomeMod
            // 
            this.buttonBecomeMod.BackColor = System.Drawing.Color.LightCoral;
            this.buttonBecomeMod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBecomeMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBecomeMod.Location = new System.Drawing.Point(7, 422);
            this.buttonBecomeMod.Name = "buttonBecomeMod";
            this.buttonBecomeMod.Size = new System.Drawing.Size(100, 48);
            this.buttonBecomeMod.TabIndex = 104;
            this.buttonBecomeMod.Text = "Become Mod";
            this.buttonBecomeMod.UseVisualStyleBackColor = false;
            this.buttonBecomeMod.Click += new System.EventHandler(this.ButtonBecomeMod_Click);
            // 
            // labelMyPosition
            // 
            this.labelMyPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMyPosition.Location = new System.Drawing.Point(4, 373);
            this.labelMyPosition.Name = "labelMyPosition";
            this.labelMyPosition.Size = new System.Drawing.Size(312, 20);
            this.labelMyPosition.TabIndex = 49;
            this.labelMyPosition.Text = "MyPosition X Y Z";
            this.labelMyPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxMyPosition
            // 
            this.textBoxMyPosition.Location = new System.Drawing.Point(4, 396);
            this.textBoxMyPosition.Name = "textBoxMyPosition";
            this.textBoxMyPosition.ReadOnly = true;
            this.textBoxMyPosition.Size = new System.Drawing.Size(312, 20);
            this.textBoxMyPosition.TabIndex = 50;
            // 
            // labelEquipment
            // 
            this.labelEquipment.AutoSize = true;
            this.labelEquipment.BackColor = System.Drawing.SystemColors.ControlDark;
            this.labelEquipment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelEquipment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEquipment.Location = new System.Drawing.Point(5, 4);
            this.labelEquipment.Name = "labelEquipment";
            this.labelEquipment.Padding = new System.Windows.Forms.Padding(5);
            this.labelEquipment.Size = new System.Drawing.Size(76, 25);
            this.labelEquipment.TabIndex = 42;
            this.labelEquipment.Text = "INACTIVE";
            // 
            // buttonEquipmentUpdate
            // 
            this.buttonEquipmentUpdate.Location = new System.Drawing.Point(265, 5);
            this.buttonEquipmentUpdate.Name = "buttonEquipmentUpdate";
            this.buttonEquipmentUpdate.Size = new System.Drawing.Size(65, 23);
            this.buttonEquipmentUpdate.TabIndex = 41;
            this.buttonEquipmentUpdate.Text = "Update";
            this.buttonEquipmentUpdate.UseVisualStyleBackColor = true;
            this.buttonEquipmentUpdate.Click += new System.EventHandler(this.ButtonEquipmentUpdate_Click);
            // 
            // listViewEquipment
            // 
            this.listViewEquipment.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnEqName,
            this.columnEqID,
            this.columnProfileID});
            this.listViewEquipment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewEquipment.FullRowSelect = true;
            this.listViewEquipment.GridLines = true;
            this.listViewEquipment.HideSelection = false;
            this.listViewEquipment.Location = new System.Drawing.Point(4, 32);
            this.listViewEquipment.Name = "listViewEquipment";
            this.listViewEquipment.Size = new System.Drawing.Size(326, 338);
            this.listViewEquipment.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewEquipment.TabIndex = 31;
            this.listViewEquipment.UseCompatibleStateImageBehavior = false;
            this.listViewEquipment.View = System.Windows.Forms.View.Details;
            this.listViewEquipment.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewEquipmentMouseDoubleClick);
            // 
            // columnEqName
            // 
            this.columnEqName.Text = "PlayerName";
            this.columnEqName.Width = 160;
            // 
            // columnEqID
            // 
            this.columnEqID.Text = "ID";
            this.columnEqID.Width = 120;
            // 
            // columnProfileID
            // 
            this.columnProfileID.Text = "Profile";
            // 
            // buttonNoRecoil
            // 
            this.buttonNoRecoil.BackColor = System.Drawing.Color.LightCoral;
            this.buttonNoRecoil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNoRecoil.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNoRecoil.Location = new System.Drawing.Point(629, 57);
            this.buttonNoRecoil.Name = "buttonNoRecoil";
            this.buttonNoRecoil.Size = new System.Drawing.Size(100, 48);
            this.buttonNoRecoil.TabIndex = 46;
            this.buttonNoRecoil.Text = "NoRecoil";
            this.buttonNoRecoil.UseVisualStyleBackColor = false;
            this.buttonNoRecoil.Click += new System.EventHandler(this.ButtonNoRecoil_Click);
            // 
            // buttonOverlay
            // 
            this.buttonOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOverlay.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonOverlay.Location = new System.Drawing.Point(169, 3);
            this.buttonOverlay.Name = "buttonOverlay";
            this.buttonOverlay.Size = new System.Drawing.Size(64, 23);
            this.buttonOverlay.TabIndex = 96;
            this.buttonOverlay.Text = "Overlay";
            this.buttonOverlay.UseVisualStyleBackColor = true;
            this.buttonOverlay.Click += new System.EventHandler(this.ButtonOverlay_Click);
            // 
            // panelSettings
            // 
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelSettings.Controls.Add(this.tabControlSettings);
            this.panelSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(344, 661);
            this.panelSettings.TabIndex = 26;
            // 
            // contextMenuStripModListView
            // 
            this.contextMenuStripModListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackListToolStripMenuItem});
            this.contextMenuStripModListView.Name = "contextMenuStrip1";
            this.contextMenuStripModListView.Size = new System.Drawing.Size(121, 26);
            // 
            // blackListToolStripMenuItem
            // 
            this.blackListToolStripMenuItem.Name = "blackListToolStripMenuItem";
            this.blackListToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.blackListToolStripMenuItem.Text = "BlackList";
            this.blackListToolStripMenuItem.Click += new System.EventHandler(this.BlacklistLootItem);
            // 
            // buttonMoveZplus
            // 
            this.buttonMoveZplus.Font = new System.Drawing.Font("Wingdings 3", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonMoveZplus.Location = new System.Drawing.Point(515, 3);
            this.buttonMoveZplus.Name = "buttonMoveZplus";
            this.buttonMoveZplus.Size = new System.Drawing.Size(50, 50);
            this.buttonMoveZplus.TabIndex = 102;
            this.buttonMoveZplus.Text = "r";
            this.buttonMoveZplus.UseVisualStyleBackColor = true;
            this.buttonMoveZplus.Click += new System.EventHandler(this.ButtonMoveUp_Click);
            // 
            // buttonMoveZminus
            // 
            this.buttonMoveZminus.Font = new System.Drawing.Font("Wingdings 3", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonMoveZminus.Location = new System.Drawing.Point(515, 59);
            this.buttonMoveZminus.Name = "buttonMoveZminus";
            this.buttonMoveZminus.Size = new System.Drawing.Size(50, 50);
            this.buttonMoveZminus.TabIndex = 101;
            this.buttonMoveZminus.Text = "s";
            this.buttonMoveZminus.UseVisualStyleBackColor = true;
            this.buttonMoveZminus.Click += new System.EventHandler(this.ButtonMoveDown_Click);
            // 
            // buttonYminus
            // 
            this.buttonYminus.Font = new System.Drawing.Font("Wingdings 3", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonYminus.Location = new System.Drawing.Point(403, 60);
            this.buttonYminus.Name = "buttonYminus";
            this.buttonYminus.Size = new System.Drawing.Size(50, 50);
            this.buttonYminus.TabIndex = 99;
            this.buttonYminus.Text = "q";
            this.buttonYminus.UseVisualStyleBackColor = true;
            this.buttonYminus.Click += new System.EventHandler(this.ButtonMoveBackward_Click);
            // 
            // buttonMoveForward
            // 
            this.buttonMoveForward.Font = new System.Drawing.Font("Wingdings 3", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonMoveForward.Location = new System.Drawing.Point(403, 3);
            this.buttonMoveForward.Name = "buttonMoveForward";
            this.buttonMoveForward.Size = new System.Drawing.Size(50, 50);
            this.buttonMoveForward.TabIndex = 97;
            this.buttonMoveForward.Text = "p";
            this.buttonMoveForward.UseVisualStyleBackColor = true;
            this.buttonMoveForward.Click += new System.EventHandler(this.ButtonMoveForward_Click);
            // 
            // buttonMoveLeft
            // 
            this.buttonMoveLeft.Font = new System.Drawing.Font("Wingdings 3", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonMoveLeft.Location = new System.Drawing.Point(347, 59);
            this.buttonMoveLeft.Name = "buttonMoveLeft";
            this.buttonMoveLeft.Size = new System.Drawing.Size(50, 50);
            this.buttonMoveLeft.TabIndex = 100;
            this.buttonMoveLeft.Text = "t";
            this.buttonMoveLeft.UseVisualStyleBackColor = true;
            this.buttonMoveLeft.Click += new System.EventHandler(this.ButtonMoveLeft_Click);
            // 
            // buttonMoveRight
            // 
            this.buttonMoveRight.Font = new System.Drawing.Font("Wingdings 3", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonMoveRight.Location = new System.Drawing.Point(459, 59);
            this.buttonMoveRight.Name = "buttonMoveRight";
            this.buttonMoveRight.Size = new System.Drawing.Size(50, 50);
            this.buttonMoveRight.TabIndex = 98;
            this.buttonMoveRight.Text = "u";
            this.buttonMoveRight.UseVisualStyleBackColor = true;
            this.buttonMoveRight.Click += new System.EventHandler(this.ButtonMoveRight_Click);
            // 
            // buttonTravel
            // 
            this.buttonTravel.BackColor = System.Drawing.Color.LightCoral;
            this.buttonTravel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTravel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTravel.Location = new System.Drawing.Point(629, 0);
            this.buttonTravel.Name = "buttonTravel";
            this.buttonTravel.Size = new System.Drawing.Size(100, 48);
            this.buttonTravel.TabIndex = 51;
            this.buttonTravel.Text = "Travel";
            this.buttonTravel.UseVisualStyleBackColor = false;
            this.buttonTravel.Click += new System.EventHandler(this.ButtonTravel_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 661);
            this.Controls.Add(this.buttonNoRecoil);
            this.Controls.Add(this.buttonTravel);
            this.Controls.Add(this.buttonMoveZplus);
            this.Controls.Add(this.buttonMoveZminus);
            this.Controls.Add(this.buttonYminus);
            this.Controls.Add(this.buttonMoveForward);
            this.Controls.Add(this.buttonMoveLeft);
            this.Controls.Add(this.buttonMoveRight);
            this.Controls.Add(this.buttonOverlay);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonStartStop);
            this.Controls.Add(this.buttonShowHide);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.openglControlMap);
            this.Name = "FormMain";
            this.Text = "NautilusFT";
            this.Load += new System.EventHandler(this.BaseLoad);
            this.Resize += new System.EventHandler(this.BaseResizeEnd);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupBoxOSD.ResumeLayout(false);
            this.groupBoxOSD.PerformLayout();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.groupBoxDraw.ResumeLayout(false);
            this.groupBoxDraw.PerformLayout();
            this.tabPageLoot.ResumeLayout(false);
            this.tabPageLoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvLootList)).EndInit();
            this.tabPageOther.ResumeLayout(false);
            this.groupBoxDebug.ResumeLayout(false);
            this.groupBoxDebug.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panelSettings.ResumeLayout(false);
            this.contextMenuStripModListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDebug;
        private System.Windows.Forms.Label labelAddressBase;
        private System.Windows.Forms.TextBox textBoxAddressGameWorld;
        private System.Windows.Forms.Label labelAddressLocalGameWorld;
        private System.Windows.Forms.Label labelAddressGameworld;
        private System.Windows.Forms.TextBox textBoxAddressBase;
        private System.Windows.Forms.Label labelFPSCamera;
        private System.Windows.Forms.Label labelPID;
        private System.Windows.Forms.TextBox textBoxPID;
        private System.Windows.Forms.TextBox textBoxAddressLocalGameWorld;
        private System.Windows.Forms.TextBox textBoxFPSCamera;
        private System.Windows.Forms.Button buttonScreenshot;
        private System.Windows.Forms.Label labelMap;
        private System.Windows.Forms.Label labelRefreshRate;
        private System.Windows.Forms.Button buttonDumpLoot;
        private System.Windows.Forms.Button buttonDumpPlayers;
        public System.Windows.Forms.ComboBox comboBoxMap;
        private System.Windows.Forms.CheckBox checkBoxFullScreen;
        private System.Windows.Forms.CheckBox checkBoxOnTop;
        private System.Windows.Forms.CheckBox checkBoxNoBorder;
        private System.Windows.Forms.CheckBox checkboxTransparent;
        public System.Windows.Forms.ComboBox comboBoxRefreshRateMap;
        private System.Windows.Forms.Button buttonOverlay;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripModListView;
        private System.Windows.Forms.ToolStripMenuItem blackListToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn columnOriginalName;
        private System.Windows.Forms.Button ButtonDotDraw;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label labelEquipment;
        private System.Windows.Forms.ListView listViewEquipment;
        private System.Windows.Forms.ColumnHeader columnEqName;
        private System.Windows.Forms.ColumnHeader columnEqID;
        private System.Windows.Forms.Button buttonEquipmentUpdate;
        private System.Windows.Forms.Button buttonMoveZplus;
        private System.Windows.Forms.Button buttonMoveZminus;
        private System.Windows.Forms.Button buttonYminus;
        private System.Windows.Forms.Button buttonMoveForward;
        private System.Windows.Forms.Button buttonMoveLeft;
        private System.Windows.Forms.Button buttonMoveRight;
        private System.Windows.Forms.ColumnHeader columnProfileID;
        private System.Windows.Forms.Label labelMyPosition;
        private System.Windows.Forms.TextBox textBoxMyPosition;
        private System.Windows.Forms.Button buttonNoRecoil;
        private System.Windows.Forms.Button buttonTravel;
        private System.Windows.Forms.Button buttonBecomeDev;
        private System.Windows.Forms.Button buttonBecomeMod;
        private System.Windows.Forms.Button buttonBecomeDef;
        private System.Windows.Forms.Button button3;
    }
}