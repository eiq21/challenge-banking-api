using AutoMapper;
using Identity.API.Contracts.Responses;
using Identity.Domain.Models;

namespace Identity.API.Contracts.Mappings
{
    public class DomainToResponseProfile: Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

            CreateMap<User, UserResponse>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

        }
    }
}
