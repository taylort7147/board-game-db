using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages;

public class PrivacyModel : PageModelBase
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(BoardGameDB.Data.BoardGameDBContext context, ILogger<PrivacyModel> logger) :
        base(context)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        LoadTheme();
        ViewData["Theme"] = Theme;
    }
}

