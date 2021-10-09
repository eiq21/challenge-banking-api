using AutoMapper;
using Exchange.API.Contracts.Responses;
using Exchange.Domain.Models;

namespace Exchange.API.Contracts.Mappings
{
    public class DomainToResponseProfile: Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Currency, CurrencyResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CurrencyId));

            CreateMap<ExchangeRate, ExchangeRateResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExchangeRateId));

        }
    }
}
