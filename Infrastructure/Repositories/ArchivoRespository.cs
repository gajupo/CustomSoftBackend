using Domain.Core;
using Infrastructure.Repositories.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories
{
    public class ArchivoRespository : IArchivoRepository
    {
        private readonly ILogger _logger;

        public ArchivoRespository(ILogger<ArchivoRespository> logger)
        {
            _logger = logger;
        }
        public async Task SaveInvoiceFile(IFormFile file, string filePath, CancellationToken cancellationToken)
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream,cancellationToken);
            _logger.LogInformation($"File saved successfully, file name= {file.Name}");
        }
    }
}
