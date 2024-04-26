using Application.Commands;
using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Common.Helpers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomSoftWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ProveedoresController(IMediator mediator, IMapper mapper)
        {
           this.mediator = mediator;
            this.mapper = mapper;
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
    }
}
