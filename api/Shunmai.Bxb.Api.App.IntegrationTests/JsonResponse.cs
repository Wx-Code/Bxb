namespace Shunmai.Bxb.Api.App.IntegrationTests
{
    public class JsonResponse
    {
        public bool success { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
