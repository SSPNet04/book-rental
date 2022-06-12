using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.DTOs
{
    public class CustomerDTO
    {
        public string CustomerID { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Point { get; set; }
        public int AllTimeSpent { get; set; }
        public int BookRenting { get; set; }
    }
}
