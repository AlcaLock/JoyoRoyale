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
    public class BarcoHabitacionesProfile : Profile
    {
        public BarcoHabitacionesProfile()
        {
            CreateMap<BarcoHabitacionesDTO, BarcoHabitaciones>().ReverseMap();

            // Mapeo de BarcoHabitaciones a BarcoHabitacionesDTO y viceversa
            CreateMap<BarcoHabitaciones, BarcoHabitacionesDTO>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.BarcoId, orig => orig.MapFrom(o => o.BarcoId))
                .ForMember(dest => dest.HabitacionId, orig => orig.MapFrom(o => o.HabitacionId))
                .ForMember(dest => dest.CantidadDisponible, orig => orig.MapFrom(o => o.CantidadDisponible))
                .ForMember(dest => dest.Barco, orig => orig.MapFrom(o => o.Barco))
                .ForMember(dest => dest.Habitacion, orig => orig.MapFrom(o => o.Habitacion));
        }
    
    }
}
