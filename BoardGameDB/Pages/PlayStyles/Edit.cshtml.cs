using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_PlayStyles
{

    [Authorize(Policy = Policy.ReadWrite)]
    public class EditModel : PageModelBase
    {
        public EditModel(BoardGameDB.Data.BoardGameDBContext context) :
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
            PlayStyle = playstyle;
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

            _context.Attach(PlayStyle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayStyleExists(PlayStyle.Id))
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

        private bool PlayStyleExists(int id)
        {
            return (_context.PlayStyle?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
