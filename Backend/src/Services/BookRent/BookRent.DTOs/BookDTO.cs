using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.DTOs
{
    public class BookDTO
    {
        public string BookUID { get; set; } = string.Empty;
        public string? MalID { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int Late { get; set; }
        public int InStock { get; set; }
        public int InRental { get; set; }
        public int Price { get; set; }
        public int AllTimeRent { get; set; }
    }
}
