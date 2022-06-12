using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Models
{
    public class Book
    {
        [Key]
        public string BookUID { get; set; } = string.Empty;
        public string? MalID { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int Late { get; set; } = 0;
        public int InStock { get; set; }
        public int InRental { get; set; } = 0;
        public int Price { get; set; }
        public int AllTimeRent { get; set; } = 0;
    }
}
