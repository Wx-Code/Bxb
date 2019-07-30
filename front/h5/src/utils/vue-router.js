import router from './../router'
import store from './../utils/local-store'
import weixin from 'weixin-js-sdk'
import wechatService from './../api/wechat'
import { jsapis } from './../utils/weixin'
import { addPrefix } from '@/utils/path'

// 配置微信 JSSDK
// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141115
const configWeixinJssdk = async (route) => {
  const meta = route.meta || {}
  const jsapiList = [ jsapis.hideOptionMenu ]
  const sharable = meta.sharable === true
  if (sharable) {
    [ jsapis.onMenuShareTimeline, jsapis.onMenuShareAppMessage, jsapis.hideMenuItems ]
  }
  const { data } = await wechatService.getJsapiConfig(route.fullPath)
  const { appId, timestamp, nonceStr, signature } = data
  weixin.config({
    debug: false,
    appId,
    timestamp,
    nonceStr,
    signature,
    jsApiList: jsapiList
  })

  // 对于不允许分享的页面，暂时做如下处理：隐藏所有基础菜单
  if (!sharable) {
    weixin.ready(() => weixin.hideOptionMenu())
  }
}

const trimEnd = (source, target) => {
  const index = source.lastIndexOf(target)
  if (index === -1) {
    return source
  }
  return source.substring(0, index)
}

const whiteList = [
  '/',
  '/login',
  '/register',
  '/myCenter',
  '/customer',
  '/myAddress',
  '/changeMyAddress',
  '/myInformation',
  '/changeCode',
].map(p => addPrefix(p))

router.beforeEach(async (to, from, next) => {
  // 配置微信 jssdk
  await configWeixinJssdk(to)

  if (whiteList.indexOf(to.path) !== -1) {
    next()
    return
  }
  const token = store.getToken()
  if (!!token) {
    next()
  } else {
    next({ path: '/', query: to.query })
  }
})
