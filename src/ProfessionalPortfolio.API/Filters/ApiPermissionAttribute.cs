using Microsoft.AspNetCore.Mvc;

namespace ProfessionalPortfolio.API.Filters
{
    public class ApiPermissionAttribute : TypeFilterAttribute
    {
        public ApiPermissionAttribute() : base(typeof(ApiPermissionAttribute)) { }
    }
}