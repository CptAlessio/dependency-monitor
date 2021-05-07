using System;
using System.IO;

namespace dependency_monitor
{
    /// <summary>
    /// Given a CSPROJ file return all dependencies used by the project
    /// </summary>
    static class Program
    {
        static void Main(string[] args)
        {
            if (!CheckArguments(args)) return;
            
            GetDependencies(args[0], args[1]);
        }

        /// <summary>
        /// Helper method to handle Arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool CheckArguments(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: missing path");
                return false;
            }
            else
            {
                if (!args[1].EndsWith(".csproj"))
                {
                    Console.WriteLine("Missing C# project file...");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Return all dependencies found in the project file
        /// </summary>
        /// <returns></returns>
        private static void GetDependencies(string targetReferenceName, string path)
        {
            // Loop through CSPROJ file and look for Dependencies
            using (var file = new StreamReader(path))
            {
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    if (line != null && line.Trim().StartsWith("<PackageReference Include="))
                    {
                        line = line.Replace("<PackageReference Include=", "Dependency=");
                        line = line.Replace("/>", "");
                        if (line.ToLower().Contains(targetReferenceName.ToLower()))
                        {
                            // Highlight vulnerable dependency in Red
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(line);
                        }
                        else
                        {
                            // Normal Values are Green
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(line);
                        }
                    }
                }
            }
        }
        
    }
}