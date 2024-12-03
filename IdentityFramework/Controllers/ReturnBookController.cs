using IdentityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityFramework.Controllers
{
    //[Authorize(Roles = "Admin, Manager")]
    public class ReturnBookController : Controller
    {
        private readonly IdentityFrameworkDbContext _context;

        public ReturnBookController(IdentityFrameworkDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var borrowedBooks = _context.BorrowingRecords
                .Where(br => br.ReturnDate == null)
                .Include(br => br.Book)
                .Include(br => br.User)
                .ToList();

            return View(borrowedBooks);
        }

        [HttpGet]
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.BorrowingRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.RecordId == id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var record = await _context.BorrowingRecords
                .FirstOrDefaultAsync(br => br.RecordId == id);

            if (record != null)
            {
                record.ReturnDate = DateTime.Now;
                record.IsDeleted = true;
                _context.Update(record);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Book returned successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }

}
