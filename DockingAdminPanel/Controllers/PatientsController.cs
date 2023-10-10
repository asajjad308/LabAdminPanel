using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DockingAdminPanel.Data;
using DockingAdminPanel.Models;
using System.Drawing.Printing;
using System.Drawing;
 
namespace DockingAdminPanel.Controllers
{
    public class PatientsController : Controller
    {
        private readonly BookingWebAppContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PatientsController(BookingWebAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

		public async Task<IActionResult> AddTestsAppointments(int id)
		{
			var getalltest= await _context.labItems.ToListAsync();
            var patient = await _context.patients.FindAsync(id);
            var patientstest=await _context.patientsTests.Where(r=>r.PatientId==id).ToListAsync();
			TestAppointmentsViewModel viewModel = new TestAppointmentsViewModel();
            viewModel.patient = patient;
            viewModel.labItems = getalltest;
            viewModel.patientsTests= patientstest;
            return View(viewModel);
		}
		
		public async Task<IActionResult> SampleCollection(int id)
		{
			var getalltest = await _context.labItems.ToListAsync();
			//here filter by today date
			var patient = await _context.patients.FindAsync(id);
			var patientstest = await _context.patientsTests.Where(r => r.PatientId == id && r.Paid==true).ToListAsync();
			TestAppointmentsViewModel viewModel = new TestAppointmentsViewModel();
			viewModel.patient = patient;
			viewModel.labItems = getalltest;
			viewModel.patientsTests = patientstest;
			return View(viewModel);
		}
		public async Task<IActionResult> CollectTestSamples(int id)
        {
            var getalltest = await _context.labItems.ToListAsync();
            //here filter by today date
            var patients = await _context.patients.ToListAsync();
            var patientstest = await _context.patientsTests.Where(r => r.PatientId == id).ToListAsync();
            TestAppointmentsViewModel viewModel = new TestAppointmentsViewModel();
            viewModel.patients = patients;
            viewModel.labItems = getalltest;
            viewModel.patientsTests = patientstest;
            return View(viewModel);
        }
        

        [HttpPost]
		public  IActionResult AddTestsAppointments(int patientid, string [] test,string fullname,string paid)
		{
			var getalltest = _context.labItems.ToList();
            if (test!=null)
            {
                foreach (var item in test)
                {
                    var singletest = getalltest.Where(r => r.Id == Convert.ToInt32(item)).FirstOrDefault();
                    if (singletest!=null)
                    {
                        PatientsTests patientsTests = new PatientsTests();
                        patientsTests.PatientId = patientid;
                        patientsTests.FullName = fullname;
                        patientsTests.TestName = singletest.TestName;
                        patientsTests.addedDatetime = DateTime.Now;
                        patientsTests.NormalValue = singletest.NormalValue;
                        if (paid=="yes")
                        {
                            patientsTests.Paid = true;
                        }
                        patientsTests.Price = singletest.Price;
                        patientsTests.TestId = Convert.ToInt32(item);
                        _context.patientsTests.Add(patientsTests);
						 _context.SaveChanges();
                    }
                }
            }
			var patient =  _context.patients.Find(patientid);
            
			var patientstest =  _context.patientsTests.Where(r => r.PatientId == patientid).ToList();
			TestAppointmentsViewModel viewModel = new TestAppointmentsViewModel();
			viewModel.patient = patient;
			viewModel.labItems = getalltest;
            viewModel.patientsTests= patientstest;
			return View(viewModel);
		}
        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public IActionResult Print(string documentText)
        //{
        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.PrintPage += (s, e) =>
        //    {
        //        e.Graphics.DrawString("sk", new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
        //    };

        //    printDocument.Print();
        //    return RedirectToAction("Index");
        //}

        //public void Print()
        //{
        //    // Create a new document
        //    Document document = new Document();

        //    // Set the file path for the PDF
        //    string fileName = "invoice.pdf";
        //   var filePath= Path.Combine(_webHostEnvironment.WebRootPath, "uploads/branding", fileName);

        //    ///// GET UPLOAD PATH /////                          
        //    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/");

        //    var uploads = Path.Combine(uploadPath, oken");
        //    var filePath = Path.Combine(uploads, fileName);

        //    if (!Directory.Exists(uploads))
        //    {
        //        Directory.CreateDirectory(uploads);
        //    }
        //    else
        //    {
        //        string[] files = Directory.GetFiles(uploads);
        //        foreach (string deleteFile in files)
        //        {
        //            System.IO.File.Delete(deleteFile);
        //        }

        //    }

        //    using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //    {
        //        // Create a PdfWriter to write the document to the FileStream
        //        PdfWriter writer = PdfWriter.GetInstance(document, fs);

        //        // Open the document for writing
        //        document.Open();

        //        // Invoice details
        //        string invoiceNumber = "INV-2023-001";
        //        DateTime invoiceDate = DateTime.Now;
        //        string customerName = "John Doe";
        //        decimal totalAmount = 500.00m;

        //        // Create a PdfPTable for the invoice
        //        PdfPTable table = new PdfPTable(2);


        //        // Add invoice details to the table
        //        table.AddCell("Invoice Number:");
        //        table.AddCell(invoiceNumber);
        //        table.AddCell("Invoice Date:");
        //        table.AddCell(invoiceDate.ToString("yyyy-MM-dd"));
        //        table.AddCell("Customer:");
        //        table.AddCell(customerName);
        //        table.AddCell("Total Amount:");
        //        table.AddCell(totalAmount.ToString("C")); // Currency formatting
        //        foreach (var cell in table.Rows.SelectMany(r => r.GetCells()))
        //        {
        //            cell.Border = PdfPCell.NO_BORDER;
        //        }

        //        // Add the table to the document
        //        document.Add(table);

        //        // Close the document
        //        document.Close();
        //    }

        //    // Print the PDF
        //    PrintPDF(filePath);
        //}

        //private void PrintPDF(string filePath)
        //{
        //    try
        //    {
        //        // Create a ProcessStartInfo to specify the print command
        //        ProcessStartInfo psi = new ProcessStartInfo()
        //        {
        //            FileName = filePath,
        //            Verb = "print",
        //            UseShellExecute = true
        //        };

        //        // Start the process (print the PDF)
        //        Process.Start(psi);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions related to printing
        //        Console.WriteLine("Error printing the PDF: " + ex.Message);
        //    }
        //}
        public IActionResult Print()
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (s, e) =>
            {
                // Receipt details
                string fullName = "John Doe";
                string fee = "$100";
                DateTime appointmentDateTime = DateTime.Now;
                string doctorName = "Dr. Smith";
                string doctorFee = "$200";
                string gender = "Male";
                int age = 30;

                // Create a fixed-width font for the table
                System.Drawing.Font font = new System.Drawing.Font("Courier New", 12);
                Brush brush = Brushes.Black;

                // Calculate the position to start printing
                float pageWidth = e.PageBounds.Width;
                float y = 0; // You can adjust this value to control the top margin

                // Define table column widths and positions
                float col1Width = 200;
                float col2Width = 150;

                // Define the horizontal space adjustment
                float horizontalSpaceAdjustment = 30; // Adjust this value as needed

                // Draw the watermark
                e.Graphics.DrawString("A+ CLINIC", new System.Drawing.Font(font, FontStyle.Bold), Brushes.Gray, 10, 10);

                // Draw the table headers
                e.Graphics.DrawString("Appointment Receipt", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, y + 40);
                e.Graphics.DrawLine(Pens.Black, 10, y + 65, pageWidth - 10, y + 65);

                // Draw the table content
                y += 70;
                e.Graphics.DrawString("Patient:", font, brush, 10, y);
                e.Graphics.DrawString(fullName, font, brush, col1Width - horizontalSpaceAdjustment, y);

                y += font.Height;

                e.Graphics.DrawString("Gender:", font, brush, 10, y); // Add gender
                e.Graphics.DrawString(gender, font, brush, col1Width - horizontalSpaceAdjustment, y);

                y += font.Height;

                e.Graphics.DrawString("Age:", font, brush, 10, y); // Add age
                e.Graphics.DrawString(age.ToString(), font, brush, col1Width - horizontalSpaceAdjustment, y);

                y += font.Height;

                e.Graphics.DrawString("Date:", font, brush, 10, y);
                e.Graphics.DrawString(appointmentDateTime.ToString("yyyy-MM-dd"), font, brush, col1Width - horizontalSpaceAdjustment, y);

                y += font.Height;

                e.Graphics.DrawString("Time:", font, brush, 10, y);
                e.Graphics.DrawString(appointmentDateTime.ToString("HH:mm:ss"), font, brush, col1Width - horizontalSpaceAdjustment, y);

                y += font.Height;

                e.Graphics.DrawString("Fee:", font, brush, 10, y);
                e.Graphics.DrawString(fee, font, brush, col1Width - horizontalSpaceAdjustment, y);

                y += font.Height + 15;

                e.Graphics.DrawLine(Pens.Black, 10, y, pageWidth - 10, y);

                y += 5;

                e.Graphics.DrawString("Doctor:", font, brush, 10, y);
                e.Graphics.DrawString(doctorName, font, brush, col1Width - horizontalSpaceAdjustment, y);

                y += font.Height;

                e.Graphics.DrawString("Doctor Fee:", font, brush, 10, y);
                e.Graphics.DrawString(doctorFee, font, brush, col1Width - horizontalSpaceAdjustment, y);

                // Draw a border around the table
                e.Graphics.DrawRectangle(Pens.Black, 5, 5, pageWidth - 10, y + font.Height + 5);

            };

            // Print the document
            printDocument.Print();

            return RedirectToAction("Index");
        }

        //public IActionResult Print()
        //{
        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.PrintPage += (s, e) =>
        //    {
        //        // Receipt details
        //        string fullName = "John Doe";
        //        string fee = "$100";
        //        DateTime appointmentDateTime = DateTime.Now;
        //        string doctorName = "Dr. Smith";
        //        string doctorFee = "$200";

        //        // Create a fixed-width font for the table
        //        System.Drawing.Font font = new System.Drawing.Font("Courier New", 12);
        //        Brush brush = Brushes.Black;

        //        // Calculate the position to start printing
        //        float pageWidth = e.PageBounds.Width;
        //        float y = 0; // You can adjust this value to control the top margin

        //        // Define table column widths and positions
        //        float col1Width = 200;
        //        float col2Width = 150;

        //        // Draw the table headers
        //        e.Graphics.DrawString("Appointment Receipt", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, y + 10);
        //        e.Graphics.DrawLine(Pens.Black, 10, y + 35, pageWidth - 10, y + 35);

        //        // Draw the table content
        //        y += 40;
        //        e.Graphics.DrawString("Patient:", font, brush, 10, y);
        //        e.Graphics.DrawString(fullName, font, brush, col1Width, y);
        //        y += font.Height + 5;

        //        e.Graphics.DrawString("Date:", font, brush, 10, y);
        //        e.Graphics.DrawString(appointmentDateTime.ToString("yyyy-MM-dd HH:mm:ss"), font, brush, col1Width, y);
        //        y += font.Height + 5;

        //        e.Graphics.DrawString("Fee:", font, brush, 10, y);
        //        e.Graphics.DrawString(fee, font, brush, col1Width, y);
        //        y += font.Height + 15;

        //        e.Graphics.DrawLine(Pens.Black, 10, y, pageWidth - 10, y);

        //        y += 5;

        //        e.Graphics.DrawString("Doctor:", font, brush, 10, y);
        //        e.Graphics.DrawString(doctorName, font, brush, col1Width, y);
        //        y += font.Height + 5;

        //        e.Graphics.DrawString("Doctor Fee:", font, brush, 10, y);
        //        e.Graphics.DrawString(doctorFee, font, brush, col1Width, y);

        //        // Draw a border around the table
        //        e.Graphics.DrawRectangle(Pens.Black, 5, 5, pageWidth - 10, y + font.Height + 5);

        //    };

        //    // Print the document
        //    printDocument.Print();

        //    return RedirectToAction("Index");
        //}

        //public IActionResult Print()
        //{
        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.PrintPage += (s, e) =>
        //    {
        //        // Receipt details
        //        string fullName = "John Doe";
        //        string fee = "$100";
        //        string token = "12345";

        //        // Create a StringBuilder to build the receipt text
        //        System.Text.StringBuilder receiptText = new System.Text.StringBuilder();

        //        // Build the receipt text
        //        receiptText.AppendLine("Receipt");
        //        receiptText.AppendLine("-----------------------");
        //        receiptText.AppendLine($"Name: {fullName}");
        //        receiptText.AppendLine($"Fee: {fee}");
        //        receiptText.AppendLine($"Token: {token}");
        //        // Add other dynamic content here

        //        // Create a font and brush for the text
        //        System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
        //        Brush brush = Brushes.Black;

        //        // Calculate the position to start printing
        //        float pageWidth = e.PageBounds.Width;
        //        float pageHeight = e.PageBounds.Height;
        //        SizeF textSize = e.Graphics.MeasureString(receiptText.ToString(), font);
        //        float x = (pageWidth - textSize.Width) / 2; // Center horizontally

        //        // Adjust the 'y' coordinate to position the text at the top of the page
        //        float y = 0; // You can set a small margin if needed

        //        // Draw the receipt text on the page
        //        e.Graphics.DrawString(receiptText.ToString(), font, brush, 0, y);
        //        // e.Graphics.DrawString(receiptText.ToString(), font, brush, x, y);
        //    };

        //    // Print the document
        //    printDocument.Print();

        //    return RedirectToAction("Index");
        //}
        //public IActionResult Print()
        //{
        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.PrintPage += (s, e) =>
        //    {
        //        // Define the text to be printed
        //        string textToPrint = "Hello, this is the text to be printed.";

        //        // Create a font and brush for the text
        //        Font font = new Font("Arial", 12);
        //        Brush brush = Brushes.Black;

        //        // Calculate the position to start printing
        //        float x = 100; // X-coordinate
        //        float y = 100; // Y-coordinate

        //        // Draw the text on the page
        //        e.Graphics.DrawString(textToPrint, font, brush, x, y);
        //    };

        //    // Print the document
        //    printDocument.Print();

        //    return RedirectToAction("Index");
        //}
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePaid(Patient patient)
		{
			patient.PaymentStatus = true;

            var tokens = _context.patientTokens.FirstOrDefault();
            int counter = 0;
            if (tokens != null)
            {
                counter = tokens.Counter;
                tokens.Counter = counter + 1;
                patient.TokenNumber = counter;
                _context.Update(tokens);
                _context.SaveChanges();
            }
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
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
			

			var tokens = _context.patientTokens.FirstOrDefault();
            int counter = 0;
            if (tokens!=null)
			{
				counter = tokens.Counter;

            }
		
			patient.TokenNumber=counter;

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
		public async Task<IActionResult> AddSampleCollection(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var patienttest = await _context.patientsTests.FindAsync(id);
			if (patienttest == null)
			{
				return NotFound();
			}
			return View(patienttest);
		}
		[HttpPost]
		public async Task<IActionResult> AddSampleCollection(int? id, PatientsTests patientsTests)
		{
			if (id == null)
			{
				return NotFound();
			}

			var patienttest = await _context.patientsTests.FindAsync(id);
			if (patienttest != null)
			{
				patienttest.SampleName = patientsTests.SampleName;
                patienttest.CollectionTime=DateTime.Now;
				_context.patientsTests.Update(patienttest);
				_context.SaveChanges();
			}
			return RedirectToAction("SampleCollection", new {id=patienttest.PatientId});
		}
		public async Task<IActionResult> AddTestResult(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var patienttest = await _context.patientsTests.FindAsync(id);
            if (patienttest == null)
            {
                return NotFound();
            }
            return View(patienttest);
        }
        [HttpPost]
		public async Task<IActionResult> AddTestResult(int? id,PatientsTests patientsTests)
		{
			if (id == null)
			{
				return NotFound();
			}

			var patienttest = await _context.patientsTests.FindAsync(id);
			if (patienttest != null)
			{
				patienttest.TestResult=patientsTests.TestResult;
				patienttest.ResultAddedDatetime = DateTime.Now;
				_context.patientsTests.Update(patienttest);
                _context.SaveChanges();
			}
			return RedirectToAction("SampleCollection", new { id = patienttest.PatientId });
		}

		// POST: Patients/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }
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

			//if (ModelState.IsValid)
			//{

			//}
			//return View(patient);
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
