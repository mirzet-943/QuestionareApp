using NewsAppData.Models;
using NewsAppData.Models.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAppData.Models
{
    public class QuestionResult
    {
        [Key]
        public int QuestionResultId { get; set; }

        [ForeignKey("Questionare")]
        public int QuestionareId { get; set; }
        public Questionare Questionare { get; set; }
        public AnswerEnum Answer { get; set; }

    }
}
