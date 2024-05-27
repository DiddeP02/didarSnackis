using Microsoft.EntityFrameworkCore;

namespace SnackisUppgift.Models
{
    public class PostReport
    {
        public int Id { get; set; }

		public int? PostId { get; set; }  

        public virtual Post Post { get; set; }  
        public string Reason { get; set; }
        public string Category { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
