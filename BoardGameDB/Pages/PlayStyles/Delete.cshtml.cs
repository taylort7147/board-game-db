using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_PlayStyles
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class DeleteModel : PageModelBase
    {
        public DeleteModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
        }

        [BindProperty]
      public PlayStyle PlayStyle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            if (id == null || _context.PlayStyle == null)
            {
                return NotFound();
            }

            var playstyle = await _context.PlayStyle.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.PlayStyle == null)
            {
                return NotFound();
            }
            var playstyle = await _context.PlayStyle.FindAsync(id);

            if (playstyle != null)
            {
                PlayStyle = playstyle;
                _context.PlayStyle.Remove(PlayStyle);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
