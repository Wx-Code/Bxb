import request from '@/utils/request'

export default {
  getAllPages() {
    return request({
      url: '/h5page',
      method: 'get',
    })
  },
  getCustomerInfo(){
    return request({
      url: '/config/customer/service',
      method: 'GET',
      params: null
    })

  },
  getPlatformAddress(){
    return request({
      url: '/config/platwallet/addr',
      method: 'GET',
      params: null
    })

  }

}
