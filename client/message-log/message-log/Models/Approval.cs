using System;
using System.ComponentModel.DataAnnotations;

namespace message_log.Models
{
    public class Approval
    {
        [Key]
        public int ApprovalID { get; set; }
        public string Name { get; set; }
    }
}
