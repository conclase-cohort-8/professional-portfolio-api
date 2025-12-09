using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal? _user;

        public ProjectService(IRepositoryManager repository,
            IHttpContextAccessor httpContext, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _user = httpContext.HttpContext?.User;
        }

        public async Task<ApiResult<List<ProjectInfoDto>>> GetAllAsync()
        {
            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<List<ProjectInfoDto>>("You are not allowed to access this resources", 403);
            }

            var experiences = await _repository.Project
                .GetByUserIdAsync(userId);
            return new ApiResult<List<ProjectInfoDto>>(_mapper.Map<List<ProjectInfoDto>>(experiences));
        }

        public async Task<ApiResult<string>> AddProject(AddProjectCommand command)
        {
            //TODO: validate the AddProjectCommand using the ProjectCommandValidator and return the appropriate response if input not valid
            // See line 79 (RegisterAsync() method) in the UserService.cs above for tips

            var userId = _user.GetLoggedInUserId();
           
            if (userId == Guid.Empty)
            {
                return new ApiResult<string>("You're not allowed to perform this action", 403);
            }
            var project =_mapper.Map <Project>( command);
            project.UserId = userId;     
             await _repository.Project.AddAsync(project);
            return new ApiResult<string>("Project record successfully added");
        }

        public async Task<ApiResult<string>> UpdateProject(Guid id, UpdateProjectCommand command)
        {
            //TODO: validate the UpdateProjectCommand using the ProjectCommandValidator and return the appropriate response if input not valid
            // See line 79 (RegisterAsync() method) in the UserService.cs above for tips

            var userId = _user.GetLoggedInUserId();
            
            if(userId == Guid.Empty)
            {
                return new ApiResult<string>("You're not allowed to perform this action", 403);
            }
            var project = await _repository.Project.GetByIdAsync(id);
            
            if(project == null)

            {
                return new ApiResult<string>("record not found and status:", 404);
            }
           
            if (userId != project.UserId)
            {
                return new ApiResult<string>("access denied", 403);
            }
            _mapper.Map(command, project);
            
            project.UpdatedOn = DateTime.UtcNow;

            await _repository.Project.UpdateAsync(project);
            return new ApiResult<string>("Project record successfully updated");
        }

       
    }
}
