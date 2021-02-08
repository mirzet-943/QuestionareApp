using System;
using System.Collections.Generic;
using System.Text;

namespace NewsAppData.ViewModels
{
    public class UserVM
    {
        public int UserID { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
    }
}
