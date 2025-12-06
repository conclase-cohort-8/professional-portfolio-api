using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services.Interfaces;
using System.Security.Claims;

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
            // TODO: Check if the userId is an empty Guid, return 
            // Message: You're not allowed to perform this action
            //Status: 403

            //TODO: user auto mapper to map the command to Project object
            // NOTE: the configuration is aready done.
            //IMPORTANT: Map the project.UserId to the above userId

            //TODO: Call the Project.AddAsync() method from the _repository to insert the record
            //Remember to await the call
            return new ApiResult<string>("Project record successfully added");
        }

        public async Task<ApiResult<string>> UpdateProject(Guid id, UpdateProjectCommand command)
        {
            //TODO: validate the UpdateProjectCommand using the ProjectCommandValidator and return the appropriate response if input not valid
            // See line 79 (RegisterAsync() method) in the UserService.cs above for tips

            var userId = _user.GetLoggedInUserId();
            // TODO: Check if the userId is an empty Guid, return 
            // Message: You're not allowed to perform this action
            //Status: 403
            var project = await _repository.Project.GetByIdAsync(id);
            // TODO return message: record not found and status: 404 if project is null

            //TODO: if the userId and project.UserId are not the same, return
            // message: access denied, status: 403
            _mapper.Map(command, project);
            //TODO: update the project object  UpdatedOn property and assign it to the date time utn now

            //TODO: Call the Project.UpdateAsync() method from the _repository to insert the record
            //Remember to await the call
            return new ApiResult<string>("Project record successfully updated");
        }
    }
}
