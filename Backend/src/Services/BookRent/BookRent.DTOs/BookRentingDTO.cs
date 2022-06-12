using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.DTOs
{
    public class BookRentingDTO
    {
        public CustomerDTO Customer { get; set; }
        public List<BookDTO> Books { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
