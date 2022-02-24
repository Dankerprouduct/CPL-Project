using System;

namespace SCL_Project // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 1)
            {
                // improper usage
            }
            else if (args.Length == 1)
            {
                // run script
                RunFile(args[0]);
            }
            else
            {
                
            }
        }

        private static void RunFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path)); 
            
            Run(BitConverter.ToString(bytes));
        }

        private static void Run(string source)
        {
            
        }
    }
}