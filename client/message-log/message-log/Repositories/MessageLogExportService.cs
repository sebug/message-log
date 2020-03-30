using System;
using message_log.Models;

namespace message_log.Repositories
{
    public class MessageLogExportService : IMessageLogExportService
    {
        private IMessageRepository MessageRepository { get; }
        private IEventRepository EventRepository { get; }
        public MessageLogExportService(IMessageRepository messageRepository,
            IEventRepository eventRepository)
        {
            this.MessageRepository = messageRepository;
            this.EventRepository = eventRepository;
        }

        public MessageLogExport ExportToExcel(int eventID)
        {
            return new MessageLogExport()
            {
                FileName = "Export.xlsx",
                Content = new byte[0]
            };
        }
    }
}
