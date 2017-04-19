using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTextBox
{
    public class SmartTextBoxImpl : ISmartTextBox
    {

        public SmartTextBoxImpl()
        {
            numOfAllowedWords = 50;
        }
        // private static string Text;
        private static List<string> punctuationMarks = new List<string> { ".", ",", "-", "?", "!", "<", ">", "&", "[", "]", "(", ")" };
     
        public static Connectors conn = new Connectors();

        //this is auto on 50, there is set to change it.
        //i think we need this here for the on client side checks..

        private int numOfAllowedWords;
        public int getNumOfAllowedWords() { return numOfAllowedWords; }
        public void setNumOfAllowedWords(int newNum) {numOfAllowedWords = newNum; }


       


        public IDictionary<string, int> GetRepeatedWords(string text)
        {

            string[] tokens = text.Split(' ', '.', ',', '-', '?', '!', '<', '>', '&', '[', ']', '(', ')');

            IDictionary<string, int> words = new SortedDictionary<string, int>(new CaseInsensitiveComparer());
            foreach (string word in tokens)
            {
                if (string.IsNullOrEmpty(word.Trim()))
                {
                    continue;
                }
                int count;
                if (!words.TryGetValue(word, out count))
                {
                    count = 0;
                }
                words[word] = count + 1;

            }

            return words;
        }
        //return a string from the smart text box with all the improvment..?


        public int GetNumberOfConnectors(string text)
        {
            int wordCounter = 0;

            string[] tokens = text.Split(' ', '.', ',', '-', '?', '!', '<', '>', '&', '[', ']', '(', ')');

            for (int i = 0; i < tokens.Length - 1; i++)
            {
                if (conn.Contains(tokens[i]))
                {
                    wordCounter++;
                    Console.WriteLine(tokens[i]);
                }

                else if (conn.Contains(tokens[i] + " " + tokens[i + 1]))
                {
                    wordCounter++;
                    Console.WriteLine(tokens[i] + " " + tokens[i + 1]);
                    i++;
                }

                else try
                    {
                        if (conn.Contains(tokens[i] + " " + tokens[i + 1].First()))
                        {
                            wordCounter++;
                            Console.WriteLine(tokens[i] + " " + tokens[i + 1]);
                            i++;
                        }
                    }
                    catch
                    {

                    }

            }

            if (conn.Contains(tokens.Last()))
            {
                Console.WriteLine(tokens.Last());
                wordCounter++;
            }

            return wordCounter;
        }

        public int GetNumberOfWords(string text)
        {
            int wordCounter = 0;

            string[] tokens = text.Split(' ', '.', ',', '-', '?', '!', '<', '>', '&', '[', ']', '(', ')');

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] != "")
                {
                    wordCounter++;
                }

            }
            return wordCounter;
        }


        public string SuggestAlternativeWord(string word)
        {
            return conn.SuggestAlternativeWord(word);
        }

        public bool IsConnector(string word)
        {
            return conn.Contains(word);
        }


        //לא יודע אם צריך בנפרד, מאמין שעדיף למדל עוד קצת כי לתקן פיסוק 
        // זה מספיק קשה בלי לטפל במשפט עם רווחים, אז קודם נוריד רווחים ואז נתקן בקלות יותר 
        public string RemoveUnnecessarySpaces(string word)
        {
            string ans = String.Join(" ", word.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            return ans;
        }

        //in progresss!!!
        public string CheckPunctuatioMarks(string text)
        {
            string[] sentences = text.Split('.');


            string correctString = "";


            int j;
            for (int i = 0; i < sentences.Length; i++)
            {
                Console.WriteLine("\n" + sentences[i] + "\n");

                string[] tokens = sentences[i].Split(',');

                for (j = 0; j < tokens.Length; j++)
                {
                    Console.WriteLine(tokens[j]);

                    if (tokens[j] == ".")
                    {
                        if ((j + 1) <= tokens.Length)
                        {
                            if (tokens[j + 1].First().ToString() == "ו")
                            {
                               // Console.WriteLine("vav can't come after psik !!!\n");
                                return ("wrong");
                            }
                            else
                            {
                                correctString = correctString + " " + tokens[j];
                            }
                        }

                    }
                    else
                    {
                        correctString = correctString + " " + tokens[j];
                    }
                }
                //correctString = correctString + " " + tokens[j];

            }

            return correctString;

        }

        private string CheckSentence(string sentence)
        {
            //here we need to check the logic of each sentence--> need to see how? 
            //by complete sentence or sub sentences?(נקודה או פסיק.. עד כמה נרד לכל אחד)

            return sentence;
        }
    }


    class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(String s1, string s2)
        {
            return string.Compare(s1, s2, true);
        }
    }


}
