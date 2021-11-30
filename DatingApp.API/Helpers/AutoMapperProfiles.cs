using System.Linq;
using AutoMapper;
using DatingApp.API.Data.DTO;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDTO>()
            .ForMember( N => N.PhotoUrl,
             opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x=> x.IsMain).Url));
            //  .ForMember(x=> x.Age, opt=> opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDTO>();

            CreateMap<MemberUpdateDTO,AppUser>();
            CreateMap<RegisterDTO,AppUser>();

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl
                , opt => opt.MapFrom(
                     src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl
                , opt => opt.MapFrom(
                     src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}