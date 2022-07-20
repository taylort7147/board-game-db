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
    public class IndexModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public class FilterCriteria
        {
            private bool _IsDirty = false;
            public bool IsDirty
            {
                get { return _IsDirty; }
            }
            

            private string? _Title { get; set; }
            public string? Title {
                get { return _Title; }
                set { 
                    _Title = value; 
                    _IsDirty = true;
                }
            }

            private Complexity? _Complexity;
            public Complexity? Complexity
            {
                get { return _Complexity; }
                set 
                { 
                    _Complexity = value;
                    _IsDirty = true;
                }
            }
            

            public string ComplexityString { 
                get
                {
                    return _Complexity.HasValue ? ComplexityExtensions.ToDisplayString(_Complexity.Value) : "";
                }
                set
                {
                    _Complexity = string.IsNullOrEmpty(value) ? null : ComplexityExtensions.From(value);                    
                    _IsDirty = true;
                }
            }

            private int? _MinimumPlayerCount;
            public int? MinimumPlayerCount
            {
                get { return _MinimumPlayerCount; }
                set 
                { 
                    _MinimumPlayerCount = value;
                    _IsDirty = true; 
                }
            }
            
            private int? _MaximumPlayerCount;
            public int? MaximumPlayerCount
            {
                get { return _MaximumPlayerCount; }
                set 
                { 
                    _MaximumPlayerCount = value;
                    _IsDirty = true;
                }
            }
            
            private int? _MinimumPlayTimeMinutes;
            public int? MinimumPlayTimeMinutes
            {
                get { return _MinimumPlayTimeMinutes; }
                set 
                { 
                    _MinimumPlayTimeMinutes = value; 
                    _IsDirty = true;
                }
            }
            
            private int? _MaximumPlayTimeMinutes;
            public int? MaximumPlayTimeMinutes
            {
                get { return _MaximumPlayTimeMinutes; }
                set 
                { 
                    _MaximumPlayTimeMinutes = value; 
                    _IsDirty = true;
                }
            }            
        }
        
        public IndexModel(BoardGameDB.Data.BoardGameDBContext context)
        {
            _context = context;
            Filter = new FilterCriteria();
            ComplexityListItems = ComplexityExtensions.AsEnumerable(includeEmptySelection: false);
        }

        public IList<Game> Game { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public FilterCriteria Filter { get; set; }

        public IEnumerable<SelectListItem> ComplexityListItems { get; set; }


        public async Task OnGetAsync()
        {
            var games = from g in _context.Game
                select g;

            if (!string.IsNullOrEmpty(Filter.Title))
            {
                games = games.Where(g => g.Title.ToLower().Contains(Filter.Title.ToLower()));
            }

            if(Filter.Complexity != null)
            {
                games = games.Where(g => g.Complexity == Filter.Complexity);
            }

            if(Filter.MinimumPlayerCount.HasValue || Filter.MaximumPlayerCount.HasValue)
            {
                games = games
                    .Where(g => g.MinimumPlayerCount >= Filter.MinimumPlayerCount.GetValueOrDefault(0)
                    && g.MaximumPlayerCount <= Filter.MaximumPlayerCount.GetValueOrDefault(int.MaxValue));
            }

            if(Filter.MinimumPlayTimeMinutes.HasValue || Filter.MaximumPlayTimeMinutes.HasValue)
            {
                games = games
                    .Where(g => g.MinimumPlayTimeMinutes >= Filter.MinimumPlayTimeMinutes.GetValueOrDefault(0)
                    && g.MaximumPlayTimeMinutes <= Filter.MaximumPlayTimeMinutes.GetValueOrDefault(int.MaxValue));
            }

            games = games.OrderBy(g => g.Title);

            if (games != null)
            {
                Game = await games.ToListAsync();
            }
        }

        public async Task OnGetClearFilterAsync()
        {
            Filter = new FilterCriteria();
            await OnGetAsync();
        }
    }
}
