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

namespace BoardGameDB.Pages_Mechanics
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class CreateModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public CreateModel(BoardGameDB.Data.BoardGameDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Mechanic Mechanic { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Mechanic == null || Mechanic == null)
            {
                return Page();
            }

            _context.Mechanic.Add(Mechanic);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
