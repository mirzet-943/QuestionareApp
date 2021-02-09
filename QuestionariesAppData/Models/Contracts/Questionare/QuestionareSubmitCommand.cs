using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionariesAppData.Models.Contracts.Questionare
{
    public class QuestionareSubmitCommand
    {
        public string QuestionarePinCode { get; set; }
        public List<QuestionResult> Results  { get; set; }
    }
}
