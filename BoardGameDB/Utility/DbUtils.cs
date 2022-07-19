using Microsoft.EntityFrameworkCore;

namespace BoardGameDB.Utility
{
    public static class DbUtils
    {
        public static void SetCaseInsensitiveSearchesForSQLite(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.UseCollation("NOCASE");

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                    .SelectMany(t => t.GetProperties())
                                                    .Where(p => p.ClrType == typeof(string)))
            {
                property.SetCollation("NOCASE");
            }
        }     
    }
}
