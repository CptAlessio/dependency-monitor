using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace dependency_monitor
{
    /// <summary>
    /// Given a CSPROJ file return all dependencies used by the project
    /// </summary>
    static class Program
    {
        static void Main(string[] args)
        {
            // ADD CODEOWNERS
            
            if (!CheckArguments(args)) return;

            // args[0]
            string zipFilePath = args[0];
            // args[1]
            string localPath = args[1];
            string referenceName = args[2];

            unzipRepository(zipFilePath, localPath);
            List<string> projectFiles = returnAllFiles(localPath);

            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Found {0} C# Project files in archive", projectFiles.Count.ToString());
            Console.WriteLine("-------------------------------------");
            Console.WriteLine();
            
            foreach (string csProjectFile in projectFiles)
            {
                GetDependencies(referenceName, csProjectFile);
            }
        }

        /// <summary>
        /// Helper method to handle Arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool CheckArguments(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("ERROR: missing path");
                return false;
            }
            else
            {
                if (!args[0].EndsWith(".zip"))
                {
                    Console.WriteLine("Missing project-code ZIP file...");
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
            int counter = 0;
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
                            counter++;
                        }
                        else
                        {
                            // Normal Values are Green
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(line);
                        }
                    }
                }
            }

            if (counter == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" - [SUCCESS] Target reference not found");
                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" - [ALERT] Target reference found {0} times" , counter.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Used to unzip repository code
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="output"></param>
        private static void unzipRepository(string zipFilePath, string output)
        {
            ZipFile.ExtractToDirectory(zipFilePath, output);
        }

        /// <summary>
        /// Return all csproject files in the solution file
        /// </summary>
        /// <param name="sDir"></param>
        /// <returns></returns>
        private static List<String> returnAllFiles(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    if (f.EndsWith(".csproj"))
                    {
                        files.Add(f);
                    }
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(returnAllFiles(d));
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return files;
        }
        
    }
}