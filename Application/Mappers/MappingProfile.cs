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

            CreateMap<Proveedor, ProveedorDto>();
            CreateMap<ProveedorDto, Proveedor>();

            CreateMap<CreateProveedorCommand, Proveedor>();
            CreateMap<ProveedorDto, CreateProveedorCommand>();

            CreateMap<UpdateProveedorCommand, Proveedor>();
            CreateMap<ProveedorDto, UpdateProveedorCommand>();

            CreateMap<InvoicesDto, AddInvoicesCommand>();
        }
    }
}
