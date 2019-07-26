using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.Admin.Models;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("admin/userinfo")]

    public class UserController : AdminBaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;


        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("GetUserList")]
        public IActionResult GetUserList([FromQuery] UserQuery query)
        {
            var (total, list) = _userService.QueryPage(query);
            var data = new ListResponse<UserResponse>
            {
                List = list.MapToList<UserResponse>(),
                Total = total
            };
            return Success(data);
        }

        [HttpGet("userdetail")]
        [SkipLoginVerification]
        public IActionResult GetUserDetail(int userId)
        {
            var res = _userService.QueryUserDetail(userId);
            if (res == null)
            {
                return Failed();
            }
            return Success(res);
        }

        [HttpGet("GetUserLogList")]
        public IActionResult GetUserLogList([FromQuery]UserLogQuery query)
        {
            var (total, list) = _userService.QueryUserLogPage(query);
            var data = new Models.ListResponse<UserLog>
            {
                List = list.MapToList<UserLog>(),
                Total = total
            };
            return Success(data);
        }

    }
}