using System.ComponentModel.DataAnnotations.Schema;

namespace DockingAdminPanel.Models
{
    public class LabItems
    {
        public int Id { get; set; }
        
        public string TestName { get; set; }
        public double Price { get; set; }
		public double Cost { get; set; }
		public string Units { get; set; }
        
        public string NormalValue { get; set; }
        public int CategoryId { get; set; }

        [NotMapped]
        public List<LabCategory>? labCategories { get; set; }
    }
}
