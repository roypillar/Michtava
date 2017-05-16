using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Models;



namespace Frontend.Models
{
    public class SmartTextViewModel
    {
        public int QuestionNumber { get; set; }

        public Question question { get; set; }

        public List<Question> Questions { get; set; }

        public List<QuestionAnswer> CompleteQuestions { get; set; }

        public List<int> CompleteQuestionsNumbers { get; set; }
    }
}
