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

namespace BoardGameDB.Pages_PlayStyles
{
    public class DetailsModel : PageModelBase
    {
        public DetailsModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
        }

      public PlayStyle PlayStyle { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            if (id == null || _context.PlayStyle == null)
            {
                return NotFound();
            }

            var playstyle = await _context.PlayStyle
                .Include(ps => ps.Games.OrderBy(g => g.Title))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playstyle == null)
            {
                return NotFound();
            }
            else 
            {
                PlayStyle = playstyle;
            }
            return Page();
        }
    }
}
