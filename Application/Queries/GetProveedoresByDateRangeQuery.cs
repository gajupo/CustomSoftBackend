using ClosedXML.Excel;
using FluentResults;
using MediatR;

namespace Application.Queries
{
    public class GetProveedoresByDateRangeQuery : IRequest<Result<XLWorkbook>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
