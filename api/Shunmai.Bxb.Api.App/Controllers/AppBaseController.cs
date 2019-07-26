using Microsoft.AspNetCore.Mvc;
using Shunmai.Bxb.Api.App.Constants;
using Shunmai.Bxb.Common;
using Shunmai.Bxb.Entities;

namespace Shunmai.Bxb.Api.App.Controllers
{
    [Route("[controller]")]
    public abstract class AppBaseController : BaseController
    {
        protected User CurrentUser
        {
            get
            {
                var cache = ControllerContext.HttpContext.Items[Names.USER_CACHE];
                return cache as User ?? new User();
            }
        }
    }
}
