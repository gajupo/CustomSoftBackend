using Domain.Entities;
using Infrastructure.Config;
using Infrastructure.Repositories.Core;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories
{
    public class DiskArchivoRepository : IDiskArchivoRepository
    {
        private readonly ILogger _logger;

        public DiskArchivoRepository(ILogger<DiskArchivoRepository> logger)
        {
            _logger = logger;
        }

        public async Task<int> SaveInvoiceFile(Archivo archivo, CancellationToken cancellationToken)
        {
            using var stream = new FileStream(archivo.Ruta, FileMode.Create);
            await archivo.Content.CopyToAsync(stream,cancellationToken);
            _logger.LogInformation($"File saved successfully, file name= {archivo.Nombre}");

            return 1;
        }
    }
}
