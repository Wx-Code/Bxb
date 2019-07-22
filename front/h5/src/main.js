import Vue from 'vue'
import App from './App'
import router from './router'
import Axios from 'axios'
import request from './utils/request'
import globalData from './store/globalData.js'
import utils from './utils/utils.js'
import './assets/iconfont/iconfont.css'
import { Toast,Popup } from 'vant'
Vue.use(require('vue-wechat-title'),Toast,Popup )
// import theConfirm  from './components/testDialog/testDialog.js'

import './utils/vue-router'

// Vue.prototype.$confirms = theConfirm;
Vue.prototype.$http = request
Vue.prototype.globalData = globalData
Vue.prototype.utils = utils
Vue.prototype.$axios = Axios

Vue.config.productionTip = false

new Vue({
  el: '#app',
  router,
  render: h => h(App)
})
