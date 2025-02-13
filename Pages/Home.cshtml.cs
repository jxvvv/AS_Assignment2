using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication1.Model;

namespace WebApplication1.Pages
{
    public class HomeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUser CurrentUser { get; set; }
        public string DecryptedCreditCard { get; set; }

        public HomeModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the user ID from the current claims (the logged-in user)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                // Retrieve the ApplicationUser from the database based on the userId
                CurrentUser = await _userManager.FindByIdAsync(userId);

                if (CurrentUser != null)
                {
                    // Optionally decrypt sensitive data, e.g., CreditCard
                    DecryptedCreditCard = Decrypt(CurrentUser.CreditCard);
                }
            }

            return Page();
        }

        // Example decryption method (replace with your actual decryption logic)
        private string Decrypt(string encryptedData)
        {
            // Example decryption logic (for illustration)
            return new string(encryptedData.Reverse().ToArray()); // Replace with real decryption code
        }
    }
}
