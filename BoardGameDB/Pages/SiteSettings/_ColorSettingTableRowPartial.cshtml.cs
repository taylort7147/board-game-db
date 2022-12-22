using BoardGameDB.Models;

namespace BoardGameDB.Pages.SiteSettings
{
    public class ColorSettingsTableRowParitalModel
    {
        public string PropertyId { get; set; }
        
        public string PropertyName { get; set; }

        public SiteSetting Setting { get; set; }
    }
}
