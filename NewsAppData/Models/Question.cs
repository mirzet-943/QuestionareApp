using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAppData.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }
    }
}
