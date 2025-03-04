using System;
using System.Collections.Generic;
using System.IO;
using Word_Search_NRCC.src.Interfaces;

public class FileReaderService : IFileReader
{
    public IEnumerable<string> ReadLines(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        return File.ReadLines(filePath); // Streams line-by-line in case of large files
    }
}
