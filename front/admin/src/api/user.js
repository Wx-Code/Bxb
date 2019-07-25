import request from '@/utils/request'

export default {
  getList(query) {
    return request({
      method: 'get',
      url: '/admin/user',
      params: query
    })
  },

  updatePwd(userId, password, oldPassword) {
    return request({
      method: 'post',
      url: '/admin/user/pwd',
      data: { userId, password, oldPassword }
    })
  },

  addUser(user) {
    return request({
      method: 'put',
      url: '/admin/user',
      data: user
    })
  },

  updateUser(user) {
    return request({
      method: 'post',
      url: '/admin/user',
      data: user
    })
  },

  getUserList(query) {
    return request({
      method: 'get',
      url: '/admin/userinfo/GetUserList',
      params: query
    })
  },

  // 获取用户详情
  getUserDetail(userId) {
    return request({
      method: 'get',
      url: '/admin/userinfo/userdetail',
      params: { userId: userId }
    })
  }
}
