using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
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
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                    ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                }
                return View(registerDTO);
            }
        }
    }
}
