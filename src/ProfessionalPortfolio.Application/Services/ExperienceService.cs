using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services.Interfaces;
using System.Security.Claims;

namespace ProfessionalPortfolio.Application.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal? _user;

        public ExperienceService(IRepositoryManager repository,
            IHttpContextAccessor httpContext, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _user = httpContext.HttpContext?.User;
        }

        public async Task<ApiResult<List<ExperienceInfoDto>>> GetAllAsync()
        {
            var userId = _user.GetLoggedInUserId();
            if(userId == Guid.Empty)
            {
                return new ApiResult<List<ExperienceInfoDto>>("Access denied", 403);
            }

            var experiences = await _repository.Experience
                .GetByUserIdAsync(userId);
            return new ApiResult<List<ExperienceInfoDto>>(_mapper.Map<List<ExperienceInfoDto>>(experiences));
        }

        public async Task<ApiResult<string>> AddExperience(AddExperienceCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            // TODO: Check if the userId is an empty Guid, return 
            // Message: You're not allowed to perform this action
            //Status: 403

            //TODO: user auto mapper to map the command to Experience object
            // NOTE: the configuration is aready done.
            //IMPORTANT: Map the project.UserId to the above userId

            //TODO: Call the Experience.AddAsync() method from the _repository to insert the record
            //Remember to await the call
            return new ApiResult<string>("Experience record successfully added");
        }

        public async Task<ApiResult<string>> UpdateExperience(Guid id, UpdateExperienceCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            // TODO: Check if the userId is an empty Guid, return 
            // Message: You're not allowed to perform this action
            //Status: 403
            var experience = await _repository.Experience.GetByIdAsync(id);
            // TODO return message: record not found and status: 404 if experience is null

            //TODO: if the userId and experience.UserId are not the same, return
            // message: access denied, status: 403
            _mapper.Map(command, experience);
            //TODO: update the experience object  UpdatedOn property and assign it to the date time utn now

            //TODO: Call the Experience.UpdateAsync() method from the _repository to insert the record
            //Remember to await the call
            return new ApiResult<string>("Experience record successfully added");
        }
    }
}