using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace message_log.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IAuthenticationService _authenticationService;
        public ChangePasswordModel(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string RepeatNewPassword { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            this.ErrorMessage = String.Empty;
            this.SuccessMessage = String.Empty;

            if (String.IsNullOrEmpty(this.UserName))
            {
                this.ErrorMessage = "Veuillez rentrer votre nom d'utilisateur";
                return;
            }
            if (String.IsNullOrEmpty(this.OldPassword))
            {
                this.ErrorMessage = "Veuillez rentrer votre mot de passe";
                return;
            }
            bool isAuthenticated = this._authenticationService.IsAuthenticated(this.UserName, this.OldPassword);
            if (!isAuthenticated)
            {
                this.ErrorMessage = "Mauvais nom d'utilisateur ou ancien mot de passe";
                return;
            }
            if (String.IsNullOrEmpty(this.NewPassword))
            {
                this.ErrorMessage = "Veuillez rentrer un nouveau mot de passe";
                return;
            }
            if (this.NewPassword != this.RepeatNewPassword)
            {
                this.ErrorMessage = "Les nouveaux mots de passe ne sont pas les mêmes";
                return;
            }
            bool changed = this._authenticationService.ChangePassword(this.UserName, this.NewPassword);
            if (!changed)
            {
                this.ErrorMessage = "Erreur pendant le changement du mot de passe, rien enregistré.";
                return;
            }
            this.Response.Cookies.Append("username", this.UserName);
            this.Response.Cookies.Append("password", Convert.ToBase64String(Encoding.UTF8.GetBytes(this.NewPassword)));

            this.SuccessMessage = "Mot de passe changé.";
        }
    }
}
