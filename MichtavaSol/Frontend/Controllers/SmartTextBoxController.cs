using SmartTextBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class SmartTextBoxController : Controller
    {
        ISmartTextBox _smartTextBox = new SmartTextBoxImpl();
        public ActionResult SmartTextBox()
        {
            ViewBag.Title = "תיבת טקסט חכמה";

            return View();
        }

        public ActionResult AnalyzeAnswer()
        {



            // here we have to call the SmartTextBox in server side
           
            TempData["Answer"] = Temp(Request.Form["myTextBox2"]);

            return RedirectToAction("SmartTextBox");
        }

        private string Temp(string text)
        {
            
            string ans = "";

            Entities.Models.Policy policy = new Entities.Models.Policy();
            policy.Id = "1";
            policy.Min_Words = 10;
            policy.Max_Words = 20;
            policy.Min_Connectors = 2;
            policy.Max_Connectors = 5;

            Connectors conn = new Connectors();

            IDictionary<string, int> wordOccurrenceMap = _smartTextBox.GetRepeatedWords(text);
            foreach (KeyValuePair<string, int> wordEntry in wordOccurrenceMap)
            {
                if (wordEntry.Value > 3)
                {
                    ans += " " + " המילה " + "\"" + wordEntry.Key + "\"" + " הופיעה: " + wordEntry.Value + " פעמים" + "\n";
                }


            }
            ans += "\n";

            int numOfConnectors = _smartTextBox.GetNumberOfConnectors(text);
            ans += "מספר מילות הקישור שכתבת הוא: " + numOfConnectors + "\n";
            int numOfWords = _smartTextBox.GetNumberOfWords(text);
            ans += "מספר המילים שכתבת הוא: " + numOfWords + "\n";
            int i;
            for (i = 0; i < wordOccurrenceMap.Count(); i++)
            {
                if (conn.Contains(wordOccurrenceMap.ElementAt(i).Key) && wordOccurrenceMap.ElementAt(i).Value > 1)
                {
                    break;
                }
            }
            if (i != wordOccurrenceMap.Count())
            {
                ans += "השתמשת במילה  " + "\"" + wordOccurrenceMap.ElementAt(i).Key + "\" " + wordOccurrenceMap.ElementAt(i).Value + " פעמים, אולי תרצה להשתמש במקום ב " + "\"" + _smartTextBox.SuggestAlternativeWord(wordOccurrenceMap.ElementAt(i).Key) + "\"";
                ans += "\n";
            }
            for (i = 0; i < wordOccurrenceMap.Count(); i++)
            {
                int tmpNum = wordOccurrenceMap.ElementAt(i).Value;

                if (tmpNum >= 4)
                {
                    ans += "השתמשת במילה " + "\"" + wordOccurrenceMap.ElementAt(i).Key + "\" " + tmpNum + " פעמים, אולי תשקול להשתמש במילה נרדפת שיום אחד אנחנו נדע להציע לך אבל לא היום";
                }
            }

            
            return ans;

        }
    }
}
