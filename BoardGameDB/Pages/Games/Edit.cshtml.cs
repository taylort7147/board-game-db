using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_Games
{
    public class EditModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public EditModel(BoardGameDB.Data.BoardGameDBContext context)
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

        [BindProperty]
        public Game Game { get; set; } = default!;

        public IEnumerable<SelectListItem> ComplexityListItems { get; set; }
        public IEnumerable<SelectListItem> MechanicsListItems { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game =  await _context.Game
                .Include(g => g.Mechanics)
                .Include(g => g.GameTypes)
                .Include(g => g.PlayStyles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            Game = game;

            var allMechanicsList = new List<SelectListItem>{ new SelectListItem { Text = "", Value = "" } };
            allMechanicsList.AddRange(
                await _context.Mechanic
                    .Select(m => 
                        new SelectListItem{ Text = m.Name, Value = m.Id.ToString() })
                    .ToListAsync()
            );
            MechanicsListItems = allMechanicsList;
            
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

            _context.Attach(Game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(Game.Id))
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

        private bool GameExists(int id)
        {
          return (_context.Game?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }   
}
