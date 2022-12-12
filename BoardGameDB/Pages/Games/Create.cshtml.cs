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

namespace BoardGameDB.Pages_Games
{
    [Authorize(Policy = Policy.ReadWrite)]
    public class CreateModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public CreateModel(BoardGameDB.Data.BoardGameDBContext context)
        {
            _context = context;

            ComplexityListItems = ComplexityExtensions.AsEnumerable(includeEmptySelection: true);
            MechanicListItems = new List<SelectListItem>();
        }

        [BindProperty]
        public IEnumerable<SelectListItem> ComplexityListItems { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> MechanicListItems { get; set; }

        public IActionResult OnGet()
        {
            var mechanicList = new List<SelectListItem> { new SelectListItem { Text = null, Value = null } };
            mechanicList.AddRange(_context.Mechanic
                .OrderBy(m => m.Name)
                .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() })
                .ToList()
            );
            MechanicListItems = mechanicList;

            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        private int _PrimaryMechanicId;

        [BindProperty]
        public string PrimaryMechanicIdString
        {
            get { return _PrimaryMechanicId.ToString(); }
            set { _PrimaryMechanicId = int.Parse(value); }
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Game == null || Game == null)
            {
                return Page();
            }

            Game.PrimaryMechanic = _context.Mechanic.First(m => m.Id == _PrimaryMechanicId);
            Game.Mechanics = new List<Mechanic> { Game.PrimaryMechanic };
            _context.Game.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = Game.Id });
        }
    }
}
