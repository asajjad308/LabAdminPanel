namespace DockingAdminPanel.Models
{
    public class PatientsTests
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

		public int TestId { get; set; }

        public string? SampleName { get; set; }

        public DateTime CollectionTime { get; set; }
		public string? TestName { get; set; }
		public string? FullName { get; set; }
		public double Price { get; set; }

        public Boolean Paid { get; set; }

        public DateTime addedDatetime { get; set; }

        public DateTime Paidtime { get; set; }


		public double NormalValue { get; set; }
		public double TestResult { get; set; }
		public DateTime ResultAddedDatetime { get; set; }




	}
}
