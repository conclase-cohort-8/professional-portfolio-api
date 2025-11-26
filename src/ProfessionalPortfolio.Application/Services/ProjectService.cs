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
            if (userId == Guid.Empty)
            {
                return new ApiResult<List<ProjectInfoDto>>("Access denied", 403);
            }

            var experiences = await _repository.Project
                .GetByUserIdAsync(userId);
            return new ApiResult<List<ProjectInfoDto>>(_mapper.Map<List<ProjectInfoDto>>(experiences));
        }

        public async Task<ApiResult<string>> AddProject(AddProjectCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            // TODO: Check if the userId is an empty Guid, return 
            // Message: You're not allowed to perform this action
            //Status: 403
            if (userId == Guid.Empty)
            {
                return new ApiResult<string>("You're not allowed to perform this action", 403);
            }

            //TODO: user auto mapper to map the command to Project object
            // NOTE: the configuration is aready done.
            //IMPORTANT: Map the project.UserId to the above userId

            var project =_mapper.Map <Project>( command);
            project.UserId = userId;

            //TODO: Call the Project.AddAsync() method from the _repository to insert the record
            //Remember to await the call
             await _repository.Project.AddAsync(project);
            return new ApiResult<string>("Project record successfully added");
        }

        public async Task<ApiResult<string>> UpdateProject(Guid id, UpdateProjectCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            // TODO: Check if the userId is an empty Guid, return 
            // Message: You're not allowed to perform this action
            //Status: 403
            if(userId == Guid.Empty)
            {
                return new ApiResult<string>("You're not allowed to perform this action", 403);
            }
            var project = await _repository.Project.GetByIdAsync(id);
            // TODO return message: record not found and status: 404 if project is null
            if(project == null)

            {
                return new ApiResult<string>("record not found and status:", 404);
            }
            
            //TODO: if the userId and project.UserId are not the same, return
            // message: access denied, status: 403
            if (userId != project.UserId)
            {
                return new ApiResult<string>("access denied", 403);
            }
            _mapper.Map(command, project);
            //TODO: update the project object  UpdatedOn property and assign it to the date time utn now
            project.UpdatedOn = DateTime.UtcNow;

            //TODO: Call the Project.UpdateAsync() method from the _repository to insert the record
            //Remember to await the call
            await _repository.Project.UpdateAsync(project);
            return new ApiResult<string>("Project record successfully updated");
        }

        public Task<IActionResult> UpdateProject(IProjectService service)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<object>> UpdateProject()
        {
            throw new NotImplementedException();
        }
    }
}
