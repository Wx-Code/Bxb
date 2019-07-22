import request from '@/utils/request'

const factory = name => () =>
  request({
    method: 'get',
    url: `/common/types/${name}`
  })

export default {
  getUserStates: factory('UserState'),
  // 活动状态
  getGameActivityState: factory('GameActivityState'),
  // 组团状态
  getUnpackGroupState: factory('UnpackGroupState'),
  // 奖励领取状态
  getReceiveState: factory('ReceiveState'),
  // 提现审核状态
  getAuditState: factory('AuditState'),
  // 提现单来源
  getWithdrawSourceState: factory('WithdrawSourceType'),
  //收支类型
  getWalletLogType: factory('WalletLogType'),
  //用户操作日志类型
  getUserLogType: factory('UserLogType'),
  //获取订单状态
  getOrderState: factory('ShakeOrderStatus'),
  //获取订单支付渠道
  getpayWayType: factory('ShakeOrderPayWay'),
  //获取商品状态
  getGoodsState: factory('ShakeGoodsState'),

  getLogOperateType: factory('LogOperateType'),

  getTerminalType: factory('LogTerminalType')
}
