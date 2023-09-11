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
    public class PatientsController : Controller
    {
        private readonly BookingWebAppContext _context;

        public PatientsController(BookingWebAppContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
              return _context.patients != null ? 
                          View(await _context.patients.ToListAsync()) :
                          Problem("Entity set 'BookingWebAppContext.patients'  is null.");
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.patients == null)
            {
                return NotFound();
            }

            var patient = await _context.patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public async Task<IActionResult> Create()
        {
            var model=new Patient();
			model.Doctors = await _context.doctors.ToListAsync();
            return View(model);
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,DateOfBirth,Address,City,State,ZipCode,Country,MedicalRecordNumber,InsuranceProvider,PolicyNumber,Illness,DoctorId")] Patient patient)
        {
			_context.Add(patient);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
			//if (ModelState.IsValid)
			//{
			//    _context.Add(patient);
			//    await _context.SaveChangesAsync();
			//    return RedirectToAction(nameof(Index));
			//}
			//return View(patient);
		}

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.patients == null)
            {
                return NotFound();
            }

            var patient = await _context.patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,DateOfBirth,Address,City,State,ZipCode,Country,MedicalRecordNumber,InsuranceProvider,PolicyNumber,Illness")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.patients == null)
            {
                return NotFound();
            }

            var patient = await _context.patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.patients == null)
            {
                return Problem("Entity set 'BookingWebAppContext.patients'  is null.");
            }
            var patient = await _context.patients.FindAsync(id);
            if (patient != null)
            {
                _context.patients.Remove(patient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
          return (_context.patients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
