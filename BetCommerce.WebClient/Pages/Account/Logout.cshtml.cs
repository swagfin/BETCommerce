using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BetCommerce.WebClient.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                await HttpContext.SignOutAsync();
            }
            catch { }
            return LocalRedirect("/account/login");
        }
    }
}
