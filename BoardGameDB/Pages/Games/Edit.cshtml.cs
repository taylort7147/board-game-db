using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Areas.Identity.Authorization;

namespace BoardGameDB.Pages_Games
{
    [Authorize(Policy=Policy.ReadWrite)]
    // TODO: Fix data transfer size
    // Right now each checkbox item has at least one
    // piece of data associated with it. This solution does
    // not scale well with large numbers of data.
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]

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

            // ComplexityListItems = ComplexityExtensions.AsEnumerable(includeEmptySelection: true);
            MechanicCheckboxes = new List<Checkbox>();
            MechanicPrimaryCheckboxes = new List<Checkbox>();
            CategoryCheckboxes = new List<Checkbox>();
            PlayStyleCheckboxes = new List<Checkbox>();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        [BindProperty]
        public int? PrimaryMechanicId { get; set; }

        // [BindProperty]
        // public IEnumerable<SelectListItem> ComplexityListItems { get; set; }

        [BindProperty]
        public List<Checkbox> MechanicCheckboxes { get; set; }

        [BindProperty]
        public List<Checkbox> MechanicPrimaryCheckboxes { get; set; }

        [BindProperty]
        public List<Checkbox> PlayStyleCheckboxes { get; set; }

        [BindProperty]
        public List<Checkbox> CategoryCheckboxes { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.PrimaryMechanic)
                .Include(g => g.Mechanics)
                .Include(g => g.Categories)
                .Include(g => g.PlayStyles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            Game = game;
            PrimaryMechanicId = Game.PrimaryMechanic == null ? null : Game.PrimaryMechanic.Id;

            MechanicCheckboxes = await _context.Mechanic
                .OrderBy(m => m.Name)
                .Select(m => new Checkbox
                {
                    Id = m.Id,
                    IsChecked = Game.Mechanics.Contains(m),
                    DisplayName = m.Name
                }).ToListAsync();

            MechanicPrimaryCheckboxes = await _context.Mechanic
                .OrderBy(m => m.Name)
                .Select(m => new Checkbox
                {
                    Id = m.Id,
                    IsChecked = m.Id == PrimaryMechanicId,
                    DisplayName = m.Name
                }).ToListAsync();

            CategoryCheckboxes = await _context.Category
                .OrderBy(c => c.Name)
                .Select(gt => new Checkbox
                {
                    Id = gt.Id,
                    IsChecked = Game.Categories.Contains(gt),
                    DisplayName = gt.Name
                }).ToListAsync();

            PlayStyleCheckboxes = await _context.PlayStyle
                .OrderBy(ps => ps.Name)
                .Select(ps => new Checkbox
                {
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

            try
            {
                UpdateMechanics();
                UpdateCategories();
                UpdatePlayStyles();
            }
            catch (InvalidDataException)
            {
                return Page();
            }

            // Updates might have added validation errors
            if (!ModelState.IsValid)
            {
                return Page();
            }

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
            var game = await _context.Game
                .Where(g => g.Id == Game.Id)
                .Include(g => g.Mechanics)
                .Include(g => g.PrimaryMechanic)
                .FirstAsync();
            Game.Mechanics = game.Mechanics;
            Game.PrimaryMechanic = game.PrimaryMechanic;

            var existing = game.Mechanics;
            var all = await _context.Mechanic.ToListAsync();
            var toRemove = new List<Mechanic>();

            foreach (var checkbox in MechanicCheckboxes)
            {
                var mechanicId = checkbox.Id;
                var mechanic = all.Find(m => m.Id == mechanicId);

                if (mechanic != null)
                {
                    if (checkbox.IsChecked == false && existing.Contains(mechanic))
                    {
                        // Remove
                        toRemove.Add(mechanic);
                    }
                    else if (checkbox.IsChecked == true && !existing.Contains(mechanic))
                    {
                        // Add
                        Game.Mechanics.Add(mechanic);
                    }
                }
            }
            foreach (var mechanic in toRemove)
            {
                Game.Mechanics.Remove(mechanic);
            }

            if (PrimaryMechanicId == null)
            {
                ModelState.AddModelError(nameof(PrimaryMechanicId), "You must choose a primary mechanic");
                return;
            }
            else
            {
                var mechanicForPrimaryMechanic = MechanicCheckboxes.First(c => c.Id == PrimaryMechanicId);
                if (!mechanicForPrimaryMechanic.IsChecked)
                {
                    ModelState.AddModelError(nameof(PrimaryMechanicId), "Primary mechanic must be one of the selected mechanics");
                    return;
                }
                var primaryMechanic = Game.Mechanics.First(m => m.Id == PrimaryMechanicId);
                Game.PrimaryMechanic = primaryMechanic;
            }
        }

        private async void UpdateCategories()
        {
            var game = await _context.Game.Where(g => g.Id == Game.Id).Include(g => g.Categories).FirstAsync();
            Game.Categories = game.Categories;

            var existing = game.Categories;
            var all = await _context.Category.ToListAsync();
            var toRemove = new List<Category>();

            foreach (var checkbox in CategoryCheckboxes)
            {
                var CategoryId = checkbox.Id;
                var Category = all.Find(gt => gt.Id == CategoryId);

                if (Category != null)
                {
                    if (checkbox.IsChecked == false && existing.Contains(Category))
                    {
                        // Remove
                        toRemove.Add(Category);
                    }
                    else if (checkbox.IsChecked == true && !existing.Contains(Category))
                    {
                        // Add
                        Game.Categories.Add(Category);
                    }
                }
            }
            foreach (var Category in toRemove)
            {
                Game.Categories.Remove(Category);
            }
        }
        private async void UpdatePlayStyles()
        {
            var game = await _context.Game.Where(g => g.Id == Game.Id).Include(g => g.PlayStyles).FirstAsync();
            Game.PlayStyles = game.PlayStyles;

            var existing = game.PlayStyles;
            var all = await _context.PlayStyle.ToListAsync();
            var toRemove = new List<PlayStyle>();

            foreach (var checkbox in PlayStyleCheckboxes)
            {
                var playStyleId = checkbox.Id;
                var playStyle = all.Find(ps => ps.Id == playStyleId);

                if (playStyle != null)
                {
                    if (checkbox.IsChecked == false && existing.Contains(playStyle))
                    {
                        // Remove
                        toRemove.Add(playStyle);
                    }
                    else if (checkbox.IsChecked == true && !existing.Contains(playStyle))
                    {
                        // Add
                        Game.PlayStyles.Add(playStyle);
                    }
                }
            }
            foreach (var playStyle in toRemove)
            {
                Game.PlayStyles.Remove(playStyle);
            }
        }
    }
}
