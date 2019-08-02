import Vue from 'vue'
import Router from 'vue-router'
import clone from 'clone'
import Empty from '@/views/components/empty'
import {addPrefix} from '@/utils/path'

// in development-env not use lazy-loading, because lazy-loading too many pages will cause webpack hot update too slow. so only in production use lazy-loading;
// detail: https://panjiachen.github.io/vue-element-admin-site/#/lazy-loading

Vue.use(Router)
const routerMap = [
  {
    path: '/',
    name: 'Empty',
    component: Empty
  },
  {
    path: '/userLogin',
    name: 'userLogin',
    meta: { title: '登录' },
    component: () => import('@/views/login/userLogin')
  },
  {
    path: '/register',
    name: 'register',
    meta: { title: '注册' },
    component: () => import('@/views/login/register')
  },
  {
    path: '/my',
    name: 'my',
    component: Empty,
    children: [
      {
        path: 'myCenter',
        name: 'myCenter',
        meta: {title: '个人中心'},
        component: () => import('@/views/myCenter/index/index')
      },
      {
        path: 'record',
        name: 'record',
        meta: {title: '记录'},
        component: () => import('@/views/myCenter/record/record')
      },
      {
        path: 'mySend',
        name: 'mySend',
        meta: {title: '发布信息'},
        component: () => import('@/views/myCenter/mySend/mySend'),
      },
      {
        path: 'changeMySend',
        name: 'changeMySend',
        meta: {title: '编辑信息'},
        component: () => import('@/views/myCenter/changeMySend/changeMySend')
      },

      {
        path: 'mySell',
        name: 'mySell',
        meta: {title: '我卖出的'},
        component: () => import('@/views/myCenter/mySell/mySell'),

      },
      {
        path: 'orderDetail',
        name: 'orderDetail',
        meta: {title: '订单详情'},
        component: () => import('@/views/myCenter/orderDetail/orderDetail')
      },
      {
        path: 'myAddress',
        name: 'myAddress',
        meta: {title: '我的钱包地址'},
        component: () => import('@/views/myCenter/myAddress/myAddress')
      },
      {
        path: 'changeMyAddress',
        name: 'changeMyAddress',
        meta: { title: '我的钱包地址' },
        component: () => import('@/views/myCenter/myAddress/changeMyAddress')
      },
      {
        path: 'platformAddress',
        name: 'platformAddress',
        meta: {title: '平台钱包地址'},
        component: () => import('@/views/myCenter/platformAddress/platformAddress')
      },

      {
        path: 'myInformation',
        name: 'myInformation',
        meta: {title: '我的资料'},
        component: () => import('@/views/myCenter/myInformation/myInformation')
      },
      {
        path: 'changeCode',
        name: 'changeCode',
        meta: {title: '我的资料'},
        component: () => import('@/views/myCenter/myInformation/changeCode')
      },
      {
        path: 'customer',
        name: 'customer',
        meta: {title: '客服'},
        component: () => import('@/views/myCenter/customer/customer')
      },

    ]
  },
  {
    path: '/tradeHall',
    name: 'tradeHallRoot',
    redirect: { name: 'tradeHall' },
    component: Empty,
    children:[
      {
      path: 'tradeHall',
      name: 'tradeHall',
      meta: { title: '交易大厅' },
      component: () => import('@/views/tradeHall/index/index')
    },
      {
        path: 'publishInformation',
        name: 'publishInformation',
        meta: { title: '发布信息' },
        component: () => import('@/views/tradeHall/publishInformation/publishInformation')
      },
    ]
  },
  // 验证码组件
  {
    path: '/phoneVerify',
    name: 'phoneVerify',
    component: () => import('@/components/phoneVerify/phoneVerify')
  },

  {
    path: '*',
    meta: {
      title: ''
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
