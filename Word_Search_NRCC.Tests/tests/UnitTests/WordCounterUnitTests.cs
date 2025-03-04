using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Word_Search_NRCC.src.Interfaces;
using Xunit;

public class WordCounterUnitTests
{
    private readonly Mock<IFileReader> _mockFileReader;
    private readonly WordCounterService _wordCounter;

    public WordCounterUnitTests()
    {
        _mockFileReader = new Mock<IFileReader>();
        _wordCounter = new WordCounterService(_mockFileReader.Object);
    }

    [Fact]
    public void CountWords_ShouldCountWordsCorrectly()
    {
        //Arrange
        var filePaths = new List<string> { "file1.txt", "file2.txt" };
        _mockFileReader.Setup(fr => fr.ReadLines("file1.txt")).Returns(new List<string> { "Go and do that thing more that you do not so well" });
        _mockFileReader.Setup(fr => fr.ReadLines("file2.txt")).Returns(new List<string> { "I do many things very well" });

        //Act
        var result = _wordCounter.CountWords(filePaths);

        //Assert
        Assert.Equal(3, result["do"]);   // "do" appears thrice
        Assert.Equal(2, result["that"]); // "that" appears twice
        Assert.Equal(1, result["many"]); // "many" appears once
    }

    [Fact]
    public void CountWords_ShouldHandleCaseInsensitivity()
    {
        //Arrange
        var filePaths = new List<string> { "file1.txt" };
        _mockFileReader.Setup(fr => fr.ReadLines("file1.txt")).Returns(new List<string> { "Hello hello HELLO" });

        //Act
        var result = _wordCounter.CountWords(filePaths, ignoreCase: true);

        // Assert
        Assert.Equal(3, result["Hello"]);
    }

    [Fact]
    public void CountWords_ShouldHandleCaseSensitivity()
    {
        //Arrange
        var filePaths = new List<string> { "file1.txt" };
        _mockFileReader.Setup(fr => fr.ReadLines("file1.txt")).Returns(new List<string> { "Hello hello HELLO" });

        //Act
        var result = _wordCounter.CountWords(filePaths, ignoreCase: false);

        // Assert
        Assert.Equal(1, result["Hello"]);
    }

    [Fact]
    public void CountWords_ShouldHandleEmptyFiles()
    {
        //Arrange
        var filePaths = new List<string> { "empty.txt" };
        _mockFileReader.Setup(fr => fr.ReadLines("empty.txt")).Returns(new List<string>());

        //Act
        var result = _wordCounter.CountWords(filePaths);

        // Assert
        Assert.Empty(result); // No words should be counted
    }

    [Fact]
    public void CountWords_ShouldHandleManyWordsOnALine()
    {
        //Arrange
        var filePaths = new List<string> { "largeLines.txt" };
        var largeLines = new List<string> { string.Join(" ", Enumerable.Repeat("word", 1_000_000)) };

        _mockFileReader.Setup(fr => fr.ReadLines("largeLines.txt")).Returns(largeLines);

        //Act
        var result = _wordCounter.CountWords(filePaths);

        //Assert
        Assert.Equal(1_000_000, result["word"]);
    }

    [Fact]
    public void CountWords_ShouldHandleManyLines()
    {
        //Arrange
        var filePaths = new List<string> { "manyLinesFile.txt" };
        var manyLines = Enumerable.Repeat("word", 1_000_000).ToList();

        _mockFileReader.Setup(fr => fr.ReadLines("manyLinesFile.txt")).Returns(manyLines);

        //Act
        var result = _wordCounter.CountWords(filePaths);

        //Assert
        Assert.Equal(1_000_000, result["word"]);
    }


}
