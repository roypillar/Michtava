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
        void SaveQuestion(string serverTemporaryFilesrPath, Question question, Guid currentTeacherId, Guid relatedText);
        Dictionary<int, string> GetCurrentHomework(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid relatedText);
        int GetNextQuestionNumber(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid relatedText);
        ICollection<Question> ParseQuestions(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid currentTextId);
        void ClearTemporaryQuestions(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid currentTextId);
    }

    public class FileManager : IFileManager
    {
        private Dictionary<string, string> _questionReaderTranslations = new Dictionary<string, string>
        {
            {"Content.txt", "תוכן השאלה"},
            {"MaxConnectors.txt", "מספר מקסימלי של מילות קישור"},
            {"MaxWords.txt", "מספר מקסימלי של מילים"},
            {"MinConnectors.txt", "מספר מינימלי של מילות קישור"},
            {"MinWords.txt", "מספר מינימלי של מילים"},
            {"suggestedOpening.txt", "משפט פתיחה"}
        };

        public string GetText(string fileName)
        {
            return "לפני שנים רבות, היה אי ובו שכנו כל הרגשות: שמחה, עצבות, ידע וכל השאר - ביניהם, גם אהבה. באחד הימים, הודיעו לכל הרגשות, שהאי עומד לשקוע. כל הרגשות החלו בהכנות נמרצות לקראת העזיבה: הם תיקנו סירותיהם, ארזו חפציהם ולאחר שהכל היה מוכן, הם החלו לנטוש את האי אחד אחרי השני. כולם, מלבד אהבה, שסירבה לעזוב את האי והחליטה להישאר עד הרגע האחרון. כשהאי כמעט שקע, החלה אהבה לקרוא לעזרה. היא ראתה את עושר, ששט בסביבה על ספינת הפאר המוזהבת שלו, וקראה לו: 'עושר, האם אתה יכול לקחתני עמך?' 'לא, אינני יכול. ספינתי מלאה בזהב, כסף ויהלומים, אין מקום פנוי עבורך', השיב עושר. אהבה החליטה לבקש עזרה מגאווה, שגם היא שטה לה להנאתה אל מול חופי האי, בסירה יפהפייה: 'גאווה, בבקשה עזרי לי!' , התחננה אהבה, אך גאווה השיבה: 'אני לא יכולה לעזור לך. את רטובה כולך, ואת על'. עצבות הייתה אף היא בסביבה ואהבה קראה לה: 'עצבות, תני לי לבוא עמך!' אך עצבות השיבה' 'אהבה, אני מצטערת, אך אני כל-כך עצובה ואני רוצה להיות לבדי'. גם שמחה חלפה על פני אהבה, אולם היא הייתה כל-כך מאושרת ושמחה, שהיא אפילו לא שמה לב לקריאתה הנואשת של אהבה.";
            //System.IO.File.ReadAllText(fileName);
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
            string path = Path.Combine(serverTemporaryFilesrPath, currentTeacherId.ToString(), relatedText.ToString(), question.Question_Number.ToString());
            CreateIfMissing(path);

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

        public Dictionary<int, string> GetCurrentHomework(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid relatedText)
        {
            string path = Path.Combine(serverTemporaryFilesrPath, currentTeacherId.ToString(), relatedText.ToString());
            var currentHomework = new Dictionary<int, string>();

            if (!Directory.Exists(path) || new DirectoryInfo(path).GetFileSystemInfos().Length == 0)
            {
                return currentHomework;
            }

            foreach (var questionDir in Directory.GetDirectories(path))
            {
                int questionNumber = int.Parse(new DirectoryInfo(questionDir).Name);

                currentHomework.Add(questionNumber, ReadQuestion(questionDir));
            }

            return currentHomework;
        }

        public int GetNextQuestionNumber(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid relatedText)
        {
            string path = Path.Combine(serverTemporaryFilesrPath, currentTeacherId.ToString(), relatedText.ToString());
            var currentHomework = new Dictionary<int, string>();

            if (!Directory.Exists(path) || new DirectoryInfo(path).GetFileSystemInfos().Length == 0)
            {
                return 1;
            }

            int maxQuestionNumber = 1;
            foreach (var questionDir in Directory.GetDirectories(path))
            {
                int questionNumber = int.Parse(new DirectoryInfo(questionDir).Name);

                if (questionNumber > maxQuestionNumber)
                {
                    maxQuestionNumber = questionNumber;
                }
            }

            return maxQuestionNumber + 1;
        }

        public ICollection<Question> ParseQuestions(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid currentTextId)
        {
            string path = Path.Combine(serverTemporaryFilesrPath, currentTeacherId.ToString(), currentTextId.ToString());
            List <Question> questions = new List<Question>();

            if (!Directory.Exists(path) || new DirectoryInfo(path).GetFileSystemInfos().Length == 0)
            {
                return questions;
            }

            foreach (var questionDir in Directory.GetDirectories(path))
            {
                int questionNumber = int.Parse(new DirectoryInfo(questionDir).Name);

                var question = ParseQuestion(questionDir, questionNumber);

                questions.Add(question);
            }
            return questions;
        }

        public void ClearTemporaryQuestions(string serverTemporaryFilesrPath, Guid currentTeacherId, Guid currentTextId)
        {
            string path = Path.Combine(serverTemporaryFilesrPath, currentTeacherId.ToString(), currentTextId.ToString());
            DirectoryInfo currentDir = new DirectoryInfo(path);

            foreach (DirectoryInfo directory in currentDir.GetDirectories())
            {
                directory.Delete(true);
            }
        }

        private Question ParseQuestion(string questionDir, int questionNumber)
        {
            Question ParsedQuestion = new Question();
            Policy ParsedPolicy = new Policy();

            // Parse content and Policy
            foreach (var questionParameter in Directory.GetFiles(questionDir))
            {
                switch (new DirectoryInfo(questionParameter).Name)
                {
                    case "Content.txt":
                    {
                        ParsedQuestion.Content = File.ReadAllText(questionParameter);
                        break;
                    }
                    case "MinWords.txt":
                    {
                            ParsedPolicy.MinWords = int.Parse(File.ReadAllText(questionParameter));
                            break;
                    }
                    case "MaxWords.txt":
                    {
                            ParsedPolicy.MaxWords = int.Parse(File.ReadAllText(questionParameter));
                            break;
                    }
                    case "MinConnectors.txt":
                    {
                            ParsedPolicy.MinConnectors = int.Parse(File.ReadAllText(questionParameter));
                            break;
                    }
                    case "MaxConnectors.txt":
                    {
                            ParsedPolicy.MaxConnectors = int.Parse(File.ReadAllText(questionParameter));
                            break;
                    }
                    default:
                    {
                            break;
                    }
                }
            }

            // Parse suggested openings:
            string[] suggestedOpeningFiles = Directory.GetFiles(questionDir, "suggestedOpening*.txt");
            List<string> suggestedOpenings = new List<string>();
            foreach (var suggestedOpening in suggestedOpeningFiles)
            {
                suggestedOpenings.Add(File.ReadAllText(suggestedOpening));
            }

            // Create Question:
            ParsedQuestion.Policy = ParsedPolicy;
            ParsedQuestion.Question_Number = questionNumber;
            ParsedQuestion.Date_Added = DateTime.Now;
            ParsedQuestion.Suggested_Openings = SuggestedOpening.convert(suggestedOpenings);

            return ParsedQuestion;
        }

        private string ReadQuestion(string questionPath)
        {
            string questionAsString =  "מספר שאלה: " +  new DirectoryInfo(questionPath).Name + "\r\n";

            int suggestedOpeningIndex = 1;

            foreach (var file in Directory.GetFiles(questionPath, "*.txt"))
            {
                string fileName = new DirectoryInfo(file).Name;

                if (fileName.Contains("suggestedOpening"))
                {
                    questionAsString += _questionReaderTranslations["suggestedOpening.txt"] + " " + suggestedOpeningIndex + ": " + File.ReadAllText(file) + "\r\n";
                }
                else
                {
                    questionAsString += _questionReaderTranslations[fileName] + ": " + File.ReadAllText(file) + "\r\n";
                }               
            }

            return questionAsString;
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
