using IdentityFramework.Models;
using IdentityFramework.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IdentityFramework.Controllers
{
    public class IssueBookController : Controller
    {
        private readonly IdentityFrameworkDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IssueBookController(IdentityFrameworkDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> BorrowingRecords()
        {
            var borrowingRecords = await _context.BorrowingRecords
                .Include(br => br.User) 
                .Include(br => br.Book)  
                .Where(br => br.ReturnDate == null) 
                .ToListAsync();

            return View(borrowingRecords);
        }

        [HttpGet]
        public IActionResult IssueBook()
        {
            var books = _context.Books.Where(b => !b.BorrowingRecords.Any(br => br.ReturnDate == null)).ToList();
            var users = _userManager.Users.ToList();
            var model = new IssueBookViewModel
            {
                Books = books.Select(b => new SelectListItem { Value = b.BookId.ToString(), Text = b.Title }).ToList(),
                Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IssueBook(IssueBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.SelectedUserId);
                var book = await _context.Books.FindAsync(model.SelectedBookId);

                if (user != null && book != null)
                {
                    var borrowingRecord = new BorrowingRecords
                    {
                        Id = user.Id,
                        BookId = book.BookId,
                        IssueDate = DateTime.Now,
                        ReturnDate = null
                    };
                    _context.BorrowingRecords.Add(borrowingRecord);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Book issued successfully!";
                    return RedirectToAction("IssueBook");
                }
            }
            var books = _context.Books.Where(b => !b.BorrowingRecords.Any(br => br.ReturnDate == null)).ToList();
            var users = _userManager.Users.ToList();
            model.Books = books.Select(b => new SelectListItem { Value = b.BookId.ToString(), Text = b.Title }).ToList();
            model.Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AvailableBooks()
        {
            var availableBooks = _context.Books
                .Include(b => b.Author)  
                .Include(b => b.Category) 
                .Where(b => !b.BorrowingRecords.Any(br => br.ReturnDate == null))
                .ToList();

            return View(availableBooks);
        }

    }
}
