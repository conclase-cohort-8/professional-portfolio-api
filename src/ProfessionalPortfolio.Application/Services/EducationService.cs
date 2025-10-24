using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Educations.Commands;
using ProfessionalPortfolio.Application.Educations.Dtos;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Services
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRepository _repository;
        private readonly IUserRepository _userRepository;

        public EducationService(IEducationRepository repository,
                                IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<ApiResult<EducationInfoDto>> AddEducation(Guid userId, 
                                                          AddEducationCommand command)
        {
            // ---------------------------------------------
            // Step 1: Validate inputs
            // ---------------------------------------------
            // Before performing any database operations, we ensure the inputs are not empty or invalid.
            // This prevents unnecessary database calls and guards against bad requests.
            // If either the userId is empty or the command object is null,
            // we immediately return an error response with a 400 (Bad Request) status code.
            // TODO: command == null || userId == Guid.Empty return new ApiResult<EducationInfoDto>("Invalid input.", 400);

            // ---------------------------------------------
            // Step 2: Validate required fields inside the command
            // ---------------------------------------------
            // The IsValid() method checks that all necessary fields in the command are provided.
            // For example, Institution, Degree, and Course must not be empty.
            // If validation fails, another 400 response is returned with an explanatory message.
            // TODO: if !IsValid(command) return new ApiResult<EducationInfoDto>("One or more required fields are missing.", 400);

            // ---------------------------------------------
            // Step 3: Retrieve the user from the database
            // ---------------------------------------------
            // We now confirm that the user actually exists before assigning education records to them.
            // If no user is found for the provided ID, we return a 404 (Not Found) response.
            var user = await _userRepository.GetByIdAsync(userId);
            // TODO: if user == null return new ApiResult<EducationInfoDto>("User not found.", 404);

            // ---------------------------------------------
            // Step 4: Create and populate a new Education entity
            // ---------------------------------------------
            // This is where we map data from the AddEducationCommand into the Education model.
            // Note that "Course" from the command is assigned to "FieldOfStudy" in the entity.
            // The entity will later be saved to the database.
            //TODO: Map the below fields
            var education = new Education
            {
                //UserId
                //Institution
                //FieldOfStudy =  maps to command.Course
                //Degree 
                //StartDate
                //EndDate
            };

            // ---------------------------------------------
            // Step 5: Save the education record
            // ---------------------------------------------
            // The repository handles persistence (e.g., saving to a database).
            // This call is asynchronous to prevent blocking the main thread.
            // TODO: call the AddAsync() method from _repository and pass in the above education object
            // TODO: you should await this call

            // ---------------------------------------------
            // Step 6: Return a successful result
            // ---------------------------------------------
            // Finally, we wrap the newly created education info in a standardized ApiResult
            // object. This makes sure every API response follows the same structure.
            return new ApiResult<EducationInfoDto>(new EducationInfoDto(education));
        }

        public ApiResult<List<EducationInfoDto>> GetEducations(Guid userId)
        {
            var educations = _repository.GetAll()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.StartDate)
                .ToList();

            return new ApiResult<List<EducationInfoDto>>(educations
                .Select(e => new EducationInfoDto(e)).ToList());
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