using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisUppgift.Models;
using SnackisUppgift.DAL;  // Include this to use your SubjectManagerAPI

namespace SnackisUppgift.Pages.Admin.SubjectAdmin
{
	[Authorize(Roles = "Owner, Admin")]
	public class IndexModel : PageModel
	{
		public IList<Subject> Subject { get; set; } = default!;

		public async Task OnGetAsync()
		{
			// Use the GetAllSubjects method from the SubjectManagerAPI to get all subjects
			Subject = await SubjectManagerAPI.GetAllSubjects();
		}
	}
}
