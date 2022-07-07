using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameDB.Models
{
    public class GameMechanic
    {
        public int Id { get; set; } 

        public int GameId { get; set; }

        public int MechanicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = default!;

        [ForeignKey(nameof(MechanicId))]
        public Mechanic Mechanic { get; set; } = default!;
    }
}