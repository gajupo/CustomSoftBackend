using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;


namespace Application.Handlers.CommandHandlers
{
    public class AddInvoicesCommandHandler : IRequestHandler<AddInvoicesCommand, bool>
    {
        private readonly IDBArchivoRepository dbArchivoRepository;
        private readonly IDiskArchivoRepository diskArchivoRepository;
        private readonly IProveedorRepository proveedorRepository;
        private readonly IConfiguration configuration;
        private readonly string fileRootPath;
        private readonly ILogger<AddInvoicesCommandHandler> logger;

        public AddInvoicesCommandHandler(
            IDBArchivoRepository dbArchivoRepository,
            IDiskArchivoRepository diskarchivoRepository,
            IProveedorRepository proveedorRepository,
            IConfiguration configuration,
            ILogger<AddInvoicesCommandHandler> logger)
        {

            this.proveedorRepository = proveedorRepository;

            this.dbArchivoRepository = dbArchivoRepository;

            this.diskArchivoRepository = diskarchivoRepository;

            this.configuration = configuration;
            
            this.logger = logger;

            fileRootPath = null;//configuration.GetValue<string>("FilesRootFolder");

        }
        public async Task<bool> Handle(AddInvoicesCommand request, CancellationToken cancellationToken)
        {
            if (request.files == null) throw new ValidationException("Files are required");
            if (string.IsNullOrEmpty(fileRootPath)) throw new ValidationException("No detination file root defined, please configure it in appsettings.json");

            try
            {
                var proveedor = await proveedorRepository.GetByIdAsync(request.ProveedorId, cancellationToken);
                if (proveedor == null)
                    throw new ValidationException("The give proveedorId does not exist");

                ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 3 };

                await Parallel.ForEachAsync(request.files.ToArray(), parallelOptions, async (file, CancellationToken) =>
                {

                    var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

                    if (request.DestinationFolder != null && !Directory.Exists(request.DestinationFolder))
                    {
                        Directory.CreateDirectory(Path.Combine(fileRootPath, request.DestinationFolder.Trim()));
                    }

                    var filePath = Path.Combine(fileRootPath, request.DestinationFolder?.Trim() ?? string.Empty, fileName);

                    var newArchivo = new Archivo()
                    {
                        ProveedorId = proveedor.Id,
                        Content = file,
                        Extension = Path.GetExtension(file.FileName),
                        Nombre = fileName,
                        Ruta = filePath,
                        TipoArchivo = FileType.Factura,
                        Tamano = file.Length
                    };

                    var rowsAffected = await dbArchivoRepository.SaveInvoiceFile(newArchivo, cancellationToken);

                    if (rowsAffected > 0)
                    {
                        await diskArchivoRepository.SaveInvoiceFile(newArchivo, cancellationToken);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to save the files");
                throw new InvalidOperationException("Unable to save the files", ex);
            }
        }
    }
}
