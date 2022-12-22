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

namespace BoardGameDB.Pages_SiteSettings
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class IndexModel : PageModel
    {
        private readonly BoardGameDBContext _context;

        public IndexModel(BoardGameDBContext context)
        {
            _context = context;
        }

        public IList<SiteSetting> SiteSetting { get; set; }

        public async Task OnGetAsync()
        {
            SiteSetting = await _context.SiteSetting.ToListAsync();
        }
    }
}
