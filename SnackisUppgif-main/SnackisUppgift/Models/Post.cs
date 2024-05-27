using SnackisUppgift.DAL;
using System;
using System.ComponentModel.DataAnnotations;

namespace SnackisUppgift.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        public virtual Subject? Subject { get; set; }  // Change to Subjects to match your new table
        public int? SubjectId { get; set; }

        public DateTime? Date { get; set; }
        public string? UserName { get; set; }
		public int Reports { get; set; }
        public int Likes { get; set; }
        public PostReport? PostReport { get; set; }
    }

}
