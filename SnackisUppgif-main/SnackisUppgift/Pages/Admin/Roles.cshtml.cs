using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisUppgift.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SnackisUppgift.Pages.Roles
{
    //[Authorize(Roles = "Owner")]
    public class IndexModel : PageModel
	{
		public List<SnackisUppgiftUser> Users { get; set; }
		public List<IdentityRole> Roles { get; set; }
		[BindProperty]
		public string RemoveRoleName { get; set; }
		[BindProperty]
		public string RoleName { get; set; }
		[BindProperty(SupportsGet = true)]
		public string AddUserId { get; set; }
		[BindProperty(SupportsGet = true)]
		public string RemoveUserId { get; set; }
        [BindProperty(SupportsGet = true)]
		public string Role { get; set; }
		public bool IsUser { get; set; }
		public bool IsAdmin { get; set; }

		public readonly UserManager<SnackisUppgiftUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<SnackisUppgiftUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}
		public async Task<IActionResult> OnGetAsync()
		{
			Roles = await _roleManager.Roles.ToListAsync();
			Users = await _userManager.Users.ToListAsync();

			if (AddUserId != null)
			{
				var alterUser = await _userManager.FindByIdAsync(AddUserId);
				var roleresult = await _userManager.AddToRoleAsync(alterUser, Role);
			}
			if (RemoveUserId != null)
			{
				var alterUser = await _userManager.FindByIdAsync(RemoveUserId);
				var roleresult = await _userManager.RemoveFromRoleAsync(alterUser, Role);
			}

			// Check if the "Admin" role exists and create it if not
			if (!await _roleManager.RoleExistsAsync("Owner"))
			{
				await _roleManager.CreateAsync(new IdentityRole("Owner"));
			}

			// Add the specified user to the "Admin" role
			var user = await _userManager.FindByEmailAsync("bilals@hotmail.com");
			if (user != null && !(await _userManager.IsInRoleAsync(user, "Owner")))
			{
				await _userManager.AddToRoleAsync(user, "Owner");	
			}

			// Demo av roller
			var currentUser = await _userManager.GetUserAsync(User);
			IsUser = await _userManager.IsInRoleAsync(currentUser, "User");
			IsAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

			return Page();
		}


		public async Task<IActionResult> OnPostAsync()
		{
			if (RoleName != null)
			{
				await CreateRole(RoleName);
			}
			if (RemoveRoleName != null)
			{
				await RemoveRole(RemoveRoleName);
			}
			return RedirectToPage("./Roles");
		}
		public async Task RemoveRole(string roleName)
		{
			var role = await _roleManager.FindByNameAsync(roleName);
			if (role != null)
			{
				var result = await _roleManager.DeleteAsync(role);
				if (!result.Succeeded)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("RoleRemoval", error.Description);
					}
				}
			}
		}
		public async Task CreateRole(string roleName)
		{
			bool exist = await _roleManager.RoleExistsAsync(roleName);
			if (!exist)
			{
				var Role = new IdentityRole
				{
					Name = roleName,
				};
				await _roleManager.CreateAsync(Role);
			}
		}
	}
}