using System;
using System.ComponentModel.DataAnnotations;

namespace message_log.Models
{
    public class MessagesUser
    {
        [Key]
        public int MessagesUserID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
