using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace PatientBackAPI.Data
{
    public class DBUsersAndRoles
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // (UPD018.1) DBUsersAndRoles.cs: create users and roles: Admin, Organizer and Practitioner.
        public DBUsersAndRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // User and role Organizer.
        public async Task<bool> Organizer()
        {
            if (await _userManager.FindByNameAsync("Organizer") is not null)
            {
                return true;
            }
            var user = new IdentityUser()
            {
                UserName = "Organizer"
            };
            // Create role Organizer if not exists.
            if (!await _roleManager.RoleExistsAsync("Organizer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Organizer"));
            }
            // Create user Organizer if not exists (with Organizer role).
            var result = await _userManager.CreateAsync(user, "Org202478*");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Organizer");
                return true;
            }
            return false;
        }

        // User and role Practitioner.
        public async Task<bool> Practitioner()
        {
            if (await _userManager.FindByNameAsync("Practitioner") is not null)
            {
                return true;
            }
            var user = new IdentityUser()
            {
                UserName = "Practitioner"
            };
            // Create role Practitioner if not exists.
            if (!await _roleManager.RoleExistsAsync("Practitioner"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Practitioner"));/**/
            }
            // Create user Practitioner if not exists (with Practitioner role).
            var result = await _userManager.CreateAsync(user, "Pra202478*");/**/
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Practitioner");
                return true;
            }
            return false;
        }

        public async Task<bool> Admin()
        {
            if (await _userManager.FindByNameAsync("Admin") is not null)
            {
                return true;
            }
            var user = new IdentityUser()
            {
                UserName = "Admin"
            };
            if (!await _roleManager.RoleExistsAsync("Practitioner"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Practitioner"));
            }
            if (!await _roleManager.RoleExistsAsync("Organizer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Organizer"));
            }
            var result = await _userManager.CreateAsync(user, "Admin202478*");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Practitioner");
                await _userManager.AddToRoleAsync(user, "Organizer");
                return true;
            }
            return false;
        }
    }
}
