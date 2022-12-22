using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_Games
{
    public class IndexModel : PageModelBase
    {
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

            private bool _IsDirty = true;
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
                get { return _MechanicsList == null ? "" : string.Join("; ", _MechanicsList); }
                set
                {
                    _IsDirty = true;
                    if (value != null)
                    {
                        _MechanicsList = value
                        .Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToList();
                    }
                }
            }

            public string MechanicsListCombinationOperation { get; set; } = "And";

            private List<string>? _PlayStylesList;
            public List<string>? PlayStylesList
            {
                get { return _PlayStylesList; }
                set { _PlayStylesList = value; }
            }

            public string PlayStylesListString
            {
                get { return _PlayStylesList == null ? "" : string.Join("; ", _PlayStylesList); }
                set
                {
                    _IsDirty = true;
                    if (value != null)
                    {
                        _PlayStylesList = value
                        .Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToList();
                    }
                }
            }

            public string PlayStylesCombinationOperation { get; set; } = "And";
        }

        public IndexModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
            Filter = new FilterCriteria();
            ComplexityListItems = ComplexityExtensions.AsEnumerable(includeEmptySelection: true);
        }

        public IEnumerable<Game> Game { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public FilterCriteria Filter { get; set; }

        public IEnumerable<SelectListItem> ComplexityListItems { get; set; }

        [BindProperty]
        public List<string> Mechanics { get; set; } = default!;

        [BindProperty]
        public List<string> PlayStyles { get; set; } = default!;

        private static Expression<Func<Game, string>> RemoveUnimportantWordsInTitle()
        {
            return g => g.Title.Replace("The ", "").Replace("A ", ""); 
        }

        public async Task OnGetAsync()
        {
            LoadSettings();
            LoadTheme();
            ViewData["Theme"] = Theme;

            Mechanics = await _context.Mechanic.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync();
            PlayStyles = await _context.PlayStyle.OrderBy(ps => ps.Name).Select(ps => ps.Name).ToListAsync();
            Game = new List<Game>();

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
                .OrderBy(RemoveUnimportantWordsInTitle());

            if (games != null)
            {
                Game = await games.ToListAsync();

                if (Filter.MechanicsList != null && Filter.MechanicsList.Count > 0)
                {
                    var mechanicsList = Filter.MechanicsList
                        .Select(ms => _context.Mechanic
                            .Where(m => m.Name.ToLower() == ms.ToLower())
                            .FirstOrDefault())
                        .ToList();
                    mechanicsList.RemoveAll(m => m == null);

                    // Client-side evaluation
                    if (Filter.MechanicsListCombinationOperation == "And")
                    {
                        Game = Game
                            .Intersect(games
                                .Include(g => g.Mechanics)
                                .AsEnumerable()
                                .Where(g => mechanicsList.All(m => g.Mechanics.Contains(m!))));
                    }
                    else if (Filter.MechanicsListCombinationOperation == "Or")
                    {
                        Game = Game
                            .Intersect(games
                                .Include(g => g.Mechanics)
                                .AsEnumerable()
                                .Where(g => mechanicsList.Any(m => g.Mechanics.Contains(m!))));
                    }
                    else if (Filter.MechanicsListCombinationOperation == "None")
                    {
                        Game = Game
                            .Intersect(games
                                .Include(g => g.Mechanics)
                                .AsEnumerable()
                                .Where(g => !mechanicsList.Any(m => g.Mechanics.Contains(m!))));
                    }
                }

                if (Filter.PlayStylesList != null && Filter.PlayStylesList.Count > 0)
                {
                    var playStylesList = Filter.PlayStylesList
                        .Select(pss => _context.PlayStyle
                            .Where(ps => ps.Name.ToLower() == pss.ToLower())
                            .FirstOrDefault())
                        .ToList();
                    playStylesList.RemoveAll(ps => ps == null);

                    // Client-side evaluation
                    if (Filter.PlayStylesCombinationOperation == "And")
                    {
                        Game = Game
                            .Intersect(games
                                .Include(g => g.PlayStyles)
                                .AsEnumerable()
                                .Where(g => playStylesList.All(ps => g.PlayStyles.Contains(ps!))));
                    }
                    else if(Filter.PlayStylesCombinationOperation == "Or")
                    {
                        Game = Game
                            .Intersect(games
                                .Include(g => g.PlayStyles)
                                .AsEnumerable()
                                .Where(g => playStylesList.Any(ps => g.PlayStyles.Contains(ps!))));
                    }
                    else if(Filter.PlayStylesCombinationOperation == "None")
                    {
                        Game = Game
                            .Intersect(games
                                .Include(g => g.PlayStyles)
                                .AsEnumerable()
                                .Where(g => !playStylesList.Any(ps => g.PlayStyles.Contains(ps!))));
                    }
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
