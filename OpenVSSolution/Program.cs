﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

// See https://davemateer.com/coding/2018/11/08/Publish-dot-net-core-console-application.html for how to publish and distribute

namespace OpenVSSolution
{
    class Program
    {
        static void Main()
        {
            var currentPath = Directory.GetCurrentDirectory();

            // Get the most recently accessed solution file or return null if none
            var slnfile = new DirectoryInfo(currentPath).GetFiles()
                .Where(x => x.Extension == ".sln")
                .OrderBy(x => x.LastAccessTimeUtc)
                .FirstOrDefault();
            if (slnfile == null)
            {
                Console.WriteLine("No .sln file found");
                return;
            }

            // Where is VS - Community or Enterprise?
            var devenvpath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\";
            var vsdiretory = new DirectoryInfo(devenvpath).GetDirectories();

            if (vsdiretory.Any(x => x.Name == "Community"))
                devenvpath += @"Community\Common7\IDE\";
            else if (vsdiretory.Any(x => x.Name == "Enterprise"))
                devenvpath += @"Enterprise\Common7\IDE\";
            else
            {
                Console.WriteLine($"Neither Visual Studio Community nor Enterprise can be found in {devenvpath}");
                return;
            }

            // Call VS in a new process and return to the shell
            Console.WriteLine($"{slnfile.Name,-20} : Opening this file! "); 
            var proc = new Process();
            proc.StartInfo.FileName = devenvpath + "devenv";
            proc.StartInfo.Arguments = currentPath + @"\" + slnfile.Name;
            proc.Start();
        }
    }
}
