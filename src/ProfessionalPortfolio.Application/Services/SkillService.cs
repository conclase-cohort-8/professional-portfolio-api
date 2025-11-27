using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using System.Security.Claims;

namespace ProfessionalPortfolio.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal? _user;

        public SkillService(IRepositoryManager repository,
                            IMapper mapper, IHttpContextAccessor accessor)
        {
            _repository = repository;
            _mapper = mapper;
            _user = accessor.HttpContext?.User;
        }

        public async Task<ApiResult<List<SkillInfoDto>>> AddSkills(List<string> commands)
        {
            if (commands.Any(s => string.IsNullOrEmpty(s)))
                return new ApiResult<List<SkillInfoDto>>("One or more entry is empty.", 400);

            var normalizedNames = commands
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingSkills = _repository.Skill.GetAsQueryable()
                .Where(s => normalizedNames.Contains(s.Name));

            var missing = normalizedNames
                .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
                .ToList();

            var newSkills = missing.Select(skill => new Skill
            {
                Name = skill
            }).ToList();

            await _repository.Skill.CreateRangeAsync(newSkills);

            var skillnfo = _mapper.Map<List<SkillInfoDto>>(newSkills);
            return new ApiResult<List<SkillInfoDto>>(skillnfo);
        }

        public ApiResult<List<SkillInfoDto>> GetAllSkills()
        {
            var skills = _repository.Skill.GetAsQueryable()
                .OrderBy(s => s.Name)
                .ToList();

            var skillnfo = _mapper.Map<List<SkillInfoDto>>(skills);
            return new ApiResult<List<SkillInfoDto>>(skillnfo);
        }

        public async Task<ApiResult<List<string>>> GetUserSkillsAsync()
        {
            var userId = _user.GetLoggedInUserId();
            if(userId == Guid.Empty)
            {
                return new ApiResult<List<string>>("You are not allowed to access this resources", 403);
            }

            var userSkills = await _repository.Skill
                .GetUserSkills(userId);

            return new ApiResult<List<string>>(userSkills);
        }

        public async Task<ApiResult<string>> AddSkillsAsync(AddUserSkillCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            if (userId == Guid.Empty)
            {
                return new ApiResult<string>("You are not allowed to access this resources", 403);
            }

            if (userId != command.UserId)
            {
                return new ApiResult<string>("You cannot add skill for a different user.", 403);
            }

            var user = await _repository.User.GetByIdAsync(userId);
            if(user == null)
            {
                return new ApiResult<string>("User not found", 404);
            }

            var skill = await _repository.Skill.GetByIdAsync(command.SkillId);
            if (skill == null)
            {
                return new ApiResult<string>("Skill not found", 404);
            }
            
            var userSkill = new UserSkill
            {
                UserId = user.Id,
                SkillId = skill.Id
            };

            await _repository.Skill.AddUserSkill(userSkill);
            return new ApiResult<string>($"{ skill.Name } successfully added to user skills.", 200, true);
        }
    }
}
