using Data.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Data.EF.Entities;
using ViewModel.Users;
using Application.System.Users;
using ViewModel.System;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        //private readonly Dio_StoreContext _context;
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        private readonly IUserService _userService;
        public AccountController(
            SignInManager<Account> signInManager,
            UserManager<Account> userManager,
            IUserService userService)
        {
            //_context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
        }

        [TempData]
        public string ErrorMessage { get; set; }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM rq)
        {
            var result = await _userService.Authencate(rq);
            if (result.IsSuccessed)
            {
                if (result.Message == SystemContants.RoleAdmin)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (result.Message == SystemContants.RoleUser)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
            return View(rq);
        }


        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider)
        {
            var listprovider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var provider_process = listprovider.Find(m => m.Name == provider);
            if (provider_process == null)
            {
                return NotFound("Dịch vụ không chính xác: " + provider);
            }

            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //var ChallengeResult  = new ChallengeResult(provider, properties);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            //var results = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                TempData["ErrorMessage"] = "Lỗi thông tin từ dịch vụ đăng nhập.";
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = new Account { UserName = email, Email = email };
                    var createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        var addLoginResult = await _userManager.AddLoginAsync(user, info);
                        if (addLoginResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View("Error"); // Hoặc trả về trang phù hợp khi có lỗi
            }
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM rq)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(rq);
                if (result.IsSuccessed)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            //ModelState.AddModelError(string.Empty, result.Message);
            return View(rq);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
