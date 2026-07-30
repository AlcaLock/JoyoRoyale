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
    public class DestinosProfile : Profile
    {
        public DestinosProfile()
        {
            CreateMap<DestinosDTO, Destinos>().ReverseMap();
        }
    }
}
