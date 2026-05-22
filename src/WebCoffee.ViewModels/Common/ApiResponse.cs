namespace WebCoffee.ViewModels.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse SuccessResult(string message = "Thành công", int statusCode = 200)
        {
            return new ApiResponse { Success = true, Message = message, StatusCode = statusCode };
        }
        public static ApiResponse ErrorResult(string message, int statusCode = 400)
        {
            return new ApiResponse { Success = false, Message = message, StatusCode = statusCode };
        }
    }
    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
        public static ApiResponse<T> SuccessResult(T data, string message = "Thành công", int statusCode = 200)
        {
            return new ApiResponse<T> { Success = true, Message = message, StatusCode = statusCode, Data = data };
        }
        public static new ApiResponse<T> ErrorResult(string message, int statusCode = 400)
        {
            return new ApiResponse<T> { Success = false, Message = message, StatusCode = statusCode, Data = default };
        }
    }
}