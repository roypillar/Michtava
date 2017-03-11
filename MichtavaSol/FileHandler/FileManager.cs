using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace FileHandler
{
    public interface IFileManager
    {
        string GetText(string fileName);

        Text GetTextById(string id);
    }

    public class FileManager : IFileManager
    {
        public string GetText(string fileName)
        {
            return System.IO.File.ReadAllText(fileName);
        }

        public Text GetTextById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
