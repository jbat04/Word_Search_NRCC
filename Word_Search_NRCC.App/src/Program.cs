using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Word_Search_NRCC.src.Interfaces;

class Program
{
    static void Main()
    {

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? folderPath = config?["TextFilesFolder"]; // Read folder path from config
        bool ignoreCaseFlag = bool.TryParse(config?["IgnoreCaseFlag"], out bool parsedFlag) && parsedFlag;
        int? maxThreadsOption = int.TryParse(config?["MaxThreadsOption"], out int maxThreads) ? maxThreads : null;


        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Error: Folder '{folderPath}' does not exist.");
            return;
        }

        // Get all .txt files in the folder
        var filePaths = Directory.GetFiles(folderPath, "*.txt");

        if (filePaths.Length == 0)
        {
            Console.WriteLine("No text files found in the folder.");
            return;
        }

        IFileReader fileReader = new FileReaderService();
        IWordCounter wordCounter = new WordCounterService(fileReader);

        try
        {
            var wordCountResult = wordCounter.CountWords(filePaths, maxThreadsOption, ignoreCaseFlag);
            Console.WriteLine("\nWord Frequency:");
            foreach (var uniqueWordCount in wordCountResult)
            {
                Console.WriteLine($"{uniqueWordCount.Key}: {uniqueWordCount.Value}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
