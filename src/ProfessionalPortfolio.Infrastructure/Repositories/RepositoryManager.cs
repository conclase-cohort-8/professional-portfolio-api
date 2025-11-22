using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly SqlServerDbContext _dbContext;
        private readonly Lazy<IEducationRepository> _educationRepository;
        private readonly Lazy<IExperienceRepository> _experienceRepository;
        private readonly Lazy<ISkillRepository> _skillRepository;
        private readonly Lazy<IProjectRepository> _projectRepository;
        private readonly Lazy<IUserRepository> _userRepository;

        public RepositoryManager(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
            _educationRepository = new Lazy<IEducationRepository>(() =>
                new EducationRepository(dbContext));
            _experienceRepository = new Lazy<IExperienceRepository>(() => 
                new ExperienceRepository(dbContext));
            _skillRepository = new Lazy<ISkillRepository>(() =>
                new SkillRepository(dbContext));
            _projectRepository = new Lazy<IProjectRepository>(() =>
                new ProjectRepository(dbContext));
            _userRepository = new Lazy<IUserRepository>(() =>
                new UserRepository(dbContext));
        }

        public IEducationRepository Education => _educationRepository.Value;
        public IExperienceRepository Experience => _experienceRepository.Value;
        public ISkillRepository Skill => _skillRepository.Value;
        public IProjectRepository Project => _projectRepository.Value;
        public IUserRepository User => _userRepository.Value;

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
