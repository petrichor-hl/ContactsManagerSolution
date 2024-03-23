using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTOs;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) 
        { 
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("/account/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/account/register")]

        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            /*
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return View(registerDTO);
            }
            */

            ApplicationUser user = new ApplicationUser()
            {
                PersonName = registerDTO.PersonName,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password!);

            if (result.Succeeded)
            {
                // SignIn
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Person");
            } 
            else
            {
                ViewBag.Errors = new List<string>();
                foreach (IdentityError error in result.Errors)
                {
                    ViewBag.Errors.Add(error.Description);
                }

                return View(registerDTO);
            }
        }

        [HttpGet]
        [Route("/account/login")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [Route("/account/login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            /*
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }
            */

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email!, loginDTO.Password!, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Person");
            }
            else
            {
                ViewBag.Errors = new List<string>()
                {
                    "Inalid email or password"
                };
            }
            return View(loginDTO);
        }

        [Route("/account/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Person");
        }
    }

}
