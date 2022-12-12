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

namespace BoardGameDB.Areas.Identity.Pages.Administrator.Users
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly BoardGameDBIdentityDbContext _context;

        public IndexModel(BoardGameDBIdentityDbContext context)
        {
            _context = context;
        }

        public IList<IdentityUser> Users { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Users = await _context.Users.ToListAsync();
        }
    }
}
