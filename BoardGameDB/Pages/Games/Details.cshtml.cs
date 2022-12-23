using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_Games
{
    public class DetailsModel : PageModelBase
    {
        public DetailsModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
        }

      public Game Game { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Mechanics.OrderBy(m => m.Name))
                .Include(g => g.Categories.OrderBy(c => c.Name))
                .Include(g => g.PlayStyles.OrderBy(ps => ps.Name))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            else 
            {
                Game = game;
            }
            return Page();
        }
    }
}
