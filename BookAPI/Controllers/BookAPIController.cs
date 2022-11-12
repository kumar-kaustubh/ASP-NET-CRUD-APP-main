using BookAPI.Data;
using BookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAPIController : ControllerBase
    {
        private readonly DataContext _db;

        public BookAPIController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _db.Books.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            Console.WriteLine(id);
            var book = await _db.Books.FindAsync(id);
            if(book != null)
            {
                return Ok(book);
            }
            return BadRequest("Not Found");
        }

        [HttpPost]
        public async Task<ActionResult<List<BookAPIDb>>> AddBook(BookAPIDb book)
        {
            try
            {
                await _db.Books.AddAsync(book);
                await _db.SaveChangesAsync();
                return Ok("Data is Added.");
            }
            catch
            {
                return BadRequest("Data is not Added.");
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<BookAPIDb>>> UpdateBook(BookAPIDb book)
        {
            try
            {
                _db.Books.Update(book);
                await _db.SaveChangesAsync();
                return Ok("Data is Updated");
            }
            catch
            {
                return BadRequest("Data is not Added.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book != null)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
                return Ok("Data is Deleted.");
            }
            return BadRequest("Data not Found.");
        }
    }
}
