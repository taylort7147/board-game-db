using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_SiteSettings
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class DeleteModel : PageModelBase
    {
        public DeleteModel(BoardGameDBContext context) :
            base(context)
        {
        }

        [BindProperty]
        public SiteSetting SiteSetting { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            if (id == null)
            {
                return NotFound();
            }

            SiteSetting = await _context.SiteSetting.FirstOrDefaultAsync(m => m.Name == id);

            if (SiteSetting == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SiteSetting = await _context.SiteSetting.FindAsync(id);

            if (SiteSetting != null)
            {
                _context.SiteSetting.Remove(SiteSetting);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
