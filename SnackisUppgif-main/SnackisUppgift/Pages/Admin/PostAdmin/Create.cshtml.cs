using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SnackisUppgift.Data;
using SnackisUppgift.Models;

namespace SnackisUppgift.Pages.Admin.PostAdmin
{
    [Authorize(Roles = "Owner, Admin")]
    public class CreateModel : PageModel
    {
        private readonly SnackisUppgift.Data.SnackisUppgiftContext _context;

        public CreateModel(SnackisUppgift.Data.SnackisUppgiftContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Category");
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Post == null || Post == null)
            {
                return Page();
            }

            _context.Post.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
