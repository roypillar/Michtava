using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartTextBox;

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
            _text = "";
        }

        [TestMethod]
        public void GetNumberOfWordsTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "זו היא בדיקה פשוטה מאוד שבאה להראות כמה מילים יש, לכן אני מאמין שיהיו 16 מילים";
            Assert.AreEqual(16, _smartTextBox.GetNumberOfWords(_text));
            Assert.AreEqual(50, _smartTextBox.getNumOfAllowedWords());
        
        }

        [TestMethod]
        public void GetRepeatedWordsTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "";
        }

        [TestMethod]
        public void SuggestAlternativeWordTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "";
        }

        [TestMethod]
        public void CheckPunctuatioMarksTests()
        {
            _smartTextBox = new SmartTextBoxImpl();
            _text = "";
        }
    }
}
