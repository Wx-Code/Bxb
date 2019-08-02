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
  // 获取平台钱包地址
  getPlatformAddress(){
    return request({
      url: '/config/platwallet/addr',
      method: 'GET',
      params: null
    })
  },
  // 发布消息
  publishInformation(data){
    return request({
      url: '/TradeHall/user/message',
      method: 'POST',
      data: data
    })
  },
  // 获取交易大厅列表
  getTradeHallList(data){
    return request({
      url: '/TradeHall/message',
      method: 'GET',
      params: data
    })
  },
  // 我发送的列表
  getMySendLsit(data){
    return request({
      url: '/TradeHall/user/message',
      method: 'GET',
      params: data
    })
  },
  // 下架发送的
  putOffMySend(data){
    return request({
      url: '/TradeHall/user/message/putOff',
      method: 'POST',
      data: data
    })
  },
  // 交易大厅提交订单
  submitOrder(data){
    return request({
      url: '/orders',
      method: 'POST',
      data: data
    })
  },



}
