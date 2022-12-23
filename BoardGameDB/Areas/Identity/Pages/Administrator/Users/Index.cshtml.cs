using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BoardGameDB.Pages.Shared;
using BoardGameDB.Data;

namespace BoardGameDB.Areas.Identity.Pages.Administrator.Users
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModelBase
    {
        private readonly BoardGameDBIdentityDbContext _identityContext;

        public IndexModel(BoardGameDBContext context, BoardGameDBIdentityDbContext identityContext) : 
            base(context)
        {
            _identityContext = identityContext;
        }

        public IList<IdentityUser> Users { get; set; } = default!;

        public string ReturnUrl { get; set; } = default!;

        public async Task OnGetAsync(string? returnUrl = null)
        {
            await LoadThemeAsync();
            ViewData["Theme"] = Theme;
            ReturnUrl = String.IsNullOrEmpty(returnUrl) ? "" : returnUrl;
            Users = await _identityContext.Users.ToListAsync();
        }
    }
}
