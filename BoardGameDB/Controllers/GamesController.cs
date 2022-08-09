using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameDB.Data;
using BoardGameDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : Controller
    {
        private readonly BoardGameDBContext _context;

        public GamesController(BoardGameDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Game
            .Include(g => g.PrimaryMechanic)
            .Include(g => g.Mechanics)
            .Include(g => g.PlayStyles)
            .Include(g => g.Categories)
            .OrderBy(g => g.Title)
            .ToListAsync();
        }
    }
}
