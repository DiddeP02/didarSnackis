using SnackisUppgift.Areas.Identity.Data;

namespace SnackisUppgift.Models
{
	public class Comment
	{
		public int? Id { get; set; }
		public string? Text { get; set; }
		public DateTime? DatePosted { get; set; }
		public string? UserId { get; set; }
		public SnackisUppgiftUser? User { get; set; }
		public int? PostId { get; set; }
		public Post? Post { get; set; }
		public int? ParentCommentId { get; set; }
		public Comment? ParentComment { get; set; }
		public ICollection<Comment>? ChildComments { get; set; } = new List<Comment>();
		public string? ProfilePicture { get; set; }


	}

}
