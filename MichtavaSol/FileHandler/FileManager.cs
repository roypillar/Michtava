using System;
using System.Collections.Generic;
using System.IO;
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

        Text UploadText(string serverUploadDirPath, Subject subject, string txtName, string txtContent);
    }

    public class FileManager : IFileManager
    {
        public string GetText(string fileName)
        {
            return "hard coded to magals computer..";//System.IO.File.ReadAllText(fileName);
        }

        public Text GetTextById(string id)
        {
            throw new NotImplementedException();
        }

        public Text UploadText(string serverUploadDirPath, Subject subject, string txtName, string txtContent)
        {
            string txtID = Guid.NewGuid().ToString();
            string FileNameToSave = txtID + "_" + txtName;
            string DirectoryToSave = Path.Combine(serverUploadDirPath, subject.Name);

            CreateIfMissing(DirectoryToSave);

            string pathToSave = Path.Combine(DirectoryToSave, FileNameToSave);
            File.WriteAllText(pathToSave, txtContent);

            Text uploadedTxt = new Text();
            uploadedTxt.Id = txtID;
            uploadedTxt.Name = RemoveExtension(txtName);
            uploadedTxt.FilePath = pathToSave;
            uploadedTxt.Subject = subject;
            uploadedTxt.UploadTime = DateTime.Now;

            return uploadedTxt;
        }

        private void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        private string RemoveExtension(string txtName)
        {
            if (!string.IsNullOrEmpty(txtName))
            {
                int fileExtPos = txtName.LastIndexOf(".", StringComparison.Ordinal);
                if (fileExtPos >= 0)
                    return txtName.Substring(0, fileExtPos);
            }

            return txtName;
        }
    }
}
