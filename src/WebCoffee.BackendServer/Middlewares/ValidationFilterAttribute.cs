using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Middlewares
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Nếu dữ liệu đầu vào không vượt qua được FluentValidation
            if (!context.ModelState.IsValid)
            {
                // Gom tất cả các câu thông báo lỗi lại thành 1 chuỗi
                var errors = context.ModelState.Values.SelectMany(v => v.Errors)
                                                      .Select(e => e.ErrorMessage)
                                                      .ToList();

                var errorMessage = string.Join(" ", errors);

                // Trả về chuẩn ApiResponse
                var response = ApiResponse<object>.ErrorResult(errorMessage);

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}