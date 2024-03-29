using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages_PlayStyles
{
    public class IndexModel : PageModelBase
    {
        public IndexModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
        }

        public IList<PlayStyle> PlayStyle { get;set; } = default!;

        public async Task OnGetAsync()
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            if (_context.PlayStyle != null)
            {
                PlayStyle = await _context.PlayStyle
                    .OrderBy(ps => ps.Name)
                    .ToListAsync();
            }
        }
    }
}
