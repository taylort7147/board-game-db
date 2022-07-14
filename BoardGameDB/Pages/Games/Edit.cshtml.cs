using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_Games
{
    public class EditModel : PageModel
    {
        public class IdCheckbox
        {
            public int Id { get; set; }
            public string DisplayName { get; set; } = default!;
            public bool IsChecked { get; set; }
        }

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

        [BindProperty]
        public List<IdCheckbox> MechanicsCheckboxes { get; set; }


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

            var gameMechanics = Game.Mechanics.ToList();
            MechanicsCheckboxes = await _context.Mechanic
                .Select(m => new IdCheckbox{
                    Id = m.Id,
                    IsChecked = gameMechanics.Contains(m),
                    DisplayName = m.Name
                }).ToListAsync();


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

            UpdateMechanics();

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

        private async void UpdateMechanics()
        {
            var game = await _context.Game.Where(g => g.Id == Game.Id).Include(g => g.Mechanics).FirstAsync();
            Game.Mechanics = game.Mechanics;

            var existingMechanics = game.Mechanics;
            var allMechanics = await _context.Mechanic.ToListAsync();
            var mechanicsToRemove = new List<Mechanic>();

            foreach(var checkbox in MechanicsCheckboxes)
            {
                var mechanicId = checkbox.Id;
                var mechanic = allMechanics.Find(m => m.Id == mechanicId);

                if(mechanic != null)
                {
                    if(checkbox.IsChecked == false && existingMechanics.Contains(mechanic))
                    {
                        // Remove
                        mechanicsToRemove.Add(mechanic);
                    }
                    else if(checkbox.IsChecked == true && !existingMechanics.Contains(mechanic))
                    {
                        // Add
                        Game.Mechanics.Add(mechanic);
                    }
                }
            }  
            foreach(var mechanic in mechanicsToRemove)
            {
                Game.Mechanics.Remove(mechanic);
            }
            _context.Update(Game);
        }
    }   
}
