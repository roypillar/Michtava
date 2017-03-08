using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTextBox
{
    public interface ISmartTextBox
    {

        int GetNumberOfConnectors(string text);

        int GetNumberOfWords(string text);

        int getNumOfAllowedWords();
        void getNumOfAllowedWords(int newNum);

        //  public List<string> GetKeySentences(); for now we get it from the policy

        IDictionary<string, int> GetRepeatedWords(string text);

        string SuggestAlternativeWord(string word);

        string CheckPunctuatioMarks(string text);

    }
}
