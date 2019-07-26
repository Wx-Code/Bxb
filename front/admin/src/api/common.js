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
  
  getLogOperateType: factory('LogOperateType'),

  getTerminalType: factory('LogTerminalType'),
  //获取交易信息状态
  getTradeHallShelfStatus: factory('TradeHallShelfStatus'),
  //获取货币类型
  getCurrencyType: factory('CurrencyType'),
}
