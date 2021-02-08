using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NewsAppData.ViewModels
{
    public class ArticleVM 
    {
        [Key]
        public int ArticleId { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }

        public string WriterFullName { get; set; }
        public int WriterId { get; set; }

    }
}
