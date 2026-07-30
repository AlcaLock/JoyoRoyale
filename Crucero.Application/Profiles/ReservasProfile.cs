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
    public class ReservasProfile : Profile
    {
        public ReservasProfile()
        {
            CreateMap<ReservasDTO, Reservas>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // El ID lo genera la base de datos
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.CruceroId, opt => opt.MapFrom(src => src.CruceroId))
                .ForMember(dest => dest.Usuario, opt => opt.Ignore()) // No mapear el objeto completo
                .ForMember(dest => dest.Crucero, opt => opt.Ignore()) // No mapear el objeto completo
                .ForPath(dest => dest.ReservasHabitaciones, opt => opt.MapFrom(src => src.ReservasHabitaciones))
                .ForPath(dest => dest.ReservasComplementos, opt => opt.MapFrom(src => src.ReservasComplementos))
                .ForPath(dest => dest.Huespedes, opt => opt.MapFrom(src => src.Huespedes))
                .ReverseMap();

            // Mapeos para las entidades relacionadas
            CreateMap<ReservasHabitacionesDTO, ReservasHabitaciones>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Habitacion, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ReservasComplementosDTO, ReservasComplementos>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Complemento, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<HuespedesDTO, Huespedes>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
