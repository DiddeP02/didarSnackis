using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisUppgift.Areas.Identity.Data;
using SnackisUppgift.Data;
using SnackisUppgift.Models;

namespace SnackisUppgift.Pages
{
	public class ReplyModel : PageModel
	{
		private readonly SnackisUppgiftContext _context;
		private readonly UserManager<SnackisUppgiftUser> _userManager;

		public ReplyModel(SnackisUppgiftContext context, UserManager<SnackisUppgiftUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public DirectMessage OriginalMessage { get; set; }

        public async Task OnGetAsync(int id)
        {
            OriginalMessage = await _context.DirectMessages.FindAsync(id);

            if (OriginalMessage == null)
            {
                NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(int id, string replyContent)
        {
            // Find the original message
            var originalMessage = await _context.DirectMessages.FindAsync(id);
            if (originalMessage == null)
            {
                return NotFound();
            }

            // Create a reply
            var reply = new DirectMessage
            {
                SenderId = _userManager.GetUserId(User),
                ReceiverId = originalMessage.SenderId,
                Subject = "REPLY: " + originalMessage.Subject,
                Message = replyContent,
                SentAt = DateTime.UtcNow
            };

            _context.DirectMessages.Add(reply);
            await _context.SaveChangesAsync();

            return RedirectToPage("DmIndex");
        }
    }
}
