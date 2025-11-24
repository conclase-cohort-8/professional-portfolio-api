namespace ProfessionalPortfolio.Application.Common
{
    public class ApiResult<T>
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResult(string message,
                         int status,
                         bool success = false)
        {
            Status = status;
            Success = success;
            Message = message;
        }

        public ApiResult(T data,
                         bool success = true,
                         int status = 200,
                         string message = "Success")
        {
            Data = data;
            Status = status;
            Success = success;
            Message = message;
        }
    }
}