using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Models.Wechat;
using System;

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
    }
}
