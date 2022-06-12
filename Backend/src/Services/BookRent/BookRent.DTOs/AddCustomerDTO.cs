using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.DTOs
{
    public class AddCustomerDTO
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
