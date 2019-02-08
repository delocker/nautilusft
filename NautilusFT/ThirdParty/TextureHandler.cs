// http://www.opentk.com/node/3219
// Original source
// Reworked a little

using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Utilities
{
    public class TexLib
    {
        public static void LoadTexture(string pImagePath, out int tex_id_out, int filtering = 0)
        {
            GL.GenTextures(1, out tex_id_out);
            GL.BindTexture(TextureTarget.Texture2D, tex_id_out);

            Bitmap image = default(Bitmap);
            BitmapData imageData = default(BitmapData);

            image = new Bitmap(pImagePath);

            LoadTexture(image, imageData, filtering);
        }

        public static void LoadTexture(Bitmap image, BitmapData imageData, int filtering)
        {
            imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, imageData.Width, imageData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, imageData.Scan0);


            if (filtering == 1)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear); // nearest for no AA / linear AA
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear); // nearest for no AA / linear AA
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest); // nearest for no AA / linear AA
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest); // nearest for no AA / linear AA
            }

            image.UnlockBits(imageData);
            image.Dispose(); // SHOULD I USE THIS?
            GL.BindTexture(TextureTarget.Texture2D, 0); // SHOULD I USE THIS?

        }

        //    /// <summary>
        //    /// Clear the Texture Buffer List
        //    /// Enable OpenGL states for texture mapping
        //    /// </summary>
        //    public static void Initialize()
        //    {
        //        mTexCache = new Dictionary<string, int>();
        //        GL.Enable(EnableCap.Texture2D);
        //    }




        //    /// <summary>
        //    /// Add an image to the texture cache held by OpenGL
        //    /// </summary>
        //    /// <param name="pImageName"></param>
        //    /// <param name="pImagePath"></param>

        public static void AddTexture(MemoryStream ms, int tex_id_out, int filtering = 0)
        {
            GL.DeleteTexture(tex_id_out);
            GL.BindTexture(TextureTarget.Texture2D, tex_id_out);

            Bitmap image = default(Bitmap);
            BitmapData imageData = default(BitmapData);

            image = new Bitmap(Image.FromStream(ms));

            AddTexture(image, imageData, filtering);
        }

        public static void AddTexture(string pImagePath, int tex_id_out, int filtering = 0)
        {
            GL.DeleteTexture(tex_id_out);
            GL.BindTexture(TextureTarget.Texture2D, tex_id_out);

            Bitmap image = default(Bitmap);
            BitmapData imageData = default(BitmapData);

            image = new Bitmap(pImagePath);

            AddTexture(image, imageData, filtering);
        }

        public static void AddTexture(Bitmap image, BitmapData imageData, int tex_id_out, int filtering = 0)
        {
            // It is necessary so that when image is missing we don`t crash and just stop loading the texture.
            // Occurs for example on Overlay resizing and we can ignore that for a split second that we see black screen.
            try
            {
                imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            }
            catch
            {
                return;
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, imageData.Width, imageData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, imageData.Scan0);


            if (filtering == 1)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear); //nearest for no AA / linear AA
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear); //nearest for no AA / linear AA
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest); //nearest for no AA / linear AA
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest); //nearest for no AA / linear AA
            }

            image.UnlockBits(imageData);
            image.Dispose(); // SHOULD I USE THIS?
            GL.BindTexture(TextureTarget.Texture2D, 0); // SHOULD I USE THIS?


        }


        //    /// <summary>
        //    /// Removes a texture from the cache
        //    /// </summary>
        //    /// <param name="pImageName"></param>
        //    public static void RemoveTexture(string pImageName)
        //    {
        //        if (mTexCache.ContainsKey(pImageName) && GL.IsTexture(mTexCache[pImageName]))
        //        {
        //            GL.DeleteTexture(mTexCache[pImageName]);
        //            mTexCache.Remove(pImageName);
        //        }
        //        else
        //        {
        //            throw new KeyNotFoundException("TexLib Error: RemoveTexture() - Texture key not found");
        //        }
        //    }




        //    /// <summary>
        //    /// Clear all texture-related resources
        //    /// </summary>
        //    public static void ClearTextureCache()
        //    {
        //        foreach (KeyValuePair<string, int> feTexBuffer in mTexCache)
        //        {
        //            GL.DeleteTexture(feTexBuffer.Value);
        //        }
        //        mTexCache.Clear();
        //    }




        //    /// <summary>
        //    ///Return an integer value of a texture buffer object
        //    ///throws KeyNotFoundException if key can't be found
        //    /// </summary>
        //    /// <param name="pImageName"></param>
        //    public static int GetTexture(string pImageName)
        //    {
        //        if (mTexCache.ContainsKey(pImageName))
        //        {
        //            return mTexCache[pImageName];
        //        }
        //        else
        //        {
        //            throw new KeyNotFoundException("TexLib Error: GetTexture() - Texture key not found");
        //        }
        //    }




        //    /// <summary>
        //    /// display a list of textures cached
        //    /// </summary>
        //    /// <returns></returns>
        //    public static string ListTextures()
        //    {
        //        string texList = "No Textures Cached";

        //        if (mTexCache.Count > 0)
        //        {
        //            foreach (KeyValuePair<string, int> feTexBuffer in mTexCache)
        //            {
        //                texList += feTexBuffer.Value + ": " + feTexBuffer.Key + "\n";
        //            }
        //        }
        //        return texList;
        //    }

    }//END CLASS
}//END NAMESPACE