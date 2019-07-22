import axios from 'axios'
import { Message, MessageBox } from 'element-ui'
import NProgress from 'nprogress'
import router from '@/router'
import local from '@/utils/auth'
import store from '../store'

// 创建axios实例
const service = axios.create({
  baseURL: process.env.BASE_API,
  timeout: 30 * 1000,
})

// request拦截器
service.interceptors.request.use(
  config => {
    const user = local.getUser() || {}
    config.headers['X-Token'] = local.getToken()
    config.headers['X-UserId'] = user.userId
    return config
  },
  error => {
    // Do something with request error
    console.log(error) // for debug
    Promise.reject(error)
  }
)

// response 拦截器
service.interceptors.response.use(
  response => {
    NProgress.done()

    const res = response.data
    if (res.errorCode === '0004') {
      MessageBox.confirm(
        '登录已失效，请重新登录',
        '确定',
        {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }
      ).then(async() => {
        console.log('[info] confirm')
        await store.dispatch('FedLogOut')
        router.push({ path: '/login' })
      })

      return Promise.reject('login expired')
    }

    return res
  },
  error => {
    NProgress.done()
    console.log('err' + error) // for debug
    Message({
      message: '服务器繁忙，请稍后重试',
      type: 'error',
      duration: 15 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
