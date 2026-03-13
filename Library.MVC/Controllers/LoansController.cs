using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Domain;
using Library.MVC.Data;

namespace Library.MVC.Controllers
{
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loans
        //modified to include book and
        //member details, and to pass the current
        public async Task<IActionResult> Index()
        {
            var loans = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member);

            ViewBag.Today = DateTime.Now;

            return View(await loans.ToListAsync());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            ViewData["BookID"] = new SelectList(
                _context.Books.Where(b => b.IsAvailable),
                 "Id", "Title");
            ViewData["MemberID"] = new SelectList(_context.Members, "Id", "Email");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BookID,MemberID,LoanDate,DueDate,ReturnDate")] Loan loan)
        {
            // Check if the book already has an active loan
            bool activeLoan = await _context.Loans
                .AnyAsync(l => l.BookID == loan.BookID && l.ReturnDate == null);

            if (activeLoan)
            {
                ModelState.AddModelError("", "This book is already on loan.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(loan);

                // Update book availability
                var book = await _context.Books.FindAsync(loan.BookID);
                if (book != null)
                {
                    book.IsAvailable = false;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["BookID"] = new SelectList(_context.Books, "Id", "Title", loan.BookID);
            ViewData["MemberID"] = new SelectList(_context.Members, "Id", "Email", loan.MemberID);

            return View(loan);
        }
        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Books, "Id", "Author", loan.BookID);
            ViewData["MemberID"] = new SelectList(_context.Members, "Id", "Email", loan.MemberID);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BookID,MemberID,LoanDate,DueDate,ReturnDate")] Loan loan)
        {
            var book = await _context.Books.FindAsync(loan.BookID);

            if (loan.ReturnDate != null && book != null)
            {
                book.IsAvailable = true;
            }
            if (id != loan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.ID))
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
            ViewData["BookID"] = new SelectList(_context.Books, "Id", "Author", loan.BookID);
            ViewData["MemberID"] = new SelectList(_context.Members, "Id", "Email", loan.MemberID);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.ID == id);
        }
    }
}
