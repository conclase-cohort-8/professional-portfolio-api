using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using System;
using System.Security.Claims;

namespace ProfessionalPortfolio.Application.Services
{
    public class EducationService : IEducationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal? _user;

        public EducationService(IRepositoryManager repository,
            IHttpContextAccessor accessor, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _user = accessor.HttpContext?.User;
        }

        public async Task<ApiResult<EducationInfoDto>> AddEducation(AddEducationCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            //TODO: Return status 403, and message "You are not allowed to access this resources" if userId is Guid.Empty
            if (command == null) return new ApiResult<EducationInfoDto>("Invalid input.", 400);

            // TODO: Go to BaseEducationCommand, add validations to the properties.Institution, Degre and Course are
            // required with maximum character length of 200 each.
            // Use the custom ValidDate attribute on StartDate and EndDate properties
            var user = await _repository.User.GetByIdAsync(userId);
            if (user == null) return new ApiResult<EducationInfoDto>("User not found.", 404);

            var education = _mapper.Map<Education>(command);
            education.UserId = userId;

            await _repository.Education.AddAsync(education);
            //TODO: Use automapper to return the result. The configuration is already added
            return new ApiResult<EducationInfoDto>(new EducationInfoDto());
        }

        public ApiResult<List<EducationInfoDto>> GetEducations()
        {
            //TODO: Get the logged in user id below. See the AddEducation method above
            var userId = Guid.Empty;
            //TODO: Return status 403, and message "You are not allowed to access this resources" if userId is Guid.Empty
            var educations = _repository.Education.GetAll()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.StartDate)
                .ToList();

            //TODO: Use automapper to return the result. The configuration is already added
            return new ApiResult<List<EducationInfoDto>>(educations
                .Select(e => new EducationInfoDto()).ToList());
        }

        public Task<EducationInfoDto> UpdateEducation(UpdateEducationCommand command)
        {
            throw new NotImplementedException();
        }

        private bool IsValid(AddEducationCommand command)
        {
            return !string.IsNullOrEmpty(command.Institution) &&
                !string.IsNullOrEmpty(command.Course) &&
                !string.IsNullOrEmpty(command.Degree) &&
                (!command.EndDate.HasValue || 
                    (command.EndDate.HasValue && command.EndDate.Value > command.StartDate)
                );
        }
    }
}