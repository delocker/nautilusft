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
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NautilusFT
{
    public static class Helper
    {
        public static float myPosXingame, myPosZingame, myPosYingame;

        public static ReaderServiceClient ClientReader { get; set; }

        public static float D3DXVec3Dot(Vector3 a, Vector3 b)
        {
            return (a.X * b.X) +
                    (a.Y * b.Y) +
                    (a.Z * b.Z);
        }

        public static float GetDistance(Vector3 v1, Vector3 v2)
        {
            Vector3 difference = new Vector3(
              v1.X - v2.X,
              v1.Y - v2.Y,
              v1.Z - v2.Z);

            double distance = Math.Sqrt(
                              Math.Pow(difference.X, 2f) +
                              Math.Pow(difference.Y, 2f) +
                              Math.Pow(difference.Z, 2f));

            return (float)distance;
        }

        public static double GetAnglePoints(double p1x, double p1y, double p2x, double p2y, double p3x, double p3y)
        {
            double numerator = (p2y * (p1x - p3x)) + (p1y * (p3x - p2x)) + (p3y * (p2x - p1x));
            double denominator = ((p2x - p1x) * (p1x - p3x)) + ((p2y - p1y) * (p1y - p3y));
            double ratio = numerator / denominator;

            double angleRad = Math.Atan(ratio);
            double angleDeg = (angleRad * 180) / Math.PI;

            if (angleDeg < 0)
            {
                angleDeg = 180 + angleDeg;
            }

            return angleDeg;
        }

        public static double GetAngleVectors(Vector3 start, Vector3 first, Vector3 second)
        {
            double numerator = (first.Y * (start.X - second.X)) + (start.Y * (second.X - first.X)) + (second.Y * (first.X - start.X));
            double denominator = ((first.X - start.X) * (start.X - second.X)) + ((first.Y - start.Y) * (start.Y - second.Y));
            double ratio = numerator / denominator;

            double angleRad = Math.Atan(ratio);
            double angleDeg = (angleRad * 180) / Math.PI;

            if (angleDeg < 0)
            {
                angleDeg = 180 + angleDeg;
            }

            return angleDeg;
        }

        public static int GetRandomNumber(int minNumber, int maxNumber)
        {
            if (minNumber < 1 || maxNumber < 1)
            {
                throw new ArgumentException("The maxNumber value should be greater than 1");
            }

            var b = new byte[4];
            using (var rngCSP = new RNGCryptoServiceProvider())
            {
                rngCSP.GetBytes(b);
                var seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
                var r = new Random(seed);
                return r.Next(minNumber, maxNumber);
            }
        }

        public static string GetRandomStringStrong(int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(Convert.ToChar(GetRandomNumber(1, 255)));
            }

            return sb.ToString();
        }

        public static string GetStringFromMemory(IntPtr address, bool unicode, int size = 32)
        {
            var stringBytes = Memory.ReadBytes(address.ToInt64(), size);
            var clearBytes = new List<byte>();

            var stop = false;
            for (int i = 0; i < stringBytes.Count(); i++)
            {
                // Universal check for UNICODE, if 0x0 bytes began to appear one after another, string is ended, break right now
                if (stringBytes[i] == 0x0 && unicode && stop)
                {
                    break;
                }

                // First 0x0 found, bool = stop, so code above breaks us from loop, but if next char is not 0x0, remove stop so code above doesn`t fire. Dumb but it works.
                stop = stringBytes[i] == 0x0 && unicode && !stop ? true : false;

                // If we are reading ASCII and see at least one 0x0, string is ended, break right now
                // If we dont see 0x0 bytes in bytes array, we can read it
                if (stringBytes[i] == 0x0 && !unicode)
                {
                    break;
                }

                clearBytes.Add(stringBytes[i]);
            }

            // If we somehow read only 0x0, we return n/a string that we can use.
            if (clearBytes.Count <= 1)
            {
                return "n/a";
            }

            // If it is NOT dividable by 2 so we return even numbers we add blank byte
            if (clearBytes.Count % 2 != 0 && unicode)
            {
                clearBytes.Add(0x00);
            }

            return unicode ? Encoding.Unicode.GetString(clearBytes.ToArray()) : Encoding.ASCII.GetString(clearBytes.ToArray());
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static String RegexTextReplace(string input)
        {
            /* HOW TO
             * http://rextester.com/SXXB8348
             * https://stackoverflow.com/questions/33319936/characters-are-not-escaped-properly-in-a-dictionary
            */

            var regex = new Regex(string.Join("|", Settings.ListTextReplace.Keys.Select(key => Regex.Escape(key))));
            var replaced = regex.Replace(input, m => Settings.ListTextReplace[m.Value]);

            return replaced;
        }

        public static bool WorldToScreen(Vector3 enemyObject, out Vector3 screenObject, GLControl glcontrol, System.Numerics.Matrix4x4? matrix)
        {
            // Found in Glumi`s code but also exists somewhere on UC
            screenObject = new Vector3(0, 0, 0);
            var temp = System.Numerics.Matrix4x4.Transpose((System.Numerics.Matrix4x4)matrix);
            var translationVector = new Vector3(temp.M41, temp.M42, temp.M43);
            var up = new Vector3(temp.M21, temp.M22, temp.M23);
            var right = new Vector3(temp.M11, temp.M12, temp.M13);

            var w = Helper.D3DXVec3Dot(translationVector, enemyObject) + temp.M44;

            // If the object is behind and out of view.
            if (w < 0.098f)
            {
                return false;
            }

            var y = D3DXVec3Dot(up, enemyObject) + temp.M24;
            var x = D3DXVec3Dot(right, enemyObject) + temp.M14;

            // I dont know why i have to substact glcontrol dimensions again, probably projection issue between D3DX and OpenGL.
            // Dont want to fix for now, maybe someday. it works right now and im happy
            screenObject.X = ((glcontrol.Width / 2) * (1f + (x / w))) - (glcontrol.Width / 2);
            screenObject.Y = (((glcontrol.Height / 2) * (1f - (y / w))) - (glcontrol.Height / 2)) * -1;
            screenObject.Z = w;

            return true;
        }
    }
}