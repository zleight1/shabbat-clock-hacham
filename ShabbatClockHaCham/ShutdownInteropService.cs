using System;
using System.Diagnostics;

namespace ShabbatClockHaCham
{
    public class ShutdownInteropService
    {
        internal void InvokeShutdown()
        {
#if DEBUG
            Debug.WriteLine("Shutdown!");
#else
            System.Diagnostics.Process processToStart = new System.Diagnostics.Process();
            processToStart.StartInfo.FileName = "shutdown.exe";
            processToStart.StartInfo.WorkingDirectory = @"C:\Windows\System32\";
            processToStart.Start();
#endif
        }
    }
}