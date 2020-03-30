using System;
using message_log.Models;

namespace message_log.Repositories
{
    public interface IMessageLogExportService
    {
        MessageLogExport ExportToExcel(int eventID);
    }
}
