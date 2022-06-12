using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.DTOs
{
    public class ReturnBookDTO
    {
        public string CustomerID { get; set; }
        public string BookUID { get; set; }
        public int LateFee { get; set; }
    }
}
