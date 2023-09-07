namespace DockingAdminPanel.Models
{
    public class Patient
    {
        public int Id { get; set; }             // Unique identifier for the patient
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}"; // Computed property for full name
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string MedicalRecordNumber { get; set; } // Unique patient identifier in the medical system
        public string InsuranceProvider { get; set; }   // e.g., Blue Cross, Aetna, etc.
        public string PolicyNumber { get; set; }        // Patient's insurance policy number
        public string Illness { get; set; }
    }
}
