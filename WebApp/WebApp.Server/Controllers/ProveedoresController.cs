using Microsoft.AspNetCore.Mvc;
using WebApp.Server.Data;
using WebApp.Server.DTOs;

namespace WebApp.Server.Controllers
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
        public async Task<IActionResult> Index()
        {
            var shirtList = await _webApiExecuter.InvokeGet<List<ProveedorDto>>("proveedores");

            return Ok(shirtList);
        }
    }
}
