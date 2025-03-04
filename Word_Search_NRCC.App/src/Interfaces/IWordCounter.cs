using System.Collections.Generic;

//Word Counter interface
public interface IWordCounter
{
    Dictionary<string, int> CountWords(IEnumerable<string> filePaths, int? maxThreds, bool ignoreCase = true);
}
