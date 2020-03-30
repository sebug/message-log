using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
            var result = new MessageLogExport()
            {
                FileName = "Export.xlsx",
                Content = new byte[0]
            };

            using (var ms = new MemoryStream())
            {
                using (var spreadsheetDocument = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                        AppendChild<Sheets>(new Sheets());

                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet()
                    {
                        Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Journal messages"
                    };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    // Close the document.
                    spreadsheetDocument.Close();
                }

                result.Content = ms.ToArray();
            }

            return result;
        }
    }
}
