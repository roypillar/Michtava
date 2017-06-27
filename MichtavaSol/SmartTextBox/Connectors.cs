using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTextBox
{
    public class Connectors
    {
        private static List<string> AddingConnectors = new List<string> { "בנוסף", "ורק", "גם", "וכן","וגם" ,"אף","כמו כן","ראשית","שנית","תחילה","לבסוף","למעלה מכך","יתרה מכך","אך ורק","מעבר לכך","נוסף על כך","מלבד","למעט", "ולבסוף", "ולמעלה מכך", "ויתרה מכך", "ואך ורק", "ומעבר לכך", "ובנוסף על כך","ונוסף על כך", "ומלבד", "ולמעט" }; //מילות קישור הוספה וצירוף
        private static List<string> TimeConnectors = new List<string> { "בזמן", "בשעה", "בטרם", "במשך", "כאשר" , "בזמן", "ובשעה", "ובטרם", "ובמשך", "וכאשר","לפני","עד","במשך","בינתיים","כעבור","לאחרונה","עכשיו","עתה", "ולפני", "ועד", "ובמשך", "ובינתיים", "וכעבור", "ולאחרונה", "ועכשיו", "ועתה" }; //מילות קישור זמן
        private static List<string> OppositConnectors = new List<string> { "אלא", "עם זאת", "אולם", "אבל", "בעוד" , "אך","ואולם","ועם זאת","ברם","לעומת זאת","בניגוד","ואילו","להפך","מצד אחד","מצד שני","מצד אחר","אמנם","מנגד","כנגד", "ולעומת זאת", "ובניגוד",  "ולהפך", "ומצד שני", "ומצד אחר", "ואמנם", "ומנגד", "וכנגד" }; //מילות קישור ניגוד
        private static List<string> ReasonConnectors = new List<string> { "לאור", "בשל", "שכן", "כי", "ולאור", "ובשל","מחמת","משום","מפני","מאחר","היות","כיוון","הודות","בגלל","עקב","בעקבות","בזכות", "ומחמת", "ומשום", "ומפני", "ומאחר", "והיות", "וכיוון", "והודות", "ובגלל", "ועקב", "ובעקבות", "ובזכות" }; //מילות קישור סיבה
        private static List<string> ConseqConnectors = new List<string> { "לכן", "לפיכך", "לאור זאת", "מכאן", "כך", "ולכן", "ולפיכך", "ולאור זאת", "ומכאן", "וכך","אם כן","אפוא","על כן","כתוצאה מכך","משום כך", "ואם כן", "ואפוא", "ועל כן", "וכתוצאה מכך", "ומשום כך" }; //מילות קישור תוצאה
        private static List<string> PurposeConnectors = new List<string> { "במטרה", "לכבוד", "כדי" , "ובמטרה", "ולכבוד", "וכדי","על מנת","למען","בשביל","פן","לבל","שמא", "ועל מנת", "ולמען", "ובשביל", "ופן", "ולבל", "ושמא" }; //מילות קישור תכלית(מטרה)ההה
        private static List<string> ComparisonConnectors = new List<string> { "פחות מ", "כאילו", "כמו", "ופחות מ", "וכאילו", "וכמו","בדומה","כשם","כפי","כך גם", "ובדומה", "וכשם", "וכפי", "וכך גם" }; //מילות קישור השוואה
        private static List<string> ConditionConnectors = new List<string> { "במקרה", "אם", "ובמקרה", "ואם","לו","בתנאי","אילולא","אלמלא","לולא","אילו", "ולו", "ובתנאי", "ואילולא", "ואלמלא", "ולולא", "ואילו" }; //מילות קישור תנאי
        private static List<string> Addingconnectors = new List<string> {"כאשר", "אבל", "כי", "גם","וכאשר", "וגם" ,"אף על פי כן","על אף","אף על פי","על אף","אפילו","בכל אופן","למרות זאת","חרף","אם כי","גם אם", "ואף על פי כן", "ועל אף", "ואף על פי", "ועל אף", "ואפילו", "ובכל אופן", "ולמרות זאת", "וחרף", "ואם כי", "וגם אם" };         //מילות קישור ויתור- ברווחים.. ..
        private static List<string> OptionConnectors = new List<string> { "לחילופין", "או" ,"ולחילופין"}; //מילות קישור ברירה
        private static List<string> ExplanationConnectors = new List<string> { "כגון", "לדוגמא", "למשל", "כלומר","זאת אומרת","דהיינו","במילים אחרות","משתמע מזאת","פירושו של דבר","לשון אחרת","ולמשל", "וזאת אומרת", "ובמילים אחרות", "ומשתמע מזאת","מתשמע מכך","ומשתמע מכך", "ופירושו של דבר", "ולשון אחרת" }; //מילות קישור הסבר
        private static List<string> IncludeConnectors = new List<string> { "רק", "מלבד", "למעט", "כולל", "בעיקר", "ביחוד", "בייחוד" , "ורק", "ומלבד", "ולמעט", "וכולל", "ובעיקר", "וביחוד", "ובייחוד" }; //מילות קישור הכללה

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
            connectors.Add(Addingconnectors);
            connectors.Add(OptionConnectors);

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