using AutoMapper;
using Crucero.Application.DTOs;
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Profiles
{
    public class CrucerosProfile : Profile
    {
        public CrucerosProfile()
        {
            // Mapeo básico con ReverseMap()
            CreateMap<CrucerosDTO, Cruceros>().ReverseMap();
            CreateMap<Cruceros, CrucerosDTO>()
    .ForMember(dest => dest.Barco, orig => orig.MapFrom(o => o.Barco))
    .ReverseMap();


            // Mapeo detallado de propiedades
            CreateMap<CrucerosDTO, Cruceros>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.Nombre, orig => orig.MapFrom(o => o.Nombre))
                .ForMember(dest => dest.Descripcion, orig => orig.MapFrom(o => o.Descripcion))
                .ForMember(dest => dest.Imagen, orig => orig.MapFrom(o => o.Imagen))
                .ForMember(dest => dest.Dias, orig => orig.MapFrom(o => o.Dias))
                .ForMember(dest => dest.BarcoId, orig => orig.MapFrom(o => o.BarcoId))
                .ForMember(dest => dest.Barco, orig => orig.MapFrom(o => o.Barco))
                .ForMember(dest => dest.FechasCruceros, orig => orig.MapFrom(o => o.FechasCruceros))
                .ForMember(dest => dest.Itinerarios, orig => orig.MapFrom(o => o.Itinerarios))
                .ForMember(dest => dest.Reservas, orig => orig.MapFrom(o => o.Reservas));
        }
    }
}

