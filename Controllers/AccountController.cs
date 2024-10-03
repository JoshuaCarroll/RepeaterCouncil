using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Data;
using RepeaterCouncil.Models;

namespace RepeaterCouncil.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<IActionResult> Login(string callsign, string password)
        {
            var user = await _userManager.FindByNameAsync(callsign);

            if (user == null)
            {
                // Validate the user against the old system using your stored procedure or legacy logic
                var result = await ValidateWithStoredProcedure(callsign, password);
                if (result > 0)
                {
                    // Create a new IdentityUserId and migrate the user
                    user = await MigrateLegacyUserToIdentity(callsign);
                }
                else
                {
                    // Invalid login attempt
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View();
                }
            }

            // Standard ASP.NET Identity login
            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: true, lockoutOnFailure: false);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        public async Task<User> MigrateLegacyUserToIdentity(string callsign)
        {
            var legacyUser = await _context.Users.FirstOrDefaultAsync(u => u.Callsign == callsign);
            if (legacyUser != null)
            {
                // Generate a new IdentityUserId for ASP.NET Identity
                legacyUser.IdentityUserId = Guid.NewGuid().ToString();

                // Save the updated user
                await _context.SaveChangesAsync();
            }

            return legacyUser;
        }

        public async Task<int> ValidateWithStoredProcedure(string callsign, string password)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "spLogin"; // Name of the stored procedure
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Add input parameters
                command.Parameters.Add(new SqlParameter("@callsign", callsign));
                command.Parameters.Add(new SqlParameter("@password", password));

                _context.Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    if (result.HasRows)
                    {
                        while (await result.ReadAsync())
                        {
                            int returnValue = result.GetInt32(result.GetOrdinal("Return"));
                            return returnValue; // Return the result from the stored procedure
                        }
                    }
                }
            }

            return 0; // If no result, return 0 (indicating failure)
        }
    }
}
