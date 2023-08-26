using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            var claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Dargebotene Hand Admin")
                })
            });
            await HttpContext.SignInAsync(claimsPrincipal);
        }
    }
}
