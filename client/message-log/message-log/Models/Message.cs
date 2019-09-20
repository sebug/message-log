using System;
using System.ComponentModel.DataAnnotations;

namespace message_log.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        public int EventID { get; set; }
        public DateTime EnteredOn { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string MessageText { get; set; }
        public int? PriorityID { get; set; }
        public int? ApprovalID { get; set; }
        public Priority Priority { get; set; }
        public Approval Approval { get; set; }

        public string EditLink
        {
            get
            {
                return "/Message/" + EventID + "/" + MessageID;
            }
        }
    }
}
