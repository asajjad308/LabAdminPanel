using DockingApi.Models;
using System.ComponentModel.DataAnnotations;

namespace DockingApi.Data
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string address { get; set; }
        public string location { get; set; }
        public double rentPerMonth { get; set; }
        public string spaceNumber { get; set; }
        public int   UserId { get; set; }
        public bool status { get; set; }
        public bool adminApproval { get; set; } = false;
        public DateTime contractDate { get; set; }
        public bool available { get; set; }
        public DateTime addedDate { get; set; }
        public String? Category { get; set; }
      
    }
}
