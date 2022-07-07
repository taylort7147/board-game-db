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
    }
}