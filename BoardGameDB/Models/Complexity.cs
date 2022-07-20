using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoardGameDB.Models
{
    public enum Complexity
    {
        Low,
        LowMedium,
        Medium,
        MediumHeavy,
        Heavy
    }

    public static class ComplexityExtensions
    {
        public static string ToDisplayString(this Complexity complexity) => complexity switch
        {
            Complexity.Low => "Low",
            Complexity.LowMedium => "Low/Medium",
            Complexity.Medium => "Medium",
            Complexity.MediumHeavy => "Medium/Heavy",
            Complexity.Heavy => "Heavy",
            _ => "Unknown"
        };

        public static Complexity? From(string? complexity) => complexity switch
        {
            "Low" => Complexity.Low,
            "Low/Medium" => Complexity.LowMedium,
            "Medium" => Complexity.Medium,
            "Medium/Heavy" => Complexity.MediumHeavy,
            "Heavy" => Complexity.Heavy,
            null => null,
            _ => throw new ArgumentOutOfRangeException($"\"{complexity}\" is not a valid Complexity")
        };

        public static IEnumerable<SelectListItem> AsEnumerable(bool includeEmptySelection = false)
        {   
            var complexityList = Enum.GetValues(typeof(Complexity))
                    .Cast<Complexity>()
                    .Select(c => new SelectListItem{ Text=c.ToDisplayString(), Value=c.ToDisplayString()}).ToList();
            if(includeEmptySelection == false)
            {
                complexityList.Insert(0, new SelectListItem{ Text = null, Value = null});
            }
            return complexityList;
        }
    }
}