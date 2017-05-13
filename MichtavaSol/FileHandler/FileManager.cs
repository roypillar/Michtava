﻿using System;
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
        void SaveQuestion(string serverTemporaryFilesrPath, Question question, Guid currentTeacherId, Guid relatedText);
        List<string> GetCurrentHomework(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid relatedText);
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
            string DirectoryToSave = Path.Combine(serverUploadDirPath, subject.Name);

            CreateIfMissing(DirectoryToSave);

            string pathToSave = Path.Combine(DirectoryToSave, txtName);
            File.WriteAllText(pathToSave, txtContent);

            Text uploadedTxt = new Text
            {
                FileName = txtName,
                Name = RemoveExtension(txtName),
                FilePath = pathToSave,
                Subject = subject,
                UploadTime = DateTime.Now,
                Content = txtContent
            };

            return uploadedTxt;
        }

        public void SaveQuestion(string serverTemporaryFilesrPath, Question question, Guid currentTeacherId, Guid relatedText)
        {
            string path = Path.Combine(serverTemporaryFilesrPath, currentTeacherId.ToString(), relatedText.ToString());
            CreateIfMissing(path);

            if (question != null)
            {
                File.WriteAllText(Path.Combine(path, "Content.txt"), question.Content);
                File.WriteAllText(Path.Combine(path, "MinWords.txt"), question.Policy.MinWords.ToString());
                File.WriteAllText(Path.Combine(path, "MaxWords.txt"), question.Policy.MaxWords.ToString());
                File.WriteAllText(Path.Combine(path, "MinConnectors.txt"), question.Policy.MinConnectors.ToString());
                File.WriteAllText(Path.Combine(path, "MaxConnectors.txt"), question.Policy.MaxConnectors.ToString());

                int suggetedOpeningNumber = 1;
                foreach (var suggestedOpening in question.Suggested_Openings)
                {
                    File.WriteAllText(Path.Combine(path, "suggestedOpening" + suggetedOpeningNumber + ".txt"), suggestedOpening.Content);
                    suggetedOpeningNumber++;
                }
            }
        }

        public List<string> GetCurrentHomework(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid relatedText)
        {
            string path = Path.Combine(serverTemporaryFilesrPath, currentTeacherId.ToString(), relatedText.ToString());

            if (!Directory.Exists(path) || new DirectoryInfo(path).GetFileSystemInfos().Length == 0)
            {
                return new List<string>();
            }

            return Directory.GetFiles(path, "*.txt").ToList();
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
