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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NautilusFT
{
    public static class Memory
    {
        /* Reference here
         * https://msdn.microsoft.com/en-us/library/windows/desktop/ms684880(v=vs.85).aspx */

        private const int ProcessVmRead = 0x0010;
        private const int ProcessVmWrite = 0x0020;
        private const int ProcessVmOperation = 0x0008;

        private static Process targetProcess;
        private static IntPtr targetProcessHandle;

        public static int RPMCount { get; set; }

        public static void OpenTargetProcess(string processName)
        {
            int pID = -1;

            // Must reset this or can`t start again.
            targetProcessHandle = IntPtr.Zero;
            if (Settings.Standalone)
            {
                // Open handle to process
                // Get an array of processes that are found by name but get  only first one > [0].
                targetProcess = Process.GetProcessesByName(processName)[0];

                // If process is NOT null i.e. FOUND and we  our process handle is zero i.e. not used already.
                if (targetProcess != null && targetProcessHandle == IntPtr.Zero)
                {
                    // Open handle and return handle.
                    targetProcessHandle = NativeMethods.OpenProcess(ProcessVmRead | ProcessVmOperation | ProcessVmWrite, false, targetProcess.Id);
                    pID = targetProcess.Id;
                }
            }
            else
            {
                Helper.ClientReader.OpenTargetProcessRemote(processName, ref targetProcessHandle, ref pID);
            }

            Settings.GamePIDCurrent = pID;
        }

        public static void CloseTargetProcess()
        {
            if (Settings.Standalone)
            {
                // If our handle is not zero, then we got open handle that needs to close
                if (targetProcessHandle != IntPtr.Zero)
                {
                    NativeMethods.CloseHandle(targetProcessHandle);
                    targetProcessHandle = IntPtr.Zero; // set it to zero i.e. reset value
                }
            }
            else
            {
                Helper.ClientReader.CloseTargetProcessRemote();
            }
        }

        // If our open handle to the process is active. We dont need more than one too.
        public static bool HasActiveHandle()
        {
            // Handle must not be zero
            if (targetProcessHandle != IntPtr.Zero)
            {
                return true;
            }

            return false;
        }

        public static IntPtr ImageBaseAddress(string dllname = null)
        {
            if (Settings.Standalone)
            {
                // If handle closed, return zero.
                if (!HasActiveHandle())
                {
                    return IntPtr.Zero;
                }

                // If handle active return base image address of process
                var baseAddress = targetProcess.MainModule.BaseAddress;

                // If for some reason we get process module here we do that
                if (dllname != null)
                {
                    var modules = targetProcess.Modules;

                    foreach (ProcessModule procmodule in modules)
                    {
                        if (dllname == procmodule.ModuleName)
                        {
                            baseAddress = procmodule.BaseAddress;
                        }
                    }
                }
                return baseAddress;
            }
            else
            {
                var baseAddress = Helper.ClientReader.ImageBaseAddressRemote(dllname);
                return baseAddress;
            }
        }

        /* Pointer crawler, iterate through array of offsets and get a memory address that is a pointer to another address
         * Decent replacement of my ReadPointerChain for ARMA 3
         * I saw it in Glumi`s code but seems appear over the Internet */

        public static IntPtr GetPtr(IntPtr entryPointer, int[] offsets, bool readEntryPointer = true)
        {
            try
            {
                var nextPointer = entryPointer;

                for (int i = 0; i < offsets.Length; i++)
                {
                    if (!readEntryPointer && i == 0)
                    {
                        var tempPointer = nextPointer;
                        nextPointer = IntPtr.Add(tempPointer, offsets[i]);
                    }
                    else
                    {
                        var tempPointer = (IntPtr)Read<long>((long)nextPointer);
                        nextPointer = IntPtr.Add(tempPointer, offsets[i]);
                    }
                }

                return nextPointer;
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        public static T Read<T>(long address)
        {
            try
            {
                var size = Marshal.SizeOf(typeof(T));
                var buffer = new byte[size];
                var read = 0;

                ReadProcessMemoryWrapper((int)targetProcessHandle, address, ref buffer, size, ref read);

                var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                var data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

                handle.Free();
                return data;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static void Write<T>(long address, T value)
        {
            try
            {
                var size = Marshal.SizeOf(typeof(T));
                var buffer = new byte[size];
                var gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
                Marshal.Copy(gchandle.AddrOfPinnedObject(), buffer, 0, size);
                var read = 0;
                gchandle.Free();
                WriteProcessMemoryWrapper((int)targetProcessHandle, address, ref buffer, size, ref read);
            }
            catch
            {
            }
        }

        public static byte[] ReadBytes(long address, int bufferSize)
        {
            if (!HasActiveHandle())
            {
                return new byte[0];
            }

            var bytesRead = 0;
            var buffer = new byte[bufferSize];
            ReadProcessMemoryWrapper((int)targetProcessHandle, address, ref buffer, buffer.Length, ref bytesRead);

            return buffer;
        }

        public static void WriteBytes(long address, int bufferSize, byte[] buffer)
        {
            if (!HasActiveHandle())
            {
                return;
            }

            var bytesWritten = 0;
            WriteProcessMemoryWrapper((int)targetProcessHandle, address, ref buffer, buffer.Length, ref bytesWritten);
        }

        private static void ReadProcessMemoryWrapper(int processHandle, long processBaseAddress, ref byte[] biffer, int size, ref int numberOfBytesRead)
        {
            RPMCount++;
            if (Settings.Standalone)
            {
                NativeMethods.ReadProcessMemory(processHandle, processBaseAddress, biffer, size, ref numberOfBytesRead);

            }
            else
            {
                Helper.ClientReader.ReadProcessMemoryRemote(processHandle, processBaseAddress, ref biffer, size, ref numberOfBytesRead);
            }
        }

        private static void WriteProcessMemoryWrapper(int processHandle, long processBaseAddress, ref byte[] biffer, int size, ref int numberOfBytesWritten)
        {
            RPMCount++;
            if (Settings.Standalone)
            {
                NativeMethods.WriteProcessMemory(processHandle, processBaseAddress, biffer, size, ref numberOfBytesWritten);
            }
            else
            {
                Helper.ClientReader.WriteProcessMemoryRemote(processHandle, processBaseAddress, ref biffer, size, ref numberOfBytesWritten);
            }
        }
    }
}