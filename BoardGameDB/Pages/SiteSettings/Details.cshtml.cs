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

namespace BoardGameDB.Pages_SiteSettings
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class DetailsModel : PageModel
    {
        private readonly BoardGameDBContext _context;

        public DetailsModel(BoardGameDBContext context)
        {
            _context = context;
        }

        public SiteSetting SiteSetting { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
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
    }
}
