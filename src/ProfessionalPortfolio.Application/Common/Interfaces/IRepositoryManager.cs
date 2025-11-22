namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IRepositoryManager
    {
        IEducationRepository Education {  get; }
        IExperienceRepository Experience { get; }
        ISkillRepository Skill { get; }
        IProjectRepository Project { get; }
        IUserRepository User { get; }
        Task SaveAsync();
    }
}