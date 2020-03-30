using System;
using System.ComponentModel.DataAnnotations;

namespace message_log.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string EventName { get; set; }

        public string MessageLink
        {
            get
            {
                return "/Message/" + this.EventID;
            }
        }

        public string LogStreamLink
        {
            get
            {
                return "/LogStream/" + this.EventID;
            }
        }

        public string ExportLink
        {
            get
            {
                return "/Export?EventID=" + this.EventID;
            }
        }
    }
}
