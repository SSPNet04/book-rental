using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.DTOs
{
    public class AddBookDTO
    {
        public string BookUID { get; set; }
        public string? MalID { get; set; }
        public string BookName { get; set; }
        public int InStock { get; set; }
        public int Price { get; set; }
    }
}