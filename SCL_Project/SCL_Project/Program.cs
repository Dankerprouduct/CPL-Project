using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace SCL_Project // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static Environment Environment = new Environment();

        private static Interpreter Interpreter = new Interpreter();
        public static bool HadRuntimeError = false;

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
                //Console.WriteLine(token);
            }

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.Parse();

            if(HadRuntimeError)
                return;

            ASTPrinter printer = new ASTPrinter();
            foreach (Stmt stmt in statements)
            {
                if (stmt != null)
                {
                    Console.WriteLine(printer.Print(stmt));
                }
            }

            //Interpreter.Interpret(statements);

            //Console.WriteLine(expr.ToString());
            Console.ReadKey();
        }
    }
}