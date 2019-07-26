using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Utilities.Check;
using System.Collections.Generic;

namespace Shunmai.Bxb.Services
{
    public class UserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserLogRepository _userLogRepository;

        public UserService(ILogger<UserService> logger
            , IUserRepository userRepository
            , IUserLogRepository userLogRepository
        )
        {
            _logger = logger;
            _userRepository = userRepository;
            _userLogRepository = userLogRepository;
        }

        public User FindById(int userId)
        {
            return _userRepository.FindById(userId);
        }

        [SmartSqlTransaction]
        public virtual bool AddUser(User user, out string message)
        {
            Check.Null(user, nameof(user));

            var exists = _userRepository.ExistsByPhone(user.Phone);
            if (exists)
            {
                message = "此账号已存在";
                return false;
            }
            exists = _userRepository.ExistsByOpenId(user.WxOpenId);
            if (exists)
            {
                message = "此微信已注册";
                return false;
            }

            var userId = _userRepository.Insert(user);
            if (userId <= 0)
            {
                message = "注册失败";
                return false;
            }

            var userLog = new UserLog
            {
                UserId = userId,
                LogContent = "注册账号",
                LogContentFront = "注册账号",
                LogType = UserLogType.Register,
                Operator = userId.ToString(),
            };
            var logId = _userLogRepository.Insert(userLog);
            if (logId <= 0)
            {
                message = "注册失败";
                return false;
            }

            message = "注册成功";
            return true;
        }

        [SmartSqlTransaction]
        public virtual bool Login(string phone, out User user, out string message)
        {
            user = _userRepository.FindByPhone(phone);
            if (user == null)
            {
                message = "用户不存在";
                return false;
            }

            var userLog = new UserLog
            {
                LogContent = "用户登录",
                LogContentFront = "用户登录",
                LogType = UserLogType.Login,
                Operator = user.UserId.ToString(),
                UserId = user.UserId,
            };
            var logId = _userLogRepository.Insert(userLog);
            if (logId <= 0)
            {
                _logger.LogError($"Add user log failed.");
                message = "登录失败";
                return false;
            }

            message = "登录成功";
            return true;
        }

        public bool UpdateWalletAddress(object condition)
        {
            return _userRepository.UpdateWalletAddress(condition);
        }

        public List<User> QueryList(object condition)
        {
            Check.Null(condition, nameof(condition));
            return _userRepository.QueryList(condition);
        }

        public int Count(object condition)
        {
            Check.Null(condition, nameof(condition));
            return _userRepository.Count(condition);
        }

        public (int Total, List<User> List) QueryPage(object condition)
        {
            Check.Null(condition, nameof(condition));
            var count = Count(condition);
            var list = QueryList(condition);
            return (count, list);
        }

        public User QueryUserDetail(int userId)
        {
            return _userRepository.QueryUserDetail(userId);
        }




        public int InsertUserLog(UserLog userlog)
        {
            return _userLogRepository.Insert(userlog);
        }

        public List<UserLog> QueryUserLogList(object condition)
        {
            Check.Null(condition, nameof(condition));
            return _userLogRepository.QueryList(condition);
        }

        public int GetUserLogCount(object condition)
        {
            Check.Null(condition, nameof(condition));
            return _userLogRepository.Count(condition);
        }

        public (int Total, List<UserLog> List) QueryUserLogPage(object condition)
        {
            Check.Null(condition, nameof(condition));
            var count = GetUserLogCount(condition);
            var list = QueryUserLogList(condition);
            return (count, list);
        }

        public UserLog QueryUserLogSigle(int userId)
        {
            return _userLogRepository.QuerySigle(userId);
        }

    }
}
