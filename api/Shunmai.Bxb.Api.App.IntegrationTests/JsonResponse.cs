namespace Shunmai.Bxb.Api.App.IntegrationTests
{
    public class JsonResponse<T>
    {
        public bool success { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
