namespace DockingAdminPanel.Models
{
    public class Doctor
    {
        public int Id { get; set; }         // Unique identifier for the doctor
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}"; // Computed property for full name
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialization { get; set; } // e.g., Cardiologist, Pediatrician, etc.
        public string LicenseNumber { get; set; } // Doctor's license number
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double fee { get; set; }
    }
}
