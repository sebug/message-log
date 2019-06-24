using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace message_log.Pages
{
    public class IndexModel : PageModel
    {
        private MessageLogOptions _options;
        public IndexModel(IOptions<MessageLogOptions> options)
        {
            this._options = options.Value;
        }

        public void OnGet()
        {

        }
    }
}
