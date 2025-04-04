using Company.Web.DAL.Models;
using Company.Web.PL.Dtos;
using Company.Web.PL.Helper;
using Company.Web.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMailServices mailServices;
        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> signInManager, IMailServices mailServices)
        {
            userManager = _userManager;
            this.signInManager = signInManager;
            this.mailServices = mailServices;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = new AppUser
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,
                        };
                        var result = await userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                            return RedirectToAction("SignIn");
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SignUp !");

            }

            return View(model);

        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid SignIn !");
            }
            return View();
        }

        [HttpGet]
        public async new Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }


        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                if (ModelState.IsValid)
                {
                    //Create token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    //Create url
                    var url = Url.Action(action: "ResetPassword", controller: "Account", new { model.Email, token }, Request.Scheme);

                    //Create email
                    Email email = new Email()
                    {
                        To = model.Email,
                        Subject = $"Reset Pawword {user.UserName}",
                        Body = url
                    };
                    //send email
                    //bool flag = EmailSetting.SendEmail(email);
                    bool flag = mailServices.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckInbox");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Reset Password !");
            return View(nameof(ForgetPassword), model);
        }

        [HttpGet]
        public IActionResult CheckInbox()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email is null || token is null) return BadRequest("Invalid Operation!");

                var user = await userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }

            }
            return View(model);
        }

        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action(nameof(GoogleResponse))
            };

            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var cliams = result.Principal.Identities.FirstOrDefault().Claims.Select(cliam => new
            {
                cliam.Type,
                cliam.Value,
                cliam.Issuer,
                cliam.OriginalIssuer
            });

            return RedirectToAction("Index", "Home");
        }


        public IActionResult FacebookLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action(nameof(FacebookResponse))
            };

            return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            var cliams = result.Principal.Identities.FirstOrDefault().Claims.Select(cliam => new
            {
                cliam.Type,
                cliam.Value,
                cliam.Issuer,
                cliam.OriginalIssuer
            });

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
