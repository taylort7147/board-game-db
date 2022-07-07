using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameDB.Models
{
    public class Game
    {
        public int Id { get; set; }
        
        [StringLength(128)]
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(8)]
        [Required]
        public string Location { get; set; } = string.Empty;

        public Complexity? Complexity { get; set; }
        public int MinimumPlayTimeMinutes { get; set; }
        public int MaximumPlayTimeMinutes { get; set; }
        public int MinimumPlayerCount { get; set; }
        public int MaximumPlayerCount { get; set; }

        [DataType(DataType.Url)]
        public string? PictureUrl { get; set; }

        [DataType(DataType.Url)]
        public string? RulesUrl { get; set; }

        [DataType(DataType.Url)]
        public string? RulesVideoUrl { get; set; }

        [DataType(DataType.Url)]
        public string? BoardGameGeekUrl { get; set; }        

        [NotMapped]
        public string? ComplexityString { 
            get
            {
                return Complexity == null ? "" : Complexity.Value.ToDisplayString()!;
            } 
            set
            {
                Complexity = ComplexityExtensions.From(value);
            } 
        }        

        public ICollection<Mechanic>? Mechanics { get; set; }
        public ICollection<GameType>? GameTypes { get; set; }
        public ICollection<PlayStyle>? PlayStyles { get; set; }
    }
}