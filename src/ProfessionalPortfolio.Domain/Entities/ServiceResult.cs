using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult Sucess (string message ,int statusCode = 200)
        {
            return new ServiceResult { Success = true , Message = message , StatusCode = statusCode}; 

        }

        public static ServiceResult Failure(string message , int statusCode)
        {
            return new ServiceResult { Success = false, Message = message , StatusCode = statusCode };
        }
    }
}
