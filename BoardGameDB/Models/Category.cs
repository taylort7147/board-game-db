using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BoardGameDB.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Game> Games { get; set; } = default!;
    }
}