using Domain.Entities;
using System.Data;
using AutoMapper;
using Application.DTOs;
using Application.Commands;

namespace Application.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            CreateMap<DataRow, Proveedor>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src["id"]))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src["nombre"]))
            .ForMember(dest => dest.FechaAlta, opt => opt.MapFrom(src => src["fechaalta"]))
            .ForMember(dest => dest.RFC, opt => opt.MapFrom(src => src["rfc"]))
            .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src["direccion"]))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src["activo"]))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src["fechacreacion"]));

            CreateMap<DataRow, Archivo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src["id"]))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src["nombre"]))
            .ForMember(dest => dest.TipoArchivo, opt => opt.MapFrom(src => src["tipoarchivo"].ToString()))
            .ForMember(dest => dest.Ruta, opt => opt.MapFrom(src => src["ruta"]))
            .ForMember(dest => dest.Tamano, opt => opt.MapFrom(src => src["tamano"]))
            .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => src["extension"]))
            .ForMember(dest => dest.ProveedorId, opt => opt.MapFrom(src => src["proveedorid"]));

            CreateMap<Proveedor, ProveedorDto>();
            CreateMap<ProveedorDto, Proveedor>();

            CreateMap<CreateProveedorCommand, Proveedor>();
            CreateMap<ProveedorDto, CreateProveedorCommand>();

            CreateMap<UpdateProveedorCommand, Proveedor>();
            CreateMap<UpdateProveedorDto, UpdateProveedorCommand>();

            CreateMap<InvoicesDto, AddInvoicesCommand>();
        }
    }
}
