using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameDB.Models
{
    public class GameGameType
    {
        public int Id { get; set; } 

        public int GameId { get; set; }

        public int GameTypeId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = default!;

        [ForeignKey(nameof(GameTypeId))]
        public GameType GameType { get; set; } = default!;
    }
}