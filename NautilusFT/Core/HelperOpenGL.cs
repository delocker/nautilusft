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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QuickFont;

namespace NautilusFT
{
    public static class HelperOpenGL
    {
        public static MemoryStream openglOverlayImage;

        public static bool PlayerListToRenderComplete = false;
        public static bool LootListToRenderComplete = false;

        public static List<EntityPlayerListRender> PlayerListToRenderPre = new List<EntityPlayerListRender>();
        public static List<EntityPlayerListRender> PlayerListToRenderFinal = new List<EntityPlayerListRender>();

        // Dictionary to reference our fontsize.
        private static Dictionary<int, QFont> fontDictionary;

        // Adressing textures when we load them.
        private static int openglMapTextureMap, openglMapTextureIcons;

        public static List<EntityLootListRender> LootListToRenderPre { get; set; } = new List<EntityLootListRender>();
        public static List<EntityLootListRender> LootListToRenderFinal { get; set; } = new List<EntityLootListRender>();
        public static List<EntityPlayerListRender> MapMakerDots { get; set; } = new List<EntityPlayerListRender>();

        public static List<RenderItem> OverlayText { get; set; } = new List<RenderItem>();
        public static List<RenderItem> OpenglOverlayIcons { get; set; } = new List<RenderItem>();
        public static List<RenderItem> OverlayGeometry { get; set; } = new List<RenderItem>();

        public static List<RenderItem> OpenglMapText { get; set; } = new List<RenderItem>();
        public static List<RenderItem> OpenglMapIcons { get; set; } = new List<RenderItem>();
        public static List<RenderItem> OpenglMapGeometry { get; set; } = new List<RenderItem>();

        public static int OpenglOverlayImageWidth { get; set; }

        public static int OpenglOverlayImageHeight { get; set; }

        public static void GlControl_DrawDotCircle(int size, float coord_X, float coord_Y, Color clr)
        {
            var radius = (float)size;
            GL.LoadIdentity();
            GL.Enable(EnableCap.Blend);
            GL.Color4(clr);
            GL.PointSize(1);
            GL.Begin(BeginMode.Points);

            for (int i = 0; i < 360; i++)
            {
                if (Helper.IsOdd(i))
                {
                    var degInRad = i * 3.1416 / 180;
                    GL.Vertex2((Math.Cos(degInRad) * radius) + coord_X, (Math.Sin(degInRad) * radius) + coord_Y);
                }
            }

            GL.Color4(Color.Transparent);
            GL.Disable(EnableCap.Blend);
            GL.End();
        }

        public static void GlControl_LoadMapTexture(string textureName)
        {
            GL.DeleteTexture(openglMapTextureMap);
            Utilities.TexLib.AddTexture(@"data\map_" + textureName + ".jpg", openglMapTextureMap, 0);
        }

        public static void GlControl_LoadTextures()
        {
            var map_texture_icons_file = new Bitmap(Image.FromFile(@"data\icons.png"));
            map_texture_icons_file.Dispose();

            // Default Textures their int values go to variables as they are static. TODO Dictionary if there are more.
            Utilities.TexLib.LoadTexture(@"data\icons.png", out openglMapTextureIcons, 1);
            Utilities.TexLib.LoadTexture(@"data\map_---.jpg", out openglMapTextureMap, 0);
        }

        public static void GlControl_GenerateFonts()
        {
            var builderConfig = new QFontBuilderConfiguration(true);

            // Reduce blur radius because font is very small.
            builderConfig.ShadowConfig.blurRadius = 0;
            builderConfig.TextGenerationRenderHint = TextGenerationRenderHint.SingleBitPerPixelGridFit;
            builderConfig.ShadowConfig.blurPasses = 0;

            // Charset: Russian + English. Need better way to get close to unicode.
            builderConfig.charSet = Settings.Charset;

            /* http://www.opentk.com/node/2628?page=3
             * Your best bet for non-latin text is to use System.Drawing:
             * render text on a Bitmap, upload it to an OpenGL texture and display that on screen. */

            var fontPath = "fonts/Ubuntu-R.ttf";

            fontDictionary = new Dictionary<int, QFont>();

            for (int i = 5; i <= 15; i++)
            {
                fontDictionary.Add(i, new QFont(fontPath, i, FontStyle.Bold, builderConfig));
                fontDictionary[i].Options.DropShadowActive = false;
            }
        }

        public static void GlControl_DrawIcons(string icon, float coord_X, float coord_Y, Color clr, float rotation)
        {
            GL.PushAttrib(AttribMask.EnableBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, openglMapTextureIcons);
            GL.Color4(clr);
            GL.Enable(EnableCap.Blend);
            GL.Translate(coord_X, coord_Y, 0);

            // Rotate and close rortate.
            GL.Rotate(rotation, Vector3.UnitZ);

            GL.Begin(BeginMode.Quads);

            // Get min for 1 pixel offset.
            var scale_start = (double)1 / 260;

            int icon_pos = (int)Enum.Parse(typeof(IconPositionTexture), icon);

            var map_SizeIcon = Settings.MapSizeIcon;

            var icon_pos_row = 0;
            var icon_pos_column = 0;

            // Arma 3 leftovers.
            if (icon_pos >= 1 && icon_pos <= 4)
            {
                // Player npc here
                icon_pos_column = icon_pos - 1;
                icon_pos_row = 0;
            }

            if (icon_pos >= 5 && icon_pos <= 8)
            {
                // Vehicles + Other
                icon_pos_column = icon_pos - 5;
                icon_pos_row = 1;
            }

            if (icon_pos >= 9 && icon_pos <= 12)
            {
                // Other
                icon_pos_column = icon_pos - 9;
                icon_pos_row = 2;
            }

            if (icon_pos >= 13 && icon_pos <= 16)
            {
                icon_pos_column = icon_pos - 13;

                // Other
                icon_pos_row = 3;
            }

            // 65 is icon size in texture.
            var scale_coeff = (float)scale_start * 65;

            var icon_offset_x_start = scale_coeff * icon_pos_column;
            var icon_offset_y_start = scale_coeff * icon_pos_row;

            var icon_offset_x_end = (scale_coeff * icon_pos_column) + scale_coeff;
            var icon_offset_y_end = (scale_coeff * icon_pos_row) + scale_coeff;

            var icon_w = (260 * scale_coeff / 2) * map_SizeIcon / 10;
            var icon_h = (260 * scale_coeff / 2) * map_SizeIcon / 10;

            /* X.Y.
            * Left Top ( - | + )
            * Left Bottom ( - | - )
            * Right Top ( + | + )
            * Right Bottom ( + | - )
            */

            GL.TexCoord2(icon_offset_x_start, icon_offset_y_start);
            GL.Vertex2(-icon_w, icon_h);
            GL.TexCoord2(icon_offset_x_start, icon_offset_y_end);
            GL.Vertex2(-icon_w, -icon_h);
            GL.TexCoord2(icon_offset_x_end, icon_offset_y_end);
            GL.Vertex2(icon_w, -icon_h);
            GL.TexCoord2(icon_offset_x_end, icon_offset_y_start);
            GL.Vertex2(icon_w, icon_h);
            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Rotate(-rotation, Vector3.UnitZ);
            GL.Translate(-coord_X, -coord_Y, 0);

            // Unbind texture when we are done with it.
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.PopAttrib();

            GL.End();
        }

        public static void GlControl_Draw_Map(int openglControlMapDragOffsetX, int openglControlMapDragOffsetZ)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, openglMapTextureMap);
            GL.Color3(Color.Transparent);

            /* X.Y.
            * Left Top ( - | + )
            * Left Bottom ( - | - )
            * Right Top ( + | + )
            * Right Bottom ( + | - )
            */

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0f, 0f);
            GL.Vertex2((-Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetX, (Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetZ);
            GL.TexCoord2(0f, 1f);
            GL.Vertex2((-Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetX, (-Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetZ);
            GL.TexCoord2(1f, 1f);
            GL.Vertex2((Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetX, (-Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetZ);
            GL.TexCoord2(1f, 0f);
            GL.Vertex2((Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetX, (Settings.MapCanvasSize * Settings.MapZoomLevelFromZoomLevels) + openglControlMapDragOffsetZ);
            GL.End();

            GL.Disable(EnableCap.Texture2D);

            // Unbind texture when we are done with it.
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public static void GlControl_SetupViewport(int width, int height)
        {
            GL.MatrixMode(MatrixMode.Projection);

            var projection = Matrix4.CreateOrthographic(width, height, -1f, 1f);

            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Use all of the glControl painting area
            GL.Viewport(0, 0, width, height);
        }

        public static void GlControl_Refresh(GLControl glcontrol)
        {
            // Force on paint event.
            glcontrol.Invalidate();
        }

        public static void GlControl_MakeCurrent(GLControl glcontrol)
        {
            glcontrol.MakeCurrent();
        }

        public static void GlControl_DrawStart(bool overlay = false)
        {
            // Clear the buffers
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            if (overlay)
            {
                GL.ClearColor(Color.YellowGreen);
            }
            else
            {
                GL.ClearColor(Color.Turquoise);
            }
        }

        public static void GlControl_Draw_End(GLControl glcontrol)
        {
            GL.Flush();
            glcontrol.SwapBuffers();
            if (Settings.Screenshot)
            {
                if ((DateTime.Now - Settings.ScreenShotTime).Milliseconds > 500)
                {
                    Settings.ScreenShotTime = DateTime.Now;
                    Image img = TakeScreenshot(glcontrol);

                    var codecs = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo codec in codecs)
                    {
                        if (codec.MimeType == "image/jpeg")
                        {
                            ici = codec;
                        }
                    }

                    //path where you need to save
                    //img.Save(@"\\0.0.0.0\screenshot.jpg", ImageFormat.Jpeg);
                }
            }
        }

        public static void GlControl_DrawPoint(int size, float coord_X, float coord_Y, Color clr)
        {
            GL.LoadIdentity();
            GL.PointSize(size);
            GL.Begin(BeginMode.Points);
            GL.Color4(clr);
            GL.Vertex2(coord_X, coord_Y);
            GL.Color4(Color.Transparent);
            GL.End();
        }

        public static void GlControl_DrawText(string text, Color clr, int size, float locX, float locY, bool outline = false)
        {
            GL.Rotate(180, Vector3.UnitX);

            /* X.Y.
             * Top Left ( - | + )
             * Top Right ( - | - )
             * Bottom Right ( + | - )
             * Bottom Left ( + | + ) */

            if (outline)
            {
                fontDictionary[size].Options.Colour = System.Windows.Forms.ControlPaint.Dark(clr);
                fontDictionary[size].Print(text, new Vector2((int)Math.Round(locX) - 1, ((int)Math.Round(locY) + 1) * -1));
                fontDictionary[size].Print(text, new Vector2((int)Math.Round(locX) - 1, ((int)Math.Round(locY) - 1) * -1));
                fontDictionary[size].Print(text, new Vector2((int)Math.Round(locX) + 1, ((int)Math.Round(locY) - 1) * -1));
                fontDictionary[size].Print(text, new Vector2((int)Math.Round(locX) + 1, ((int)Math.Round(locY) + 1) * -1));
            }

            fontDictionary[size].Options.Colour = clr;
            fontDictionary[size].Print(text, new Vector2((int)Math.Round(locX), (int)Math.Round(locY) * -1));

            GL.Rotate(-180, Vector3.UnitX);
            GL.Color4(Color.Transparent);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }

        public static void GlControl_DrawCircle(int size, float coord_X, float coord_Y, Color clr)
        {
            float radius = size;
            var length = 360;
            GL.LoadIdentity();
            GL.Enable(EnableCap.Blend);
            GL.Color4(clr);
            GL.LineWidth(2);
            GL.Begin(BeginMode.LineLoop);

            for (int i = 0; i < length; i++)
            {
                var degInRad = i * 3.1416 / 180;
                GL.Vertex2((Math.Cos(degInRad) * radius) + coord_X, (Math.Sin(degInRad) * radius) + coord_Y);
            }

            /// GL.Color4(Color.Transparent);
            GL.Disable(EnableCap.Blend);
            GL.End();
        }

        public static void GlControl_DrawCircleFill(int size, float coord_X, float coord_Y, Color clr)
        {
            var radius = (float)size;
            var length = 360;

            GL.LoadIdentity();
            GL.Enable(EnableCap.Blend);
            GL.Color4(clr);
            GL.Begin(BeginMode.TriangleFan);

            for (int i = 0; i < length; i++)
            {
                var degInRad = i * 3.1416 / 180;
                GL.Vertex2((Math.Cos(degInRad) * radius) + coord_X, (Math.Sin(degInRad) * radius) + coord_Y);
            }

            GL.Disable(EnableCap.Blend);
            GL.End();
        }

        public static void GlControl_DrawLines(int size, float coord_X, float coord_Y, float coord_X_end, float coord_Y_end, Color clr)
        {
            GL.Enable(EnableCap.Blend);
            GL.Color4(clr);
            GL.LineWidth(size);
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y_end);
            GL.Color4(Color.Transparent);
            GL.Disable(EnableCap.Blend);
            GL.End();
        }

        public static void GlControl_DrawLinesStripped(int size, float coord_X, float coord_Y, float coord_X_end, float coord_Y_end, Color clr, bool invert = false)
        {
            GL.PushAttrib(AttribMask.EnableBit);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.LineStipple);
            GL.Color4(clr);
            GL.LineWidth(size);

            // Line mask.
            if (invert)
            {
                GL.LineStipple(1, Convert.ToInt16("1111000000000000", 2));
            }
            else
            {
                GL.LineStipple(1, Convert.ToInt16("0000111100000000", 2));
            }

            GL.Begin(BeginMode.Lines);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y_end);
            GL.Color4(Color.Transparent);
            GL.Disable(EnableCap.LineStipple);
            GL.Disable(EnableCap.Blend);
            GL.End();
            GL.PopAttrib();
        }

        public static void GlControl_DrawRectangle(int size, float coord_X, float coord_Y, float coord_X_end, float coord_Y_end, Color clr)
        {
            GL.Enable(EnableCap.Blend);
            GL.Color4(clr);
            GL.LineWidth(size);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y_end);
            GL.Vertex2(coord_X, coord_Y_end);
            GL.Color4(Color.Transparent);
            GL.Disable(EnableCap.Blend);
            GL.End();
        }

        private static Bitmap TakeScreenshot(GLControl glcontrol)
        {
            if (GraphicsContext.CurrentContext == null)
            {
                throw new GraphicsContextMissingException();
            }

            var w = glcontrol.ClientSize.Width;
            var h = glcontrol.ClientSize.Height;

            var bmp = new Bitmap(w, h);
            var data =
                bmp.LockBits(glcontrol.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, w, h, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
    }
}
