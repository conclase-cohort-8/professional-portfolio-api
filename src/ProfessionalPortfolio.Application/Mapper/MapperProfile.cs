using AutoMapper;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            CreateMap<AppUser, UserInfoDtoV2>()
                .ForMember(dest => dest.DateRegistered, src => src.MapFrom(u => u.CreatedOn));

            CreateMap<Skill, SkillInfoDto>().ReverseMap();
            CreateMap<UserUpdateCommand, AppUser>();

            CreateMap<BaseEducationCommand, Education>()
                .ForMember(dest => dest.FieldOfStudy, src => src.MapFrom(ed => ed.Course));
            CreateMap<Education, EducationInfoDto>()
                .ForMember(dest => dest.Date, src => src.MapFrom(ed => ed.EndDate.HasValue ?
                    string.Concat(ed.StartDate.ToString("Y"), " - ", ed.EndDate.Value.ToString("Y")) :
                    string.Concat(ed.StartDate.ToString("Y"), " - ", "Current")));

            CreateMap<BaseExperienceCommand, Experience>()
                .ForMember(dest => dest.Title, src => src.MapFrom(ex => ex.JobTitle))
                .ForMember(dest => dest.Organization, src => src.MapFrom(ex => ex.Company));
            CreateMap<Experience, ExperienceInfoDto>()
                .ForMember(dest => dest.Date, src => src.MapFrom(ex => ex.EndDate.HasValue ?
                    string.Concat(ex.StartDate.ToString("Y"), " - ", ex.EndDate.Value.ToString("Y")) :
                    string.Concat(ex.StartDate.ToString("Y"), " - ", "Current")));

            CreateMap<BaseProjectCommand, Project>();
            CreateMap<Project, ProjectInfoDto>();
        }
    }
}