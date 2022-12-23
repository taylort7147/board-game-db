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

namespace BoardGameDB.Pages_Mechanics
{
    public class DetailsModel : PageModelBase
    {
        public DetailsModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
        }

        public Mechanic Mechanic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            if (id == null || _context.Mechanic == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanic
                .Include(m => m.Games.OrderBy(g => g.Title))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mechanic == null)
            {
                return NotFound();
            }
            else
            {
                Mechanic = mechanic;
            }
            return Page();
        }
    }
}
