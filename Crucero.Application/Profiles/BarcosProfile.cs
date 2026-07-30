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
    public class BarcoProfile : Profile
    {
        public BarcoProfile()
        {
            // Mapeo principal
            // Mapeo BarcosDTO -> Barcos
            CreateMap<BarcosDTO, Barcos>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BarcoHabitaciones, opt => opt.Ignore())
                .ForMember(dest => dest.Cruceros, opt => opt.Ignore());

            // Mapeo Barcos -> BarcosDTO (¡Este es el que faltaba!)
            CreateMap<Barcos, BarcosDTO>();

            // Mapeo para las habitaciones
            CreateMap<(int HabitacionId, int CantidadDisponible), BarcoHabitaciones>()
                .ForMember(dest => dest.HabitacionId, opt => opt.MapFrom(src => src.HabitacionId))
                .ForMember(dest => dest.CantidadDisponible, opt => opt.MapFrom(src => src.CantidadDisponible));

            // Mapeo para las habitaciones inverso (si lo necesitas)
            CreateMap<BarcoHabitaciones, BarcoHabitacionesDTO>();
        }
    }
}
