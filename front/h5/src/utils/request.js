import axios from 'axios'
import store from '@/utils/local-store'
import router from '../router'
import { Toast, Dialog } from 'vant'

// 创建axios实例
const service = axios.create({
  baseURL: process.env.BASE_API,
  timeout: 30 * 1000,
})

// request拦截器
service.interceptors.request.use(
  config => {
    // 可在此向服务器添加统一的头部信息
    config.headers['X-Token'] = store.getToken() || 0
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
  // 在此添加请求之后的统一处理逻辑
  response => {
    // NProgress.done()

    const res = response.data
    // Toast.clear()

    const throwError = (message) => {
      throw new Error(message)
    }

    if (res.errorCode === '0005') {
      store.setToken('')
      Dialog.confirm({
        title: '温馨提示',
        message: '登录已失效，请重新登录'
      }).then(() => {
        const redictUrl = window.location.href
        router.push({ name: 'userLogin', query: { redirect: redictUrl, action: 'login' } })
      }).catch(() => {
        // on cancel
      });
      throwError('token 已失效')
    }

    return res
  },
  error => {
    // NProgress.done()
    console.log('err' + error) // for debug
    // Message({
    //   message: '服务器繁忙，请稍后重试',
    //   type: 'error',
    //   duration: 15 * 1000
    // })
    return Promise.reject(error)
  }
)

export default service
