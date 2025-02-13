using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.ViewModels;
using WebApplication1.Model;

namespace WebApplication1.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await signInManager.UserManager.FindByEmailAsync(LModel.Email);

                if (user != null && await signInManager.UserManager.IsLockedOutAsync(user))
                {
                    ModelState.AddModelError("", "Your account has been locked out due to multiple failed login attempts. Please try again later.");
                    return Page();
                }

                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email,
                    LModel.Password, LModel.RememberMe, false);

                if (identityResult.Succeeded)
                {
                    HttpContext.Session.SetString("UserId", user.Id);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    return RedirectToPage("Home");
                }

                // Handle failed login attempt
                if (user != null)
                {
                    await signInManager.UserManager.AccessFailedAsync(user);
                }

                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }
    }
}
