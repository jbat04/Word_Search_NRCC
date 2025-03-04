using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Word_Search_NRCC.src.Interfaces;
using Xunit;

public class WordCounterIntegrationTests : IDisposable
{
    private readonly string _testDirectory;

    public WordCounterIntegrationTests()
    {
        // Create a temporary test directory
        _testDirectory = Path.Combine(Path.GetTempPath(), "Test_TextFiles");
        if(!Directory.Exists(_testDirectory))
            Directory.CreateDirectory(_testDirectory);
    }

    private string CreateTestFile(string fileName, string content)
    {
        string filePath = Path.Combine(_testDirectory, fileName);
        File.WriteAllText(filePath, content);
        return filePath;
    }

    [Fact]
    public void CountWords_ShouldProcessMultipleFilesCorrectly()
    {
        // Arrange
        var file1 = CreateTestFile("File1.txt", "Go and do that thing more that you do not so well");
        var file2 = CreateTestFile("File2.txt", "I do many things very well");

        var filePaths = new List<string> { file1, file2 };
        IFileReader fileReader = new FileReaderService();
        IWordCounter wordCounter = new WordCounterService(fileReader);

        // Act
        var result = wordCounter.CountWords(filePaths, null);

        // Assert
        Assert.Equal(3, result["do"]);
        Assert.Equal(1, result["thing"]);
        Assert.Equal(2, result["that"]);
        Assert.Equal(1, result["things"]);
    }

    [Fact]
    public void CountWords_ShouldHandleLargeFileEfficiently()
    {
        // Arrange
        string largeFile = CreateTestFile("largeFile.txt", string.Join(" ", Enumerable.Repeat("test", 100000)));

        var filePaths = new List<string> { largeFile };
        IFileReader fileReader = new FileReaderService();
        IWordCounter wordCounter = new WordCounterService(fileReader);

        // Act
        var result = wordCounter.CountWords(filePaths, null);

        // Assert
        Assert.Equal(100000, result["test"]);
    }

    public void Dispose()
    {
        // Cleanup
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}
