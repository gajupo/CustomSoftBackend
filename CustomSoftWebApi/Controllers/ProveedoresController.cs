using Application.Commands;
using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Common.Exceptions;
using Domain.Core;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomSoftWebApi.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Proveedores()
        {
            var proveedores = await mediator.Send(new GetProveedoresListQuery());

            return Ok(proveedores);
        }

        [HttpGet("{proveedorId:int}")]
        public async Task<IActionResult> Proveedores(int proveedorId)
        {
            var proveedor = await mediator.Send(new GetProveedorByIdQuery() { Id = proveedorId });

            if(proveedor == null)
            {
                return ModelState.ThrowNotFoundObjectResult("Proveedores", "Proveedor not found");
            }

            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProveedorAsync(ProveedorDto proveedor)
        {
            var insertedProveedor = await mediator.Send(mapper.Map<CreateProveedorCommand>(proveedor));

            if (insertedProveedor == null)
            {
                return ModelState.ThrowNotFoundObjectResult("Proveedores", "Proveedor not found");
            }

            return CreatedAtAction(nameof(Proveedores), new { proveedorId = insertedProveedor.Id }, insertedProveedor);
        }

        [HttpPut("{proveedorId:int}")]
        public async Task<IActionResult> UpdateProveedorAsync(int proveedorId,[FromBody]ProveedorDto proveedor)
        {
            if(proveedorId != proveedor.Id)
                return ModelState.ThrowBadRequestObjectResult("Proveedores", "id paramter should be the same as the id in proveedor object");

            var insertedProveedor = await mediator.Send(mapper.Map<UpdateProveedorCommand>(proveedor));

            if (insertedProveedor == 0)
            {
                return ModelState.ThrowBadRequestObjectResult("Proveedores", "Unable to update the given proveedor");
            }

            return NoContent();
        }

        [HttpDelete("{proveedorId:int}")]
        public async Task<IActionResult> DeleteProveedorAsync(int proveedorId)
        {

            var insertedProveedor = await mediator.Send(new DeleteProveedorCommand() { Id = proveedorId });

            if (insertedProveedor == 0)
            {
                return ModelState.ThrowBadRequestObjectResult("Proveedores", "Unable to delete the given proveedor");
            }

            return NoContent();
        }
        [HttpPost("add-invoices")]
        public async Task<IActionResult> AddInvoicesToProveedorAsync([FromForm] InvoicesDto invoices)
        {
            if (invoices.files == null) return ModelState.ThrowBadRequestObjectResult("Proveedores", "Missing file, please provide a valid one");
            if (invoices.ProveedorId <= 0)
                return ModelState.ThrowBadRequestObjectResult("Proveedores", "ProveedorId paramter is required");

            var filesSaved = await mediator.Send(mapper.Map<AddInvoicesCommand>(invoices));

            if(filesSaved)
            {
                return Ok();
            }

            return ModelState.ThrowBadRequestObjectResult("Proveedores", "Unable to save files, please try again");
        }
    }
}
