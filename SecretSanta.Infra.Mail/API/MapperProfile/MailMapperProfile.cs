using AutoMapper;
using SecretSanta.Infra.Mail.API.DTOs;
using SecretSanta.Infra.Mail.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Mail.API.MapperProfile
{
    public class MailMapperProfile : Profile
    {
        public MailMapperProfile()
        {
            CreateMap<GiftCoupleWithEmailDto, GiftCoupleWithEmail>()
                .ForMember(dest => dest.Gifter, map => map.MapFrom(src => src.Gifter))
                .ForMember(dest => dest.Receiver, map => map.MapFrom(src => src.Receiver))
                .ForMember(dest => dest.GifterEmail, map => map.MapFrom(src => src.GifterEmail));
        }
    }
}
