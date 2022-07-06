using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BoardGameDB.Data
{
    public class BoardGameDBContext : DbContext
    {
        public BoardGameDBContext(DbContextOptions<BoardGameDBContext> options)
            : base(options)
        {
        }

        public DbSet<BoardGameDB.Models.Game> Game { get; set; } = null!;
        public DbSet<BoardGameDB.Models.GameType> GameType { get; set; } = null!;
        public DbSet<BoardGameDB.Models.Mechanic> Mechanic { get; set; } = null!;
        public DbSet<BoardGameDB.Models.PlayStyle> PlayStyle { get; set; } = null!;
    }
}