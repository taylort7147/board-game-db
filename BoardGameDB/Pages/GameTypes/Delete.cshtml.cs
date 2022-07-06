using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_GameTypes
{
    public class DeleteModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public DeleteModel(BoardGameDB.Data.BoardGameDBContext context)
        {
            _context = context;
        }

        [BindProperty]
      public GameType GameType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GameType == null)
            {
                return NotFound();
            }

            var gametype = await _context.GameType.FirstOrDefaultAsync(m => m.Id == id);

            if (gametype == null)
            {
                return NotFound();
            }
            else 
            {
                GameType = gametype;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.GameType == null)
            {
                return NotFound();
            }
            var gametype = await _context.GameType.FindAsync(id);

            if (gametype != null)
            {
                GameType = gametype;
                _context.GameType.Remove(GameType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
