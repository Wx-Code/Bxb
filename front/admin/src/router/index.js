import Vue from 'vue'
import Router from 'vue-router'

// in development-env not use lazy-loading, because lazy-loading too many pages will cause webpack hot update too slow. so only in production use lazy-loading;
// detail: https://panjiachen.github.io/vue-element-admin-site/#/lazy-loading

Vue.use(Router)

/* Layout */
import Layout from '../views/layout/Layout'

/**
* hidden: true                   if `hidden:true` will not show in the sidebar(default is false)
* alwaysShow: true               if set true, will always show the root menu, whatever its child routes length
*                                if not set alwaysShow, only more than one route under the children
*                                it will becomes nested mode, otherwise not show the root menu
* redirect: noredirect           if `redirect:noredirect` will no redirect in the breadcrumb
* name:'router-name'             the name is used by <keep-alive> (must set!!!)
* meta : {
    title: 'title'               the name show in subMenu and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar
    breadcrumb: false            if false, the item will hidden in breadcrumb(default is true)
  }
**/
export const constantRouterMap = [
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  { path: '/404', component: () => import('@/views/404'), hidden: true },

  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    name: 'Dashboard',
    hidden: true,
    children: [
      {
        path: 'dashboard',
        component: () => import('@/views/dashboard/index')
      }
    ]
  },

  // 系统设置
  {
    path: '/system',
    name: 'System',
    redirect: '/system/user',
    meta: { title: '系统设置', icon: 'settings' },
    component: Layout,
    children: [
      {
        path: 'user',
        name: 'User',
        meta: { title: '管理员管理', icon: 'user' },
        component: () => import('@/views/system/user')
      }
    ]
  },
  // 用户管理
  {
    path: '/user',
    name: 'user',
    redirect: '/user/tradehall',
    meta: { title: '用户管理', icon: 'user' },
    component: Layout,
    children: [
      {
        path: 'tradehall',
        name: 'tradehall',
        meta: { title: '消息管理', icon: 'message' },
        component: () => import('@/views/user/tradehall')
      }
    ]
  },
  // 配置管理
  {
    path: '/config',
    name: 'SystemConfig',
    redirect: '/config/platwalletaddr',
    meta: { title: '配置管理', icon: 'settings' },
    component: Layout,
    children: [
      {
        path: 'platwalletaddr',
        name: 'PlatWalletAddr',
        meta: { title: '平台钱包地址配置', icon: 'fieid' },
        component: () => import('@/views/config/platwalletaddr')
      },
      {
        path: 'ordertime',
        name: 'OrderTime',
        meta: { title: '订单时间配置', icon: 'fieid' },
        component: () => import('@/views/config/ordertime')
      },
      {
        path: 'tradefee',
        name: 'TradeFee',
        meta: { title: '手续费配置', icon: 'fieid' },
        component: () => import('@/views/config/tradefee')
      },
      {
        path: 'traderules',
        name: 'TradeRules',
        meta: { title: '交易规则配置', icon: 'fieid' },
        component: () => import('@/views/config/traderules')
      },
      {
        path: 'customerservice',
        name: 'CustomerService',
        meta: { title: '客服信息配置', icon: 'fieid' },
        component: () => import('@/views/config/customerservice')
      }
    ]
  },

  { path: '*', redirect: '/404', hidden: true }
]

export default new Router({
  // mode: 'history', //后端支持可开
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap
})
