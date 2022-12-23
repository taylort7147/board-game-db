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
using BoardGameDB.Models;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_SiteSettings
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class IndexModel : PageModelBase
    {
        public IndexModel(BoardGameDBContext context) :
            base(context)
        {
        }

        public IList<SiteSetting> SiteSetting { get; set; }

        public async Task OnGetAsync()
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            SiteSetting = await _context.SiteSetting.ToListAsync();
        }
    }
}
