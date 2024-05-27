using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisUppgift.Data;
using SnackisUppgift.Models;

namespace SnackisUppgift.Pages.Admin.PostAdmin
{
    [Authorize(Roles = "Owner, Admin")]
    public class DeleteModel : PageModel
    {
        private readonly SnackisUppgift.Data.SnackisUppgiftContext _context;

        public DeleteModel(SnackisUppgift.Data.SnackisUppgiftContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            else 
            {
                Post = post;
            }
            return Page();
        }

		public async Task<IActionResult> OnPostAsync()
		{
			var post = await _context.Post
				.Include(p => p.Comments) // Include the comments in the query
				.FirstOrDefaultAsync(p => p.Id == Post.Id);

			if (post == null)
			{
				return NotFound();
			}

			// Remove the comments first
			_context.Comments.RemoveRange(post.Comments);

			// Remove the post
			_context.Post.Remove(post);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}

	}
}
