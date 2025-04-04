using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationModel : PageModel
    {
        public string? Email { get; set; }
        public string? EmailConfirmationUrl { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public void OnGet(string email, string returnUrl = null, string confirmationUrl = null)
        {
            Email = email;
            returnUrl ??= Url.Content("~/");

            DisplayConfirmAccountLink = true;
            EmailConfirmationUrl = confirmationUrl;

        }
    }
}
