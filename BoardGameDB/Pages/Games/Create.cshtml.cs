using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_Games
{
    public class CreateModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public CreateModel(BoardGameDB.Data.BoardGameDBContext context)
        {
            _context = context;
            
            var complexityList = new List<SelectListItem>{ new SelectListItem{ Text = null, Value = null}};
            complexityList.AddRange(
                Enum.GetValues(typeof(Complexity))
                    .Cast<Complexity>()
                    .Select(c => new SelectListItem{ Text=c.ToDisplayString(), Value=c.ToDisplayString()})
            );
            ComplexityListItems = complexityList.AsEnumerable();
        }

        public IEnumerable<SelectListItem> ComplexityListItems { get; set;}

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Game == null || Game == null)
            {
                return Page();
            }

            _context.Game.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
