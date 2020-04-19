using System;
using System.Diagnostics;
using System.Threading;

namespace JITFreezer
{
    internal class MainCLI
    {
        private static void Main(string[] args)
        {
            Console.Title = "JIT Freezer - Made by ZrCulillo#1998";
            Console.ForegroundColor = ConsoleColor.Red;

            if (args.Length < 1)
            {
                Utils.PrintLogo();

                for (int i = 0; i < "\n[»] Drag & Drop your native files <3.".Length; i++)
                {
                    Console.Write("\n[»] Drag & Drop your native files <3."[i].ToString());
                    Thread.Sleep(35);
                }
                Console.ReadLine();
                return;
            }

            try
            {
                int pID;
                string freezedPath = args[0];
                Process freezedProcess = new Process();
                freezedProcess.StartInfo.FileName = freezedPath;
                freezedProcess.StartInfo.CreateNoWindow = true;
                freezedProcess.StartInfo.UseShellExecute = false;
                freezedProcess.Start();
                pID = freezedProcess.Id;
                while (true)
                {
                    if (Utils.GetCLRModule(pID))
                    {
                        Utils.SuspendT(pID);
                        Utils.PrintLogo();

                        Console.WriteLine("[!] .NET has been loaded.\n");
                        Console.WriteLine("[»] Manually dump it (Ex: MegaDumper / Scylla / ExtremeDumper).");
                        Console.WriteLine("\n[!] Press Enter to kill the process.");

                        Console.ReadLine();

                        freezedProcess.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("[x] Error: \n" + e);
            }
        }
    }
}