import request from '@/utils/request'

export default {
  getList() {
    return request({
      method: 'get',
      url: '/config/getplatwalletconfig'
    })
  }
}
