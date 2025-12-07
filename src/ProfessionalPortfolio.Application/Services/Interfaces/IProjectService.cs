using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResult<string>> AddProject(AddProjectCommand command);
        Task<ApiResult<List<ProjectInfoDto>>> GetAllAsync();
        Task<ApiResult<string>> UpdateProject(Guid id, UpdateProjectCommand command);
    }
}
