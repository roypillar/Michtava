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
            _text = "";
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
