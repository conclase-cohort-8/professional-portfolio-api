using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Educations.Dtos
{
    public class EducationInfoDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string Course { get; set; }
        public string Date { get; set; }

        public EducationInfoDto(Education education)
        {
            Id = education.Id;
            UserId = education.UserId;
            Institution = education.Institution;
            Degree = education.Degree;
            Course = education.FieldOfStudy;
            Date = education.EndDate.HasValue ?
                $"{education.StartDate:Y} - {education.EndDate.Value:Y}" :
                $"{education.StartDate:Y} - Current";
        }
    }
}