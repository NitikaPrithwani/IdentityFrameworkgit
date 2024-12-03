using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using IdentityFramework.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityFramework.Controllers
{
    public class BooksController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityFrameworkDbContext _context;

        public BooksController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IdentityFrameworkDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [Authorize(Policy = "CreateRolePolicy")]
        public IActionResult Create()
        {
            var authors = _context.Authors.ToList();
            var categories = _context.BookCategories.ToList();

            var viewModel = new BookViewModel
            {
                Authors = authors != null && authors.Any() ? new SelectList(authors, "AuthorId", "AuthorName") : new SelectList(new List<string>()),
                Categories = categories != null && categories.Any() ? new SelectList(categories, "CategoryId", "CategoryName") : new SelectList(new List<string>())
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateRolePolicy")]
        public async Task<IActionResult> Create([Bind("Title, AuthorId, CategoryId, Year")] Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var authors = _context.Authors.ToList();
            var categories = _context.BookCategories.ToList();
            var viewModel = new BookViewModel
            {
                Title = book.Title,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Year = book.Year,
                Authors = authors != null && authors.Any() ? new SelectList(authors, "AuthorId", "AuthorName") : new SelectList(new List<string>()),
                Categories = categories != null && categories.Any() ? new SelectList(categories, "CategoryId", "CategoryName") : new SelectList(new List<string>())
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "AuthorName", book.AuthorId);
            ViewBag.Categories = new SelectList(_context.BookCategories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("BookId, Title, AuthorId, CategoryId, Year")] Books book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "AuthorName", book.AuthorId);
            ViewBag.Categories = new SelectList(_context.BookCategories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            var categories = _context.BookCategories.ToList();
            var newCategory = new BookCategory(); 

            var model = new CategoryViewModel
            {
                NewCategory = newCategory,
                CategoriesList = categories
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory(CategoryViewModel model)
        {
                _context.BookCategories.Add(model.NewCategory);
                _context.SaveChanges();
                model.CategoriesList = _context.BookCategories.ToList();
                model.NewCategory = new BookCategory();
            return View(model);
        }
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.BookCategories.Find(id);
            _context.BookCategories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("AddCategory");
        }

        [HttpGet]
        public IActionResult AddAuthors()
        {
            var authors = _context.Authors.ToList();  
            var newAuthor = new Authors();  
            var model = new AuthorViewModel
            {
                NewAuthor = newAuthor,
                AuthorsList = authors
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAuthors(AuthorViewModel model)
        {
                _context.Authors.Add(model.NewAuthor);
                _context.SaveChanges();
                model.NewAuthor = new Authors();
                model.AuthorsList = _context.Authors.ToList();
            

            return View(model);
        }


        public IActionResult DeleteAuthor(int id)
        {
            var author = _context.Authors.Find(id);
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return RedirectToAction("AddAuthors");
        }
    }
}
