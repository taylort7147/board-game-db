using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public class Checkbox
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
            MechanicCheckboxes = new List<Checkbox>();
            GameTypeCheckboxes = new List<Checkbox>();
            PlayStyleCheckboxes = new List<Checkbox>();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public IEnumerable<SelectListItem> ComplexityListItems { get; set; }

        [BindProperty]
        public List<Checkbox> MechanicCheckboxes { get; set; }

        [BindProperty]
        public List<Checkbox> PlayStyleCheckboxes { get; set; }

        [BindProperty]
        public List<Checkbox> GameTypeCheckboxes { get; set; }


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

            MechanicCheckboxes = await _context.Mechanic
                .Select(m => new Checkbox{
                    Id = m.Id,
                    IsChecked = Game.Mechanics.Contains(m),
                    DisplayName = m.Name
                }).ToListAsync();

            GameTypeCheckboxes = await _context.GameType
                .Select(gt => new Checkbox{
                    Id = gt.Id,
                    IsChecked = Game.GameTypes.Contains(gt),
                    DisplayName = gt.Name
                }).ToListAsync();

            PlayStyleCheckboxes = await _context.PlayStyle
                .Select(ps => new Checkbox{
                    Id = ps.Id,
                    IsChecked = Game.PlayStyles.Contains(ps),
                    DisplayName = ps.Name
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
            UpdateGameTypes();
            UpdatePlayStyles();

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

            var existing = game.Mechanics;
            var all = await _context.Mechanic.ToListAsync();
            var toRemove = new List<Mechanic>();

            foreach(var checkbox in MechanicCheckboxes)
            {
                var mechanicId = checkbox.Id;
                var mechanic = all.Find(m => m.Id == mechanicId);

                if(mechanic != null)
                {
                    if(checkbox.IsChecked == false && existing.Contains(mechanic))
                    {
                        // Remove
                        toRemove.Add(mechanic);
                    }
                    else if(checkbox.IsChecked == true && !existing.Contains(mechanic))
                    {
                        // Add
                        Game.Mechanics.Add(mechanic);
                    }
                }
            }  
            foreach(var mechanic in toRemove)
            {
                Game.Mechanics.Remove(mechanic);
            }
        }

        private async void UpdateGameTypes()
        {
            var game = await _context.Game.Where(g => g.Id == Game.Id).Include(g => g.GameTypes).FirstAsync();
            Game.GameTypes = game.GameTypes;

            var existing = game.GameTypes;
            var all = await _context.GameType.ToListAsync();
            var toRemove = new List<GameType>();

            foreach(var checkbox in GameTypeCheckboxes)
            {
                var gameTypeId = checkbox.Id;
                var gameType = all.Find(gt => gt.Id == gameTypeId);

                if(gameType != null)
                {
                    if(checkbox.IsChecked == false && existing.Contains(gameType))
                    {
                        // Remove
                        toRemove.Add(gameType);
                    }
                    else if(checkbox.IsChecked == true && !existing.Contains(gameType))
                    {
                        // Add
                        Game.GameTypes.Add(gameType);
                    }
                }
            }  
            foreach(var gameType in toRemove)
            {
                Game.GameTypes.Remove(gameType);
            }
        }
        private async void UpdatePlayStyles()
        {
            var game = await _context.Game.Where(g => g.Id == Game.Id).Include(g => g.PlayStyles).FirstAsync();
            Game.PlayStyles = game.PlayStyles;

            var existing = game.PlayStyles;
            var all = await _context.PlayStyle.ToListAsync();
            var toRemove = new List<PlayStyle>();

            foreach(var checkbox in PlayStyleCheckboxes)
            {
                var playStyleId = checkbox.Id;
                var playStyle = all.Find(ps => ps.Id == playStyleId);

                if(playStyle != null)
                {
                    if(checkbox.IsChecked == false && existing.Contains(playStyle))
                    {
                        // Remove
                        toRemove.Add(playStyle);
                    }
                    else if(checkbox.IsChecked == true && !existing.Contains(playStyle))
                    {
                        // Add
                        Game.PlayStyles.Add(playStyle);
                    }
                }
            }  
            foreach(var playStyle in toRemove)
            {
                Game.PlayStyles.Remove(playStyle);
            }
        }
    }   
}
