using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using message_log.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace message_log.Pages
{
    public class ExportModel : PageModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageLogExportService _messageLogExportService;

        public ExportModel(IAuthenticationService authenticationService,
            IMessageLogExportService messageLogExportService)
        {
            this._authenticationService = authenticationService;
            this._messageLogExportService = messageLogExportService;
        }

        private bool CheckAuthentication()
        {
            string username = this.Request.Cookies["username"];
            string password = this.Request.Cookies["password"];

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                return false;
            }
            var passwordBytes = Convert.FromBase64String(password);
            password = Encoding.UTF8.GetString(passwordBytes);

            return this._authenticationService.IsAuthenticated(username, password);
        }

        public IActionResult OnGet(int eventID)
        {
            if (!this.CheckAuthentication())
            {
                return this.Redirect("/Login?eventID=" + eventID);
            }

            var doc = this._messageLogExportService.ExportToExcel(eventID);

            return File(doc.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                doc.FileName);
        }
    }
}
