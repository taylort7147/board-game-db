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

namespace BoardGameDB.Pages.Shared
{
    public class PageModelBase : PageModel
    {
        protected readonly BoardGameDBContext _context;
        public IQueryable<SiteSetting> Settings { get; set; } = default!;

        public ThemeSettings Theme { get; set; }

        public PageModelBase(BoardGameDBContext context)
        {
            Theme = new ThemeSettings();
            _context = context;
        }


        public void LoadSettings()
        {
            Settings = from s in _context.SiteSetting select s;
        }

        public async Task LoadThemeAsync()
        {
            LoadSettings();
            Theme = new ThemeSettings();
            await Theme.ReadAsync(_context);
        }

        public void LoadTheme()
        {
            var task = LoadThemeAsync();
            task.Wait();
        }
    }
}
