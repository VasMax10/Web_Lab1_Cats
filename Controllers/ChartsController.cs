using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web_Lab1_Cats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly CatsContext _context;

        public ChartsController(CatsContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var genres = _context.Species.Include(l => l.Cats).ToList();
            List<object> genBook = new List<object>();
            genBook.Add(new[] { "Жанр", "Кількість книжок" });
            foreach (var g in genres)
            {
                genBook.Add(new object[] { g.Name, g.Cats.Count() });
            }
            return new JsonResult(genBook);
        }
    }
}