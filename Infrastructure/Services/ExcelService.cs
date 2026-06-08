using Application.Common.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        public byte[] GenerateTicketsReport(IEnumerable<TicketReportModel> tickets)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Tickets");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre del solicitante";
            worksheet.Cell(1, 3).Value = "Tipo de ticket";
            worksheet.Cell(1, 4).Value = "Estatus del ticket";
            worksheet.Cell(1, 5).Value = "Fecha de la solicitud";
            worksheet.Cell(1, 6).Value = "Fecha de resolución";
            worksheet.Cell(1, 7).Value = "Días transcurridos";
            worksheet.Cell(1, 8).Value = "Resuelto por";

            var headerRange = worksheet.Range("A1:H1");
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#0071ab");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            var list = tickets.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var rowNumber = i + 2;
                var currentRow = worksheet.Row(rowNumber);

                worksheet.Cell(rowNumber, 1).Value = list[i].Id;
                worksheet.Cell(rowNumber, 2).Value = list[i].Name;
                worksheet.Cell(rowNumber, 3).Value = list[i].Category;
                worksheet.Cell(rowNumber, 4).Value = list[i].Status;

                worksheet.Cell(rowNumber, 5).Value = list[i].RegistrationDate.ToString("dd/MM/yyyy HH:mm");

                worksheet.Cell(rowNumber, 6).Value = list[i].ResolutionDate.HasValue
                    ? list[i].ResolutionDate!.Value.ToString("dd/MM/yyyy HH:mm")
                    : " ";

                if (list[i].ResolutionDate.HasValue)
                {
                    TimeSpan difference = list[i].ResolutionDate!.Value - list[i].RegistrationDate;
                    worksheet.Cell(rowNumber, 7).Value = difference.Days;
                    worksheet.Cell(rowNumber, 7).Style.NumberFormat.Format = "0";
                }
                else
                {
                    TimeSpan difference = DateTime.Now - list[i].RegistrationDate;
                    worksheet.Cell(rowNumber, 7).Value = difference.Days;
                    worksheet.Cell(rowNumber, 7).Style.NumberFormat.Format = "0";
                }

                worksheet.Cell(rowNumber, 8).Value = list[i].ResolvedBy;

                if (rowNumber % 2 == 0)
                {
                    currentRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#F9FAFB");
                }
            }

            var statusColumn = worksheet.Range($"D2:D{list.Count + 1}");

            statusColumn.AddConditionalFormat().WhenContains("Cerrado")
                .Fill.SetBackgroundColor(XLColor.FromHtml("#C6EFCE"))
                .Font.SetFontColor(XLColor.FromHtml("#006100"));

            statusColumn.AddConditionalFormat().WhenContains("Abierto")
                .Fill.SetBackgroundColor(XLColor.FromHtml("#BDD7EE"))
                .Font.SetFontColor(XLColor.FromHtml("#1A4E8A"));

            statusColumn.AddConditionalFormat().WhenContains("Pendiente")
                .Fill.SetBackgroundColor(XLColor.FromHtml("#FFEB9C"))
                .Font.SetFontColor(XLColor.FromHtml("#9C6500"));

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}