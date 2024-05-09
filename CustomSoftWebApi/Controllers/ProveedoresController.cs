using Application.Commands;
using Application.DTOs;
using Application.Queries;
using AutoMapper;
using CustomSoftWebApi.Extensions;
using CustomSoftWebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomSoftWebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment _env;

        public ProveedoresController(IMediator mediator, IMapper mapper, IWebHostEnvironment env)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            _env = env;
        }


        [HttpGet]
        [Proveedor_ValidatedPagedImputParamsAttribute]
        public async Task<IActionResult> Proveedores([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var result = await mediator.Send(new GetProveedoresListQuery() { pageNumber = pageNumber, pageSize = pageSize});
            if(result.IsFailed)
            {
                return result.ToErrorResponse();
            }
            var mappedProveedoredToDto = mapper.Map<List<ProveedorDto>>(result.Value.Item1);
            return Ok(mappedProveedoredToDto.ToPagedResult(pageNumber, pageSize, result.Value.Item2));
        }

        [HttpGet("{proveedorId:int}")]
        public async Task<IActionResult> Proveedores(int proveedorId)
        {
            var result = await mediator.Send(new GetProveedorByIdQuery() { Id = proveedorId });

            if (result.IsFailed)
            {
                return result.ToErrorResponse();
            }

            return Ok(mapper.Map<ProveedorDto>(result.Value));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProveedorAsync(ProveedorDto proveedor)
        {
            var result = await mediator.Send(mapper.Map<CreateProveedorCommand>(proveedor));

            if (result.IsFailed)
            {
                return result.ToErrorResponse();
            }
            return CreatedAtAction(nameof(Proveedores), new { proveedorId = result.Value.Id }, mapper.Map<ProveedorDto>(result.Value));
        }

        [HttpPut("{proveedorId:int}")]
        public async Task<IActionResult> UpdateProveedorAsync(int proveedorId,[FromBody]UpdateProveedorDto proveedor)
        {
            if(proveedorId != proveedor.Id)
                return ModelState.ThrowBadRequestObjectResult("Proveedores", "id paramter should be the same as the id in proveedor object");

            var result = await mediator.Send(mapper.Map<UpdateProveedorCommand>(proveedor));

            if (result.IsFailed)
            {
                return result.ToErrorResponse();
            }

            return NoContent();
        }

        [HttpDelete("{proveedorId:int}")]
        public async Task<IActionResult> DeleteProveedorAsync(int proveedorId)
        {

            var result = await mediator.Send(new DeleteProveedorCommand() { Id = proveedorId });

            if (result.IsFailed)
            {
                return result.ToErrorResponse();
            }


            return NoContent();
        }
        [HttpPost("add-invoices")]
        public async Task<IActionResult> AddInvoicesToProveedorAsync([FromForm] InvoicesDto invoices)
        {
            if (invoices.files == null) return ModelState.ThrowBadRequestObjectResult("Proveedores", "Missing file, please provide a valid one");
            if (invoices.ProveedorId <= 0)
                return ModelState.ThrowBadRequestObjectResult("Proveedores", "ProveedorId paramter is required");

            var result = await mediator.Send(mapper.Map<AddInvoicesCommand>(invoices));

            if (result.IsFailed)
            {
                return result.ToErrorResponse();
            }
            
            return Ok();
        }
        [HttpGet("export-xlsx")]
        public async Task<IActionResult> ExportProveedoresByDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await mediator.Send(new GetProveedoresByDateRangeQuery() { StartDate = startDate, EndDate = endDate });

            if (result.IsFailed)
            {
                return result.ToErrorResponse();
            }

            return File(result.ToStream(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
