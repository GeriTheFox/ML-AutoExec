using System;
using System.IO;
using System.Collections.Generic;
using MelonLoader;

[assembly: MelonInfo(typeof(AutoExec.main), "AutoExec", "1.1", "Geri")]
[assembly: MelonColor(ConsoleColor.Yellow)]
[assembly: MelonGame]

namespace AutoExec
{
    public class main : MelonMod
    {
        public static MelonLogger.Instance Logger;
        public override void OnApplicationStart()
        {
            Logger = new MelonLogger.Instance("AutoExec", ConsoleColor.Yellow);

            if (Directory.Exists("autoexec"))
            {
                Logger.Msg("autoexec directory found!");
                if (File.Exists(@"autoexec\autoexec.config"))
                {
                    Logger.Msg("config found!");
                    exec(readconfig());
                }
                else
                {
                    try
                    {
                        using (FileStream fs = File.Create(@"autoexec\autoexec.config"));
                        string createText = "#put your file extensions here\n#exe\n#jar\nbat" + Environment.NewLine;
                        File.WriteAllText(@"autoexec\autoexec.config", createText);
                    }
                    catch
                    {
                        Logger.Msg("Something went wrong...");
                    }
                    finally
                    {
                        exec(readconfig());
                    }
                }
            }
            else
            {
                Logger.Msg("autoexec directory not found, we will create one for you...");
                try
                {
                    Directory.CreateDirectory("autoexec");
                    using (FileStream fs = File.Create(@"autoexec\autoexec.config"));
                    string createText = "#put your file extensions here\n#exe\n#jar\nbat" + Environment.NewLine;
                    File.WriteAllText(@"autoexec\autoexec.config", createText);
                }
                catch
                {
                    Logger.Msg("Something went wrong...");
                }
                finally
                {
                    exec(readconfig());
                }
            }
            Logger.Msg("Initialized");
        }

        public static void exec(List<string> ext)
        {
            foreach (string a in ext)
            {
                string[] files = Directory.GetFiles(@"autoexec\", "*."+a, SearchOption.AllDirectories);
                foreach(string b in files)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(b);
                        Logger.Msg("Program/Script '" + b + "' started");
                    }
                    catch
                    {
                        Logger.Msg("Something went wrong...");
                    }
                }
            }
        }

        public static List<string> readconfig()
        {
            List<string> list = new List<string>();

            string[] lines = System.IO.File.ReadAllLines(@"autoexec\autoexec.config");

            foreach (string line in lines)
            {
                if(line[0] != '#')
                {
                    list.Add(line);
                }
            }
            return list;
        }
    }
}
