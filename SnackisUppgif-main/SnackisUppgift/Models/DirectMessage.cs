using SnackisUppgift.Areas.Identity.Data;

namespace SnackisUppgift.Models
{
    public class DirectMessage
    {
        public int? Id { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? ReadAt { get; set; }

        public virtual SnackisUppgiftUser? Sender { get; set; }
        public virtual SnackisUppgiftUser? Receiver { get; set; }
    }
}
