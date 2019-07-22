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

  // 获取资讯列表
  getInfomationList(query) {
    return request({
      method: 'get',
      url: '/News',
      params: query
    })
  },
  // 获取资讯详情
  getInfomation(id) {
    return request({
      method: 'get',
      url: '/News/QuerySingle',
      params: { id: id }
    })
  },
  // 添加资讯
  addInfomation(model) {
    return request({
      method: 'post',
      url: '/News/AddInfomation',
      data: model
    })
  },
  // 修改资讯
  updateInfomation(model) {
    return request({
      method: 'post',
      url: '/News/UpdateInfomation',
      data: model
    })
  },
  // 修改资讯状态
  updateInfomationState(id, state) {
    return request({
      method: 'get',
      url: '/News/UpdateState',
      params: { id: id, state: state }
    })
  },
  // 删除资讯状态
  deleteInfomation(id) {
    return request({
      method: 'get',
      url: '/News/DeleteInfomation',
      params: { id: id }
    })
  },
  // 删除资讯状态(多条)
  deleteInfomations(id) {
    return request({
      method: 'get',
      url: '/News/DeleteInfomations',
      params: { ids: id }
    })
  },
  // 获取用户提现列表
  GetUserCashList(model) {
    return request({
      method: 'get',
      url: '/UserCash/list',
      params: model
    })
  },
  // 修改用户提现状态
  updateUserCashState(id, remark, state) {
    return request({
      method: 'get',
      url: '/UserCash/updatestate',
      params: { WithdrawalId: id, AuditRemark: remark, State: state }
    })
  },
  // 批量拒绝提现申请
  refusePass(ids, remark, state) {
    return request({
      method: 'get',
      url: '/UserCash/refusepass',
      params: { ids: ids, AuditRemark: remark, State: state }
    })
  },
  // 通过提现申请
  withdrawAccept(id) {
    return request({
      method: 'get',
      url: '/UserCash/withdraw/accept',
      params: { id: id }
    })
  },
  // 批量通过提现申请
  batchWithdrawAccept(ids) {
    return request({
      method: 'get',
      url: '/UserCash/batch/withdraw/accept',
      params: { ids: ids }
    })
  },
  exportWithdrawList(model) {
    return request({
      method: 'post',
      url: '/UserCash/withdraw/export',
      data: model
    })
  },
  // 用户收支日志列表
  getwalletLog(model) {
    return request({
      method: 'get',
      url: '/User/walletLog',
      params: model
    })
  },
  // 获取用户列表
  getUserList(query) {
    return request({
      method: 'get',
      url: '/User',
      params: query
    })
  },
  // 获取用户积分明细
  getUserScoreList(query) {
    return request({
      method: 'get',
      url: '/User/GetWalletDetail',
      params: query
    })
  },
  // 获取用户提现记录
  getWithDrawAuitList(query) {
    return request({
      method: 'get',
      url: '/User/GetWithDrawAuitList',
      params: query
    })
  },
  // 统计已经提现到账的金额总数
  getWithdrawSum() {
    return request({
      method: 'get',
      url: 'User/GetWithdrawSum'
    })
  },

  // 提现申请
  checkWithdraw(id, draw, auit) {
    return request({
      method: 'get',
      url: 'User/CheckWithDraw',
      params: { withdrawalId: id, drawDesc: draw, auitDesc: auit }
    })
  },

  // 拒绝提现申请
  refuseWithdraw(id, desc) {
    return request({
      method: 'get',
      url: 'User/RefuseWithdraw',
      params: { withdrawId: id, auditRemark: desc }
    })
  },

  // 获取收支日志列表
  getWalletLogList(query) {
    return request({
      method: 'get',
      url: '/admin/walletlog/list',
      params: query
    })
  },

  // 获取用户操作日志列表
  getUserLogList(query) {
    return request({
      method: 'get',
      url: '/User/userlog/list',
      params: query
    })
  },
}
