using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunWebAppGroup.Data;
using RunWebAppGroup.Models;
using RunWebAppGroup.ViewModels;

namespace RunWebAppGroup.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DataContext _context;


        public AccountController(
            UserManager<AppUser> userManager,
            DataContext context, 
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if(!ModelState.IsValid) return View(loginVM);
           var user= await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordChek = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordChek)
                {

                    //Password correct, sign in
                    var result= await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong credentials, please, try again";
                return View(loginVM);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. please try again";
            return View(loginVM);
        }


        public async Task<IActionResult> Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            
            if (user != null)
            {
                TempData["Error"] = "This email addres is already in use";
                return View(registerVM);

            }
            var newUser = new AppUser()
            {
                UserName = registerVM.EmailAddress,
                Email = registerVM.EmailAddress,

            };
            var newUserResponse=await _userManager.CreateAsync(newUser, registerVM.Password);

            if(newUserResponse.Succeeded) 
            

                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            

                return RedirectToAction("Index", "Race");

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        { 
                await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Race");
        
        }
        
    }
}
