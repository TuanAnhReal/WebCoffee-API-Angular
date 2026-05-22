using System.Net;
using System.Text.Json;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 1. Cho phép Request đi qua các Middleware khác (Auth, Routing, Controllers...)
                await _next(context);

                // 2. BẮT CÁC MÃ LỖI 4xx (Do hệ thống trả về như 401, 403, 404...)
                if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 500 && !context.Response.HasStarted)
                {
                    await HandleStatusCodeAsync(context);
                }
            }
            catch (Exception ex)
            {
                // 3. BẮT CÁC LỖI 500 (Ngoại lệ làm crash hệ thống)
                _logger.LogError(ex, "Có lỗi hệ thống chưa được xử lý!");
                await HandleExceptionAsync(context, ex);
            }
        }

        // --- Hàm xử lý riêng cho lỗi 4xx (Xác thực, Phân quyền, Sai URL) ---
        private static async Task HandleStatusCodeAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string errorMessage = "Đã xảy ra lỗi request.";
            switch (context.Response.StatusCode)
            {
                case 401:
                    errorMessage = "Bạn chưa đăng nhập hoặc Token đã hết hạn.";
                    break;
                case 403:
                    errorMessage = "Bạn không có quyền truy cập vào chức năng này.";
                    break;
                case 404:
                    errorMessage = "Đường dẫn API không tồn tại.";
                    break;
                case 405:
                    errorMessage = "Phương thức HTTP không được hỗ trợ (VD: API yêu cầu POST nhưng bạn gọi GET).";
                    break;
                default:
                    errorMessage = $"Lỗi Client: Cú pháp không hợp lệ (Mã {context.Response.StatusCode}).";
                    break;
            }

            var apiResponse = ApiResponse<object>.ErrorResult(errorMessage);
            await WriteJsonResponseAsync(context, apiResponse);
        }

        // --- Hàm xử lý riêng cho lỗi 500 (Ngoại lệ Code) ---
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.ErrorResult("Đã có lỗi hệ thống xảy ra. Vui lòng liên hệ Admin hoặc thử lại sau.");

            await WriteJsonResponseAsync(context, response);
        }

        // --- Hàm Helper để xuất JSON chuẩn tiếng Việt ---
        private static async Task WriteJsonResponseAsync(HttpContext context, ApiResponse<object> apiResponse)
        {
            var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Fix lỗi font tiếng Việt
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}