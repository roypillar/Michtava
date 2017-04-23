using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTextBox
{
    public class Connectors
    {
        private static List<string> AddingConnectors = new List<string> { "ו", "בנוסף", "ורק", "גם", "וכן" }; //מילות קישור הוספה וצירוף
        private static List<string> TimeConnectors = new List<string> { "בזמן ש", "בשעה ש", "בטרם", "במשך", "כאשר" }; //מילות קישור זמן
        private static List<string> OppositConnectors = new List<string> { "אלא", "עם זאת", "אולם", "אבל", "בעוד" , "אך"}; //מילות קישור ניגוד
        private static List<string> ReasonConnectors = new List<string> { "לאור", "בשל", "שכן", "כי" }; //מילות קישור סיבה
        private static List<string> ConseqConnectors = new List<string> { "לכן", "לפיכך", "לאור זאת", "מכאן ש", "כך ש" }; //מילות קישור תוצאה
        private static List<string> PurposeConnectors = new List<string> { "במטרה ש", "לכבוד ה", "כדי ש" }; //מילות קישור תכלית(מטרה)ההה
        private static List<string> ComparisonConnectors = new List<string> { "פחות מ", "כאילו", "כמו" }; //מילות קישור השוואה
        private static List<string> ConditionConnectors = new List<string> { "במקרה", "אם" }; //מילות קישור תנאי
        // private static List<string> Addingconnectors = new List<string> { "ו", "כאשר", "אבל", "כי", "גם" }; 
        //מילות קישור ויתור- ברווחים.. נטפל בזה אחר כך..
        private static List<string> OptionConnectors = new List<string> { "לחילופין", "או" }; //מילות קישור ברירה
        private static List<string> ExplanationConnectors = new List<string> { "כגון", "לדוגמא", "למשל", "כלומר" }; //מילות קישור הסבר
        private static List<string> IncludeConnectors = new List<string> { "רק", "מלבד", "למעט", "כולל", "בעיקר", "ביחוד", "בייחוד" }; //מילות קישור הכללה

        private List<List<string>> connectors = new List<List<string>>();

        public Connectors()
        {
            connectors.Add(AddingConnectors);
            connectors.Add(TimeConnectors);
            connectors.Add(OppositConnectors);
            connectors.Add(ReasonConnectors);
            connectors.Add(ConseqConnectors);
            connectors.Add(PurposeConnectors);
            connectors.Add(ComparisonConnectors);
            connectors.Add(ConditionConnectors);
            connectors.Add(OppositConnectors);
            connectors.Add(ExplanationConnectors);
            connectors.Add(IncludeConnectors);

        }
        public string SuggestAlternativeWord(string word)
        {


            foreach (List<string> connector in connectors)
            {

                if (connector.Contains(word))
                {
                    try
                    {
                        int tmp = connector.IndexOf(word);
                        if (tmp >= 2)
                        {
                            return connector.ElementAt(tmp - 1);
                        }
                        return connector.ElementAt(tmp + 1);
                    }
                    catch
                    {
                        return "כרגע יש מילים חלופיות רק למילות קישור..";
                    }
                }

            }
            return word;

        }

        public Boolean Contains(string word)
        {

            foreach (List<string> connector in connectors)
            {

                if (connector.Contains(word))
                {
                    return true;
                }

            }
            return false;
        }

    }
}