using BookRent.DTOs;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalHistoryController : ControllerBase
    {
        private DatabaseContext DB;
        public RentalHistoryController(DatabaseContext db)
        {
            DB = db;
        }


        [HttpGet]
        public async Task<ActionResult> GetRentalHistory()
        {
            var historyQuery = from h in DB.RentalHistory
                               join c in DB.Customers on h.CustomerID equals c.CustomerID
                               join b in DB.Books on h.BookUID equals b.BookUID
                               select new
                               {
                                   RentalId = h.RentalId,
                                   CustomerID = c.CustomerID,
                                   RentalDate = h.RentalDate,
                                   CustomerName = c.CustomerName,
                                   BookName = b.BookName,
                                   BookUID = b.BookUID,
                                   Price = b.Price,
                                   ReturnDate = h.ReturnDate,
                                   Return = h.Return
                               };
            var historyList = await historyQuery.OrderByDescending(x => x.RentalDate).ToListAsync();
            List<RentalHistoryDTO> rentalHistoryResults = new List<RentalHistoryDTO>();
            foreach (var history in historyList)
            {
                RentalHistoryDTO dto = new RentalHistoryDTO();
                dto.RentalId = history.RentalId;
                dto.CustomerID = history.CustomerID;
                dto.RentalDate = history.RentalDate;
                dto.CustomerName = history.CustomerName;
                dto.BookName = history.BookName;
                dto.BookUID = history.BookUID;
                dto.Price = history.Price;
                dto.ReturnDate = history.ReturnDate;
                dto.Return = history.Return;
                rentalHistoryResults.Add(dto);
            }
            return Ok(rentalHistoryResults);
        }

        [HttpGet("Search")]
        public async Task<ActionResult> SearchRentalHistory(bool renting,bool late,string? id=null,string? bookUID=null)
        {
            var searchHistoryQuery =(from h in DB.RentalHistory
                                     join c in DB.Customers on h.CustomerID equals c.CustomerID
                                     join b in DB.Books on h.BookUID equals b.BookUID
                                     select new
                                     {
                                         RentalId = h.RentalId,
                                         CustomerID = c.CustomerID,
                                         RentalDate = h.RentalDate,
                                         CustomerName = c.CustomerName,
                                         BookName = b.BookName,
                                         BookUID = b.BookUID,
                                         Price = b.Price,
                                         ReturnDate = h.ReturnDate,
                                         Return = h.Return
                                     });
            if(renting)
            {
                searchHistoryQuery = searchHistoryQuery.Where(x => x.Return != renting);
            }
            if(late)
            {
                DateTime today = DateTime.UtcNow;
                searchHistoryQuery = searchHistoryQuery.Where(x => DateTime.Compare(today,x.ReturnDate) > 0);
            }
            if(id != null)
            {
                searchHistoryQuery = searchHistoryQuery.Where
                    (x => x.CustomerID == id || x.CustomerName.ToLower().Contains(id.ToLower()));
            }
            if(bookUID != null)
            {
                searchHistoryQuery = searchHistoryQuery.Where
                    (x => x.BookUID == bookUID);
            }

            searchHistoryQuery = searchHistoryQuery.OrderByDescending(x => x.RentalDate);
            var searchHistory = await searchHistoryQuery.ToListAsync();
            List<RentalHistoryDTO> rentalHistoryResults = new List<RentalHistoryDTO>();
            foreach (var history in searchHistory)
            {
                RentalHistoryDTO dto = new RentalHistoryDTO();
                dto.RentalId = history.RentalId;
                dto.CustomerID = history.CustomerID;
                dto.RentalDate = history.RentalDate;
                dto.CustomerName = history.CustomerName;
                dto.BookName = history.BookName;
                dto.BookUID = history.BookUID;
                dto.Price = history.Price;
                dto.ReturnDate = history.ReturnDate;
                dto.Return = history.Return;
                rentalHistoryResults.Add(dto);
            }

            return Ok(rentalHistoryResults);
        }
    }
}
