using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnackisUppgift.Areas.Identity.Data;
using SnackisUppgift.Data;
namespace SnackisUppgift
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
   var connectionString = builder.Configuration.GetConnectionString("SnackisUppgiftContextConnection") ?? throw new InvalidOperationException("Connection string 'SnackisUppgiftContextConnection' not found.");

   builder.Services.AddDbContext<SnackisUppgiftContext>(options => options.UseSqlServer(connectionString));


			builder.Services.AddDefaultIdentity<SnackisUppgiftUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddRoles<IdentityRole>() //Lägg till roller här
			.AddEntityFrameworkStores<SnackisUppgiftContext>();

			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("AdminKrav",
					policy => policy.RequireRole("Admin"));
			});
			// Add services to the container.
			builder.Services.AddRazorPages(options =>
			{
				options.Conventions.AuthorizeFolder("/Secret");
				options.Conventions.AuthorizeFolder("/Secret", "AdminKrav");
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}


			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}