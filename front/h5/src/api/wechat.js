import request from './../utils/request'

export default {
  getJsapiConfig(url) {
    return request({
      method: 'GET',
      url: '/wechat/jsapi/config',
      params: { url }
    })
  }
}
