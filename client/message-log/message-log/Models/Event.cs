using System;
using System.ComponentModel.DataAnnotations;

namespace message_log.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string EventName { get; set; }
    }
}
