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
    public class DetailsModel : PageModel
    {
        private readonly SnackisUppgift.Data.SnackisUppgiftContext _context;

        public DetailsModel(SnackisUppgift.Data.SnackisUppgiftContext context)
        {
            _context = context;
        }

      public Post Post { get; set; } = default!;

        public PostReport PostReport { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FirstOrDefaultAsync(m => m.Id == id);
            var postReport = await _context.PostReport.FirstOrDefaultAsync(pr => pr.PostId == id);

            if (post == null || postReport == null)
            {
                return NotFound();
            }

            Post = post;
            PostReport = postReport;

            return Page();
        }

    }
}
