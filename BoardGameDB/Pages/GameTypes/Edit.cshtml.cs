using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_GameTypes
{
    public class EditModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public EditModel(BoardGameDB.Data.BoardGameDBContext context)
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

            var gametype =  await _context.GameType.FirstOrDefaultAsync(m => m.Id == id);
            if (gametype == null)
            {
                return NotFound();
            }
            GameType = gametype;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(GameType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameTypeExists(GameType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GameTypeExists(int id)
        {
          return (_context.GameType?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
