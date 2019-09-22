using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult OnPost()
        {
            bool isAuthenticated = this._authenticationService.IsAuthenticated(this.Username, this.Password);
            if (isAuthenticated)
            {
                this.AuthenticationFailed = false;
            }
            else
            {
                this.AuthenticationFailed = true;
            }
            return null;
        }
    }
}
