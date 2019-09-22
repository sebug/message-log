using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace message_log.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public bool AuthenticationFailed { get; set; }


        private readonly IAuthenticationService _authenticationService;
        public LoginModel(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(int? eventID = null)
        {
            bool isAuthenticated = this._authenticationService.IsAuthenticated(this.Username, this.Password);
            if (isAuthenticated)
            {
                this.AuthenticationFailed = false;
                this.Response.Cookies.Append("username", this.Username);
                this.Response.Cookies.Append("password", Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Password)));
                if (eventID.HasValue)
                {
                    return this.Redirect("/Message/" + eventID);
                }
                this.Redirect("/");
            }
            else
            {
                this.AuthenticationFailed = true;
            }
            return null;
        }
    }
}
