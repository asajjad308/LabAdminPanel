﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DockingAdminPanel.Models
{
    public class Patient
    {
        public int Id { get; set; }             // Unique identifier for the patient
        public string FirstName { get; set; }
        public string ?LastName { get; set; }
        public string? LabAppointment { get; set; }
        public int TokenNumber { get; set; }
		public int AppointmentNumber { get; set; }
		public string FullName => $"{FirstName} {LastName}"; // Computed property for full name
        public string? Email { get; set; }
        public string ?PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string ?Address { get; set; }
		public string Sex { get; set; }
		public DateTime AddedDate { get; set; }
		public Boolean PaymentStatus { get; set; }

		public Boolean External { get; set; }
        public Boolean IsDeleted { get; set; }
        public double Fee { get; set; }
        public string? MedicalRecordNumber { get; set; } // Unique patient identifier in the medical system
              // Patient's insurance policy number
        public string? Illness { get; set; }
        public int DoctorId { get; set; }

         
        [NotMapped]
        public List<Doctor>? Doctors { get; set; }

        [NotMapped]
        public List<PatientsTests>? patientTest { get; set; }
    }
}
