using BookStoresWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoresWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly BookStoresDBContext _context;

        public AuthorController(BookStoresDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> Get()
        {
            return await _context.Authors.ToListAsync();
            //using (var context = new BookStoresDBContext())
            //{
            //    return context.Authors.ToList();
            //}
        }

        //[HttpGet("{id}")]
        //public Author Get(int id)
        //{
        //    using (var context = new BookStoresDBContext())
        //    {
        //        return context.Authors.Where(a => a.AuthorId == id).FirstOrDefault();
        //    }
        //}
    }
}