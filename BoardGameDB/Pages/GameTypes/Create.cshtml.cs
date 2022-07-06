using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_GameTypes
{
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
        public GameType GameType { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.GameType == null || GameType == null)
            {
                return Page();
            }

            _context.GameType.Add(GameType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
