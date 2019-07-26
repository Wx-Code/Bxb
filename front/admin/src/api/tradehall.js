import request from '@/utils/request';

export default {
  getList(query) {
    return request({
      method: 'GET',
      url: '/TradeHall/getTradeHallList',
      params: query
    });
  },
  putOff(tradeId) {
    return request({
      method: 'POST',
      url: '/TradeHall/putOff',
      data: { 'tradeId': tradeId }
    });
  }
};
