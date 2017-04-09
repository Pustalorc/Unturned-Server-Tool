using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace ServerUpdateTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Updater & Installer for Unturned 3.0 servers.");
            Console.WriteLine("");
            if (args.Length == 1)
            {
                if (args[0].ToLower() == "-r")
                {
                    UpdateRocket();
                }
                else if (args[0].ToLower() == "-u")
                {
                    UpdateUnturned();
                }
                else
                {
                    ShowHelp();
                    return;
                }
            }
            else if (args.Length == 2)
            {
                if ((args[0].ToLower() == "-r" && args[1].ToLower() == "-u") || (args[0].ToLower() == "-u" && args[1].ToLower() == "-r"))
                {
                    UpdateRocket();
                    UpdateUnturned();
                }
                else if (args[0].ToLower() == "-i" && (args[1].ToLower() != "-r" || args[1].ToLower() != "-u" || args[1].ToLower() != "-i"))
                {
                    StartServer(args[1]);
                }
                else
                {
                    ShowHelp();
                    return;
                }
            }
            else if (args.Length == 3)
            {
                if (args[0].ToLower() == "-u" && args[1].ToLower() == "-i" && (args[2].ToLower() != "-r" || args[2].ToLower() != "-u" || args[2].ToLower() != "-i"))
                {
                    UpdateUnturned();
                    StartServer(args[2]);
                }
                else if (args[0].ToLower() == "-r" && args[1].ToLower() == "-i" && (args[2].ToLower() != "-r" || args[2].ToLower() != "-u" || args[2].ToLower() != "-i"))
                {
                    UpdateRocket();
                    StartServer(args[2]);
                }
                else if (args[0].ToLower() == "-i" && (args[1].ToLower() != "-r" || args[1].ToLower() != "-u" || args[1].ToLower() != "-i") && args[2].ToLower() == "-u")
                {
                    UpdateUnturned();
                    StartServer(args[1]);
                }
                else if (args[0].ToLower() == "-i" && (args[1].ToLower() != "-r" || args[1].ToLower() != "-u" || args[1].ToLower() != "-i") && args[2].ToLower() == "-r")
                {
                    UpdateRocket();
                    StartServer(args[1]);
                }
                else
                {
                    ShowHelp();
                    return;
                }
            }
            else if (args.Length >= 4)
            {
                if (args[0].ToLower() == "-u" && args[1].ToLower() == "-r" && args[2].ToLower() == "-i" && (args[3].ToLower() != "-r" || args[3].ToLower() != "-u" || args[3].ToLower() != "-i")
                    || args[0].ToLower() == "-r" && args[1].ToLower() == "-u" && args[2].ToLower() == "-i" && (args[3].ToLower() != "-r" || args[3].ToLower() != "-u" || args[3].ToLower() != "-i"))
                {
                    UpdateRocket();
                    UpdateUnturned();
                    StartServer(args[3]);
                }
                else if (args[0].ToLower() == "-u" && args[1].ToLower() == "-i" && (args[2].ToLower() != "-r" || args[2].ToLower() != "-u" || args[2].ToLower() != "-i") && args[3].ToLower() == "-r"
                    || args[0].ToLower() == "-r" && args[1].ToLower() == "-i" && (args[2].ToLower() != "-r" || args[2].ToLower() != "-u" || args[2].ToLower() != "-i") && args[3].ToLower() == "-u")
                {
                    UpdateRocket();
                    UpdateUnturned();
                    StartServer(args[2]);
                }
                else if (args[0].ToLower() == "-i" && (args[1].ToLower() != "-r" || args[1].ToLower() != "-u" || args[1].ToLower() != "-i") && args[2].ToLower() == "-u" && args[3].ToLower() == "-r"
                    || args[0].ToLower() == "-i" && (args[1].ToLower() != "-r" || args[1].ToLower() != "-u" || args[1].ToLower() != "-i") && args[2].ToLower() == "-r" && args[3].ToLower() == "-u")
                {
                    UpdateRocket();
                    UpdateUnturned();
                    StartServer(args[1]);
                }
                else
                {
                    ShowHelp();
                    return;
                }
            }
            else
            {
                ShowHelp();
                return;
            }
        }

        static void UpdateUnturned()
        {
            Console.WriteLine("Updating Unturned...");
            string DirTarget = Directory.GetCurrentDirectory() + "\\SteamCMD";
            string ExeTarget = Directory.GetCurrentDirectory() + "\\SteamCMD\\steamcmd.exe";
            string ZipTarget = Directory.GetCurrentDirectory() + "\\SteamCMD\\steamcmd.zip";
            if (!Directory.Exists(Path.GetFullPath(DirTarget)))
            {
                Directory.CreateDirectory(Path.GetFullPath(DirTarget));
            }
            if (!File.Exists(Path.GetFullPath(ExeTarget)))
            {
                Console.WriteLine("Steam CMD not installed. Downloading steamcmd from steam's servers.");
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", Path.GetFullPath(ZipTarget));
                }
                Console.WriteLine("Extracting zip container...");
                ZipFile.ExtractToDirectory(Path.GetFullPath(ZipTarget), Path.GetFullPath(DirTarget));
            }
            Console.WriteLine("Starting SteamCMD to update unturned...");
            string UntTarget = Path.GetFullPath(Directory.GetCurrentDirectory());
            Process SteamCMD = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.GetFullPath(ExeTarget);
            startInfo.Arguments = " +login unturnedrocksupdate force_update +force_install_dir \"" + UntTarget + "\" +app_update 304930 +exit";
            SteamCMD.StartInfo = startInfo;
            SteamCMD.Start();
            SteamCMD.WaitForExit();
            Console.WriteLine("Latest version of unturned installed!");
        }

        static void UpdateRocket()
        {
            Console.WriteLine("Updating Rocket...");
            string DirTarget = Directory.GetCurrentDirectory();
            string ZipTarget = Directory.GetCurrentDirectory() + "\\Rocket_Latest.zip";
            string RocTarget = Directory.GetCurrentDirectory() + "\\Modules\\Rocket.Unturned";
            string ScrTarget = Directory.GetCurrentDirectory() + "\\Scripts";
            using (var client = new WebClient())
            {
                Console.WriteLine("Downloading latest version of rocket...");
                client.DownloadFile("https://ci.rocketmod.net/job/Rocket.Unturned/lastSuccessfulBuild/artifact/Rocket.Unturned/bin/Release/Rocket.zip", Path.GetFullPath(ZipTarget));
            }
            if (Directory.Exists(Path.GetFullPath(RocTarget)))
            {
                Console.WriteLine("Old rocket installation found, deleting...");
                Directory.Delete(Path.GetFullPath(RocTarget), true);
            }
            if (Directory.Exists(Path.GetFullPath(ScrTarget)))
            {
                Directory.Delete(Path.GetFullPath(ScrTarget), true);
            }
            Console.WriteLine("Installing latest version of rocket...");
            ZipFile.ExtractToDirectory(Path.GetFullPath(ZipTarget), Path.GetFullPath(DirTarget));
            Console.WriteLine("Latest version of rocket installed!");
        }

        static void StartServer(string Instance)
        {
            Console.WriteLine("Starting server...");
            Process Game = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\Unturned.exe");
            startInfo.Arguments = " -batchmode -nographics +secureserver/" + Instance;
            Game.StartInfo = startInfo;
            Game.Start();
            Console.WriteLine("Server with instance name " + Instance + " successfully started.");
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Valid Arguments:");
            Console.WriteLine("-R         Updates or Installs Rocket.");
            Console.WriteLine("-U         Updates or Installs Unturned.");
            Console.WriteLine("-I         Specifies The Instance Name Of The Server To Start Up");
            Console.WriteLine("");
            Console.WriteLine("Examples:");
            Console.WriteLine("ServerUpdateTool.exe -R");
            Console.WriteLine("ServerUpdateTool.exe -U");
            Console.WriteLine("ServerUpdateTool.exe -R -U");
            Console.WriteLine("ServerUpdateTool.exe -U -R");
            Console.WriteLine("ServerUpdateTool.exe -I MyServer");
            Console.WriteLine("ServerUpdateTool.exe -R -I MyServer");
            Console.WriteLine("ServerUpdateTool.exe -I MyServer -R");
            Console.WriteLine("ServerUpdateTool.exe -U -I MyServer");
            Console.WriteLine("ServerUpdateTool.exe -I MyServer -U");
            Console.WriteLine("ServerUpdateTool.exe -U -R -I MyServer");
            Console.WriteLine("ServerUpdateTool.exe -R -U -I MyServer");
            Console.WriteLine("ServerUpdateTool.exe -U -I MyServer -R");
            Console.WriteLine("ServerUpdateTool.exe -R -I MyServer -U");
            Console.WriteLine("ServerUpdateTool.exe -I MyServer -R -U");
            Console.WriteLine("ServerUpdateTool.exe -I MyServer -U -R");
        }
    }
}
