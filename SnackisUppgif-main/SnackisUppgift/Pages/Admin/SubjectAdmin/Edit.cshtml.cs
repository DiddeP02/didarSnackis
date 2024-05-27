using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisUppgift.Models;
using SnackisUppgift.DAL;

namespace SnackisUppgift.Pages.Admin.SubjectAdmin
{
	[Authorize(Roles = "Owner, Admin")]
	public class EditModel : PageModel
	{
		[BindProperty]
		public Subject Subject { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var subject = await SubjectManagerAPI.GetSubject(id.Value);

			if (subject == null)
			{
				return NotFound();
			}
			else
			{
				Subject = subject;
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// Update the subject using the UpdateSubject method from the SubjectManagerAPI
			await SubjectManagerAPI.UpdateSubject(Subject);

			return RedirectToPage("./Index");
		}



	}
}
