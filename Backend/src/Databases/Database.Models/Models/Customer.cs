using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Models
{
    public class Customer
    {
        [Key]
        public string CustomerID { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty ;
        public int Point { get; set; } = 0;
        public int AllTimeSpent { get; set; } = 0;
        public int BookRenting { get; set; } = 0;
    }
}
