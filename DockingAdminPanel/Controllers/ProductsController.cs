using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DockingAdminPanel.Models;
using DockingAdminPanel.Data;
using Humanizer;

namespace DockingAdminPanel.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BookingWebAppContext _context;

        public ProductsController(BookingWebAppContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
              return _context.products != null ? 
                          View(await _context.products.ToListAsync()) :
                          Problem("Entity set 'BookingWebAppContext.products'  is null.");
        }
        public async Task<IActionResult> PendingItems()
        {
            return _context.products != null ?
                        View(await _context.products.Where(m=>m.adminApproval==false).ToListAsync()) :
                        Problem("Entity set 'BookingWebAppContext.products'  is null.");
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            Products products = new Products();
         
            return View(products);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,address,location,rentPerMonth,spaceNumber,status,adminApproval,contractDate,available,addedDate,CategoryId")] Products products)
        {
            return await Task.Run<ActionResult>(() =>
            {


                try
                {

                    if (ModelState.IsValid)
                    {
                        _context.Add(products);
                         _context.SaveChangesAsync();
                        var result = new { Success = "true", Message = "Your work has been saved" };
                        return Json(result);
                        // return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var result = new { Success = "false", Message = "Error while Adding the Data" };
                        return Json(result);
                    }

                    //TempData["ShowAlert"] = "true";
                    //TempData["AlertType"] = "success";
                    //TempData["AlertTitle"] = "Your work has been saved";
                 


                }
                catch (Exception ex)
                {

                    var result = new { Success = "false", Message = "Error while Adding the Data"};
                    return Json(result);
                }



            });
           
           // return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var products = await _context.products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,address,location,rentPerMonth,spaceNumber,status,adminApproval,contractDate,available,addedDate,CategoryId")] Products products)
        {
            return await Task.Run<ActionResult>(() =>
            {


                try
                {
                    if (id != products.Id)
                    {
                        var result = new { Success = "false", Message = "Error Id Not Found" };
                        return Json(result);
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(products);
                             _context.SaveChangesAsync();
                            var result = new { Success = "true", Message = "Your work has been Updated" };
                            return Json(result);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ProductsExists(products.Id))
                            {
                                var result = new { Success = "false", Message = "Error while Updating the Data" };
                                return Json(result);
                            }
                            else
                            {
                                throw;
                            }
                        }
                        
                    }
                    else
                    {
                        var result = new { Success = "false", Message = "Error while Updating the Data" };
                        return Json(result);
                    }
                   // return View(products);
                    //if (ModelState.IsValid)
                    //{
                    //    _context.Add(products);
                    //    _context.SaveChangesAsync();
                    //    var result = new { Success = "true", Message = "Your work has been saved" };
                    //    return Json(result);
                    //    // return RedirectToAction(nameof(Index));
                    //}
                    //else
                    //{
                    //    var result = new { Success = "false", Message = "Error while Adding the Data" };
                    //    return Json(result);
                    //}

                    //TempData["ShowAlert"] = "true";
                    //TempData["AlertType"] = "success";
                    //TempData["AlertTitle"] = "Your work has been saved";



                }
                catch (Exception ex)
                {

                    var result = new { Success = "false", Message = "Error while Adding the Data" };
                    return Json(result);
                }



            });
         
        }

        [HttpPost]
        
        public async Task<IActionResult> Approve(int id, [Bind("Id,address,location,rentPerMonth,spaceNumber,status,adminApproval,contractDate,available,addedDate,CategoryId")] Products products)
        {
            return await Task.Run<ActionResult>(() =>
            {


                try
                {
                    if (id ==0)
                    {
                        var result = new { Success = "false", Message = "Error Id Not Found" };
                        return Json(result);
                    }

                    try
                    {
                        var product =  _context.products
                         .Where(m => m.Id == id).SingleOrDefault();
                        if (product!=null)
                        {
                            product.adminApproval = true;
                            _context.Update(product);
                            _context.SaveChangesAsync();
                            var result = new { Success = "true", Message = "Item Approved" };
                            return Json(result);
                        }
                        else
                        {
                            var result = new { Success = "false", Message = "Error while Updating the Data" };
                            return Json(result);

                        }
                      
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductsExists(products.Id))
                        {
                            var result = new { Success = "false", Message = "Error while Updating the Data" };
                            return Json(result);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    // return View(products);
                    //if (ModelState.IsValid)
                    //{
                    //    _context.Add(products);
                    //    _context.SaveChangesAsync();
                    //    var result = new { Success = "true", Message = "Your work has been saved" };
                    //    return Json(result);
                    //    // return RedirectToAction(nameof(Index));
                    //}
                    //else
                    //{
                    //    var result = new { Success = "false", Message = "Error while Adding the Data" };
                    //    return Json(result);
                    //}

                    //TempData["ShowAlert"] = "true";
                    //TempData["AlertType"] = "success";
                    //TempData["AlertTitle"] = "Your work has been saved";



                }
                catch (Exception ex)
                {

                    var result = new { Success = "false", Message = "Error while Adding the Data" };
                    return Json(result);
                }



            });

        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.products == null)
            {
                return Problem("Entity set 'BookingWebAppContext.products'  is null.");
            }
            var products = await _context.products.FindAsync(id);
            if (products != null)
            {
                _context.products.Remove(products);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
          return (_context.products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
