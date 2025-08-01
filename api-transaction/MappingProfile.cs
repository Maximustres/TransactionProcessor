using api_transaction.DTOs;
using api_transaction.Entities;
using api_transaction.Enums;
using AutoMapper;

namespace api_transaction
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<TransactionDetail, TransactionDetailDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TransactionStatus>(src.Status, true)));

        }

    }
}
