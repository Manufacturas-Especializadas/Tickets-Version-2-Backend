using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Tickets.Queries.ExportTickets
{
    public class ExportTicketsQueryHandler(
    IApplicationDbContext dbContext,
    IExcelService excelService) : IRequestHandler<ExportTicketsQuery, ExportTicketsResponse>
    {
        public async Task<ExportTicketsResponse> Handle(ExportTicketsQuery request, CancellationToken cancellationToken)
        {
            var ticketsData = await dbContext.Tickets
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Include(t => t.User)
                .OrderByDescending(t => t.Id)
                .AsNoTracking()
                .Select(t => new TicketReportModel(
                    t.Id,
                    t.Name,
                    t.Category != null ? t.Category.Name : "Sin categoría",
                    t.Status != null ? t.Status.Name : "Pendiente",
                    t.RegistrationDate,
                    t.ResolutionDate,
                    t.User != null ? t.User.Name : "Sin asignar"
                ))
                .ToListAsync(cancellationToken);

            var fileBytes = excelService.GenerateTicketsReport(ticketsData);

            var fileName = $"ReporteDeTickets_{DateTime.Now:ddMMyyyy}.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return new ExportTicketsResponse(fileBytes, fileName, contentType);
        }
    }
}