import request from '@/utils/request'

export default {

  login(code,activityId,groupId) { //通过code获取用户信息
    return request({
      method: 'POST',
      url: '/user/login',
      data: {
        code :code,
        activityId:activityId,
        groupId:groupId
      }
    })

  },
  getUserInfo() {//通过token获取用户信息
    return request({
      method: 'GET',
      url: '/user',
      params: null
    })

  },

}
