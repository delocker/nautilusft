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

using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;
using Numerics = System.Numerics;
namespace NautilusFT
{
    public partial class Overlay : Form
    {
        #region Declaration
        public static bool ingame = false;

        public bool overlayLoaded = false;
        public bool renderOverlayComplete = true;

        private readonly float playerFeetVector = 0.0f;

        private readonly float heightForStand = 1.6f;
        private readonly float heightForProne = 0.3f;
        private readonly float heightForCrouch = 1.2f;

        private Margins marg;

        private static WindowRenderTarget device;
        private HwndRenderTargetProperties renderProperties;

        // Fonts
        private FontFactory fontFactory = new FontFactory();

        private IntPtr handle;
        private float[] viewMatrix = new float[16];

        private NativeMethods.Rectangle targetWindowRect;

        private float heightToHead = 0;

        private IntPtr windowsSourceHandle;

        private System.Timers.Timer windowFollower;
        private Factory factory = new Factory();

        private float coeffResolutionTo = 2.5f;
        #endregion

        public Overlay()
        {
            InitializeComponent();
            handle = Handle;
            Width = 100;
            Height = 100;
        }


        public IntPtr CameraMatrixPointer { get; set; }

        [DllImport("dwmapi.dll")]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);

        public void DirectXThread()
        {
            if (Settings.DemoMode)
            {
                PrepareTestModeObjects();
            }
            else
            {
                PrepareObjects();
            }

            try
            {
                device.BeginDraw();
                device.Clear(SharpDX.Color.Transparent);
                device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Aliased;
                StringBuilder strBuild = new StringBuilder();
                PrepareCrossHair();
                RenderItem renderItem;

                // Window Border
                renderItem = new RenderItem();
                renderItem.structs.Text = "rectangle";
                renderItem.structs.Size = 1;
                renderItem.structs.DrawColor = Color.Fuchsia;
                renderItem.structs.MapPosX = -Width / 2f;
                renderItem.structs.MapPosZ = -Height / 2f;
                renderItem.structs.MapPosXend = (Width / 2) - 1;
                renderItem.structs.MapPosZend = (Height / 2) - 1;
                HelperOpenGL.OverlayGeometry.Add(renderItem);

                renderItem = new RenderItem();
                renderItem.structs.Text = "rectangle";
                renderItem.structs.Size = 1;
                renderItem.structs.DrawColor = Color.Fuchsia;
                renderItem.structs.MapPosX = (-Width / 2) + 1;
                renderItem.structs.MapPosZ = (-Height / 2) + 1;
                renderItem.structs.MapPosXend = (Width / 2) - 2;
                renderItem.structs.MapPosZend = (Height / 2) - 2;
                HelperOpenGL.OverlayGeometry.Add(renderItem);

                GlControl_Overlay_RenderObjects();

                device.Flush();
                device.EndDraw();
                renderOverlayComplete = true;
            }
            catch
            {
                try
                {
                    device.Flush();
                    device.EndDraw();
                }
                catch
                {
                }
            }
        }

        private void LoadOverlay(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, 
                true);

            // Find VNC window and position itself on top.
            KeepWindowInPlace();

            factory = new Factory();
            renderProperties = new HwndRenderTargetProperties
            {
                Hwnd = Handle,
                PixelSize = new SharpDX.Size2(Size.Width, Size.Height),

                PresentOptions = PresentOptions.None
            };

            // Initialize DirectX
            marg.Left = 0;
            marg.Top = 0;
            marg.Right = Width;
            marg.Bottom = Height;

            DwmExtendFrameIntoClientArea(Handle, ref marg);
            device = new WindowRenderTarget(factory, new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties)
            {
                AntialiasMode = AntialiasMode.PerPrimitive,
                TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Default
            };

            overlayLoaded = true;
            TopMost = true;

            // Timer that follows our VNC window, if we decide to move it.
            windowFollower = new System.Timers.Timer
            {
                Interval = 3000
            };

            windowFollower.Elapsed += KeepWindowInPlace;
            windowFollower.AutoReset = false;
            windowFollower.Enabled = true;
            windowFollower.Stop();
        }

        private void KeepWindowInPlace(object source, ElapsedEventArgs e)
        {
            /// targetWindowRect = GetTargetProcessWindowData();
            /// Location = new Point(targetWindowRect.left, targetWindowRect.top);
            ///
            /// Width = targetWindowRect.right;
            /// Height = targetWindowRect.bottom;
            /// windowFollower.Start();
        }

        private void KeepWindowInPlace()
        {
            targetWindowRect = GetTargetProcessWindowData();
            if (targetWindowRect.bottom == 0)
            {
                Location = new Point(0, 0);
                Width = 1600;
                Height = 900;
            }
            else
            {
                Location = new Point(targetWindowRect.left, targetWindowRect.top);
                Width = targetWindowRect.right;
                Height = targetWindowRect.bottom;
            }
        }

        private void ClosedOverlay(object sender, FormClosingEventArgs e)
        {
            try
            {
                device.Flush();
                device.EndDraw();
                device.Dispose();
                device = null;
                windowFollower.Stop();
                windowFollower.Dispose();
            }
            catch
            {
            }
        }

        private void PrepareCrossHair()
        {
            // Improvised Crosshair.
            var crosshairOffset = 8;
            var crosshairLength = crosshairOffset + 8;
            RenderItem renderItem;

            // CrossHair Main
            // Left 
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Fuchsia;
            renderItem.structs.MapPosX = -crosshairOffset;
            renderItem.structs.MapPosZ = 0;
            renderItem.structs.MapPosXend = -crosshairLength;
            renderItem.structs.MapPosZend = 0;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            // Right
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Fuchsia;
            renderItem.structs.MapPosX = crosshairOffset;
            renderItem.structs.MapPosZ = 0;
            renderItem.structs.MapPosXend = crosshairLength;
            renderItem.structs.MapPosZend = 0;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            // Top
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Fuchsia;
            renderItem.structs.MapPosX = 0;
            renderItem.structs.MapPosZ = -crosshairOffset;
            renderItem.structs.MapPosXend = 0;
            renderItem.structs.MapPosZend = -crosshairLength;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            // Bottom
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Fuchsia;
            renderItem.structs.MapPosX = 0;
            renderItem.structs.MapPosZ = crosshairOffset;
            renderItem.structs.MapPosXend = 0;
            renderItem.structs.MapPosZend = crosshairLength;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            // CrossHair Outline
            // Left 
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Black;
            renderItem.structs.MapPosX = -crosshairOffset + 1;
            renderItem.structs.MapPosZ = -1;
            renderItem.structs.MapPosXend = -crosshairLength - 1;
            renderItem.structs.MapPosZend = 1;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            // Right
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Black;
            renderItem.structs.MapPosX = crosshairOffset - 1;
            renderItem.structs.MapPosZ = -1;
            renderItem.structs.MapPosXend = crosshairLength + 1;
            renderItem.structs.MapPosZend = 1;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            // Top
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Black;
            renderItem.structs.MapPosX = -1;
            renderItem.structs.MapPosZ = -crosshairOffset + 1;
            renderItem.structs.MapPosXend = 1;
            renderItem.structs.MapPosZend = -crosshairLength - 1;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            // Bottom
            renderItem = new RenderItem();
            renderItem.structs.Text = "rectangle";
            renderItem.structs.Size = 1;
            renderItem.structs.DrawColor = Color.Black;
            renderItem.structs.MapPosX = -1;
            renderItem.structs.MapPosZ = crosshairOffset - 1;
            renderItem.structs.MapPosXend = 1;
            renderItem.structs.MapPosZend = crosshairLength + 1;
            HelperOpenGL.OverlayGeometry.Add(renderItem);
        }

        private void PrepareObjects()
        {
            if (HelperOpenGL.PlayerListToRenderFinal.Count == 0)
            {
                return;
            }

            var myPosX = HelperOpenGL.PlayerListToRenderFinal[0].MapLocX;
            var myPosY = HelperOpenGL.PlayerListToRenderFinal[0].MapLocY;
            var myPosZ = HelperOpenGL.PlayerListToRenderFinal[0].MapLocZ;
            Numerics.Matrix4x4? matrix = null;

            // Draw player ESP.
            var vectorMe = new OpenTK.Vector3(myPosX, myPosY, myPosZ);

            RenderItem renderItem;

            try
            {
                for (int i = 0; i < HelperOpenGL.PlayerListToRenderFinal.Count; i++)
                {
                    // Don`t draw who at 0.0.0
                    if (HelperOpenGL.PlayerListToRenderFinal[i].MapLocX == 0 || HelperOpenGL.PlayerListToRenderFinal[i].MapLocZ == 0)
                    {
                        continue;
                    }

                    // Because 0 is always our player and we don`t need esp for ourselves.
                    if (i != 0)
                    {
                        var entityPosXoriginal = HelperOpenGL.PlayerListToRenderFinal[i].MapLocX;
                        var entityPosYoriginal = HelperOpenGL.PlayerListToRenderFinal[i].MapLocY;
                        var entityPosZoriginal = HelperOpenGL.PlayerListToRenderFinal[i].MapLocZ;

                        switch (HelperOpenGL.PlayerListToRenderFinal[i].Pose)
                        {
                            case "Standing":
                                heightToHead = heightForStand;
                                break;
                            case "Crouch":
                                heightToHead = heightForCrouch;
                                break;
                            case "Prone":
                                heightToHead = heightForProne;
                                break;
                            default:
                                heightToHead = heightForStand;
                                break;
                        }

                        var vectorTarget = new OpenTK.Vector3(entityPosXoriginal, entityPosYoriginal + playerFeetVector, entityPosZoriginal);
                        var vectorTargetHead = new OpenTK.Vector3(vectorTarget.X, vectorTarget.Y + heightToHead, vectorTarget.Z);

                        float distance = (int)Math.Round(Helper.GetDistance(vectorMe, vectorTarget));

                        if (distance < 500 && distance > 2)
                        {
                            if (matrix == null)
                            {
                                matrix = Memory.Read<System.Numerics.Matrix4x4>(CameraMatrixPointer.ToInt64());
                            }

                            var wts = WorldToScreen(vectorTarget, out OpenTK.Vector3 coords, matrix, Width, Height);
                            var wtsHead = WorldToScreen(vectorTargetHead, out OpenTK.Vector3 coordsHead, matrix, Width, Height);

                            if (wts)
                            {
                                /* X.Y.
                                * Left Top ( - | - )
                                * Right Bottom ( + | + )
                                */

                                var rectX1 = coordsHead.X - ((coordsHead.Y - coords.Y) / 3);
                                var rectY1 = coordsHead.Y;
                                var rectX2 = coords.X + ((coordsHead.Y - coords.Y) / 3);
                                var rectY2 = coords.Y;
                                var offsetY = 0;

                                renderItem = new RenderItem();
                                renderItem.structs.Text = "rectangle";
                                renderItem.structs.Size = 1;
                                renderItem.structs.DrawColor = Color.Black;
                                renderItem.structs.MapPosX = rectX1 + 8;
                                renderItem.structs.MapPosZ = rectY1 - 8;
                                renderItem.structs.MapPosXend = rectX2 - 8;
                                renderItem.structs.MapPosZend = rectY2 + 8;
                                HelperOpenGL.OverlayGeometry.Add(renderItem);

                                // Main Rect.
                                renderItem = new RenderItem();
                                renderItem.structs.Text = "rectangle";
                                renderItem.structs.Size = 1;
                                renderItem.structs.DrawColor = HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor;
                                renderItem.structs.MapPosX = rectX1 + 7;
                                renderItem.structs.MapPosZ = rectY1 - 7;
                                renderItem.structs.MapPosXend = rectX2 - 7;
                                renderItem.structs.MapPosZend = rectY2 + 7;
                                HelperOpenGL.OverlayGeometry.Add(renderItem);

                                // Head.
                                renderItem = new RenderItem();
                                renderItem.structs.Text = "rectangle";
                                renderItem.structs.Size = 1;
                                renderItem.structs.DrawColor = HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor;
                                renderItem.structs.MapPosX = coordsHead.X;
                                renderItem.structs.MapPosZ = coordsHead.Y;
                                renderItem.structs.MapPosXend = coordsHead.X;
                                renderItem.structs.MapPosZend = coordsHead.Y;
                                HelperOpenGL.OverlayGeometry.Add(renderItem);

                                HelperOpenGL.OverlayGeometry.Add(renderItem);
                                renderItem = new RenderItem();
                                renderItem.structs.Text = "rectangle";
                                renderItem.structs.Size = 1;
                                renderItem.structs.DrawColor = Color.Black;
                                renderItem.structs.MapPosX = coordsHead.X + 1;
                                renderItem.structs.MapPosZ = coordsHead.Y - 1;
                                renderItem.structs.MapPosXend = coordsHead.X - 1;
                                renderItem.structs.MapPosZend = coordsHead.Y + 1;
                                HelperOpenGL.OverlayGeometry.Add(renderItem);

                                // Text.

                                // Nickname (DISABLED)
                                if (Settings.MapShowPlayerName && false)
                                {
                                    renderItem = new RenderItem();
                                    renderItem.structs.Text = HelperOpenGL.PlayerListToRenderFinal[i].Nickname;
                                    renderItem.structs.TextOutline = true;
                                    renderItem.structs.MapPosX = coords.X;
                                    renderItem.structs.MapPosZ = coords.Y + 10 + offsetY;
                                    renderItem.structs.Size = (int)FontSizes.misc;
                                    renderItem.structs.DrawColor = HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor;
                                    HelperOpenGL.OverlayText.Add(renderItem);
                                    offsetY += 10;
                                }

                                // Distance
                                if (Settings.MapShowPlayerDistance)
                                {
                                    renderItem = new RenderItem();
                                    renderItem.structs.Text = (int)distance + "m";
                                    renderItem.structs.TextOutline = true;
                                    renderItem.structs.MapPosX = coords.X;
                                    renderItem.structs.MapPosZ = coords.Y + 15 + offsetY;
                                    renderItem.structs.Size = (int)FontSizes.misc;
                                    renderItem.structs.DrawColor = Settings.ColorText;
                                    HelperOpenGL.OverlayText.Add(renderItem);
                                    offsetY += 15;
                                }

                                // Weapon (DISABLED)
                                if (Settings.MapShowPlayerWeapons && false)
                                {
                                    // Weapon
                                    renderItem = new RenderItem();
                                    renderItem.structs.Text = HelperOpenGL.PlayerListToRenderFinal[i].Weapon;
                                    renderItem.structs.TextOutline = true;
                                    renderItem.structs.MapPosX = coords.X;
                                    renderItem.structs.MapPosZ = coords.Y + 15 + offsetY;
                                    renderItem.structs.Size = (int)FontSizes.misc;
                                    renderItem.structs.DrawColor = Settings.ColorText;
                                    HelperOpenGL.OverlayText.Add(renderItem);
                                    offsetY += 15;
                                }

                                if (Settings.MapShowPlayerName)
                                {
                                    // Pose
                                    renderItem = new RenderItem();
                                    renderItem.structs.Text = HelperOpenGL.PlayerListToRenderFinal[i].Pose;
                                    renderItem.structs.TextOutline = true;
                                    renderItem.structs.MapPosX = coords.X;
                                    renderItem.structs.MapPosZ = coords.Y + 10 + offsetY;
                                    renderItem.structs.Size = (int)FontSizes.misc;
                                    renderItem.structs.DrawColor = HelperOpenGL.PlayerListToRenderFinal[i].Drawcolor;
                                    HelperOpenGL.OverlayText.Add(renderItem);
                                }
                            }
                        }
                    }
                }

                if (Settings.FindLoot)
                {
                    // Draw loot ESP.
                    for (int i = 0; i < HelperOpenGL.LootListToRenderFinal.Count; i++)
                    {
                        // Don`t draw who at 0.0.0
                        if (HelperOpenGL.LootListToRenderFinal[i].MapLocX == 0 || HelperOpenGL.LootListToRenderFinal[i].MapLocZ == 0)
                        {
                            continue;
                        }

                        var indexOfL = Settings.LootListToLookFor.IndexOf(HelperOpenGL.LootListToRenderFinal[i].Lootname);
                        if (indexOfL < 0)
                        {
                            // We dont.
                        }

                        if (indexOfL >= 0)
                        {
                            // We do.
                            var lootPosXoriginal = HelperOpenGL.LootListToRenderFinal[i].MapLocX;
                            var lootPosYoriginal = HelperOpenGL.LootListToRenderFinal[i].MapLocY;
                            var lootPosZoriginal = HelperOpenGL.LootListToRenderFinal[i].MapLocZ;

                            var vectorTarget = new OpenTK.Vector3(lootPosXoriginal, lootPosYoriginal, lootPosZoriginal);

                            float distance = (int)Math.Round(Helper.GetDistance(vectorMe, vectorTarget));

                            if (distance < 150)
                            {
                                if (matrix == null)
                                {
                                    matrix = Memory.Read<System.Numerics.Matrix4x4>(CameraMatrixPointer.ToInt64());
                                }

                                var wts = WorldToScreen(vectorTarget, out OpenTK.Vector3 coords, matrix, Width, Height);
                                if (wts)
                                {
                                    renderItem = new RenderItem();
                                    renderItem.structs.Text = "rectangle";
                                    renderItem.structs.MapPosX = coords.X - 2;
                                    renderItem.structs.MapPosZ = coords.Y - 2;
                                    renderItem.structs.MapPosXend = coords.X + 2;
                                    renderItem.structs.MapPosZend = coords.Y + 2;
                                    renderItem.structs.DrawColor = Color.Black;
                                    renderItem.structs.Size = 5;
                                    HelperOpenGL.OverlayGeometry.Add(renderItem);

                                    renderItem = new RenderItem();
                                    renderItem.structs.Text = "rectangle";
                                    renderItem.structs.MapPosX = coords.X - 2;
                                    renderItem.structs.MapPosZ = coords.Y - 2;
                                    renderItem.structs.MapPosXend = coords.X + 2;
                                    renderItem.structs.MapPosZend = coords.Y + 2;
                                    renderItem.structs.DrawColor = Color.White;
                                    renderItem.structs.Size = 3;
                                    HelperOpenGL.OverlayGeometry.Add(renderItem);

                                    renderItem = new RenderItem();
                                    renderItem.structs.Text = HelperOpenGL.LootListToRenderFinal[i].Lootname + " (" + (int)distance + "m)";
                                    renderItem.structs.TextOutline = true;
                                    renderItem.structs.MapPosX = coords.X;
                                    renderItem.structs.MapPosZ = coords.Y;
                                    renderItem.structs.DrawColor = Color.White;
                                    renderItem.structs.Size = (int)FontSizes.misc;
                                    HelperOpenGL.OverlayText.Add(renderItem);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                //// Hmm.... watch live him showing fingers to the cheaters and sucking my dick off... lol... nah jk... my dick is too precious by spoiling it to such abomination.
            }
        }

        private void PrepareTestModeObjects()
        {
            var degressFOV = DateTime.Now.Second * 6;
            var fov_line_X = (float)(Math.Cos(((degressFOV * Math.PI) / -180) + (Math.PI / 2)) * 100);
            var fov_line_Y = (float)(Math.Sin(((degressFOV * Math.PI) / -180) - (Math.PI / 2)) * 100);

            RenderItem renderItem;

            renderItem = new RenderItem();
            renderItem.structs.Text = "line";
            renderItem.structs.Size = 2;
            renderItem.structs.DrawColor = Color.LimeGreen;
            renderItem.structs.MapPosX = 0;
            renderItem.structs.MapPosZ = 0;
            renderItem.structs.MapPosXend = fov_line_X;
            renderItem.structs.MapPosZend = fov_line_Y;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.structs.Text = "circle";
            renderItem.structs.Size = 2;
            renderItem.structs.DrawColor = Color.LimeGreen;
            renderItem.structs.MapPosX = 0;
            renderItem.structs.MapPosZ = 0;
            renderItem.structs.Size = 80;
            HelperOpenGL.OverlayGeometry.Add(renderItem);

            renderItem = new RenderItem();
            renderItem.structs.Text = DateTime.Now.ToString();
            renderItem.structs.Size = 2;
            renderItem.structs.DrawColor = Color.LimeGreen;
            renderItem.structs.MapPosX = 0;
            renderItem.structs.MapPosZ = 100;
            renderItem.structs.Size = (int)FontSizes.misc;
            renderItem.structs.TextOutline = true;
            HelperOpenGL.OverlayText.Add(renderItem);
        }

        private void GlControl_Overlay_RenderObjects()
        {
            var screenWidthOffset = Width / 2;
            var screenHeightOffset = Height / 2;

            // Draw all the geometry.
            HelperOpenGL.OverlayGeometry.ForEach(u =>
            {
                switch (u.structs.Text)
                {
                    case "line":
                        device.DrawLine(new RawVector2(u.structs.MapPosX + screenWidthOffset, u.structs.MapPosZ + screenHeightOffset), new RawVector2(u.structs.MapPosXend + screenWidthOffset, u.structs.MapPosZend + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)u.structs.DrawColor.R / 256, (float)u.structs.DrawColor.G / 256, (float)u.structs.DrawColor.B / 256, (float)u.structs.DrawColor.A / 256)));
                        break;

                    case "point":
                        /// _device.DrawRectangle(new RawRectangleF(u.structs.MapPosX + w, u.structs.MapPosZ + h, u.structs.MapPosX + w + 3, u.structs.MapPosZ + h + 3), new SolidColorBrush(_device, new RawColor4((float)u.structs.DrawColor.R / 256, (float)u.structs.DrawColor.G / 256, (float)u.structs.DrawColor.B / 256, (float)u.structs.DrawColor.A / 256)));
                        break;

                    case "circle":
                        device.DrawEllipse(new Ellipse(new RawVector2(u.structs.MapPosX + screenWidthOffset, u.structs.MapPosZ + screenHeightOffset), u.structs.Size, u.structs.Size), new SolidColorBrush(device, new RawColor4((float)u.structs.DrawColor.R / 256, (float)u.structs.DrawColor.G / 256, (float)u.structs.DrawColor.B / 256, (float)u.structs.DrawColor.A / 256)));
                        break;

                    case "rectangle":
                        device.DrawRectangle(new RawRectangleF(u.structs.MapPosX + screenWidthOffset + 0.5f, u.structs.MapPosZ + screenHeightOffset + 0.5f, u.structs.MapPosXend + screenWidthOffset + 0.5f, u.structs.MapPosZend + screenHeightOffset + 0.5f), new SolidColorBrush(device, new RawColor4((float)u.structs.DrawColor.R / 256, (float)u.structs.DrawColor.G / 256, (float)u.structs.DrawColor.B / 256, (float)u.structs.DrawColor.A / 256)));
                        break;

                    default:
                        break;
                }
            });

            /// Draw icons? Nah...

            // Font stuff
            float fontSize = 13;
            string fontFamily = "Arial Unicode MS";
            HelperOpenGL.OverlayText.ForEach(u =>
            {
                Size measure = System.Windows.Forms.TextRenderer.MeasureText(u.structs.Text, new System.Drawing.Font(fontFamily, fontSize - 3));
                if (u.structs.TextOutline)
                {
                    var darkerColor = System.Windows.Forms.ControlPaint.Dark(u.structs.DrawColor);
                    device.DrawText(u.structs.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.structs.MapPosX + screenWidthOffset - 1, u.structs.MapPosZ + screenHeightOffset - 1, u.structs.MapPosX + measure.Width + screenWidthOffset, u.structs.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                    device.DrawText(u.structs.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.structs.MapPosX + screenWidthOffset + 1, u.structs.MapPosZ + screenHeightOffset + 1, u.structs.MapPosX + measure.Width + screenWidthOffset, u.structs.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                    device.DrawText(u.structs.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.structs.MapPosX + screenWidthOffset + 1, u.structs.MapPosZ + screenHeightOffset - 1, u.structs.MapPosX + measure.Width + screenWidthOffset, u.structs.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                    device.DrawText(u.structs.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.structs.MapPosX + screenWidthOffset - 1, u.structs.MapPosZ + screenHeightOffset + 1, u.structs.MapPosX + measure.Width + screenWidthOffset, u.structs.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                }

                device.DrawText(u.structs.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.structs.MapPosX + screenWidthOffset, u.structs.MapPosZ + screenHeightOffset, u.structs.MapPosX + measure.Width + screenWidthOffset, u.structs.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)u.structs.DrawColor.R / 256, (float)u.structs.DrawColor.G / 256, (float)u.structs.DrawColor.B / 256, (float)u.structs.DrawColor.A / 256)));
            });

            // And clear as we dont need it anymore.
            HelperOpenGL.OverlayGeometry.Clear();
            HelperOpenGL.OpenglOverlayIcons.Clear();
            HelperOpenGL.OverlayText.Clear();
        }

        private bool WorldToScreen(OpenTK.Vector3 enemy, out OpenTK.Vector3 screen, System.Numerics.Matrix4x4? matrix, int width, int height)
        {
            screen = new OpenTK.Vector3(0, 0, 0);
            Numerics.Matrix4x4 temp = Numerics.Matrix4x4.Transpose((Numerics.Matrix4x4)matrix);

            OpenTK.Vector3 translationVector = new OpenTK.Vector3(temp.M41, temp.M42, temp.M43);
            OpenTK.Vector3 up = new OpenTK.Vector3(temp.M21, temp.M22, temp.M23);
            OpenTK.Vector3 right = new OpenTK.Vector3(temp.M11, temp.M12, temp.M13);

            float w = D3DXVec3Dot(translationVector, enemy) + temp.M44;

            if (w < 0.098f)
            {
                return false;
            }

            float y = D3DXVec3Dot(up, enemy) + temp.M24;
            float x = D3DXVec3Dot(right, enemy) + temp.M14;

            screen.X = (((Width / 2) * (1f + (x / w))) - (Width / 2)) * coeffResolutionTo;
            screen.Y = (((Height / 2) * (1f - (y / w))) - (Height / 2)) * coeffResolutionTo;
            screen.Z = w;
            return true;
        }

        private float D3DXVec3Dot(OpenTK.Vector3 a, OpenTK.Vector3 b)
        {
            return (a.X * b.X) +
                   (a.Y * b.Y) +
                   (a.Z * b.Z);
        }

        private NativeMethods.Rectangle GetTargetProcessWindowData()
        {
            // What to capture.
            if (windowsSourceHandle == IntPtr.Zero)
            {
                windowsSourceHandle = GetWindowHandle(Settings.ProcessNameScreen);
            }

            // Get capture area.
            NativeMethods.GetWindowRect(windowsSourceHandle, out NativeMethods.Rectangle captureAreaTemp);

            // We need to get real area of what we need to capture, excluding window title border etc. So getting the correction.
            NativeMethods.ClientToScreen(windowsSourceHandle, out NativeMethods.Point pnt);

            // Left and right borders of the window if it has any
            var windowMetricsCorrectionX = (pnt.x - captureAreaTemp.left) * 2;

            // Caption + bottom border of the window if it has any
            var windowMetricsCorrectionY = (pnt.y - captureAreaTemp.top) + (pnt.x - captureAreaTemp.left);

            // Calculate the window size taking offsets from 0,0 and also applying correction.
            var rect = new NativeMethods.Rectangle
            {
                left = captureAreaTemp.left + (pnt.x - captureAreaTemp.left),
                right = (captureAreaTemp.right - captureAreaTemp.left) - windowMetricsCorrectionX,
                top = captureAreaTemp.top + (pnt.y - captureAreaTemp.top),
                bottom = (captureAreaTemp.bottom - captureAreaTemp.top) - windowMetricsCorrectionY
            };

            return rect;
        }

        private IntPtr GetWindowHandle(string windowName)
        {
            IntPtr hwnd = IntPtr.Zero;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower().Contains(windowName))
                {
                    hwnd = process.MainWindowHandle;
                }
            }

            return hwnd;
        }

        internal struct Margins
        {
            public int Left, Right, Top, Bottom;
        }
    }
}
