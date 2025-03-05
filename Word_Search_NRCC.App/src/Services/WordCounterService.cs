using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Word_Search_NRCC.src.Interfaces;

public class WordCounterService : IWordCounter
{
    private readonly IFileReader _fileReader;

    //Dependency Injection
    public WordCounterService(IFileReader fileReader)
    {
        _fileReader = fileReader;
    }

    public Dictionary<string, int> CountWords(IEnumerable<string> filePaths, int? maxThreads=null, bool ignoreCase = true)
    {
        StringComparer comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        //Stores unique words and their counts, ConcurrentDictionary due to thread safety
        var wordCounts = new ConcurrentDictionary<string, int>(comparer);

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxThreads != null? (int)maxThreads : Environment.ProcessorCount 
        };

        //Improves processing of many files
        Parallel.ForEach(filePaths, parallelOptions, filePath =>
        {
            //As Parallel optimizes for large files
            foreach (var line in _fileReader.ReadLines(filePath)
                                .AsParallel()
                                .WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism))
            {
                var words = Regex.Split(line, @"\W+");
                foreach (string word in words)
                {
                    if(String.IsNullOrWhiteSpace(word)) continue;
                    wordCounts.AddOrUpdate(word, 1, (_, count) => count + 1);
                }
            }
        });

        //Sort and cast to return type(Dictionary)
        return wordCounts.OrderByDescending(wordCount => wordCount.Value).ToDictionary(wordCount => wordCount.Key, wordCount => wordCount.Value);
    }

}
