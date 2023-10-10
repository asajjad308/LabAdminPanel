namespace DockingAdminPanel.Models
{
	public class TestAppointmentsViewModel
	{
		public Patient patient { get; set; }
		public List<LabItems> labItems { get; set; }
        public List<Patient> patients { get; set; }
        public List<PatientsTests> patientsTests { get; set; }
	}
}
