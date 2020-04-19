using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace JITFreezer
{
    internal class Utils
    {
        public static void PrintLogo()
        {
            Colorful.Console.WriteLine();
            string[] JITFreezerArr = new string[]
            {
                "\t\t\t   __   _____  _____     ___             ",
                "\t\t\t   \\ \\  \\_   \\/__   \\   / __\\ __ ___  ___ _______ _ __ ",
                "\t\t\t    \\ \\  / /\\/  / /\\/  / _\\| '__/ _ \\/ _ \\_  / _ \\ '__|",
                "\t\t\t /\\_/ /\\/ /_   / /    / /  | | |  __/  __// /  __/ |   ",
                "\t\t\t \\___/\\____/   \\/     \\/   |_|  \\___|\\___/___\\___|_|   ",
            };
            int r = 220;
            int g = 20;
            int b = 60;
            for (int i = 0; i < JITFreezerArr.Length; i++)
            {
                Colorful.Console.WriteLine(JITFreezerArr[i], Color.FromArgb(r, g, b));
                g += 8;
                b += 8;
            }
            Colorful.Console.WriteLine();
        }

        /* Thanks to: 
         * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp 
         * https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processmodulecollection?view=netframework-4.8
         */

        public static bool GetCLRModule(int pID)
        {
            ProcessModuleCollection pModuleCollection = Process.GetProcessById(pID).Modules;
            for (int i = 0; i < pModuleCollection.Count; i++)
            {
                if (pModuleCollection[i].ModuleName.ToLower() == "clr.dll")
                    return true;
            }
            return false;
        }

        [Flags]
        public enum ThreadAccess : int
        {
            SUSPEND = (0x0002),
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        public static void SuspendT(int pid)
        {
            var pID = Process.GetProcessById(pid);

            if (string.IsNullOrEmpty(pID.ProcessName))
                return;

            foreach (ProcessThread pThread in pID.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND, false, (uint)pThread.Id);

                if (pOpenThread == IntPtr.Zero) continue;

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }
    }
}