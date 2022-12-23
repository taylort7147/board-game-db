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

namespace BoardGameDB.Pages_Categories
{
    public class IndexModel : PageModelBase
    {
        public IndexModel(BoardGameDB.Data.BoardGameDBContext context) :
            base(context)
        {
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            
            if (_context.Category != null)
            {
                Category = await _context.Category
                    .OrderBy(gt => gt.Name)
                    .ToListAsync();
            }
        }
    }
}
