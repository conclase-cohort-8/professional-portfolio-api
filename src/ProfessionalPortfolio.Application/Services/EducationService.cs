using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Validations;
using ProfessionalPortfolio.Domain.Entities;
using System.Security.Claims;

namespace ProfessionalPortfolio.Application.Services
{
    public class EducationService : IEducationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal? _user;

        public EducationService(IRepositoryManager repository,
            IHttpContextAccessor accessor, 
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _user = accessor.HttpContext?.User;
        }

        public async Task<ApiResult<EducationInfoDto>> AddEducation(AddEducationCommand command)
        {
            //TODO: validate the AddEducationCommand using the EducationCommandValidator and return the appropriate response if input not valid
            // See line 79 (RegisterAsync() method) in the UserService.cs above for tips
            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<EducationInfoDto>("You are not allowed to access this resources", 403);
            }

            if (command == null) return new ApiResult<EducationInfoDto>("Invalid input.", 400);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new ApiResult<EducationInfoDto>("User not found.", 404);

            var education = _mapper.Map<Education>(command);
            education.UserId = userId;

            await _repository.Education.AddAsync(education);
            return new ApiResult<EducationInfoDto>(_mapper.Map<EducationInfoDto>(education));
        }

        public ApiResult<List<EducationInfoDto>> GetEducations()
        {
            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<List<EducationInfoDto>>("You are not allowed to access this resources", 403);
            }

            var educations = _repository.Education.GetAll()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.StartDate)
                .ToList();

            return new ApiResult<List<EducationInfoDto>>(_mapper.Map<List<EducationInfoDto>>(educations));
        }

        public async Task<ApiResult<string>> UpdateEducation(Guid id, UpdateEducationCommand command)
        {
            //TODO: validate the UpdateEducationCommand using the EducationCommandValidator and return the appropriate response if input not valid
            // See line 79 (RegisterAsync() method) in the UserService.cs above for tips

            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<string>("You are not allowed to access this resources", 403);
            }
            
            var education = await _repository.Education.GetByIdAsync(id);
            if (education == null)
            {
                return new ApiResult<string>("Record not found", 404);
            }

            if(userId != education.UserId)
            {
                return new ApiResult<string>("Access denied", 403);
            }

            _mapper.Map(command, education);
            education.UpdatedOn = DateTime.UtcNow;
            
            await _repository.Education.UpdateAsync(education);
            return new ApiResult<string>("Education record successfully updated");
        }

        public async Task<ApiResult<string>> DeleteEducation(Guid id)
        {
            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<string>("You are not allowed to access this resources", 403);
            }

            var education = await _repository.Education.GetByIdAsync(id);
            if (education == null)
            {
                return new ApiResult<string>("Record not found", 404);
            }

            if (userId != education.UserId)
            {
                return new ApiResult<string>("Access denied", 403);
            }

            await _repository.Education.Deprecate(education);
            return new ApiResult<string>("Education record successfully updated");
        }
    }
}