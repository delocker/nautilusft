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
using System.Windows.Forms;

namespace NautilusFT
{
    public static class Program
    {
        public static FormMain mainForm;

        [STAThread]
        private static void Main()
        {
            // This code is kept in case we decide to embed assembly ever again.
            /// EmbeddedAssembly.Load("NautilusFT.dll.QuickFont.dll", "QuickFont.dll");
            /// EmbeddedAssembly.Load("NautilusFT.dll.OpenTK.GLControl.dll", "OpenTK.GLControl.dll");
            /// EmbeddedAssembly.Load("NautilusFT.dll.OpenTK.dll", "OpenTK.dll");
            /// EmbeddedAssembly.Load("NautilusFT.dll.UnityEngine.dll", "UnityEngine.dll");
            /// EmbeddedAssembly.Load("NautilusFT.dll.ObjectListView.dll", "ObjectListView.dll");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainForm = new FormMain();

            mainForm.Show();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Application.Run();
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }
    }
}