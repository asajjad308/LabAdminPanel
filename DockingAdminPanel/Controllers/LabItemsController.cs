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
    public class LabItemsController : Controller
    {
        private readonly BookingWebAppContext _context;

        public LabItemsController(BookingWebAppContext context)
        {
            _context = context;
        }

        // GET: LabItems
        public async Task<IActionResult> Index()
        {var categories=await _context.labCategories.ToListAsync();
              return _context.labItems != null ? 
                          View(await _context.labItems.ToListAsync()) :
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
        public async Task<IActionResult> Create([Bind("Id,TestName,Price,Units,NormalValue")] LabItems labItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labItems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labItems);
        }

        // GET: LabItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.labItems == null)
            {
                return NotFound();
            }

            var labItems = await _context.labItems.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,TestName,Price,Units,NormalValue")] LabItems labItems)
        {
            if (id != labItems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabItemsExists(labItems.Id))
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
            return View(labItems);
        }

        // GET: LabItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: LabItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.labItems == null)
            {
                return Problem("Entity set 'BookingWebAppContext.labItems'  is null.");
            }
            var labItems = await _context.labItems.FindAsync(id);
            if (labItems != null)
            {
                _context.labItems.Remove(labItems);
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
