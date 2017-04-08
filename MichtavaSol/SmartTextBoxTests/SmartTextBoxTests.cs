using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartTextBox;
using System.Collections.Generic;

namespace SmartTextBoxTests
{
    [TestClass]
    public class SmartTextBoxTests
    {
        private ISmartTextBox _smartTextBox;
        private string _text;

        [TestMethod]
        public void GetNumberOfConnectorsTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "את מספר מילות הקישור נספור כאשר מיד פעם נזרוק מילה כמו זו אולם ספרתי כבר 3";
            Assert.AreEqual(3, _smartTextBox.GetNumberOfConnectors(_text));
            _text = "כמו אבל אולם נספור כך, בשל לאור לפיכך כבר 6";
            Assert.AreEqual(6, _smartTextBox.GetNumberOfConnectors(_text));
            _text = "במשפט הזה אני לא אשים שום מילת חיבור";
            Assert.AreEqual(0, _smartTextBox.GetNumberOfConnectors(_text));

        }

        [TestMethod]
        public void SetGetNumberOfAllowedWordsTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            Assert.AreEqual(50, _smartTextBox.getNumOfAllowedWords());
            _smartTextBox.setNumOfAllowedWords(30);
            Assert.AreEqual(30, _smartTextBox.getNumOfAllowedWords());
            //מקרי קצה..
            _smartTextBox.setNumOfAllowedWords(0);
            Assert.AreEqual(0, _smartTextBox.getNumOfAllowedWords());
            _smartTextBox.setNumOfAllowedWords(2000);
            Assert.AreEqual(2000, _smartTextBox.getNumOfAllowedWords());

        }

        [TestMethod]
        public void GetNumberOfWordsTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "זו היא בדיקה פשוטה מאוד שבאה להראות כמה מילים יש, לכן אני מאמין שיהיו 16 מילים";
            Assert.AreEqual(16, _smartTextBox.GetNumberOfWords(_text));
            _text = "רק על ידי מאמצים מאוחדים של המדינה... של אנשים המוכנים למאמץ התנדבותי גדול, של נוער בעל רוח נועזת המעוררת גבורה עילאית, של מדענים המשוחררים מכבלי המחשבה המקובלת, המוכשרים לבחון לעומק את בעיותיה הייחודיות של ארץ זו.. יעלה בידנו להצליח לעמוד במשימה הכבירה של פיתוח הדרום והנגב.";
            Assert.AreEqual(46, _smartTextBox.GetNumberOfWords(_text));
            //מקרי קצה מינימום ומקסימום 
            _text = "מילה";
            Assert.AreEqual(1, _smartTextBox.GetNumberOfWords(_text));
            _text = "";
            Assert.AreEqual(0, _smartTextBox.GetNumberOfWords(_text));

            for (int i = 0; i < 1000; i++)
            {
                _text = _text + "חמשת אלפים מילים בסוף היום,";
            }
            _text = _text + "סוף.";

            Assert.AreEqual(5001, _smartTextBox.GetNumberOfWords(_text));

        }

        [TestMethod]
        public void GetRepeatedWordsTests()
        {
            _smartTextBox = new SmartTextBoxImpl();             
            _text = "נתחיל מבדיקה בה אין אף מילה אשר חוזרת על עצמה עצמה יותר יותר יותר יותר מפעם אחת.";
            IDictionary<string, int> wordList = _smartTextBox.GetRepeatedWords(_text);
            Assert.IsTrue(wordList["עצמה"] == 2);
            Assert.IsTrue(wordList["יותר"] == 4);
            Assert.IsTrue(wordList["אין"] == 1);
            Assert.IsTrue(wordList["אחת"] == 1);
            
            for (int i = 0; i < 10; i++)
            {
                _text = _text + " הלילה אף אחד לא יושב פה, ";
                if (i % 2 == 0)
                {
                    _text = _text + " במקום זוגי ";
                }
                if (i % 3 == 0)
                {
                    _text = _text + " עם שלישיה ";
                }
             
            }
            _text = _text + "לא יכול להפסיק לזוז";
            wordList = _smartTextBox.GetRepeatedWords(_text);
            
            Assert.IsTrue(wordList["לא"] == 11);
            Assert.IsTrue(wordList["הלילה"] == 10);
            Assert.IsTrue(wordList["לזוז"] == 1);
            Assert.IsTrue(wordList["זוגי"] == 5);
            Assert.IsTrue(wordList["שלישיה"] == 4);
            
        }

        [TestMethod]
        public void SuggestAlternativeWordTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
             Connectors conn = new Connectors();
            _text = "בנוסף";
            List<string> AddingConnectors = new List<string> { "ו", "בנוסף", "ורק", "גם", "וכן" }; //מילות קישור הוספה וצירוף
            string suggestedWord = _smartTextBox.SuggestAlternativeWord(_text);
            //בודק גם שבחרנו מסוג מילת הקישור הנכונה, גם שלא הצענו אותה במקום עצמה  
            Assert.IsTrue(conn.Contains(_text));
            Assert.IsTrue(AddingConnectors.Contains(_text));
            Assert.IsTrue(AddingConnectors.Contains(suggestedWord));
            Assert.IsTrue(suggestedWord != _text);

        }

        [TestMethod]
        public void CheckPunctuatioMarksTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "משפט רגיל.ואז";
           // Assert.AreEqual("wrong", _smartTextBox.CheckPunctuatioMarks(_text));
        }

        [TestMethod]
        public void RemoveUnnecessarySpacesTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "   בואו    נתחיל      ממשהו קל";
             Assert.AreEqual("בואו נתחיל ממשהו קל", _smartTextBox.RemoveUnnecessarySpaces(_text));
             _text = "נמשיך עם משהו .  קצת יותר   מרווח    .";
             Assert.AreEqual("נמשיך עם משהו . קצת יותר מרווח .", _smartTextBox.RemoveUnnecessarySpaces(_text));

        }
    }
}


