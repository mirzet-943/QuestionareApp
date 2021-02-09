using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionariesAppData.Models
{
    public class Questionare
    {
        [Key]
        public Guid QuestionareId { get; set; }

        public string PinCode { get; set; }

        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public Candidate User { get; set; }
        public bool IsSubmitted { get; set; }

        public DateTime AccessTime { get; set; }

        public DateTime FinishTime { get; set; }

        public int WrongPinsEntered { get; set; }
    }
}
