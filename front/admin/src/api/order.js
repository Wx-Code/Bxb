 

import request from '@/utils/request'

export default {
  getList(query) {
    return request({
      method: 'get',
      url: '/admin/orders/list',
      params: query
    })
  }, 

  //确认收币
  confirmReceipt(user) {
    return request({
      method: 'post',
      url: '/admin/orders/Confirm',
      data: user
    })
  },

  //日志列表
  getOrderLogList(query) {
    return request({
      method: 'get',
      url: '/admin/orders/orderloglist',
      params: query
    })
  }

}
