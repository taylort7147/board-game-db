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

        [Display(Name = "Play Time (Min)")]
        public int MinimumPlayTimeMinutes { get; set; }

        [Display(Name = "Play Time (Max)")]
        public int MaximumPlayTimeMinutes { get; set; }


        [Display(Name = "Player Count (Min)")]
        public int MinimumPlayerCount { get; set; }

        [Display(Name = "Player Count (Max)")]
        public int MaximumPlayerCount { get; set; }

        [Display(Name = "Picture URL")]
        [DataType(DataType.Url)]
        public string? PictureUrl { get; set; }

        [Display(Name = "Rules URL")]
        [DataType(DataType.Url)]
        public string? RulesUrl { get; set; }

        [Display(Name = "Rules Video URL")]
        [DataType(DataType.Url)]
        public string? RulesVideoUrl { get; set; }

        [Display(Name = "BGG URL")]
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

        public List<Mechanic> Mechanics { get; set; } = default!;
        [Display(Name = " Game Types")]
        public List<GameType> GameTypes { get; set; } = default!;
        [Display(Name = " Play Styles")]
        public List<PlayStyle> PlayStyles { get; set; } = default!;
    }
}