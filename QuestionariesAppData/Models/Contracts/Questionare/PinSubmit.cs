using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionariesAppData.Models.Contracts.Questionare
{
    public class PinSubmit
    {
        public string Pincode { get; set; }

        public string CaptchaChallenge { get; set; }
    }
}
