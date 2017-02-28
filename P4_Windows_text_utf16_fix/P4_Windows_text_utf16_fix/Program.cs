using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4_Windows_text_utf16_fix
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Arguments: <depot files path>");

                return;
            }

            string depotFilesPath = args[0];

            List<string> depotPaths = GetTextFileTypeDepotPaths(depotFilesPath);
            Console.WriteLine($"Text type file count: {depotPaths.Count}");

            int Index = 0;
            int utf16Count = 0;
            foreach (string depotPath in depotPaths)
            {
                Console.WriteLine($"{Index + 1}/{depotPaths.Count}");

                try
                {
                    string localPath = ConvertDepotPathToLocalPath(depotPath);
                    if (IsUTF16Encoded(localPath))
                    {
                        ++utf16Count;

                        ChangeFileTypeToUTF16(depotPath);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed. depot path: {depotPath}, exception: {e.Message}");
                }

                ++Index;
            }

            Console.WriteLine();
            Console.WriteLine("Completed");
            Console.WriteLine($"utf16 file count: {utf16Count}");
        }

        private static List<string> GetTextFileTypeDepotPaths(string depotFilesPath)
        {
            var textFileTypeDepotPaths = new List<string>();

            foreach (var line in RunP4CommandWithResults($"files {depotFilesPath}"))
            {
                var p4FilesResult = P4FilesResultParser.Parse(line);

                if (p4FilesResult.FileType == P4FileTypes.Text)
                {
                    textFileTypeDepotPaths.Add(p4FilesResult.DepotPath);
                }
            }

            return textFileTypeDepotPaths;
        }

        private static string ConvertDepotPathToLocalPath(string depotPath)
        {
            int indexOfLastSlash = depotPath.LastIndexOf('/');
            string fileName = depotPath.Substring(indexOfLastSlash + 1);

            string p4WhereResult = RunP4CommandWithResults($"where \"{depotPath}\"").Last();
            int firstFileNameIndex = p4WhereResult.IndexOf(fileName);
            int secondFileNameIndex = p4WhereResult.IndexOf(fileName, firstFileNameIndex + fileName.Length);

            string localPath = p4WhereResult.Substring(secondFileNameIndex + fileName.Length + 1);

            return localPath;
        }

        private static bool IsUTF16Encoded(string path)
        {
            var encoding = SimpleHelpers.FileEncoding.DetectFileEncoding(path);

            return encoding == Encoding.Unicode || encoding == Encoding.BigEndianUnicode;
        }

        private static void ChangeFileTypeToUTF16(string depotPath)
        {
            RunP4Command($"edit -t utf16 \"{depotPath}\"");
        }

        private static IEnumerable<string> RunP4CommandWithResults(string args)
        {
            var processStartInfo = new System.Diagnostics.ProcessStartInfo("p4", args);
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;

            var process = System.Diagnostics.Process.Start(processStartInfo);
            while (true)
            {
                string line = process.StandardOutput.ReadLine();
                if (line == null)
                {
                    yield break;
                }

                yield return line;
            }
        }

        private static void RunP4Command(string args)
        {
            RunP4CommandWithResults(args).ToList();
        }
    }
}
