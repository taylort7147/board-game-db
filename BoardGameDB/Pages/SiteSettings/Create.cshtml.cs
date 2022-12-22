using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_SiteSettings
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class CreateModel : PageModel
    {
        private readonly BoardGameDBContext _context;

        public CreateModel(BoardGameDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SiteSetting SiteSetting { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SiteSetting.Add(SiteSetting);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
