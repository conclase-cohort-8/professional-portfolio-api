using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Users.Dtos
{
    public record UserInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }

        public UserInfoDto(AppUser user)
        {
            var middleInitial = !string.IsNullOrWhiteSpace(user.OtherName) ?
                string.Concat(" ", user.OtherName.First(), ".") :
                string.Empty;

            Id = user.Id;
            Name = string.Concat(user.FirstName, middleInitial, " ", user.LastName);
            Email = user.Email;
            RegistrationDate = user.CreatedOn;
            Status = user.Status;
            Role = user.Role;
        }
    }
}