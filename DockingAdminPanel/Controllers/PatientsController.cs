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
			var labcategories = _context.labCategories.ToList();
			viewModel.labCategories = labcategories;
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
                        patientsTests.Paid = true;
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
            var labcategories=_context.labCategories.ToList();
            viewModel.labCategories= labcategories;
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
        public IActionResult Printx(int Id)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (s, e) =>
            {
                // Receipt details
                string cashierName = "Jane Smith";
                string fullName = "John Doe";
                string patientAddress = "123 Main St, City";
                string patientPhone = "(555) 123-4567";
                string invoiceNumber = "INV-2023001";
                string receiptDate = DateTime.Now.ToString("yyyy-MM-dd");
                string fee = "$100.00";
                string tax = "$10.00";
                string totalAmount = "$110.00";
                string doctorName = "Dr. Smith";
                string doctorSpecialty = "Cardiologist";
                string doctorLicense = "License #12345";
                string gender = "Male";
                string tokenNumber = "Token Number: 12345";
                string InoviceNumber = "Invoice Number: 12345";
                int age = 30;

                var patient = _context.patients.Find(Id);
                fullName = patient.FirstName;
                patientAddress = patient.Address;
                patientPhone = patient.PhoneNumber;
                invoiceNumber = patient.TokenNumber.ToString();
                gender = patient.Sex;
                age = patient.Age;
                tokenNumber = patient.TokenNumber.ToString();
                invoiceNumber = patient.Id.ToString();

                // Create a font and brush for text
                System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
                Brush brush = Brushes.Black;

                // Calculate the position to start printing
                float pageWidth = e.PageBounds.Width;
                float y = 0; // You can adjust this value to control the top margin

                // Define table column widths and positions
                float col1Width = 200;
                float col2Width = 150;

                // Define the horizontal space adjustment
                float horizontalSpaceAdjustment = 70; // Adjust this value as needed

                // Draw the watermark
                e.Graphics.DrawString("A+ CLINIC", new System.Drawing.Font(font, FontStyle.Regular), Brushes.Gray, 10, 10);

                // Include the token number
                e.Graphics.DrawString("Invoice No:" + invoiceNumber, font, brush, 10, 40);

                // Include the date and time
                string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Adjust the date and time format as needed

                // Create a table for the cashier information
                float cashierTableY = 70;
                e.Graphics.DrawRectangle(Pens.Black, 5, cashierTableY, pageWidth - 10, 100);
                e.Graphics.DrawString("Cashier Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, cashierTableY + 10);
                e.Graphics.DrawString("Issued By:" + cashierName, font, brush, 10, cashierTableY + 40);
                e.Graphics.DrawString("Date: " + DateTime.Now.ToString("yyyy-MM-dd"), font, brush, 10, cashierTableY + 60);
                e.Graphics.DrawString("Time: " + DateTime.Now.ToString("HH:mm:ss"), font, brush, 10, cashierTableY + 80);

                // Create a table for the patient information
                float patientTableY = cashierTableY + 110;
                e.Graphics.DrawRectangle(Pens.Black, 5, patientTableY, pageWidth - 10, 180);
                e.Graphics.DrawString("Patient Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, patientTableY);

                // Define the column headers
                e.Graphics.DrawString("Field", font, brush, 10, patientTableY + 20);
                e.Graphics.DrawString("Value", font, brush, col1Width - horizontalSpaceAdjustment, patientTableY + 20);

                // Define patient details and print them in a loop
                string[] patientFields = { "Token No:", "Patient Name:", "Address:", "Phone:", "Gender:", "Age:" };
                string[] patientValues = { tokenNumber, fullName, patientAddress, patientPhone, gender, age.ToString() };

                for (int i = 0; i < patientFields.Length; i++)
                {
                    e.Graphics.DrawString(patientFields[i], font, brush, 10, patientTableY + 40 + i * 20);
                    e.Graphics.DrawString(patientValues[i], font, brush, col1Width - horizontalSpaceAdjustment, patientTableY + 40 + i * 20);
                }

                // Continue drawing the rest of the information...

                // Draw the billing details...

                // Create a table for the doctor information
                float doctorTableY = patientTableY + 190;
                e.Graphics.DrawRectangle(Pens.Black, 5, doctorTableY, pageWidth - 10, 120);
                e.Graphics.DrawString("Doctor Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, doctorTableY + 10);
                e.Graphics.DrawString("Doctor:", font, brush, 10, doctorTableY + 40);
                e.Graphics.DrawString(doctorName, font, brush, col1Width - horizontalSpaceAdjustment, doctorTableY + 40);
                e.Graphics.DrawString("Specialty:", font, brush, 10, doctorTableY + 60);
                e.Graphics.DrawString(doctorSpecialty, font, brush, col1Width - horizontalSpaceAdjustment, doctorTableY + 60);
                e.Graphics.DrawString("License:", font, brush, 10, doctorTableY + 80);
                e.Graphics.DrawString(doctorLicense, font, brush, col1Width - horizontalSpaceAdjustment, doctorTableY + 80);

                // Draw the description and amount (same as before)...

                // Create a table for the billing information
                float billingTableY = doctorTableY + 130;
                horizontalSpaceAdjustment = 60;
                e.Graphics.DrawRectangle(Pens.Black, 5, billingTableY, pageWidth - 10, 100);
                e.Graphics.DrawString("Billing Details", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, billingTableY + 10);
                e.Graphics.DrawString("Description", font, brush, 10, billingTableY + 40);
                e.Graphics.DrawString("Amount", font, brush, col1Width - horizontalSpaceAdjustment, billingTableY + 40);
                e.Graphics.DrawString("doctor Fee", font, brush, 10, billingTableY + 60);
                e.Graphics.DrawString(fee, font, brush, col1Width - horizontalSpaceAdjustment, billingTableY + 60);
                e.Graphics.DrawString("Tax (10%)", font, brush, 10, billingTableY + 80);
                e.Graphics.DrawString(tax, font, brush, col1Width - horizontalSpaceAdjustment, billingTableY + 80);

                e.Graphics.DrawRectangle(Pens.Black, 5, 5, pageWidth - 10, billingTableY + 100 + font.Height + 5);

                // Draw the total amount
                float totalAmountY = billingTableY + 130;
                e.Graphics.DrawString("Total Amount", font, brush, 10, totalAmountY);

                e.Graphics.DrawString(totalAmount, font, brush, col1Width - horizontalSpaceAdjustment, totalAmountY);
            };

            // Print the document
            printDocument.Print();

            return RedirectToAction("Index");
        }
        public IActionResult Print(int Id)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (s, e) =>
            {
                // Receipt details
                string cashierName = "Jane Smith";
                string fullName = "John Doe";
                string patientAddress = "123 Main St, City";
                string patientPhone = "(555) 123-4567";
                string invoiceNumber = "INV-2023001";
                string receiptDate = DateTime.Now.ToString("yyyy-MM-dd");
                string fee = "$100.00";
                string tax = "$10.00";
                string totalAmount = "$110.00";
                string doctorName = "Dr. Smith";
                string doctorSpecialty = "Cardiologist";
                string doctorLicense = "License #12345";
                string gender = "Male";
                string tokenNumber = "Token Number: 12345";
                string InoviceNumber = "Invoice Number: 12345";
                int age = 30;

                var patient = _context.patients.Find(Id);
                fullName = patient.FirstName;
                patientAddress = patient.Address;
                patientPhone = patient.PhoneNumber;
                invoiceNumber = patient.TokenNumber.ToString();
                gender = patient.Sex;
                age = patient.Age;
                tokenNumber = patient.TokenNumber.ToString();
                invoiceNumber = patient.Id.ToString();

                // Create a font and brush for text
                System.Drawing.Font font = new System.Drawing.Font("Arial",8);
                Brush brush = Brushes.Black;

                // Calculate the position to start printing
                float pageWidth = e.PageBounds.Width;
                float y = 0; // You can adjust this value to control the top margin

                // Define table column widths and positions
                float col1Width = 60;
                float col2Width = 60;

                // Define the horizontal space adjustment
                float horizontalSpaceAdjustment = 70;

                // Create a table for the cashier information
                float cashierTableY = 70;
                
                e.Graphics.DrawString("Cashier Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, cashierTableY + 10);

                // Define cashier details and print them
                string[] cashierFields = { "Issued By:", "Date:", "Time:" };
                string[] cashierValues = { cashierName, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm:ss") };

                for (int i = 0; i < cashierFields.Length; i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black, 10 + i * col1Width, cashierTableY + 30, col1Width, 40);
                    e.Graphics.DrawString(cashierFields[i], new System.Drawing.Font(font, FontStyle.Bold), brush, 10 + i * col1Width, cashierTableY + 40);
                    e.Graphics.DrawRectangle(Pens.Black, 10 + i * col1Width, cashierTableY + 70, col1Width, 40);
                    e.Graphics.DrawString(cashierValues[i], font, brush, 10 + i * col1Width, cashierTableY + 80);
                }

                // Create a table for the patient information
                float patientTableY = cashierTableY + 120;
               
                e.Graphics.DrawString("Patient Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, patientTableY);

                // Define the column headers for patient information
                string[] patientFields = { "Token No", "Name", "Address", "Phone", "Gender" };
                string[] patientValues = { tokenNumber, fullName, patientAddress, patientPhone, gender };

                for (int i = 0; i < patientFields.Length; i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black, 10 + i * col1Width, patientTableY + 30, col1Width, 40);
                    e.Graphics.DrawString(patientFields[i], new System.Drawing.Font(font, FontStyle.Bold), brush, 10 + i * col1Width, patientTableY + 40);
                    e.Graphics.DrawRectangle(Pens.Black, 10 + i * col1Width, patientTableY + 70, col1Width, 40);
                    e.Graphics.DrawString(patientValues[i], font, brush, 10 + i * col1Width, patientTableY + 80);
                }

                // Draw Age as a separate row
                e.Graphics.DrawRectangle(Pens.Black, 10, patientTableY + 110, col1Width, 40);
                e.Graphics.DrawString("Age", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, patientTableY + 120);
                e.Graphics.DrawRectangle(Pens.Black, 10, patientTableY + 150, col1Width, 40);
                e.Graphics.DrawString(age.ToString(), font, brush, 10, patientTableY + 160);

                // ... (continue drawing other sections similarly)

                // Draw the total amount
                float billingTableY = patientTableY + 140;
                e.Graphics.DrawRectangle(Pens.Black, 5, billingTableY, pageWidth - 10, 80);
                e.Graphics.DrawString("Total Amount", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, billingTableY + 10);
                e.Graphics.DrawRectangle(Pens.Black, 10, billingTableY + 30, col1Width, 40);
                e.Graphics.DrawString(totalAmount, font, brush, 10, billingTableY + 40);
            };

            // Print the document
            printDocument.Print();

            return RedirectToAction("Index");
        }


        //public IActionResult Print(int Id)
        //{
        //	PrintDocument printDocument = new PrintDocument();
        //	printDocument.PrintPage += (s, e) =>
        //	{
        //		// Receipt details
        //		string cashierName = "Jane Smith";
        //		string fullName = "John Doe";
        //		string patientAddress = "123 Main St, City";
        //		string patientPhone = "(555) 123-4567";
        //		string invoiceNumber = "INV-2023001";
        //		string receiptDate = DateTime.Now.ToString("yyyy-MM-dd");
        //		string fee = "$100.00";
        //		string tax = "$10.00";
        //		string totalAmount = "$110.00";
        //		string doctorName = "Dr. Smith";
        //		string doctorSpecialty = "Cardiologist";
        //		string doctorLicense = "License #12345";
        //		string gender = "Male";
        //              string tokenNumber = "Token Number: 12345";
        //              string InoviceNumber = "Invoice Number: 12345";
        //              int age = 30;
        //		var patient=	_context.patients.Find(Id);
        //		fullName = patient.FirstName;
        //		patientAddress = patient.Address;
        //		patientPhone = patient.PhoneNumber; 
        //		invoiceNumber = patient.TokenNumber.ToString();
        //		gender = patient.Sex;
        //		age=patient.Age;
        //		tokenNumber= patient.TokenNumber.ToString();
        //		invoiceNumber = patient.Id.ToString();
        //		// Create a font and brush for text
        //		System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
        //		Brush brush = Brushes.Black;

        //		// Calculate the position to start printing
        //		float pageWidth = e.PageBounds.Width;
        //		float y = 0; // You can adjust this value to control the top margin

        //		// Define table column widths and positions
        //		float col1Width = 200;
        //		float col2Width = 150;

        //		// Define the horizontal space adjustment
        //		float horizontalSpaceAdjustment = 70; // Adjust this value as needed

        //		// Draw the watermark
        //		e.Graphics.DrawString("A+ CLINIC", new System.Drawing.Font(font, FontStyle.Regular), Brushes.Gray, 10, 10);
        //              // Include the token number

        //              // Adjust the Y coordinate (40) as needed
        //              e.Graphics.DrawString("Invoice No:" + invoiceNumber, font, brush, 10, 40);
        //              // Include the date and time
        //              string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Adjust the date and time format as needed

        //              // Create a table for the cashier information
        //              float cashierTableY = 70;
        //		e.Graphics.DrawRectangle(Pens.Black, 5, cashierTableY, pageWidth - 10, 100);
        //		e.Graphics.DrawString("Cashier Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, cashierTableY + 10);
        //		e.Graphics.DrawString("Issued By:"+cashierName, font, brush, 10, cashierTableY + 40);
        //              e.Graphics.DrawString("Date: " + DateTime.Now.ToString("yyyy-MM-dd"), font, brush, 10, cashierTableY+60);
        //              e.Graphics.DrawString("Time: " + DateTime.Now.ToString("HH:mm:ss"), font, brush, 10, cashierTableY+80);
        //              // Create a table for the patient information
        //              float patientTableY = cashierTableY + 110;
        //		e.Graphics.DrawRectangle(Pens.Black, 5, patientTableY, pageWidth - 10, 180);
        //		e.Graphics.DrawString("Patient Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, patientTableY );
        //		 e.Graphics.DrawString("Token No:"+tokenNumber, font, brush, 10, patientTableY+20);
        //		e.Graphics.DrawString("Patient Name:", font, brush, 10, patientTableY + 40);
        //		e.Graphics.DrawString(fullName, font, brush, col1Width - horizontalSpaceAdjustment, patientTableY + 40);
        //		e.Graphics.DrawString("Address:", font, brush, 10, patientTableY + 60);
        //		e.Graphics.DrawString(patientAddress, font, brush, col1Width - horizontalSpaceAdjustment, patientTableY + 60);
        //		e.Graphics.DrawString("Phone:", font, brush, 10, patientTableY + 80);
        //		e.Graphics.DrawString(patientPhone, font, brush, col1Width - horizontalSpaceAdjustment, patientTableY + 80);
        //		e.Graphics.DrawString("Gender:", font, brush, 10, patientTableY + 100);
        //		e.Graphics.DrawString(gender, font, brush, col1Width - horizontalSpaceAdjustment, patientTableY + 100);
        //		e.Graphics.DrawString("Age:", font, brush, 10, patientTableY + 120);
        //		e.Graphics.DrawString(age.ToString(), font, brush, col1Width - horizontalSpaceAdjustment, patientTableY + 120);

        //		// Continue drawing the rest of the information...

        //		// Draw the billing details...

        //		// Create a table for the doctor information
        //		float doctorTableY = patientTableY + 190;
        //		e.Graphics.DrawRectangle(Pens.Black, 5, doctorTableY, pageWidth - 10, 120);
        //		e.Graphics.DrawString("Doctor Information", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, doctorTableY + 10);
        //		e.Graphics.DrawString("Doctor:", font, brush, 10, doctorTableY + 40);
        //		e.Graphics.DrawString(doctorName, font, brush, col1Width - horizontalSpaceAdjustment, doctorTableY + 40);
        //		e.Graphics.DrawString("Specialty:", font, brush, 10, doctorTableY + 60);
        //		e.Graphics.DrawString(doctorSpecialty, font, brush, col1Width - horizontalSpaceAdjustment, doctorTableY + 60);
        //		e.Graphics.DrawString("License:", font, brush, 10, doctorTableY + 80);
        //		e.Graphics.DrawString(doctorLicense, font, brush, col1Width - horizontalSpaceAdjustment, doctorTableY + 80);

        //		// Draw the description and amount (same as before)...

        //		// Create a table for the billing information
        //		float billingTableY = doctorTableY + 130;
        //		horizontalSpaceAdjustment = 60;
        //		e.Graphics.DrawRectangle(Pens.Black, 5, billingTableY, pageWidth - 10, 100);
        //		e.Graphics.DrawString("Billing Details", new System.Drawing.Font(font, FontStyle.Bold), brush, 10, billingTableY + 10);
        //		e.Graphics.DrawString("Description", font, brush, 10, billingTableY + 40);
        //		e.Graphics.DrawString("Amount", font, brush, col1Width - horizontalSpaceAdjustment, billingTableY + 40);
        //		e.Graphics.DrawString("doctor Fee", font, brush, 10, billingTableY + 60);
        //		e.Graphics.DrawString(fee, font, brush, col1Width - horizontalSpaceAdjustment, billingTableY + 60);
        //		e.Graphics.DrawString("Tax (10%)", font, brush, 10, billingTableY + 80);
        //		e.Graphics.DrawString(tax, font, brush, col1Width - horizontalSpaceAdjustment, billingTableY + 80);

        //		e.Graphics.DrawRectangle(Pens.Black, 5, 5, pageWidth - 10, billingTableY + 100 + font.Height + 5);

        //		// Draw the total amount
        //		float totalAmountY = billingTableY + 130;
        //		e.Graphics.DrawString("Total Amount", font, brush, 10, totalAmountY);

        //		e.Graphics.DrawString(totalAmount, font, brush, col1Width - horizontalSpaceAdjustment, totalAmountY);

        //	};

        //	// Print the document
        //	printDocument.Print();

        //	return RedirectToAction("Index");
        //}

        //public IActionResult Print()
        //{
        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.PrintPage += (s, e) =>
        //    {
        //        // Receipt details
        //        string cashierName = "Cashier: Jane Smith";
        //        string fullName = "John Doe";
        //        string patientAddress = "123 Main St, City";
        //        string patientPhone = "(555) 123-4567";
        //        string invoiceNumber = "INV-2023001";
        //        string receiptDate = DateTime.Now.ToString("yyyy-MM-dd");
        //        string fee = "$100.00";
        //        string tax = "$10.00";
        //        string totalAmount = "$110.00";
        //        string doctorName = "Dr. Smith";
        //        string doctorSpecialty = "Cardiologist";
        //        string doctorLicense = "License #12345";
        //        string gender = "Male";
        //        int age = 30;

        //        // Create a font and brush for text
        //        System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
        //        Brush brush = Brushes.Black;

        //        // Calculate the position to start printing
        //        float pageWidth = e.PageBounds.Width;
        //        float y = 0; // You can adjust this value to control the top margin

        //        // Define table column widths and positions
        //        float col1Width = 200;
        //        float col2Width = 150;

        //        // Define the horizontal space adjustment
        //        float horizontalSpaceAdjustment = 30; // Adjust this value as needed

        //        // Draw the watermark
        //        e.Graphics.DrawString("A+ CLINIC", new System.Drawing.Font(font, FontStyle.Bold), Brushes.Gray, 10, 10);

        //        // Create a table for the first row
        //        float tableY = 70;
        //        float tableRowHeight = 20;
        //        float tableCol1Width = 100;
        //        float tableCol2Width = 200;

        //        // Draw the cashier name in the first row
        //        e.Graphics.DrawString("Cashier:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(cashierName, font, brush, tableCol1Width, tableY);

        //        tableY += tableRowHeight;

        //        // Draw the table headers
        //        e.Graphics.DrawString("Invoice Receipt", new System.Drawing.Font(font, FontStyle.Bold), brush, 0, tableY);
        //        e.Graphics.DrawLine(Pens.Black, 0, tableY + 25, pageWidth - 10, tableY + 25);

        //        // Draw the patient details
        //        tableY += 30;
        //        e.Graphics.DrawString("Patient Name:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(fullName, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Address:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(patientAddress, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Phone:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(patientPhone, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        // Draw the invoice details
        //        tableY += 30;
        //        e.Graphics.DrawString("Invoice Number:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(invoiceNumber, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Date:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(receiptDate, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Doctor:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(doctorName, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Specialty:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(doctorSpecialty, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("License:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(doctorLicense, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        // Draw the billing details
        //        tableY += 30;
        //        e.Graphics.DrawString("Gender:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(gender, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Age:", font, brush, 10, tableY);
        //        e.Graphics.DrawString(age.ToString(), font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 30;
        //        e.Graphics.DrawString("Description", font, brush, 10, tableY);
        //        e.Graphics.DrawString("Amount", font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Consultation Fee", font, brush, 10, tableY);
        //        e.Graphics.DrawString(fee, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        tableY += 20;
        //        e.Graphics.DrawString("Tax (10%)", font, brush, 10, tableY);
        //        e.Graphics.DrawString(tax, font, brush, col1Width - horizontalSpaceAdjustment, tableY);

        //        // Draw a border around the table
        //        e.Graphics.DrawRectangle(Pens.Black, 5, 5, pageWidth - 10, tableY + font.Height + 5);

        //        // Draw the total amount
        //        tableY += 30;
        //        e.Graphics.DrawString("Total Amount", font, brush, 10, tableY);
        //        e.Graphics.DrawString(totalAmount, font, brush, col1Width - horizontalSpaceAdjustment, tableY);
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
		public async Task<IActionResult> CreateLabUnPaid(Patient patient)
		{
			patient.PaymentStatus = false;

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


			return RedirectToAction("AddTestsAppointments", new { id = patient.Id });

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
