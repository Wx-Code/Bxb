import Vue from 'vue'
import Router from 'vue-router'
import clone from 'clone'
import Empty from '@/views/components/empty'
import { addPrefix } from '@/utils/path'

// in development-env not use lazy-loading, because lazy-loading too many pages will cause webpack hot update too slow. so only in production use lazy-loading;
// detail: https://panjiachen.github.io/vue-element-admin-site/#/lazy-loading

Vue.use(Router)

const routerMap = [

  {
    path: '/login',
    name: 'Login',
    meta: { title: '登录' },
    component: () => import('@/views/login')
  },

  {
    path: '*',
    meta: {
      title: '拆包赢数贝'
    },
    redirect: addPrefix('/')
  }
]

export const constantRouterMap = clone(routerMap).map(r => {
  if (r.path.startsWith('/')) {
    r.path = addPrefix(r.path)
  }
  return r
})

export default new Router({
  mode: 'history', //后端支持可开
  // scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap
})
