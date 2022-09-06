using System;
using System.IO;

namespace Kutikarn
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid number of arguments");
                Environment.Exit(0);
            }
            else
            {
                int num;
                if (int.TryParse(args[0], out num) == false)
                {
                    Console.WriteLine("invalid first argument {0}", args[0]);
                    Environment.Exit(0);
                }
                else
                {
                    if (num < 0 || num > 1)
                    {
                        Console.WriteLine("invalid first argument {0}", args[0]);
                        Environment.Exit(0);
                    }
                }
                if (Directory.Exists(args[1]) == false && File.Exists(args[1]) == false)
                {
                    Console.WriteLine("Folder or file doesn't exist {0}", args[1]);
                    Environment.Exit(0);
                }
                if (String.IsNullOrWhiteSpace(args[2]) == true)
                {
                    Console.WriteLine("Invalid blank key");
                    Environment.Exit(0);
                }

                Kutikarn cls;
                if (args.Length > 3 && args[3] == "d")
                {
                    cls = new Kutikarn(true);
                }
                else
                {
                    cls = new Kutikarn(false);
                }
                if (num == 0)
                {
                    cls.Kuti(args[1], args[2]);
                }
                else if (num == 1)
                {
                    cls.Duti(args[1], args[2]);
                }

            }
        }
    }
}
