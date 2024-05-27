using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using SnackisUppgift.Areas.Identity.Data;
using SnackisUppgift.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SnackisUppgift.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Data.SnackisUppgiftContext _context;
        private readonly UserManager<SnackisUppgiftUser> _userManager;

        public IndexModel(Data.SnackisUppgiftContext context, UserManager<SnackisUppgiftUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ICollection<Comment> Comments { get; set; }
        public string ProfilePicture { get; set; }
        public List<Subject> Subjects { get; set; }
        public Models.Subject Subject { get; set; }
        public List<Models.Post> Posts { get; set; }
        [BindProperty]
        public Models.Post Post { get; set; }
        [BindProperty]
        public bool IsAnonymous { get; set; }
        public bool ShowForm { get; set; }
        public IFormFile UploadedImage { get; set; }
        public SnackisUppgiftUser MyUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int subjectId = 0, int postId = 0, int deleteid = 0, bool showForm = false)
        {
            Comments = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.ChildComments) // Load child comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null) // Only top-level comments
                .ToListAsync();

            ShowForm = showForm;
            Subjects = await DAL.SubjectManagerAPI.GetAllSubjects();

            if (subjectId > 0) // if subjectId is less than or equal to zero, it makes no sense to call GetSubject
            {
                Subject = await DAL.SubjectManagerAPI.GetSubject(subjectId);
            }

            // Get the logged in user
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                MyUser = await _userManager.FindByIdAsync(userId);

                // Set ProfilePicture if user has one
                if (MyUser != null && MyUser.ProfilePicture != null) //ensure MyUser isn't null before checking ProfilePicture
                {
                    //Ensure Post object is not null
                    if (Post == null)
                    {
                        Post = new Models.Post();
                    }
                    Post.ProfilePicture = MyUser.ProfilePicture;
                }
            }

            if (deleteid > 0) // if deleteid is less than or equal to zero, it makes no sense to delete
            {
                Models.Post blog = await _context.Post.FindAsync(deleteid);

                if (blog != null)
                {
                    // Get the current user
                    var currentUser = await _userManager.GetUserAsync(User);

                    // Check if the current user is either an admin or the owner of the post
                    if (User.IsInRole("Admin") || (currentUser != null && blog.UserName == currentUser.UserName) || User.IsInRole("Owner"))
                    {
                        if (System.IO.File.Exists("./wwwroot/img/" + blog.Image))
                        {
                            System.IO.File.Delete("./wwwroot/img/" + blog.Image);
                        }
                        _context.Post.Remove(blog);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        // If the current user is not authorized to delete the post, you can handle this however you like.
                        // For example, you can redirect them to the homepage and display an error message.
                        TempData["ErrorMessage"] = "You are not authorized to delete this post.";
                        return RedirectToPage("./Index");
                    }
                }
            }

            // Sort posts in descending order based on the date
            if (subjectId > 0) // if subjectId is less than or equal to zero, it makes no sense to filter posts based on subjectId
            {
                Posts = await _context.Post
                    .Where(p => p.SubjectId == subjectId)
                    .OrderByDescending(p => p.Date)
                    .ToListAsync();
            }
            else
            {
                Posts = await _context.Post
                    .OrderByDescending(p => p.Date)
                    .ToListAsync();
            }

            // Fetch comments for each post
            foreach (var post in Posts)
            {
                post.Comments = await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.ChildComments) // Load child comments
                    .Where(c => c.PostId == post.Id && c.ParentCommentId == null) // Only top-level comments
                    .ToListAsync();
            }

            return Page();
        }


        [HttpPost]
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				// Reload subjects and return page to display validation errors.
				Subjects = await DAL.SubjectManagerAPI.GetAllSubjects();
				return Page();
			}

			string filename = string.Empty;
			if (UploadedImage != null)
			{
				Random rnd = new();
				filename = rnd.Next(0, 100000).ToString() + UploadedImage.FileName;
				var file = "./wwwroot/img/" + filename;
				using (var filestream = new FileStream(file, FileMode.Create))
				{
					await UploadedImage.CopyToAsync(filestream);
				}
			}
			var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

			Post.Date = DateTime.Now;
			Post.Image = filename;

			if (IsAnonymous == true)
			{
				Post.UserName = null;
			}
			else
			{
				Post.UserName = user.UserName;
				if (user.ProfilePicture != null)
				{
					Post.ProfilePicture = user.ProfilePicture;
				}
			}

			_context.Add(Post);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}

		public async Task<IActionResult> OnPostDeleteAsync(int deleteId)
		{
			var post = await _context.Post.FindAsync(deleteId);
			if (post == null)
			{
				return NotFound();
			}

			// Delete associated comments first
			var comments = _context.Comments.Where(c => c.PostId == deleteId).ToList();
			_context.Comments.RemoveRange(comments);

			_context.Post.Remove(post);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}



		public async Task<IActionResult> OnPostPostCommentAsync(int postId, string text)
        {
            var user = await _userManager.GetUserAsync(User);
            var post = await _context.Post.FindAsync(postId);

            if (user == null || post == null)
            {
                return BadRequest();
            }

            var comment = new Comment
            {
                Text = text,
                DatePosted = DateTime.Now,
                UserId = user.Id,
                PostId = postId,
                ProfilePicture = user.ProfilePicture
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

		public async Task<IActionResult> OnPostDeleteCommentAsync(int commentId)
		{
			var comment = await _context.Comments.FindAsync(commentId);

			if (comment != null)
			{
				var currentUser = await _userManager.GetUserAsync(User);

				// Kontrollera om den nuvarande användaren är antingen ägaren av inlägget eller en administratör/ägare
				if (currentUser != null && (comment.UserId == currentUser.Id || User.IsInRole("Admin") || User.IsInRole("Owner")))
				{
					_context.Comments.Remove(comment);
					await _context.SaveChangesAsync();
				}
			}

			return RedirectToPage();
		}
		public async Task<IActionResult> OnPostReportPostAsync(int postId, string category, string reason)
		{
			var post = await _context.Post.FindAsync(postId);

			if (post == null)
			{
				return NotFound();
			}

			// Increment the Reports count for the post
			post.Reports++;

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var postReport = new PostReport
			{
				PostId = postId,
				Category = category,
				Reason = reason,
				UserId = userId,
				CreatedDate = DateTime.Now
			};

			_context.PostReport.Add(postReport);
			await _context.SaveChangesAsync();

			return RedirectToPage();
		}





		public async Task<IActionResult> OnPostToggleLikeAsync(int postId)

		{
			var userId = _userManager.GetUserId(User);

			var existingLike = await _context.PostLike
				.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);

			var post = await _context.Post.FindAsync(postId);
			if (post == null)
			{
				return NotFound();
			}

			if (existingLike == null)
			{
				post.Likes++;
				var newLike = new PostLike { PostId = postId, UserId = userId };
				_context.PostLike.Add(newLike);
			}
			else
			{
				post.Likes--;
				_context.PostLike.Remove(existingLike);
			}

			await _context.SaveChangesAsync();

			return RedirectToPage();
		}




	}
}
