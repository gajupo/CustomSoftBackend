using Application.Commands;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using FluentResults;
using Infrastructure.Repositories.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.ComponentModel.DataAnnotations;


namespace Application.Handlers.CommandHandlers
{
    public class AddInvoicesCommandHandler : IRequestHandler<AddInvoicesCommand, Result<bool>>
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

            fileRootPath = configuration.GetValue<string>("FilesRootFolder");

        }
        public async Task<Result<bool>> Handle(AddInvoicesCommand request, CancellationToken cancellationToken)
        {
           
            if (request.files == null) return Result.Fail(new BadRequestError($"Files are required"));
            if (string.IsNullOrEmpty(fileRootPath)) return Result.Fail(new BadRequestError($"No destination file root defined, please configure it in appsettings.json"));

            try
            {
                var proveedor = await proveedorRepository.GetByIdAsync(request.ProveedorId, cancellationToken);
                if (proveedor == null) return Result.Fail(new NotFoundError($"The given id = ${request.ProveedorId} was not found"));


                ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 3 };

                await Parallel.ForEachAsync(request.files.ToArray(), parallelOptions, async (file, CancellationToken) =>
                {

                    //var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

                    if (request.DestinationFolder != null && !Directory.Exists(request.DestinationFolder))
                    {
                        Directory.CreateDirectory(Path.Combine(fileRootPath, request.DestinationFolder.Trim()));
                    }

                    var filePath = Path.Combine(fileRootPath, request.DestinationFolder?.Trim() ?? string.Empty, file.FileName);

                    var newArchivo = new Archivo()
                    {
                        ProveedorId = proveedor.Id,
                        Content = file,
                        Extension = Path.GetExtension(file.FileName),
                        Nombre = file.FileName,
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

                return Result.Ok(true);
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "Something bad happened when working with the  DB");
                return Result.Fail(new DatabaseError("Something bad happened when working with the  DB")
                    .CausedBy(ex));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UnExpected error please review the log for more details");
                return Result.Fail(new UnExpectedError("UnExpected error please review the log for more details")
                    .CausedBy(ex));
            }
        }
    }
}
