using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepeaterCouncil.Web.Services;

namespace RepeaterCouncil.Web.Controllers
{
    public class QrzController : Controller
    {
        private readonly QrzAuthService _qrzAuth;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public QrzController(QrzAuthService qrzAuth,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _qrzAuth = qrzAuth;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string callsign, string password)
        {
            var result = await _qrzAuth.LoginAsync(callsign, password);
            if (!result.Success)
            {
                ViewBag.Error = result.Error;
                return View();
            }

            var user = await _userManager.FindByLoginAsync("QRZ", callsign);
            if (user == null)
            {
                // optionally: search by email & link automatically
                TempData["Callsign"] = callsign;
                TempData["Email"] = result.Email;
                return RedirectToAction("Link");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Link() => View();

        [HttpPost]
        public async Task<IActionResult> LinkConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);
            var callsign = TempData["Callsign"]?.ToString();

            await _userManager.AddLoginAsync(user, new UserLoginInfo("QRZ", callsign, "QRZ.com"));
            return RedirectToAction("Manage", "Account");
        }
    }
}
