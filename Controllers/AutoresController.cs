using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context) 
           {
            this.context = context;
        }

        [HttpGet] 
        public async Task<ActionResult<List<Author>>> Get()
        {
            return await context.Authors.Include(x => x.Books).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author author)
        {
            context.Add(author);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Author author, int id)
        {
            if (author.id != id)
            {
                return BadRequest("not found");
            }

            context.Update(author);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Authors.AnyAsync(x=> x.id== id);

            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Author() { id = id});
            await   context.SaveChangesAsync();
            return Ok();

        }
    }
}
