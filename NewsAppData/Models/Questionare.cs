using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAppData.Models
{
    public class Questionare
    {
        [Key]
        public Guid QuestionareId { get; set; }

        [Required]
        public int PinCode { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsSubmitted { get; set; }
    }
}
