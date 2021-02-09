using QuestionariesAppData.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionariesAppData.Models
{
    public class QuestionResult
    {
        [Key]
        public int QuestionResultId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        [ForeignKey("Questionare")]
        public Guid QuestionareId { get; set; }
        public Questionare Questionare { get; set; }
        public int Answer { get; set; }

    }
}
