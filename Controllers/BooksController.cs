using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            return await context.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.id == id);
        }

        [HttpPost]
        public async Task<ActionResult>Post(Book book)
        {
            var existAuthor = await context.Authors.AnyAsync(x => x.id == book.authorId);

            if (!existAuthor)
            {
                return BadRequest($"not found Author with id: {book.authorId}");
            }

            context.Add(book);
            await context.SaveChangesAsync();
            return Ok(book);
        }
    }
}
