// Dependency monitor Command line tool
// Alessio Marziali 2021

using System;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Collections.Generic;

namespace dependency_monitor
{
    static class Program
    {
        /// <summary>
        /// GitHub API Personal access token
        /// </summary>
        private const string Token = "<YOUR-TOKEN-HERE>";
        /// <summary>
        /// Analysis folder used to download/unzip and search Dependencies
        /// </summary>
        private const string OutputZipAnalysisFolder = @"<YOUR-LOCAL-PATH-HERE>";
        /// <summary>
        /// Vulnerable dependency
        /// </summary>
        private static string ReferenceToLookFor { get; set; }

        /// <summary>
        /// Remove analysis folder from disk when done
        /// </summary>
        private const bool DeleteAfterAnalysis = true;

        static void Main(string[] args)
        {

            if (args[0].ToLower().Equals("-batchscan"))
            {
                ReferenceToLookFor = args[2];
                // Batch Scan mode 
                BatchScanRepositories(args);
            }

            else
            {
                ReferenceToLookFor = args[2];
                // Single Repository scan mode
                CheckArguments(args);
                // Download Repository using Github Apis
                var output = DownloadRepository(args[0], args[1]);
                // Perform Dependency analysis and output results
                ProcessLocalZipArchive(output, args[1]);
                Console.WriteLine("Done");
            }
        }

        /// <summary>
        /// Scan multiple repositories in one go. Repositories are stored inside the "repositories.txt" file
        /// </summary>
        /// <param name="args"></param>
        private static void BatchScanRepositories(IReadOnlyList<string> args)
        {
            var output = File.ReadAllText("repositories.txt");
            var repositoryNames = output.Split("\n");
            
            foreach (var repositoryName in repositoryNames)
            {
                var _repositoryName = repositoryName;
                
                if (repositoryName.EndsWith("\r"))
                {
                    _repositoryName = repositoryName.Replace("\r", "");
                }
                
                var downloadRepository = DownloadRepository(args[1], _repositoryName);
                ProcessLocalZipArchive(downloadRepository, _repositoryName);
                System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
            }
            Console.WriteLine("Batch scan mode complete");
        }
        
        /// <summary>
        /// Helper method to handle Arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool CheckArguments(string[] args)
        {
            if (!Token.Equals("<YOUR-TOKEN>")) return true;
            Console.WriteLine("[ERROR] Github API token not set!");
            return false;
        }

        /// <summary>
        /// Download private repository in zip format using GitHub APIs
        /// </summary>
        /// <param name="organisation">Name of the organisation or user</param>
        /// <param name="repositoryName">Name of the repository</param>
        /// <returns></returns>
        private static string DownloadRepository(string organisation, string repositoryName)
        {
            var url = "https://github.com/"+ organisation+ "/" + repositoryName + "/archive/master.zip";
            var path = OutputZipAnalysisFolder + repositoryName + ".zip";
            var outputAnalysisFolder = new DirectoryInfo(OutputZipAnalysisFolder);
            
            if (!outputAnalysisFolder.Exists) { outputAnalysisFolder.Create(); }

            using (var client = new System.Net.Http.HttpClient())
            {
                var credentials = Token;
                credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                var contents = client.GetByteArrayAsync(url).Result;
                File.WriteAllBytes(path, contents);
            }
            return path;
        }

        /// <summary>
        /// Method to process GitHub repositories downloaded as ZIP file
        /// </summary>
        /// <param name="args"></param>
        private static void ProcessLocalZipArchive(string zipLocation, string repositoryName)
        {
            UnzipRepository(zipLocation, OutputZipAnalysisFolder);
            var projectFiles = ReturnAllFiles(OutputZipAnalysisFolder);
            
            var headerMessage = "Found "+ projectFiles.Count +" C# project(s) in repository " + repositoryName;
            printHeaderMessage(headerMessage);
            
            // For each C# project file found perform analysis
            foreach (var csProjectFile in projectFiles)
            {
                GetDependencies(ReferenceToLookFor, csProjectFile);
            }

            // Remove analysis folder when done
            if (DeleteAfterAnalysis)
            {
                DeleteAnalysisFolder();
            }
        }

        /// <summary>
        /// Delete analysis folder from the disk at the end of the process
        /// </summary>
        private static void DeleteAnalysisFolder()
        {
            var di = new DirectoryInfo(OutputZipAnalysisFolder);
                foreach (var file in di.GetFiles()) { file.Delete(); }
                foreach (var dir in di.GetDirectories()) {  dir.Delete(true); }
            di.Delete();
        }

        /// <summary>
        /// Return all dependencies found in the project file
        /// </summary>
        /// <returns></returns>
        private static void GetDependencies(string targetReferenceName, string path)
        {
            var vulnerable_counter = 0;
            var nonvulnerable_counter = 0;
            Console.WriteLine("Dependencies in repository: ");
            Console.WriteLine();
            using (var file = new StreamReader(path))
            {
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    if (line != null && line.Trim().StartsWith("<PackageReference Include="))
                    {
                        line = line.Trim().Replace("<PackageReference Include=", "Dependency name = ");
                        line = line.Replace("/>", "");
                        
                        if (line.ToLower().Contains(targetReferenceName.ToLower()))
                        {
                            vulnerable_counter++;
                            Console.WriteLine("- " + line);
                        }
                        else
                        {
                            nonvulnerable_counter++;
                            Console.WriteLine("- " + line);    
                        }
                    }
                }
            }

            if (nonvulnerable_counter == 0 && vulnerable_counter == 0)
            {
                Console.WriteLine();
                Console.WriteLine("[OK] No dependencies in project.");
                Console.WriteLine();
            }
            else
            {
                if (vulnerable_counter <= 0) return;
                Console.WriteLine();
                Console.WriteLine("[WARNING] Vulnerable dependency found {0} time(s)" , vulnerable_counter.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Unzip repository archieve using .NET built-in library
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="output"></param>
        private static void UnzipRepository(string zipFilePath, string output)
        {
            ZipFile.ExtractToDirectory(zipFilePath, output);
        }

        /// <summary>
        /// Return all C# project files in the repository
        /// </summary>
        /// <param name="sDir"></param>
        /// <returns></returns>
        private static List<String> ReturnAllFiles(string sDir)
        {
            var files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(sDir).Where(f => f.EndsWith(".csproj")));
                foreach (var d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(ReturnAllFiles(d));
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return files;
        }
        
        /// <summary>
        /// Helper method for headers
        /// </summary>
        /// <param name="headerMessage"></param>
        private static void printHeaderMessage(string headerMessage)
        {
            Console.WriteLine();
            printHeaderLine(headerMessage);
            Console.WriteLine();
            Console.WriteLine(headerMessage);
            printHeaderLine(headerMessage);
            Console.WriteLine();
        }
        
        /// <summary>
        /// Helper method for headers
        /// </summary>
        /// <param name="headerMessage"></param>
        private static void printHeaderLine(string headerMessage)
        {
            for (int i = 0; i < headerMessage.Length; i++)
            {
                Console.Write("-");
            }
        }
    }
}