using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DockingAdminPanel.Data;
using DockingAdminPanel.Models;

namespace DockingAdminPanel.Controllers
{
    public class LabCategoriesController : Controller
    {
        private readonly BookingWebAppContext _context;

        public LabCategoriesController(BookingWebAppContext context)
        {
            _context = context;
        }

        // GET: LabItems
        public async Task<IActionResult> Index()
        {var categories=await _context.labCategories.ToListAsync();
              return _context.labCategories != null ? 
                          View(await _context.labCategories.ToListAsync()) :
                          Problem("Entity set 'BookingWebAppContext.labItems'  is null.");
        }

        // GET: LabItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.labItems == null)
            {
                return NotFound();
            }

            var labItems = await _context.labItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labItems == null)
            {
                return NotFound();
            }

            return View(labItems);
        }

        // GET: LabItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LabItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] LabCategory labCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labCategory);
        }

        // GET: LabItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.labCategories == null)
            {
                return NotFound();
            }

            var labItems = await _context.labCategories.FindAsync(id);
            if (labItems == null)
            {
                return NotFound();
            }
            return View(labItems);
        }

        // POST: LabItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] LabCategory labCategory)
        {
            if (id != labCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabItemsExists(labCategory.Id))
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
            return View(labCategory);
        }

        // GET: LabItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.labCategories == null)
            {
                return NotFound();
            }

            var labItems = await _context.labCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labItems == null)
            {
                return NotFound();
            }

            return View(labItems);
        }

        // POST: LabItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.labCategories == null)
            {
                return Problem("Entity set 'BookingWebAppContext.labItems'  is null.");
            }
            var labItems = await _context.labCategories.FindAsync(id);
            if (labItems != null)
            {
                _context.labCategories.Remove(labItems);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabItemsExists(int id)
        {
          return (_context.labItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
