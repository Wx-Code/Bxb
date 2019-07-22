using Microsoft.AspNetCore.Mvc;
using Shunmai.Bxb.Common;
using Shunmai.Bxb.Common.Constants;
using Shunmai.Bxb.Entities;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("[controller]")]
    public abstract class AdminBaseController : BaseController
    {
        protected AdminUser LogonUser
        {
            get
            {
                var cache = ControllerContext.HttpContext.Items;
                var name = Names.CURRENT_LOGON_USER_CACHE_NAME;
                if (cache.TryGetValue(name, out object user))
                {
                    return user as AdminUser;
                }
                return null;
            }
        }
    }
}
