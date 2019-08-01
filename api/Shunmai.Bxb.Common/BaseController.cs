using Microsoft.AspNetCore.Mvc;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Utilities.Check;
using System.Net;

namespace Shunmai.Bxb.Common
{
    [ApiController]
    public class BaseController: Controller
    {
        public BaseController()
        {
            
        }

        protected JsonResult Success(object data = null)
        {
            var response = ApiResponse.OfSuccess(ErrorInfo.OfRequestSuccess(), data);
            return Json(response);
        }

        protected JsonResult Failed(string message = null)
        {
            var response = ApiResponse.OfFailed(ErrorInfo.OfRequestFailed(message));
            return Json(response);
        }

        protected JsonResult Failed(ErrorInfo errorInfo, object data = null)
        {
            Check.Null(errorInfo, nameof(errorInfo));
            var response = ApiResponse.OfFailed(errorInfo);
            return Json(response);
        }

        protected JsonResult Unautherized(string message = null)
        {
            var response = ApiResponse.OfFailed(ErrorInfo.OfUnauthorized(message));
            var result = new JsonResult(response)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return result;
        }
    }
}
