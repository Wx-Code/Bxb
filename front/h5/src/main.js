import Vue from 'vue'
import App from './App'
import router from './router'
import Axios from 'axios'
import request from './utils/request'
import globalData from './store/globalData'
import utils from './utils/utils.js'
import './assets/iconfont/iconfont.css'
import VueClipboard from 'vue-clipboard2';
import './utils/vue-router'
import dialog  from './components/dialog/dialog'
import { Toast,Popup,Picker,Tabbar, TabbarItem,Tab, Tabs,CountDown } from 'vant'
Vue.use(require('vue-wechat-title')).use(VueClipboard).use(Picker).use(Toast).use(Popup).use(Tabbar).use(TabbarItem).use(Tab).use(Tabs).use(CountDown)

Vue.prototype.$dialog = dialog;
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
