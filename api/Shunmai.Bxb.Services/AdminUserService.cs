using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Utilities.Helpers;
using Shunmai.Bxb.Utilities.Validation;
using System;
using System.Collections.Generic;

namespace Shunmai.Bxb.Services
{
    public class AdminUserService
    {
        private readonly IAdminUserRepository _userRepository;

        public AdminUserService(IAdminUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private string GenerateSalt()
        {
            return Guid.NewGuid().ToString("N");
        }

        public string EncryptPwd(string username, string pwd, string salt)
        {
            return Encrypt.Md5By32($"{salt}:{username}:{salt}:{pwd}:{salt}");
        }

        /// <summary>
        /// 添加用户，成功后返回新增的 ID
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="message">当用户添加失败时，通过此参数获取失败原因</param>
        /// <returns>新增数据的 ID</returns>
        public int AddUser(AdminUser user, out string message)
        {
            Check.Null(user, nameof(user));

            if (_userRepository.Exists(user.Username))
            {
                message = "该用户名已存在";
                return 0;
            }

            user.Salt = GenerateSalt();
            user.Password = EncryptPwd(user.Username, user.Password, user.Salt);
            user.CreatedTime = DateTime.Now;

            var id = _userRepository.Insert(user);
            message = id > 0 ? "用户添加成功" : "用户添加失败";

            return id;
        }

        public bool Update(AdminUser user, out string message)
        {
            Check.Null(user, nameof(user));

            message = "";
            if (_userRepository.Exists(user.Username, user.AdminUserId))
            {
                message = "该用户名已存在";
                return false;
            }

            return _userRepository.Update(user);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">待修改用户 ID</param>
        /// <param name="newPwd">新密码</param>
        /// <returns>修改成功返回<c>true</c>，否则返回<c>false</c></returns>
        public bool UpdatePassword(int userId, string newPwd, string oldPwd = null)
        {
            Check.EnsureGreaterThanZero(userId, nameof(userId));
            Check.Empty(newPwd, nameof(newPwd));

            var user = QuerySingle(userId);
            if (user == null)
            {
                return false;
            }

            // 验证旧密码是否正确
            if (!string.IsNullOrEmpty(oldPwd))
            {
                var oldEncrypedPwd = EncryptPwd(user.Username, oldPwd, user.Salt);
                if (oldEncrypedPwd != user.Password)
                {
                    return false;
                }
            }

            var salt = GenerateSalt();
            var encryptedPwd = EncryptPwd(user.Username, newPwd, salt);
            var model = new AdminUser
            {
                AdminUserId = userId,
                Password = encryptedPwd,
                Salt = salt
            };

            return _userRepository.Update(model);
        }

        public AdminUser QuerySingle(int userId)
        {
            return _userRepository.QuerySingle(new { AdminUserId = userId });
        }

        public AdminUser QuerySingle(string username)
        {
            return _userRepository.QuerySingle(new { Username = username });
        }

        public List<AdminUser> QueryList(object condition)
        {
            Check.Null(condition, nameof(condition));
            return _userRepository.QueryList(condition);
        }

        public int Count(object condition)
        {
            Check.Null(condition, nameof(condition));
            return _userRepository.Count(condition);
        }

        public (int Total, List<AdminUser> List) QueryPage(object condition)
        {
            Check.Null(condition, nameof(condition));
            var count = Count(condition);
            var list = QueryList(condition);
            return (count, list);
        }
    }
}
