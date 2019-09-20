using System;
using System.Collections.Generic;
using System.Linq;
using message_log.Models;

namespace message_log.Repositories
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ApprovalContext _approvalContext;
        public ApprovalRepository(ApprovalContext approvalContext)
        {
            this._approvalContext = approvalContext;
        }

        public IEnumerable<Approval> GetAll()
        {
            return this._approvalContext.Approval.ToList();
        }
    }
}
