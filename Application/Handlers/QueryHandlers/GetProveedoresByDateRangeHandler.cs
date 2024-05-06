using Application.Queries;
using ClosedXML.Excel;
using Common.Exceptions;
using Domain.Core;
using FluentResults;
using Infrastructure.Repositories.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Application.Handlers.QueryHandlers
{
    public class GetProveedoresByDateRangeHandler : IRequestHandler<GetProveedoresByDateRangeQuery, Result<XLWorkbook>>
    {
        private readonly ILogger _logger;
        private readonly IExcelService _excelService;
        private readonly IProveedorRepository _proveedorRepository;

        public GetProveedoresByDateRangeHandler(ILogger<GetProveedoresByDateRangeHandler> logger, IExcelService excelService,
            IProveedorRepository proveedorRepository)
        {
            _logger = logger;
            _excelService = excelService;
            _proveedorRepository = proveedorRepository;
        }

        public async Task<Result<XLWorkbook>> Handle(GetProveedoresByDateRangeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.StartDate > request.EndDate) return Result.Fail(new BadRequestError($"The end date could not be greater than start date"));
                
                var proveedoresDt = await _proveedorRepository.GetAllByCreatedRangeDateAsync(request.StartDate, request.EndDate, cancellationToken);

                var workBook = await _excelService.CreateFile(proveedoresDt);

                return Result.Ok(workBook);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Something bad happened when trying to get the proveedores");
                return Result.Fail(new DatabaseError("Something bad happened when trying to get the proveedores")
                    .CausedBy(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UnExpected error please review the log for more details");
                return Result.Fail(new UnExpectedError("UnExpected error please review the log for more details")
                    .CausedBy(ex));
            }
        }
    }
}
