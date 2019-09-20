using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace message_log.Models
{
    public class Approval
    {
        [Key]
        public int ApprovalID { get; set; }
        public string Name { get; set; }

        public List<Message> Messages { get; set; }
    }
}
