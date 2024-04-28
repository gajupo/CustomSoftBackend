using Application.Commands;
using AutoMapper;
using Infrastructure.Repositories.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;


namespace Application.Handlers.CommandHandlers
{
    public class AddInvoicesCommandHandler : IRequestHandler<AddInvoicesCommand, bool>
    {
        private readonly IArchivoRepository archivoRepository;
        private readonly IMapper mapper;
        private readonly IProveedorRepository proveedorRepository;
        private readonly IConfiguration configuration;
        private readonly string fileRootPath;

        public AddInvoicesCommandHandler(
            IArchivoRepository archivoRepository,
            IMapper mapper,
            IProveedorRepository proveedorRepository,
            IConfiguration configuration)
        {
            this.mapper = mapper;

            this.proveedorRepository = proveedorRepository;

            this.archivoRepository = archivoRepository;

            this.configuration = configuration;

            fileRootPath = configuration.GetValue<string>("FilesRootFolder");

        }
        public async Task<bool> Handle(AddInvoicesCommand request, CancellationToken cancellationToken)
        {
            if (request.files == null) throw new ValidationException("Files are required");
            if (string.IsNullOrEmpty(fileRootPath)) throw new ValidationException("No detination file root defined, please configure it in appsettings.json");
            

            // var proveedor = await proveedorRepository.GetByIdAsync(request.ProveedorId, cancellationToken);

            ParallelOptions parallelOptions = new ParallelOptions() {  MaxDegreeOfParallelism = 3 };

            await Parallel.ForEachAsync(request.files.ToArray(), parallelOptions, async (file, CancellationToken) =>
            {
               
                var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

                if(request.DestinationFolder != null && !Directory.Exists(request.DestinationFolder))
                {
                    Directory.CreateDirectory(Path.Combine(fileRootPath, request.DestinationFolder.Trim()));
                }

                var filePath = Path.Combine(fileRootPath, request.DestinationFolder?.Trim() ?? string.Empty, fileName);

                
                await archivoRepository.SaveInvoiceFile(file, filePath, cancellationToken);

            });

            return true;

        }
    }
}
