using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BoardGameDB.Data;
using BoardGameDB.Models;

namespace BoardGameDB.Pages.Shared
{
    public class ThemeSettings
    {
        [BindProperty]
        public SiteSetting MainBackgroundColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-main-background-color", Value = "#fff", DisplayName = "Main Background Color" };

        [BindProperty]
        public SiteSetting MainTextColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-main-text-color", Value = "#212529", DisplayName = "Main Text Color" };

        [BindProperty]
        public SiteSetting NavbarBackgroundColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-navbar-background-color", Value = "#fff", DisplayName = "Navbar Background Color" };

        [BindProperty]
        public SiteSetting NavbarTextColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-navbar-text-color", Value = "#212529", DisplayName = "Navbar Text Color" };

        [BindProperty]
        public SiteSetting CardBackgroundColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-card-background-color", Value = "#f8f9fa", DisplayName = "Card Background Color" };

        [BindProperty]
        public SiteSetting CardTextColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-card-text-color", Value = "#212529", DisplayName = "Card Text Color" };

        [BindProperty]
        public SiteSetting AccentBackgroundColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-accent-background-color", Value = "#212529", DisplayName = "Accent Background Color" };

        [BindProperty]
        public SiteSetting AccentTextColor { get; set; } = new SiteSetting { Name = "--bgdb-theme-accent-text-color", Value = "#eee", DisplayName = "Accent Text Color" };

        public IDictionary<string, SiteSetting> GetCssRootVarSettings()
        {
            var settings = new Dictionary<string, SiteSetting>();
            settings.Add(nameof(MainBackgroundColor), MainBackgroundColor);
            settings.Add(nameof(MainTextColor), MainTextColor);
            settings.Add(nameof(NavbarBackgroundColor), NavbarBackgroundColor);
            settings.Add(nameof(NavbarTextColor), NavbarTextColor);
            settings.Add(nameof(CardBackgroundColor), CardBackgroundColor);
            settings.Add(nameof(CardTextColor), CardTextColor);
            settings.Add(nameof(AccentBackgroundColor), AccentBackgroundColor);
            settings.Add(nameof(AccentTextColor), AccentTextColor);
            return settings;
        }

        public async Task ReadAsync(BoardGameDBContext context)
        {
            await MainBackgroundColor.ReadAsync(context);
            await MainTextColor.ReadAsync(context);
            await NavbarBackgroundColor.ReadAsync(context);
            await NavbarTextColor.ReadAsync(context);
            await CardBackgroundColor.ReadAsync(context);
            await CardTextColor.ReadAsync(context);
            await AccentBackgroundColor.ReadAsync(context);
            await AccentTextColor.ReadAsync(context);
        }

        public async Task WriteAsync(BoardGameDBContext context)
        {
            MainBackgroundColor.Write(context);
            MainTextColor.Write(context);
            NavbarBackgroundColor.Write(context);
            NavbarTextColor.Write(context);
            CardBackgroundColor.Write(context);
            CardTextColor.Write(context);
            AccentBackgroundColor.Write(context);
            AccentTextColor.Write(context);
            await context.SaveChangesAsync();
        }
    }

}