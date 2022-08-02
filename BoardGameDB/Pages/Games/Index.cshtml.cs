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
            public FilterCriteria()
            {
                PlayTimeRangeListItems = new List<SelectListItem>{
                    new SelectListItem{ Text = "", Value = ""},
                    new SelectListItem{ Text = Pages_Games.PlayTimeRange.LessThan30Minutes.ToDisplayString(), Value = Pages_Games.PlayTimeRange.LessThan30Minutes.ToDisplayString()},
                    new SelectListItem{ Text = Pages_Games.PlayTimeRange.Between30And60Minutes.ToDisplayString(), Value = Pages_Games.PlayTimeRange.Between30And60Minutes.ToDisplayString()},
                    new SelectListItem{ Text = Pages_Games.PlayTimeRange.Between1And2Hours.ToDisplayString(), Value = Pages_Games.PlayTimeRange.Between1And2Hours.ToDisplayString()},
                    new SelectListItem{ Text = Pages_Games.PlayTimeRange.MoreThan2Hours.ToDisplayString(), Value = Pages_Games.PlayTimeRange.MoreThan2Hours.ToDisplayString()},
                };
                _MechanicsList = new List<string>();
            }

            private bool _IsDirty = false;
            public bool IsDirty
            {
                get { return _IsDirty; }
            }


            private string? _Title { get; set; }
            public string? Title
            {
                get { return _Title; }
                set
                {
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


            public string ComplexityString
            {
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

            private int? _PlayerCount;
            public int? PlayerCount
            {
                get { return _PlayerCount; }
                set
                {
                    _PlayerCount = value;
                    _IsDirty = true;
                }
            }

            public List<SelectListItem> PlayTimeRangeListItems { get; set; }

            private PlayTimeRange? _PlayTimeRange;
            public PlayTimeRange? PlayTimeRange { get { return _PlayTimeRange; } set { _PlayTimeRange = value; } }
            public string PlayTimeRangeString
            {
                get { if (_PlayTimeRange != null) { return _PlayTimeRange.Value.ToDisplayString(); } return ""; }
                set
                {
                    _IsDirty = true;
                    _PlayTimeRange = PlayTimeRangeExtensions.From(value);
                }
            }

            private List<string>? _MechanicsList;
            public List<string>? MechanicsList
            {
                get { return _MechanicsList; }
                set { _MechanicsList = value; }
            }

            public string MechanicsListString
            {
                get { return _MechanicsList == null ? "" : string.Join(", ", _MechanicsList); }
                set
                {
                    _IsDirty = true;
                    if (value != null)
                    {
                        _MechanicsList = value
                        .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToList();
                    }
                }
            }
        }

        public IndexModel(BoardGameDB.Data.BoardGameDBContext context)
        {
            _context = context;
            Filter = new FilterCriteria();
            ComplexityListItems = ComplexityExtensions.AsEnumerable(includeEmptySelection: true);
        }

        public IList<Game> Game { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public FilterCriteria Filter { get; set; }

        public IEnumerable<SelectListItem> ComplexityListItems { get; set; }

        [BindProperty]
        public List<string> Mechanics { get; set; } = default!;


        public async Task OnGetAsync()
        {
            Mechanics = await _context.Mechanic.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync();

            var games = from g in _context.Game
                        select g;

            if (!string.IsNullOrEmpty(Filter.Title))
            {
                games = games.Where(g => g.Title.ToLower().Contains(Filter.Title.ToLower()));
            }

            if (Filter.Complexity != null)
            {
                var complexityRange = ComplexityExtensions.MapComplexityToFloatRange(Filter.Complexity);
                if (complexityRange != null)
                {
                    games = games.Where(g => g.Complexity >= complexityRange.Item1 && g.Complexity < complexityRange.Item2);
                }
            }

            if (Filter.PlayerCount.HasValue)
            {
                games = games
                    .Where(g => g.MinimumPlayerCount <= Filter.PlayerCount.Value
                    && g.MaximumPlayerCount >= Filter.PlayerCount.Value);
            }

            if (Filter.PlayTimeRange != null)
            {
                switch (Filter.PlayTimeRange)
                {
                    case PlayTimeRange.LessThan30Minutes:
                        {
                            games = games
                                .Where(g => g.MaximumPlayTimeMinutes <= 30);
                        }
                        break;
                    case PlayTimeRange.Between30And60Minutes:
                        {
                            games = games
                                .Where(g => g.MaximumPlayTimeMinutes > 30
                                && g.MaximumPlayTimeMinutes < 60);
                        }
                        break;
                    case PlayTimeRange.Between1And2Hours:
                        {
                            games = games
                                .Where(g => g.MaximumPlayTimeMinutes >= 60
                                && g.MaximumPlayTimeMinutes <= 120);
                        }
                        break;
                    case PlayTimeRange.MoreThan2Hours:
                    default:
                        {
                            games = games
                                .Where(g => g.MaximumPlayTimeMinutes > 120);
                        }
                        break;
                }
            }
            games = games
                .Include(g => g.PrimaryMechanic)
                .OrderBy(g => g.Title);

            if (games != null)
            {
                if (Filter.MechanicsList != null && Filter.MechanicsList.Count > 0)
                {
                    var mechanicsList = Filter.MechanicsList
                        .Select(ms => _context.Mechanic
                            .Where(m => m.Name.ToLower() == ms.ToLower())
                            .First())
                        .ToList();

                    // Client-side evaluation
                    Game = games
                        .Include(g => g.Mechanics)
                        .AsEnumerable()
                        .Where(g => mechanicsList.All(m => g.Mechanics.Contains(m)))
                        .ToList();
                }
                else
                {
                    Game = await games.ToListAsync();
                }
            }



        }

        public async Task OnGetClearFilterAsync()
        {
            Filter = new FilterCriteria();
            await OnGetAsync();
        }
    }
}
