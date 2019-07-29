import request from '@/utils/request'

export default {
  login(data) { //通过code获取用户信息
    return request({
      method: 'POST',
      url: '/user/login',
      data: data
    })

  },
  register(data) {
    return request({
      method: 'POST',
      url: '/user/register',
      data: data
    })

  },
  getUserInfo(){
    return request({
      method: 'GET',
      url: '/user',
      params: null
    })
  }



}
