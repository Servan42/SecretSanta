using AutoMapper;
using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.MapperProfile
{
    public class BusinessMapperProfile : Profile
    {
        public BusinessMapperProfile()
        {
            CreateMap<GiftCouple, GiftCoupleDto>()
                .ForMember(destDto => destDto.Gifter, map => map.MapFrom(src => src.Gifter))
                .ForMember(destDto => destDto.Receiver, map => map.MapFrom(src => src.Receiver))
                .ForMember(destDto => destDto.CypheredReceiver, map => map.MapFrom(src => src.CypheredReceiver));
        }
    }
}
