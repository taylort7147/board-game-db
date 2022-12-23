using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameDB.Pages.Shared;

namespace BoardGameDB.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModelBase
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(BoardGameDB.Data.BoardGameDBContext context, ILogger<ErrorModel> logger) :
        base(context)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        LoadTheme();
        ViewData["Theme"] = Theme;

        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}

