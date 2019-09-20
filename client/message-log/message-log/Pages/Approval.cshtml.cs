using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using message_log.Models;
using message_log.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace message_log.Pages
{
    public class ApprovalModel : PageModel
    {
        private readonly IApprovalRepository _approvalRepository;

        public ApprovalModel(IApprovalRepository approvalRepository)
        {
            this._approvalRepository = approvalRepository;
        }

        public List<Approval> Approvals { get; set; }

        public void OnGet()
        {
            this.Approvals = this._approvalRepository.GetAll().ToList();
        }
    }
}
