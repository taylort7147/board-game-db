using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_SiteSettings
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class EditModel : PageModelBase
    {
        public EditModel(BoardGameDBContext context) :
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SiteSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteSettingExists(SiteSetting.Name))
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

        private bool SiteSettingExists(string id)
        {
            return _context.SiteSetting.Any(e => e.Name == id);
        }
    }
}
