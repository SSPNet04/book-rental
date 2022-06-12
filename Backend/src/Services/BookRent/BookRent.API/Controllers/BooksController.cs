using BookRent.DTOs;
using Database.Models;
using Database.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private DatabaseContext DB;

        public BooksController(DatabaseContext db)
        {
            DB = db;
        }


        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            var books = await DB.Books.ToListAsync();
            List<BookDTO> bookResults = new List<BookDTO>();
            foreach (var book in books)
            {
                BookDTO bookDTO = new BookDTO();
                bookDTO.BookUID = book.BookUID;
                bookDTO.MalID = book.MalID;
                bookDTO.BookName = book.BookName;
                bookDTO.Late = book.Late;
                bookDTO.InStock = book.InStock;
                bookDTO.InRental = book.InRental;
                bookDTO.Price = book.Price;
                bookDTO.AllTimeRent= book.AllTimeRent;
                bookResults.Add(bookDTO);
            }
            return Ok(bookResults);
        }


        [HttpPost]
        public async Task<ActionResult> AddBook(AddBookDTO addbookDTO)
        {
            var existed = await DB.Books.Where(s => s.BookUID == addbookDTO.BookUID).AnyAsync();
            if (existed)
            {
                return BadRequest("Duplicate UID");
            }

            var book = new Book
            {
                BookUID = addbookDTO.BookUID,
                MalID = addbookDTO.MalID,
                BookName = addbookDTO.BookName,
                InStock = addbookDTO.InStock,
                Price = addbookDTO.Price
            };
            DB.Books.Add(book);
            await DB.SaveChangesAsync();
            return Ok("Book Added");
        }


        [HttpPut]
        public async Task<ActionResult> UpdateBook(string bookUID,UpdateBookDTO updateBookDTO)
        {
            var update = await DB.Books.Where(s => s.BookUID == bookUID).FirstOrDefaultAsync();
            if (update == null)
            {
                return NotFound("No book found");
            }
            update.BookName = updateBookDTO.BookName;
            update.MalID = updateBookDTO.MalID;
            update.InStock = updateBookDTO.InStock;
            update.Price = updateBookDTO.Price;
            await DB.SaveChangesAsync();
            return Ok("Book Updated");
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteBook(string bookUID)
        {
            var delete = await DB.Books.Where(s => s.BookUID == bookUID).FirstOrDefaultAsync();
            if(delete == null)
            {
                return NotFound("No book found");
            }
            DB.Books.Remove(delete);
            await DB.SaveChangesAsync();
            return Ok("Book Deleted");
        }


        [HttpGet("Search")]
        public async Task<ActionResult> SearchBooks(string sortBy, bool descending,string? name = null,string? bookInfo = null)
        {
            var query = DB.Books.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where
                (c => c.BookName.ToLower().Contains(name.ToLower()) 
                || c.BookUID.Contains(name)
                || c.MalID.Contains(name));
            if(!string.IsNullOrEmpty(bookInfo))
                query = query.Where(b => b.BookUID==bookInfo);

            if (sortBy == "BookUID")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.BookUID);
                }
                else
                {
                    query = query.OrderByDescending(s => s.BookUID);
                }
            }
            else if (sortBy == "MalID")
            {
                if (descending == false)
                {
                    query = query.OrderBy(s => s.MalID);
                }
                else
                {
                    query = query.OrderByDescending(s => s.MalID);
                }
            }
            else if (sortBy == "BookName")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.BookName);
                }
                else
                {
                    query = query.OrderByDescending(s => s.BookName);
                }
            }
            else if (sortBy == "Late")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.Late);
                }
                else
                {
                    query=query.OrderByDescending(s => s.Late);
                }
            }
            else if (sortBy == "InStock")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.InStock);
                }
                else
                {
                    query = query.OrderByDescending(s => s.InStock);
                }
            }
            else if (sortBy == "InRental")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.InRental);
                }
                else
                {
                    query = query.OrderByDescending(s => s.InRental);
                }
            }
            else if (sortBy == "Price")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.Price);
                }
                else
                {
                    query = query.OrderByDescending(s => s.Price);
                }
            }
            else if (sortBy == "AllTimeRent")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.AllTimeRent);
                }
                else
                {
                    query = query.OrderByDescending(s => s.AllTimeRent);
                }
            }

            var searchBook = await query.ToListAsync();
            List<BookDTO> bookResults = new List<BookDTO>();
            foreach (var book in searchBook)
            {
                BookDTO bookDTO = new BookDTO();
                bookDTO.BookUID = book.BookUID;
                bookDTO.MalID = book.MalID;
                bookDTO.BookName = book.BookName;
                bookDTO.Late = book.Late;
                bookDTO.InStock = book.InStock;
                bookDTO.InRental = book.InRental;
                bookDTO.Price = book.Price;
                bookDTO.AllTimeRent = book.AllTimeRent;
                bookResults.Add(bookDTO);
            }
            return Ok(bookResults);
        }

        //[HttpGet("Info")]
        //public async Task<ActionResult> BookInfo(string id)
        //{

        //    return Ok();
        //}


    }
}
