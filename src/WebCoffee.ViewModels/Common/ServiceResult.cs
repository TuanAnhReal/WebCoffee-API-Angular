using System.Collections.Generic;

namespace WebCoffee.ViewModels.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
    }
}