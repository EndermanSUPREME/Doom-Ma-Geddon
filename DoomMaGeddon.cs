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
            // runs as administrator
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = @"-NoExit -Command 'Test-Path C:\Windows\System32\drivers\etc\hosts'",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}