import request from '@/utils/request'

export default {
  sendCode(data){
    return request({
      method: 'POST',
      url: '/common/sms/code',
      data: data
    })

  }
}
