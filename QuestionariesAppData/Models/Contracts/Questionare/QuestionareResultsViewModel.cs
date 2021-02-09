using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionariesAppData.Models.Contracts.Questionare
{
    public class QuestionareResultsViewModel
    {
        public int QuestionarePoints { get; set; }

        public Guid QuestionareId { get; set; }
    }
}
