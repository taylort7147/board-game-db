using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameDB.Areas.Identity.Authorization;
using BoardGameDB.Areas.Identity.Data;
using BoardGameDB.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BoardGameDB.Areas.Identity.Pages.Administrator.Users
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BoardGameDBIdentityDbContext _context;

        public EditModel(UserManager<IdentityUser> userManager,
                         BoardGameDBIdentityDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public IdentityUser IdentityUser { get; set; }

        [BindProperty]
        public CheckBoxModel IsEditorCheckBox { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser = await _userManager.FindByIdAsync(id);
            if (IdentityUser == null)
            {
                return NotFound();
            }

            IsEditorCheckBox = new CheckBoxModel { DisplayName = "Editor"};
            IsEditorCheckBox.IsChecked = await _userManager.IsInRoleAsync(IdentityUser, Role.Editor);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await UpdateRole(Role.Editor, IsEditorCheckBox.IsChecked);

            return RedirectToPage("./Index");
        }

        private async Task UpdateRole(string role, bool isChecked)
        {
            if (!isChecked &&
                    await _userManager.IsInRoleAsync(IdentityUser, role))
            {
                await _userManager.UpdateSecurityStampAsync(IdentityUser);
                await _userManager.RemoveFromRoleAsync(IdentityUser, role);
                await _context.SaveChangesAsync();
            }
            else if (isChecked &&
                    !(await _userManager.IsInRoleAsync(IdentityUser, role)))
            {
                await _userManager.UpdateSecurityStampAsync(IdentityUser);
                await _userManager.AddToRoleAsync(IdentityUser, role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
