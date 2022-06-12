using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.DTOs
{
    public class RentalHistoryDTO
    {
        public int RentalId { get; set; }
        public string CustomerID { get; set; }
        public DateTime RentalDate { get; set; }
        public string CustomerName { get; set; }
        public string BookName { get; set; }
        public string BookUID { get; set; }
        public int Price { get; set; }
        public DateTime ReturnDate { get; set; }
        public Boolean Return { get; set; }
    }
}
