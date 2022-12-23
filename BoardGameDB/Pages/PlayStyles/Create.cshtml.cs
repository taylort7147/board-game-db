using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_PlayStyles
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class CreateModel : PageModelBase
    {
        public CreateModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
        }

        public IActionResult OnGet()
        {
            LoadTheme();
            ViewData["Theme"] = Theme;
            
            return Page();
        }

        [BindProperty]
        public PlayStyle PlayStyle { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.PlayStyle == null || PlayStyle == null)
            {
                return Page();
            }

            _context.PlayStyle.Add(PlayStyle);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
