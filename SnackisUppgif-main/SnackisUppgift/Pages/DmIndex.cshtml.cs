using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisUppgift.Areas.Identity.Data;
using SnackisUppgift.Data;
using SnackisUppgift.Models;

namespace SnackisUppgift.Pages
{
    public class DmIndexModel : PageModel
    {
        private readonly SnackisUppgiftContext _context;
        private readonly UserManager<SnackisUppgiftUser> _userManager;

        public DmIndexModel(SnackisUppgiftContext context, UserManager<SnackisUppgiftUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<DirectMessage> DirectMessages { get;set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            DirectMessages = await _context.DirectMessages
                .Where(dm => dm.ReceiverId == userId || dm.SenderId == userId)
                .Select(dm => new DirectMessage
                {
                    Id = dm.Id,
                    SenderId = dm.SenderId,
                    ReceiverId = dm.ReceiverId,
                    Sender = _context.Users.FirstOrDefault(u => u.Id == dm.SenderId),
                    Receiver = _context.Users.FirstOrDefault(u => u.Id == dm.ReceiverId),
                    Subject = dm.Subject,
                    Message = dm.Message,
                    SentAt = dm.SentAt
                })
                .OrderByDescending(dm => dm.SentAt) // Add this line to sort the messages
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostSendDMAsync(string receiverEmail, string dmContent)
        {
            // Find the receiver by their email
            var receiver = await _userManager.FindByEmailAsync(receiverEmail);
            if (receiver == null)
            {
                ModelState.AddModelError("", "No user with that email found");
                return Page();
            }

            // Get the sender's ID
            var senderId = _userManager.GetUserId(User);

            // Create a new DM
            var dm = new DirectMessage
            {
                SenderId = senderId,
                ReceiverId = receiver.Id,
                Message = dmContent,
                SentAt = DateTime.UtcNow
            };

            _context.DirectMessages.Add(dm);
            await _context.SaveChangesAsync();

            // TODO: send a notification to the receiver

            return RedirectToPage();
        }
    }
}
