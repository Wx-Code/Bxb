import router from './router'
// import store from './store'
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
// import { Message } from 'element-ui'
import local from '@/utils/auth'
import store from './store';

NProgress.configure({ showSpinner: false })

const whiteList = ['/login']
router.beforeEach(async(to, from, next) => {
  console.log('[info] to, from', to, from)

  NProgress.start()
  if (local.getToken()) {
    if (to.path === '/login') {
      next({ path: '/' })
      NProgress.done() // if current page is dashboard will not trigger	afterEach hook, so manually handle it
    } else {
      await store.dispatch('GetInfo')
      next()
    }
  } else {
    if (whiteList.indexOf(to.path) !== -1) {
      next()
    } else {
      next(`/login?redirect=${to.path}`) // 否则全部重定向到登录页
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  NProgress.done() // 结束Progress
})
