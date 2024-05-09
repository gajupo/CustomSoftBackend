using CustomSoft.WebApp.Filters;
using CustomSoft.WebApp.Server.Data;
using CustomSoft.WebApp.Server.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CustomSoft.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IWebApiExecuter _webApiExecuter;

        public ProveedoresController(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }
        [Proveedor_ValidatedPagedImputParamsAttribute]
        public async Task<IActionResult> Proveedores([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var shirtList = await _webApiExecuter.InvokeGet<PagedProveedorDTO>($"proveedores?pageNumber={pageNumber}&pageSize={pageSize}");

            return Ok(shirtList);
        }

        [HttpGet("{proveedorId:int}")]
        public async Task<IActionResult> Proveedores(int proveedorId)
        {
            var proveedor = await _webApiExecuter.InvokeGet<ProveedorDto>($"proveedores/{proveedorId}");

            return Ok(proveedor);
        }

        [HttpDelete("{proveedorId:int}")]
        public async Task<IActionResult> RemoveProveedor(int proveedorId)
        {
            try
            {
                if (proveedorId > 0)
                {
                    await _webApiExecuter.InvokeDelete($"proveedores/{proveedorId}");
                    return NoContent();
                }

                // in case of some error, we need to keep the model state
                ModelState.AddModelError("BadRequest", "The given id is not valid");
                var errorDeatils = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                };

                return new BadRequestObjectResult(errorDeatils);
            }
            catch (WebApiException ex)
            {
                return HandleWebApiException(ex);
            }
        }

        [HttpPut("{proveedorId:int}")]
        public async Task<IActionResult> UpdateProveedor(ProveedorDto proveedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _webApiExecuter.InvokePut($"proveedores/{proveedor.Id}", proveedor);
                    return NoContent();
                }
                catch (WebApiException ex)
                {
                    return HandleWebApiException(ex);
                }
            }

            ModelState.AddModelError("BadRequest", "The given proveedor is not valid");
            var errorDeatils = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };

            return new BadRequestObjectResult(errorDeatils);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProveedor(ProveedorDto proveedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _webApiExecuter.InvokePost<ProveedorDto>("proveedores", proveedor);
                    if (response != null)
                    {
                        return Ok(response);
                    }

                }
                catch (WebApiException ex)
                {
                    return HandleWebApiException(ex);
                }
            }
            ModelState.AddModelError("BadRequest", "The given proveedor was not saved.");
            var errorDeatils = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };

            return new BadRequestObjectResult(errorDeatils);
        }

        [HttpPost("add-invoices")]
        public async Task<IActionResult> AddInvoicesToProveedorAsync([FromForm] InvoicesDto invoices)
        {
            var formContents = new List<KeyValuePair<string, HttpContent>>();

            // Handle files
            if (invoices.files != null)
            {
                foreach (var file in invoices.files)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"files\"",
                        FileName = "\"" + file.FileName + "\""
                    };
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    formContents.Add(new KeyValuePair<string, HttpContent>("files", fileContent));
                }
            }

            formContents.Add(new KeyValuePair<string, HttpContent>("ProveedorId", new StringContent(invoices.ProveedorId.ToString())));

            if (!string.IsNullOrEmpty(invoices.DestinationFolder))
            {
                formContents.Add(new KeyValuePair<string, HttpContent>("DestinationFolder", new StringContent(invoices.DestinationFolder)));
            }

            try
            {
                await _webApiExecuter.InvokePostWithFiles("proveedores/add-invoices", formContents);
                return Ok();
            }
            catch (WebApiException ex)
            {
                return HandleWebApiException(ex);
            }
        }

        [HttpGet("export-xlsx")]
        public async Task<IActionResult> ExportProveedoresByDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var streamFile = await _webApiExecuter.InvokeGetAsStream($"proveedores/export-xlsx?startDate={startDate}&endDate={endDate}");
            streamFile.Seek(0, SeekOrigin.Begin);
            return File(streamFile,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"ListaDeProveedores-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.xlsx");
        }

        private ObjectResult HandleWebApiException(WebApiException ex)
        {
            if (ex.ErrorResponse != null && ex.ErrorResponse.Errors != null && ex.ErrorResponse.Errors.Count > 0)
            {
                foreach (var error in ex.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
                }
                
                var errorDeatils = new ValidationProblemDetails(ModelState);
                switch (ex.ErrorResponse.Status)
                {
                    case 400:
                        errorDeatils.Status = StatusCodes.Status400BadRequest;
                        return new BadRequestObjectResult(errorDeatils);
                    case 404:
                        errorDeatils.Status = StatusCodes.Status404NotFound;
                        return new NotFoundObjectResult(errorDeatils);
                    case 403:
                        errorDeatils.Status = StatusCodes.Status401Unauthorized;
                        return new UnauthorizedObjectResult(errorDeatils);
                    default:
                        break;
                }
            }
            return null;
        }
    }
}
