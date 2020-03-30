using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using message_log.Models;
using System.Linq;
using DocumentFormat.OpenXml;

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

            var events = this.EventRepository.GetAll();
            string eventName = events.Where(ev => ev.EventID == eventID)
                .Select(ev => ev.EventName).FirstOrDefault() ?? "Journal";

            result.FileName = eventName + ".xlsx";

            using (var ms = new MemoryStream())
            {
                using (var spreadsheetDocument = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    SharedStringTablePart sharedStringTable;
                    if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    {
                        sharedStringTable = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    }
                    else
                    {
                        sharedStringTable = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
                    }

                    Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                        AppendChild<Sheets>(new Sheets());

                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet()
                    {
                        Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = eventName.Replace("Journal", String.Empty).Substring(0, 31)
                    };
                    sheets.Append(sheet);

                    var a1 = InsertCellInWorksheet("A", 1, worksheetPart);
                    a1.CellValue = new CellValue(InsertSharedStringItem("Date d'envoi", sharedStringTable).ToString());
                    a1.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);

                    var b1 = InsertCellInWorksheet("B", 1, worksheetPart);
                    b1.CellValue = new CellValue(InsertSharedStringItem("De", sharedStringTable).ToString());
                    b1.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);

                    var c1 = InsertCellInWorksheet("C", 1, worksheetPart);
                    c1.CellValue = new CellValue(InsertSharedStringItem("A", sharedStringTable).ToString());
                    c1.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);

                    var d1 = InsertCellInWorksheet("D", 1, worksheetPart);
                    d1.CellValue = new CellValue(InsertSharedStringItem("Message", sharedStringTable).ToString());
                    d1.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);

                    var e1 = InsertCellInWorksheet("E", 1, worksheetPart);
                    e1.CellValue = new CellValue(InsertSharedStringItem("Urgence", sharedStringTable).ToString());
                    e1.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);

                    var f1 = InsertCellInWorksheet("F", 1, worksheetPart);
                    f1.CellValue = new CellValue(InsertSharedStringItem("Quittance", sharedStringTable).ToString());
                    f1.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);

                    var messages = this.MessageRepository.GetAllByEventID(eventID);

                    uint rowIdx = 2;

                    Stylesheet styleSheet = new Stylesheet();

                    CellFormat cf = new CellFormat();
                    cf.NumberFormatId = 22;
                    cf.ApplyNumberFormat = true;

                    CellFormats cfs = new CellFormats();
                    cfs.Append(cf);
                    styleSheet.CellFormats = cfs;

                    styleSheet.Borders = new Borders();
                    styleSheet.Borders.Append(new Border());
                    styleSheet.Fills = new Fills();
                    styleSheet.Fills.Append(new Fill());
                    styleSheet.Fonts = new Fonts();
                    styleSheet.Fonts.Append(new Font());

                    workbookPart.AddNewPart<WorkbookStylesPart>();
                    workbookPart.WorkbookStylesPart.Stylesheet = styleSheet;

                    CellStyles css = new CellStyles();
                    CellStyle cs = new CellStyle();
                    cs.FormatId = 0;
                    cs.BuiltinId = 0;
                    css.Append(cs);
                    css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
                    styleSheet.Append(css);

                    foreach (var message in messages)
                    {
                        var a = InsertCellInWorksheet("A", rowIdx, worksheetPart);
                        a.CellValue = new CellValue(message.EnteredOn);
                        a.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.Date);
                        a.StyleIndex = 0;

                        if (!String.IsNullOrEmpty(message.Sender))
                        {
                            var b = InsertCellInWorksheet("B", rowIdx, worksheetPart);
                            b.CellValue = new CellValue(InsertSharedStringItem(message.Sender, sharedStringTable).ToString());
                            b.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);
                        }

                        if (!String.IsNullOrEmpty(message.Recipient))
                        {
                            var c = InsertCellInWorksheet("C", rowIdx, worksheetPart);
                            c.CellValue = new CellValue(InsertSharedStringItem(message.Recipient, sharedStringTable).ToString());
                            c.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);
                        }

                        if (!String.IsNullOrEmpty(message.MessageText))
                        {
                            var d = InsertCellInWorksheet("D", rowIdx, worksheetPart);
                            d.CellValue = new CellValue(InsertSharedStringItem(message.MessageText
                                .Replace("\n", String.Empty)
                                .Replace("\r", String.Empty)
                                , sharedStringTable).ToString());
                            d.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);
                        }

                        if (message.Priority != null && !String.IsNullOrEmpty(message.Priority.Name))
                        {
                            var e = InsertCellInWorksheet("E", rowIdx, worksheetPart);
                            e.CellValue = new CellValue(InsertSharedStringItem(message.Priority.Name, sharedStringTable).ToString());
                            e.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);
                        }

                        if (message.Approval != null && !String.IsNullOrEmpty(message.Approval.Name))
                        {
                            var f = InsertCellInWorksheet("F", rowIdx, worksheetPart);
                            f.CellValue = new CellValue(InsertSharedStringItem(message.Approval.Name, sharedStringTable).ToString());
                            f.DataType = new DocumentFormat.OpenXml.EnumValue<CellValues>(CellValues.SharedString);
                        }

                        rowIdx += 1;
                    }

                    workbookPart.Workbook.Save();

                    // Close the document.
                    spreadsheetDocument.Close();
                }

                result.Content = ms.ToArray();
            }

            return result;
        }

        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }
    }
}
