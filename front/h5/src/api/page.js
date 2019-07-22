import request from '@/utils/request'

export default {
  getAllPages() {
    return request({
      url: '/h5page',
      method: 'get',
    })
  }
}
