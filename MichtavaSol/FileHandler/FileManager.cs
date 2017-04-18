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
            return "לפני שנים רבות, היה אי ובו שכנו כל הרגשות: שמחה, עצבות, ידע וכל השאר - ביניהם, גם אהבה. באחד הימים, הודיעו לכל הרגשות, שהאי עומד לשקוע. כל הרגשות החלו בהכנות נמרצות לקראת העזיבה: הם תיקנו סירותיהם, ארזו חפציהם ולאחר שהכל היה מוכן, הם החלו לנטוש את האי אחד אחרי השני. כולם, מלבד אהבה, שסירבה לעזוב את האי והחליטה להישאר עד הרגע האחרון. כשהאי כמעט שקע, החלה אהבה לקרוא לעזרה. היא ראתה את עושר, ששט בסביבה על ספינת הפאר המוזהבת שלו, וקראה לו: 'עושר, האם אתה יכול לקחתני עמך?' 'לא, אינני יכול. ספינתי מלאה בזהב, כסף ויהלומים, אין מקום פנוי עבורך', השיב עושר. אהבה החליטה לבקש עזרה מגאווה, שגם היא שטה לה להנאתה אל מול חופי האי, בסירה יפהפייה: 'גאווה, בבקשה עזרי לי!' , התחננה אהבה, אך גאווה השיבה: 'אני לא יכולה לעזור לך. את רטובה כולך, ואת על'. עצבות הייתה אף היא בסביבה ואהבה קראה לה: 'עצבות, תני לי לבוא עמך!' אך עצבות השיבה' 'אהבה, אני מצטערת, אך אני כל-כך עצובה ואני רוצה להיות לבדי'. גם שמחה חלפה על פני אהבה, אולם היא הייתה כל-כך מאושרת ושמחה, שהיא אפילו לא שמה לב לקריאתה הנואשת של אהבה.";//System.IO.File.ReadAllText(fileName);
        }

        public Text GetTextById(string id)
        {
            throw new NotImplementedException();
        }

        public Text UploadText(string serverUploadDirPath, Subject subject, string txtName, string txtContent)
        {
            Guid txtFileName = Guid.NewGuid();
            string FileNameToSave = txtFileName + "_" + txtName;//changing small things here to try getting rid of database error
            string DirectoryToSave = Path.Combine(serverUploadDirPath, subject.Name);

            CreateIfMissing(DirectoryToSave);

            string pathToSave = Path.Combine(DirectoryToSave, FileNameToSave);
            File.WriteAllText(pathToSave, txtContent);

            Text uploadedTxt = new Text();
            //uploadedTxt.Id = txtID;
            uploadedTxt.FileName = FileNameToSave;//changed this line
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
