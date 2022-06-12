using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Models
{
    public class RentalHistory
    {
        [Key]
        public int RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public string CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customer? Customer { get; set; }

        public string BookUID { get; set; }
        [ForeignKey("BookUID")]
        public Book? Book { get; set; } 
        public Boolean Return { get; set; }


    }
}
