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
    public class CustomersController : ControllerBase
    {

        private DatabaseContext DB;     
        public CustomersController(DatabaseContext db)
        {
            DB = db;
        }


        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            var customers = await DB.Customers.OrderBy(x=>x.CustomerID).ToListAsync();
            List < CustomerDTO > customerResults = new List<CustomerDTO>();
            foreach (var customer in customers)
            {
                CustomerDTO customerDTO = new CustomerDTO();
                customerDTO.CustomerID = customer.CustomerID;
                customerDTO.CustomerName = customer.CustomerName;
                customerDTO.PhoneNumber = customer.PhoneNumber;
                customerDTO.Point = customer.Point;
                customerDTO.AllTimeSpent = customer.AllTimeSpent;
                customerDTO.BookRenting = customer.BookRenting;
                customerResults.Add(customerDTO);
            }
            return Ok(customerResults);
        }


        [HttpPost]
        public async Task<ActionResult> AddCustomers(AddCustomerDTO addCustomerDTO)
        {
            var existed = await DB.Customers.Where(c => c.CustomerID == addCustomerDTO.CustomerID).AnyAsync();
            if (existed)
            {
                return BadRequest("This ID already register");
            }
            var customer = new Customer
            {
                CustomerID = addCustomerDTO.CustomerID,
                CustomerName = addCustomerDTO.CustomerName,
                PhoneNumber = addCustomerDTO.PhoneNumber
            };
            DB.Customers.Add(customer);
            await DB.SaveChangesAsync();
            return Ok("Customer Added");
        }


        [HttpPut]
        public async Task<ActionResult> UpdateCustomer(string customerId,UpdateCustomerDTO updateCustomerDTO)
        {
            var update = await DB.Customers.Where(s => s.CustomerID == customerId).FirstOrDefaultAsync();
            if (update == null)
            {
                return NotFound("No Customer Found");
            }
            update.CustomerName = updateCustomerDTO.CustomerName;
            update.PhoneNumber = updateCustomerDTO.PhoneNumber;
            await DB.SaveChangesAsync();
            return Ok("Customer Updated");
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            var delete = await DB.Customers.Where(c => c.CustomerID == id).FirstOrDefaultAsync();
            if(delete == null)
            {
                return NotFound("Customer Not Found");
            }    

            var existed = await DB.RentalHistory.Where(c => c.CustomerID == id && c.Return == false).AnyAsync();
            if(existed)
            {
                return BadRequest("This Customer is renting");
            }

            DB.Customers.Remove(delete);
            await DB.SaveChangesAsync();
            return Ok("Customer Deleted");
        }

        [HttpGet("Search")]
        public async Task<ActionResult> SearchCustomers(string sortBy,bool descending, string? name = null)
        {
            var query = DB.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.CustomerName.ToLower().Contains(name.ToLower()) || c.CustomerID.Contains(name));
            if (sortBy == "CustomerID")
            {
                if (descending == false)
                {
                    query = query.OrderBy(s => s.CustomerID);
                }
                else
                {
                    query = query.OrderByDescending(s => s.CustomerID);
                }
            }
            else if (sortBy == "CustomerName")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.CustomerName);
                }
                else
                {
                    query = query.OrderByDescending(s => s.CustomerName);
                }
            }
            else if (sortBy == "Point")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.Point);
                }
                else
                {
                    query = query.OrderByDescending(s => s.Point);
                }
            }
            else if (sortBy == "AllTimeSpent")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.AllTimeSpent);
                }
                else
                {
                    query = query.OrderByDescending(s => s.AllTimeSpent);
                }
            }
            else if (sortBy == "BookRenting")
            {
                if (descending==false)
                {
                    query = query.OrderBy(s => s.BookRenting);
                }
                else
                {
                    query = query.OrderByDescending(s => s.BookRenting);
                }
            }
            var searchCustomer = await query.ToListAsync();
            List<CustomerDTO> customerResults = new List<CustomerDTO>();
            foreach (var customer in searchCustomer)
            {
                CustomerDTO customerDTO = new CustomerDTO();
                customerDTO.CustomerID = customer.CustomerID;
                customerDTO.CustomerName = customer.CustomerName;
                customerDTO.PhoneNumber = customer.PhoneNumber;
                customerDTO.Point = customer.Point;
                customerDTO.AllTimeSpent = customer.AllTimeSpent;
                customerDTO.BookRenting = customer.BookRenting;
                customerResults.Add(customerDTO);
            }
            return Ok(customerResults);
        }

        //---------------------------------------------------------------------------//


        [HttpPost("Rent")]
        public async Task<ActionResult> Renting(BookRentingDTO bookRentingDTO)
        {
            var customerId = await DB.Customers.Where(s => s.CustomerID == bookRentingDTO.Customer.CustomerID).FirstOrDefaultAsync();
            var updateCustomer = await DB.Customers.Where(c => c.CustomerID == bookRentingDTO.Customer.CustomerID).FirstOrDefaultAsync();
            var rentalId = await DB.RentalHistory.CountAsync();
            if(rentalId != 0)
                rentalId = await DB.RentalHistory.MaxAsync(r=>r.RentalId);
            int total = 0;
            if (customerId == null)
                return NotFound("No Customer Found");

            using (var tran= await DB.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var book in bookRentingDTO.Books)
                    {

                        //////////////////////////////////////////// Add Rental History
                        rentalId = rentalId + 1;
                        var rent = new RentalHistory
                        {
                            RentalId = rentalId,
                            RentalDate = DateTime.UtcNow,
                            ReturnDate = DateTime.UtcNow.AddDays(7),
                            CustomerID = bookRentingDTO.Customer.CustomerID,
                            BookUID = book.BookUID
                        };
                        DB.RentalHistory.Add(rent);
                        await DB.SaveChangesAsync();
                        ///////////////////////////////////////////// Update Book Data
                        var updateBook = await DB.Books.Where(b => b.BookUID == book.BookUID).FirstOrDefaultAsync();
                        if (updateBook == null)
                        {
                            return BadRequest("No book Found");
                        }
                        updateBook.InRental++;
                        updateBook.InStock--;
                        updateBook.AllTimeRent++;
                        if (updateBook.InStock < 0)
                        {
                            return BadRequest("No book left");
                        }
                        await DB.SaveChangesAsync();
                        ////////////////////////////////////////////// Calculate Total Price
                        var price = updateBook.Price;
                        total += price;
                        await DB.SaveChangesAsync();
                        ///////////////////////////////////////////// Update Customer Data
                        updateCustomer.AllTimeSpent += updateBook.Price;
                        updateCustomer.BookRenting++;
                        await DB.SaveChangesAsync();
                        ///////////////////////////////////////////// Save Change
                        //await DB.SaveChangesAsync();

                    }
                    var points = Math.Floor((float)total / 20);
                    updateCustomer.Point += (int)points;
                    await DB.SaveChangesAsync();

                    await tran.CommitAsync();
                    return Ok(total);

                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                    throw;
                }
            }

            
        }
        

        [HttpPost("Return")]
        public async Task<ActionResult> ReturnBook(ReturnBookDTO returnBookDTO)
        {
            var bookReturn = await DB.RentalHistory.Where
                (c => (c.BookUID == returnBookDTO.BookUID) && ( c.CustomerID == returnBookDTO.CustomerID) && (c.Return == false) )
                .FirstOrDefaultAsync();
            if(bookReturn == null)
            {
                return NotFound("Already return");
            }
            bookReturn.Return = true;

            var bookUpdate = await DB.Books.Where(b => b.BookUID == returnBookDTO.BookUID).FirstOrDefaultAsync();
            bookUpdate.InRental--;
            bookUpdate.InStock++;

            var customerUpdate = await DB.Customers.Where(c=>c.CustomerID == returnBookDTO.CustomerID).FirstOrDefaultAsync();
            customerUpdate.BookRenting--;

            await DB.SaveChangesAsync();

            return Ok(customerUpdate.BookRenting);
        }
    }
}
