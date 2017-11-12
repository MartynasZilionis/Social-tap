using System;
using System.Collections.Generic;
using System.Text;

namespace social_tapX
{
    public class FeedbackAndDate
    {
        public string Feedback { get; private set; }
        public DateTime Date { get; private set; }
        
        public FeedbackAndDate(string feedback, DateTime date)
        {
            Feedback = feedback;
            Date = date;
        }
    }
}
