// dont work for multiple ppl, only u can use it skid!!
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace IndigoAPI
{
    public class ExploitAPI
    {
        public static string Exploit = "IndigoAPI";
        private static readonly string InjectorLink = "https://github.com/XOSPOnTop/Indigo/raw/main/injector.exe";

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        static void CloseProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch (Exception) { }
            }
        }

        static void DownloadInjector()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string outputPath = Path.Combine(currentDirectory, "injector.exe");

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri(InjectorLink), outputPath);
                }
            }
            catch (Exception ex)
            {
                Message(3, $"An error occurred while downloading the injector: {ex.Message}");
            }
        }

        static async void NOTRACIST()
        {
            string rbx = "RobloxPlayerBeta";
            string indigo = "injector";

            while (true)
            {
                if (!IsProcessRunning(rbx))
                {
                    CloseProcess(indigo);
                }

                await Task.Delay(1);
            }
        }

        public static void Attach()
        {
            NOTRACIST();
            DownloadInjector();

            Directory.CreateDirectory("workspace");
            Directory.CreateDirectory("bin");
            Directory.CreateDirectory("autoexec");

            if (File.Exists("injector.exe"))
            {
                if (IsProcessRunning("RobloxPlayerBeta"))
                {
                    if (InjectionStatus())
                    {
                        Message(2, Exploit + " is already attached");
                    }
                    else
                    {
                        STARTUPINFO si = new STARTUPINFO();
                        si.cb = (uint)Marshal.SizeOf(si);
                        PROCESS_INFORMATION pi = new PROCESS_INFORMATION();

                        try
                        {
                            if (CreateProcess(null, "injector.exe", IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref si, out pi))
                            {
                                Message(2, "Attach successful");
                                ExploitAPI.ExecuteScript(new WebClient().DownloadString("https://raw.githubusercontent.com/XOSPOnTop/Indigo/main/water.lua"));
                            }
                            else
                            {
                                Message(1, "Exception caught for attaching! " + Marshal.GetLastWin32Error());
                            }
                        }
                        catch (Exception ex)
                        {
                            Message(1, "Attach(); exception -> " + ex.Message);
                        }
                    }
                }
                else
                {
                    Message(1, "Roblox Is Not Open! Please Open Roblox Web Version");
                }
            }
            else
            {
                Message(3, "Could not find the injector\nDid you turn off antivirus?");
            }
        }

        public static void AutoAttach()
        {
            Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");
            if (processes.Length > 0)
            {
                Process proc = processes[0];
                do
                {
                    Task.Delay(1500).Wait();
                }
                while (proc.MainWindowHandle == IntPtr.Zero);

                Attach();
            }
        }

        public static void MoreUNC()
        {
            ExploitAPI.ExecuteScript(new WebClient().DownloadString("https://raw.githubusercontent.com/IndigoLLC/IndigoAPI/main/UNC/MoreUNC.lua"));
        }

        public static void UNCTest()
        {
            ExploitAPI.ExecuteScript(new WebClient().DownloadString("https://raw.githubusercontent.com/IndigoLLC/IndigoAPI/main/UNC/UNC.lua"));
        }

        public static void ExecuteScript(string editorContent)
        {
            ExecuteAsync(editorContent).ConfigureAwait(continueOnCapturedContext: false);
        }

        private static async Task ExecuteAsync(string editorContent)
        {
            try
            {
                using (ClientWebSocket ws = new ClientWebSocket())
                {
                    Uri serverUri = new Uri("ws://localhost:8050/ws");
                    await ws.ConnectAsync(serverUri, CancellationToken.None);
                    ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(editorContent));
                    await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
                    ArraySegment<byte> receivedBytes = new ArraySegment<byte>(new byte[1024]);
                    WebSocketReceiveResult result = await ws.ReceiveAsync(receivedBytes, CancellationToken.None);
                    string response = Encoding.UTF8.GetString(receivedBytes.Array, 0, result.Count);
                    Console.WriteLine("Received from server: " + response);
                }
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                Console.WriteLine("Error during execution: " + ex.Message);
            }
        }

        public static bool InjectionStatus()
        {
            Process[] processes = Process.GetProcessesByName("main");
            return processes.Length > 0;
        }

        public static void Message(int shitcase, string message)
        {
            string title = Exploit;
            MessageBoxIcon icon;

            switch (shitcase)
            {
                case 1:
                    icon = MessageBoxIcon.Warning;
                    title = Exploit + " Warning";
                    break;
                case 2:
                    icon = MessageBoxIcon.Information;
                    title = Exploit + " Information";
                    break;
                case 3:
                    icon = MessageBoxIcon.Error;
                    title = Exploit + " Error";
                    break;
                default:
                    icon = MessageBoxIcon.None;
                    title = Exploit + " Message";
                    break;
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }
}