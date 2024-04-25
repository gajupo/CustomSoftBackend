using Domain.Entities;
using System.Data;
using AutoMapper;

namespace Application.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<DataRow, Proveedor>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src["Id"]))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src["Nombre"]))
                .ForMember(dest => dest.FechaAlta, opt => opt.MapFrom(src => src["FechaAlta"]))
                .ForMember(dest => dest.RFC, opt => opt.MapFrom(src => src["RFC"]))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src["Direccion"]))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src["Activo"]))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src["FechaCreacion"]))
                .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => src["FechaModificacion"]));
        }
    }
}
