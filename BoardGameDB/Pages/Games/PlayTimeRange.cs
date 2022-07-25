namespace BoardGameDB.Pages_Games
{
    public enum PlayTimeRange
    {
        LessThan30Minutes,
        Between30And60Minutes,
        Between1And2Hours,
        MoreThan2Hours
    }

    public static class PlayTimeRangeExtensions
    {
        public static string ToDisplayString(this PlayTimeRange range) => range switch
        {
            PlayTimeRange.LessThan30Minutes => "Less than 30 minutes",
            PlayTimeRange.Between30And60Minutes => "30-60 minutes",
            PlayTimeRange.Between1And2Hours => "1-2 hours",
            PlayTimeRange.MoreThan2Hours => "More than 2 hours",
            _ => "Unknown"
        };

        public static PlayTimeRange? From(string? range) => range switch
        {
            "Less than 30 minutes" => PlayTimeRange.LessThan30Minutes,
            "30-60 minutes" => PlayTimeRange.Between30And60Minutes,
            "1-2 hours" => PlayTimeRange.Between1And2Hours,
            "More than 2 hours" => PlayTimeRange.MoreThan2Hours,
            _ => null
        };
    }
}
