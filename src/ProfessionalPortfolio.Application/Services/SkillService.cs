using AutoMapper;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Skills.Commands;
using ProfessionalPortfolio.Application.Skills.Dtos;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SkillService(ISkillRepository skillRepository,
                            IUserRepository userRepository,
                            IMapper mapper)
        {
            _skillRepository = skillRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResult<List<SkillInfoDto>>> AddSkills(List<string> commands)
        {
            if (commands.Any(s => string.IsNullOrEmpty(s)))
                return new ApiResult<List<SkillInfoDto>>("One or more entry is empty.", 400);

            var normalizedNames = commands
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingSkills = _skillRepository.GetAsQueryable()
                .Where(s => normalizedNames.Contains(s.Name));

            var missing = normalizedNames
                .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
                .ToList();

            var newSkills = missing.Select(skill => new Skill
            {
                Name = skill
            }).ToList();

            await _skillRepository.CreateRangeAsync(newSkills);

            var skillnfo = _mapper.Map<List<SkillInfoDto>>(newSkills);
            return new ApiResult<List<SkillInfoDto>>(skillnfo);
        }

        public ApiResult<List<SkillInfoDto>> GetAllSkills()
        {
            var skills = _skillRepository.GetAsQueryable()
                .OrderBy(s => s.Name)
                .ToList();

            var skillnfo = _mapper.Map<List<SkillInfoDto>>(skills);
            return new ApiResult<List<SkillInfoDto>>(skillnfo);
        }

        public async Task<ApiResult<List<string>>> GetUserSkillsAsync(Guid userId)
        {
            var userSkills = await _skillRepository
                .GetUserSkills(userId);

            return new ApiResult<List<string>>(userSkills);
        }

        public async Task<ApiResult<string>> AddSkillsAsync(Guid userId, AddUserSkillCommand command)
        {
            if(userId == Guid.Empty || command == null)
            {
                return new ApiResult<string>("Invalid input", 400);
            }

            if(userId != command.UserId)
            {
                return new ApiResult<string>("You cannot add skill for a different user.", 403);
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if(user == null)
            {
                return new ApiResult<string>("User not found", 404);
            }

            var skill = await _skillRepository.GetByIdAsync(command.SkillId);
            if (skill == null)
            {
                return new ApiResult<string>("Skill not found", 404);
            }
            
            var userSkill = new UserSkill
            {
                UserId = user.Id,
                SkillId = skill.Id
            };

            await _skillRepository.AddUserSkill(userSkill);
            return new ApiResult<string>($"{ skill.Name } successfully added to user skills.", 200, true);
        }
    }
}
