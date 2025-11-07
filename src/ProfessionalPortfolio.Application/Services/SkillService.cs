using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Skills.Dtos;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUserRepository _userRepository;

        public SkillService(ISkillRepository skillRepository,
                            IUserRepository userRepository)
        {
            _skillRepository = skillRepository;
            _userRepository = userRepository;
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
            return new ApiResult<List<SkillInfoDto>>(newSkills.Select(SkillInfoDto.Map).ToList());
        }

        public ApiResult<List<SkillInfoDto>> GetAllSkills()
        {
            var skills = _skillRepository.GetAsQueryable()
                .OrderBy(s => s.Name)
                .ToList();

            return new ApiResult<List<SkillInfoDto>>(skills.Select(SkillInfoDto.Map).ToList());
        }

        public async Task<ApiResult<List<string>>> GetUserSkillsAsync(Guid userId)
        {
            var userSkills = await _skillRepository
                .GetUserSkills(userId);

            return new ApiResult<List<string>>(userSkills);
        }

        //TODO: Service Method: AddSkillsAsync
        //To add skills for a user:

        //1. If the UserId is empty or the command is missing:
        //      → Return a response with message "Invalid input" and status 400.

        //2. If the UserId does not match the UserId inside the command:
        //      → Return a response with message "You cannot add skill for a different user." and status 403.

        //3. Retrieve the user record from the user repository using the UserId received in step 1.
        //      → If the user is not found, return "User not found".

        //4. Retrieve the skill record from the skill repository using the SkillId from the command.

        //5. Create a new UserSkill record using:
        //      - Id obtained from step 3 (user record) as UserId
        //      - Id obtained from step 4 (skill record) as SkillId

        //6. Save the new UserSkill record into the skill repository by calling AddUserSkill method.

        //7. Return a success response with a message:
        //      “{ SkillName } successfully added to user skills.”
    }
}
