using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace QuestionariesAppData
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

        public DateTime CreatedAt { get; set; }
        [Required, MinLength(4)]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Email == user.Email;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
