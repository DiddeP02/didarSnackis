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
	public class CreateModel : PageModel
	{
		[BindProperty]
		public Subject Subject { get; set; } = default!;

		public IActionResult OnGet()
		{
			return Page();
		}

		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// Use the SaveSubject method from the SubjectManagerAPI to save the new subject
			await SubjectManagerAPI.SaveSubject(Subject);

			return RedirectToPage("./Index");
		}
	}
}
