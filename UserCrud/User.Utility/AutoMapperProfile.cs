using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.DTOs;
using User.Model;

namespace User.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserInformation, UserInformationDTO>()
                 .ForMember(destination =>
                    destination.IsActive,
                    options => options.MapFrom(origin => origin.IsActive == true ? 1 : 0)
                );

            CreateMap<UserInformationDTO, UserInformation>()
               .ForMember(destination =>
                   destination.IsActive,
                   options => options.MapFrom(origin => origin.IsActive == 1 ? true : false)
               );

            CreateMap<UserInformation, TokenAuthorizationRequestDTO>().ReverseMap();
            CreateMap<UserInformation, TokenAuthorizationResponseDTO>().ReverseMap();
            CreateMap<UserInformation, RefreshTokenHistory>().ReverseMap();

        }

    }

}
