using System;
using System.ComponentModel.DataAnnotations;

namespace message_log.Models
{
    public class Priority
    {
        [Key]
        public int PriorityID { get; set; }
        public string Name { get; set; }
    }
}
