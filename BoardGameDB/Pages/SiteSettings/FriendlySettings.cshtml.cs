using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Data;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_SiteSettings
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class FriendlySettingsModel : PageModelBase
    {
        [BindProperty]
        public ThemeSettings Theme { get; set; }

        public FriendlySettingsModel(BoardGameDBContext context) :
            base(context)
        {
        }

        public async Task OnGetAsync()
        {
            Theme = new ThemeSettings();
            await Theme.ReadAsync(_context);
            ViewData["Theme"] = Theme;
        }

        public async Task<IActionResult> OnPostAsync()
        {            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await Theme.WriteAsync(_context);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./FriendlySettings");
        }
    }
}
