using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisUppgift.Data;
using SnackisUppgift.Models;

namespace SnackisUppgift.Pages.Admin.SubjectAdmin
{
	[Authorize(Roles = "Owner, Admin")]
	public class DetailsModel : PageModel
	{
		private readonly SnackisUppgiftContext _context;

		public DetailsModel(SnackisUppgiftContext context)
		{
			_context = context;
		}

		public Subject Subject { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Subject = await _context.Subjects.FirstOrDefaultAsync(m => m.Id == id);

			if (Subject == null)
			{
				return NotFound();
			}

			return Page();
		}
	}
}
