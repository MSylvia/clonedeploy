﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Helpers;

namespace BLL.Workflows
{
    public class CancelAllImagingTasks
    {
        public static bool Run()
        {
            var tftpPath = Settings.TftpPath;
            var pxePaths = new List<string>
            {
                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar,
                tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar,
                tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar,
                tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar
            };

            foreach (var pxePath in pxePaths)
            {
                var pxeFiles = Directory.GetFiles(pxePath, "01*");
                try
                {
                    foreach (var pxeFile in pxeFiles)
                    {
                        File.Delete(pxeFile);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    return false;
                }
            }

            BLL.ActiveImagingTask.DeleteAll();
            BLL.ActiveMulticastSession.DeleteAll();
          
            if (Environment.OSVersion.ToString().Contains("Unix"))
            {
                for (var x = 1; x <= 10; x++)
                {
                    try
                    {
                        var killProcInfo = new ProcessStartInfo
                        {
                            FileName = ("killall"),
                            Arguments = (" -s SIGKILL udp-sender")
                        };
                        Process.Start(killProcInfo);
                    }
                    catch
                    {
                        // ignored
                    }

                    Thread.Sleep(200);
                }
            }

            else
            {
                for (var x = 1; x <= 10; x++)
                {
                    foreach (var p in Process.GetProcessesByName("udp-sender"))
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.ToString());
                        }
                    }
                    Thread.Sleep(200);
                }
            }

            //Recreate any custom boot menu's that were just deleted
            foreach (var computer in BLL.Computer.ComputersWithCustomBootMenu())
            {
                BLL.ComputerBootMenu.CreateBootFiles(computer);
            }
            return true;
        }        
    }
}