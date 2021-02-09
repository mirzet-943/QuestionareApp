using System.ComponentModel.DataAnnotations;

namespace QuestionariesAppData.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }

        public string GradePattern { get; set; }
    }
}
