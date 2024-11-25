using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Deno.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class AccountController :Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                //not create mapper and profile it simple so make it manual 
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };

                var Result = await _userManager.CreateAsync(User,model.Password);

                if (Result.Succeeded)
                    RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                   var Flag = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Flag)
                    {
                     var Result = await  _signInManager.PasswordSignInAsync(User,model.Password,model.RememberMe ,false);
                        if (Result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "InCorrect Password!");

                }
                else
                    ModelState.AddModelError(string.Empty, "Email is not Exist !");

            }
            return View(model);
        }

        public new async Task<IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model )
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account" ,new
                    {
                       email= User.Email,
                       Token = token

                    }, Request.Scheme);
                    var email = new Email()
                    {
                        Subject = "ResetPassword",
                        To = model.Email,
                        Body = ResetPasswordLink

                    };

                    try
                    {
                         await EmailSetting.SendEmailAsync(email);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                    }
                    return RedirectToAction(nameof(CheckYourInbox));

                }
                else
                    ModelState.AddModelError(string.Empty, "Email Not Exist");
              
            }
            return View("ForgetPassword", model);

        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email ,string token)
        {
            TempData["email"]= email;
            TempData["token"]= token;

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(User,token,model.NewPassword);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                   foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

    }
}
