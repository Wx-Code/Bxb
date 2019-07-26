import request from '@/utils/request'

export default {
  get(configName) {
    return request({
      url: `/config/${configName}`,
      method: 'GET'
    })
  },

  addOrUpdate(config) {
    return request({
      url: '/config',
      method: 'POST',
      data: config
    })
  }
}
