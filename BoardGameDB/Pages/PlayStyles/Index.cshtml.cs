using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages_PlayStyles
{
    public class IndexModel : PageModel
    {
        private readonly BoardGameDB.Data.BoardGameDBContext _context;

        public IndexModel(BoardGameDB.Data.BoardGameDBContext context)
        {
            _context = context;
        }

        public IList<PlayStyle> PlayStyle { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PlayStyle != null)
            {
                PlayStyle = await _context.PlayStyle.ToListAsync();
            }
        }
    }
}
