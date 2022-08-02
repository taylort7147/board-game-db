using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoardGameDB.Models
{
    public enum Complexity
    {
        Low,
        Medium,
        MediumHeavy,
        Heavy,
        ExtremelyHeavy
    }

    public static class ComplexityExtensions
    {
        public static string ToDisplayString(this Complexity complexity) => complexity switch
        {
            Complexity.Low => "Low",
            Complexity.Medium => "Medium",
            Complexity.MediumHeavy => "Medium/Heavy",
            Complexity.Heavy => "Heavy",
            Complexity.ExtremelyHeavy => "Extremely Heavy",
            _ => "Unknown"
        };

        public static Complexity? From(string? complexity) => complexity switch
        {
            "Low" => Complexity.Low,
            "Medium" => Complexity.Medium,
            "Medium/Heavy" => Complexity.MediumHeavy,
            "Heavy" => Complexity.Heavy,
            "Extremely Heavy" => Complexity.ExtremelyHeavy,
            null => null,
            _ => throw new ArgumentOutOfRangeException($"\"{complexity}\" is not a valid Complexity")
        };

        public static Complexity? MapFloatToComplexity(float value)
        {
            if(value < 1.0f)
            {
                return null;
            }
            if(value < 2.0f)
            {
                return Complexity.Low;
            }
            if(value < 3.0f)
            {
                return Complexity.Medium;
            }
            if(value < 4.0f)
            {
                return Complexity.MediumHeavy;
            }
            if(value < 5.0f)
            {
                return Complexity.Heavy;
            }
            return Complexity.ExtremelyHeavy;
        }

        public static Tuple<float, float>? MapComplexityToFloatRange(Complexity? complexity) => complexity switch
        {
            Complexity.Low => new Tuple<float, float>(1.0f, 2.0f),
            Complexity.Medium => new Tuple<float, float>(2.0f, 3.0f),
            Complexity.MediumHeavy => new Tuple<float, float>(3.0f, 4.0f),
            Complexity.Heavy => new Tuple<float, float>(4.0f, 5.0f),
            Complexity.ExtremelyHeavy => new Tuple<float, float>(5.0f, 6.0f),
            _ => null
        };

        public static IEnumerable<SelectListItem> AsEnumerable(bool includeEmptySelection = false)
        {   
            var complexityList = Enum.GetValues(typeof(Complexity))
                    .Cast<Complexity>()
                    .Select(c => new SelectListItem{ Text=c.ToDisplayString(), Value=c.ToDisplayString()}).ToList();
            if(includeEmptySelection)
            {
                complexityList.Insert(0, new SelectListItem{ Text = null, Value = null});
            }
            return complexityList;
        }
    }
}