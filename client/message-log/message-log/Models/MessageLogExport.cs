using System;
namespace message_log.Models
{
    public class MessageLogExport
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
