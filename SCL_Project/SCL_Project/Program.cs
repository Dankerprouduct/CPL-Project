using System;
using System.Diagnostics;

namespace SCL_Project // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 1)
            {
                // improper usage
                Debug.WriteLine("Improper Usage");
            }
            else if (args.Length == 1)
            {
                // run script
                RunFile(args[0]);
            }
            else
            {
                RunFile("welcome.scl");
            }
        }

        private static void RunFile(string path)
        {
            string source = File.ReadAllText(Path.GetFullPath(path)); 
            
            Run(source);
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
    }
}