using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameDB.Models
{
    public class GamePlayStyle
    {
        public int Id { get; set; } 

        public int GameId { get; set; }

        public int PlayStyleId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = default!;

        [ForeignKey(nameof(PlayStyleId))]
        public PlayStyle PlayStyle { get; set; } = default!;
    }
}