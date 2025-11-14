using AutoMapper;
using ProfessionalPortfolio.Application.Skills.Dtos;
using ProfessionalPortfolio.Application.Users.Commands;
using ProfessionalPortfolio.Application.Users.Dtos;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterUserCommand, AppUser>()
                .ForMember(dest => dest.Email, src => src.MapFrom(r => r.EmailAddress));

            CreateMap<AppUser, UserInfoDto>()
                .ForMember(dest => dest.RegistrationDate, src => src.MapFrom(u => u.CreatedOn))
                .ForMember(dest => dest.Name, src => src.MapFrom(u => !string.IsNullOrEmpty(u.OtherName) ?
                    string.Concat(u.FirstName, " ", u.OtherName.First(), ".", " ", u.LastName) : 
                    string.Concat(u.FirstName, " ", u.LastName)));

            CreateMap<Skill, SkillInfoDto>().ReverseMap();
            CreateMap<UserUpdateCommand, AppUser>();
        }
    }
}