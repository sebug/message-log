﻿using System;
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

        public Message Copy()
        {
            var copy = new Message();
            copy.MessageID = this.MessageID;
            copy.EventID = this.EventID;
            copy.EnteredOn = this.EnteredOn;
            copy.Sender = this.Sender;
            copy.Recipient = this.Recipient;
            copy.MessageText = this.MessageText;
            copy.PriorityID = this.PriorityID;
            copy.ApprovalID = this.ApprovalID;

            return copy;
        }

        public string MessageAnchor
        {
            get
            {
                return "messageline-" + this.MessageID;
            }
        }

        public string EditLink
        {
            get
            {
                return "/Message/" + EventID + "/" + MessageID + "#message-form";
            }
        }

        public string ApprovalClass
        {
            get
            {
                if (this.ApprovalID.HasValue)
                {
                    if (this.ApprovalID.Value == 1)
                    {
                        return "approval-yes";
                    }
                    else if (this.ApprovalID == 2)
                    {
                        return "approval-partial";
                    }
                }
                return "approval-not-entered";
            }
        }

        public string PriorityClass
        {
            get
            {
                if (this.PriorityID.HasValue)
                {
                    if (this.PriorityID.Value == 1)
                    {
                        return "priority-elevated";
                    }
                    else if (this.PriorityID.Value == 2)
                    {
                        return "priority-medium";
                    }
                    else if (this.PriorityID.Value == 3)
                    {
                        return "priority-low";
                    }
                    else if (this.PriorityID.Value == 4)
                    {
                        return "priority-none";
                    }
                }
                return "priority-not-entered";
            }
        }
    }
}
