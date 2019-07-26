namespace Shunmai.Bxb.Common.Models
{
    public class ApiResponse
    {
        public bool Success { get; private set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        private ApiResponse() { }

        public static ApiResponse OfSuccess(ErrorInfo errorInfo, object data = null)
        {
            return new ApiResponse
            {
                Success = true,
                ErrorCode = errorInfo.Code,
                Message = errorInfo.Message,
                Data = data,
            };
        }

        public static ApiResponse OfFailed(ErrorInfo errorInfo = null, object data = null)
        {
            var info = errorInfo;
            return new ApiResponse
            {
                Success = false,
                ErrorCode = info.Code,
                Message = info.Message,
                Data = data,
            };
        }
    }
}
