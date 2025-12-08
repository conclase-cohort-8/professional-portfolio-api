using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using System.Security.Claims;

namespace ProfessionalPortfolio.Application.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal? _user;

        public ExperienceService(IRepositoryManager repository,
            IHttpContextAccessor httpContext, 
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _user = httpContext.HttpContext?.User;
        }

        public async Task<ApiResult<List<ExperienceInfoDto>>> GetAllAsync()
        {
            var userId = _user.GetLoggedInUserId();
            if(!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<List<ExperienceInfoDto>>("Access denied", 403);
            }

            var experiences = await _repository.Experience
                .GetByUserIdAsync(userId);
            return new ApiResult<List<ExperienceInfoDto>>(_mapper.Map<List<ExperienceInfoDto>>(experiences));
        }

        public async Task<ApiResult<ExperienceInfoDto>> GetByIdAsync(Guid id)
        {
            var experiences = await _repository.Experience
                .GetByIdAsync(id);
            if(experiences == null)
            {
                return new ApiResult<ExperienceInfoDto>("Record not found", 404);
            }
            return new ApiResult<ExperienceInfoDto>(_mapper.Map<ExperienceInfoDto>(experiences));
        }

        public async Task<ApiResult<string>> AddAsync(AddExperienceCommand command)
        {
            //TODO: validate the AddExperienceCommand using the ExperienceCommandValidator and return the appropriate response if input not valid
            // See line 79 (RegisterAsync() method) in the UserService.cs above for tips

            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<string>("You are not allowed to access this resources", 403);
            }

            if (command == null) return new ApiResult<string>("Invalid input.", 400);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new ApiResult<string>("User not found.", 404);

            var education = _mapper.Map<Experience>(command);
            education.UserId = userId;

            await _repository.Experience.AddAsync(education);
            return new ApiResult<string>("Experience record successfully added");
        }

        public async Task<ApiResult<string>> UpdateAsync(Guid id, UpdateExperienceCommand command)
        {
            //TODO: validate the UpdateExperienceCommand using the ExperienceCommandValidator and return the appropriate response if input not valid
            // See line 79 (RegisterAsync() method) in the UserService.cs above for tips

            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<string>("You are not allowed to access this resources", 403);
            }

            var experience = await _repository.Experience.GetByIdAsync(id);
            if (experience == null)
            {
                return new ApiResult<string>("Record not found", 404);
            }

            if (userId != experience.UserId)
            {
                return new ApiResult<string>("Access denied", 403);
            }

            _mapper.Map(command, experience);
            experience.UpdatedOn = DateTime.UtcNow;

            await _repository.Experience.UpdateAsync(experience);
            return new ApiResult<string>("Experience record successfully added");
        }

        public async Task<ApiResult<string>> DeleteAsync(Guid id)
        {
            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<string>("You are not allowed to access this resources", 403);
            }

            var experience = await _repository.Experience.GetByIdAsync(id);
            if (experience == null)
            {
                return new ApiResult<string>("Record not found", 404);
            }

            if (userId != experience.UserId)
            {
                return new ApiResult<string>("Access denied", 403);
            }

            await _repository.Experience.Deprecate(experience);
            return new ApiResult<string>("Experience record successfully updated");
        }
    }
}