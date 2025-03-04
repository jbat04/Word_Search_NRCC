using System.Collections.Generic;

namespace Word_Search_NRCC.src.Interfaces
{
    //File handler interface
    public interface IFileReader
    {
        IEnumerable<string> ReadLines(string filePath);
    }

}
