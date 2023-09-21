using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;
using WebApiAutores.Services;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;

        public AutoresController(ApplicationDbContext context,
                                IService service, 
                                ServiceTransient serviceTransient,
                                ServiceScoped serviceScoped,
                                ServiceSingleton serviceSingleton) 
           {
            this.context = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
        }

        [HttpGet("GUID")]
        public ActionResult GetGuids() {

            return Ok(new {
                  AutoresController_Transient = serviceTransient.Guid,
                  ServiceA_Transient = service.GetTransient(),
                  AutoresControllerScoped= serviceScoped.Guid,
                  ServiceA_Scoped = service.GetScoped(),
                  AutoresControllerSingleton = serviceSingleton.Guid,
                  ServiceA_Singleton = service.GetSingleton(), 
            });
        }


        [HttpGet("/list")]
        public async Task<List<Author>> GetList()
        {
            service.DoTask();
            return await context.Authors.Include(x=> x.Books).ToListAsync();
        }


        [HttpGet] 
        public async Task<ActionResult<List<Author>>> Get()
        {
            return await context.Authors.Include(x => x.Books).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author author)
        {
                
            var existAuthor = await context.Authors.AnyAsync(x=> x.name == author.name);

            if (existAuthor)
            {
                return BadRequest($"Author name alredy exist {author.name}");
            }

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
