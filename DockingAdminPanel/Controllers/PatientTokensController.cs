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
    public class PatientTokensController : Controller
    {
        private readonly BookingWebAppContext _context;

        public PatientTokensController(BookingWebAppContext context)
        {
            _context = context;
        }

        // GET: PatientTokens
        public async Task<IActionResult> Index()
        {
              return _context.patientTokens != null ? 
                          View(await _context.patientTokens.FirstOrDefaultAsync()) :
                          Problem("Entity set 'BookingWebAppContext.patientTokens'  is null.");
        }

        // GET: PatientTokens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.patientTokens == null)
            {
                return NotFound();
            }

            var patientToken = await _context.patientTokens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientToken == null)
            {
                return NotFound();
            }

            return View(patientToken);
        }

        // GET: PatientTokens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PatientTokens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Counter,DailyTotalCounter,TotalCounterAddedDate")] PatientToken patientToken)
        {
            _context.Add(patientToken);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PatientTokens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.patientTokens == null)
            {
                return NotFound();
            }

            var patientToken = await _context.patientTokens.FindAsync(id);
            if (patientToken !=null)
            {
                patientToken.TotalCounterAddedDate = DateTime.Now;
                patientToken.DailyTotalCounter = patientToken.Counter;
                patientToken.Counter = 1;
                _context.Update(patientToken);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
            
        }

        // POST: PatientTokens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Counter,DailyTotalCounter,TotalCounterAddedDate")] PatientToken patientToken)
        {
            if (id != patientToken.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientToken);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTokenExists(patientToken.Id))
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
            return View(patientToken);
        }

        // GET: PatientTokens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.patientTokens == null)
            {
                return NotFound();
            }

            var patientToken = await _context.patientTokens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientToken == null)
            {
                return NotFound();
            }

            return View(patientToken);
        }

        // POST: PatientTokens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.patientTokens == null)
            {
                return Problem("Entity set 'BookingWebAppContext.patientTokens'  is null.");
            }
            var patientToken = await _context.patientTokens.FindAsync(id);
            if (patientToken != null)
            {
                _context.patientTokens.Remove(patientToken);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTokenExists(int id)
        {
          return (_context.patientTokens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
