using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProektRSWEB.Data;
using ProektRSWEB.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace ProektRSWEB.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string Title, string Author)
        {
            var books = _context.Book.AsQueryable(); // Start with IQueryable

            // Filter by title
            if (!string.IsNullOrEmpty(Title))
            {
                books = books.Where(b => b.Title.Contains(Title));
            }

            // Filter by author
            if (!string.IsNullOrEmpty(Author))
            {
                books = books.Where(b => b.Author.FirstName.Contains(Author));
            }

            // Include author assuming it's a related entity
            books = books.Include(b => b.Author)
                         .Include(b => b.BookGenre)
                         .Include("BookGenre.Genre"); // Include BookGenres

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName");
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,YearPublished,NumberOfPages,Description,Publisher,FrontPage,DownloadURL,AuthorId")] Book book, List<int> GenreId)
        {
            
            if (ModelState.IsValid)
            {
                book.BookGenre = new List<BookGenre>();
                GenreId.ForEach(g=>
                {
                    var genreId = g;
                    var genre = _context.Genre.FirstOrDefault(ge => ge.Id == genreId);
                    var bookGenre = new BookGenre { BookId = book.Id, Book = book, GenreId = genreId, Genre = genre };
                    book.BookGenre.Add(bookGenre);
                    _context.Add(bookGenre);
                });
                
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName");
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName");
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,YearPublished,NumberOfPages,Description,Publisher,FrontPage,DownloadURL,AuthorId,GenreId")] Book book, List<int> GenreId)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                

                try
                {
                    var bookGenres = new List<BookGenre>();

                    
                    book.BookGenre = _context.BookGenre.Where(x => x.BookId == book.Id).ToList();

                    for (int i = 0; i < book.BookGenre.Count; i++)
                    {
                        var b = _context.BookGenre.Find(book.BookGenre.ElementAt(i).Id);
                        _context.BookGenre.Remove(b);
                    }
                    

                    GenreId.ForEach(g =>
                    {
                        var genreId = g;
                        var genre = _context.Genre.FirstOrDefault(ge => ge.Id == genreId);
                        var bGenre = new BookGenre();
                        bGenre.Genre = genre;
                        bGenre.GenreId = genreId;
                        bGenre.BookId = book.Id;
                        bGenre.Book = book;
                        bookGenres.Add(bGenre);
                        _context.Add(bGenre);
                    });
                    book.BookGenre.Clear();
                    book.BookGenre=bookGenres;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName");
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
