using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PatchFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootDirectory = Directory.GetCurrentDirectory();
            string outputFilePath = Path.Combine(rootDirectory, "patch.json");

            Console.WriteLine("Scanning files in the current directory...");

            try
            {
                PatchFile patchFile = ScanDirectory(rootDirectory, outputFilePath);

                // Serialize to JSON and save
                string jsonResult = JsonSerializer.Serialize(patchFile, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(outputFilePath, jsonResult);

                Console.WriteLine($"Patch file created successfully at: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Scans the directory, excluding specific files
        static PatchFile ScanDirectory(string rootDirectory, string outputFilePath)
        {
            List<FileEntry> fileList = new();
            long totalSize = 0;
            int fileCount = 0;

            // Identify the currently running executable file
            string currentExePath = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
            string currentExeName = Path.GetFileName(currentExePath);

            // Identify the output patch file name
            string patchFileName = Path.GetFileName(outputFilePath);

            // Define exclusion patterns
            Regex launcherRegex = new Regex("launcher.*\\.exe", RegexOptions.IgnoreCase);

            string[] allFiles = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);

            foreach (var filePath in allFiles)
            {
                string fileName = Path.GetFileName(filePath);

                // Exclusion conditions
                if (fileName.Equals(currentExeName, StringComparison.OrdinalIgnoreCase) || // Currently running exe
                    fileName.Equals(patchFileName, StringComparison.OrdinalIgnoreCase) ||   // patch.json output file
                    fileName.Equals("config.xml", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals("SKGen.dll", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals("makeupdate.exe", StringComparison.OrdinalIgnoreCase) ||
                    launcherRegex.IsMatch(fileName) ||
                    fileName.IndexOf("patch", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    fileName.EndsWith(".pdb", StringComparison.OrdinalIgnoreCase) ||
                    fileName.EndsWith(".config", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Add file details
                string relativePath = Path.GetRelativePath(rootDirectory, filePath);
                long fileSize = new FileInfo(filePath).Length;
                string fileHash = ComputeSHA256Hash(filePath);

                fileList.Add(new FileEntry
                {
                    Path = relativePath.Replace('\\', '/'), // Standardize path format
                    Size = fileSize,
                    Hash = fileHash
                });

                fileCount++;
                totalSize += fileSize;

                Console.WriteLine($"Processed: {relativePath} (Size: {fileSize} bytes)");
            }

            return new PatchFile
            {
                Files = fileList,
                TotalFiles = fileCount,
                TotalSize = totalSize
            };
        }

        // Computes the SHA256 hash of a file
        static string ComputeSHA256Hash(string filePath)
        {
            using FileStream fs = File.OpenRead(filePath);
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(fs);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }

    // Class representing a file entry
    public class FileEntry
    {
        public required string Path { get; set; }   // Relative file path (required)
        public long Size { get; set; }              // File size in bytes
        public required string Hash { get; set; }   // SHA256 hash of the file (required)
    }

    // Class representing the full patch file
    public class PatchFile
    {
        public List<FileEntry> Files { get; set; } = new List<FileEntry>();
        public int TotalFiles { get; set; }        // Number of files
        public long TotalSize { get; set; }        // Total size in bytes
    }
}
