using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using BoardGameDB.Data;


namespace BoardGameDB.Models
{
    public class SiteSetting
    {
        [Key]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        public string DisplayName { get; set; }

        public async Task ReadAsync(BoardGameDBContext context)
        {
            var setting = await context.SiteSetting.Where(s => s.Name == Name).FirstOrDefaultAsync();
            if (setting != null)
            {
                Value = setting.Value;
                DisplayName = setting.DisplayName;
            }
        }

        public void Write(BoardGameDBContext context)
        {            
            if (SiteSettingExists(context, Name))
            {
                context.Attach(this).State = EntityState.Modified;
            }
            else
            {
                context.SiteSetting.Add(this);
            }
        }

        private bool SiteSettingExists(BoardGameDBContext context, string name)
        {
            return context.SiteSetting.Any(e => e.Name == name);
        }
    }
}
