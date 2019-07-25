using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Utilities.Validation;
using System;
using System.Collections.Generic;

namespace Shunmai.Bxb.Services
{
    public class UserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public UserService(ILogger<UserService> logger
            , IUserRepository userRepository
        )
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public User FindById(int userId)
        {
            return _userRepository.FindById(userId);
        }

        public User FindByOpenId(string openId)
        {
            return _userRepository.FindByOpenId(openId);
        }

        public bool AddUser(WechatUserInfo wechatUser, out User user)
        {
            throw new NotImplementedException();
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
    }
}
