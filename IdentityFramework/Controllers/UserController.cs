using IdentityFramework.Areas.Identity.Data;
using IdentityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityFramework.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IdentityFrameworkDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IdentityFrameworkDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: User/BorrowingHistory
        public async Task<IActionResult> BorrowingHistory()
        {
            var userId = _userManager.GetUserId(User);
            var borrowingHistory = await _context.BorrowingRecords
                .Where(br => br.Id == userId)
                .Include(br => br.Book)
                .OrderByDescending(br => br.IssueDate)
                .ToListAsync();

            return View(borrowingHistory);
        }
    }
}
