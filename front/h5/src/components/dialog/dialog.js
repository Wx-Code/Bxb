
import Vue from 'vue'
import LayerComponent from './dialog.vue'  //引入layer 组件
let confirmConstructor = Vue.extend(LayerComponent);
let dialog = function (settingContent) {
  return new Promise((res, rej) => { //promise封装，ok执行resolve，no执行rejectlet
    let confirmDom = new confirmConstructor({
      el: document.createElement('div')
    })
    document.body.appendChild(confirmDom.$el);  //new一个对象，然后插入body里面
    if(settingContent){
      // confirmDom.type = settingContent.type || 'defalut'
      confirmDom.title = settingContent.title || '温馨提示'
      confirmDom.content = settingContent.content
      confirmDom.content_txt = settingContent.content_txt
      confirmDom.cancelText = settingContent.cancelText || '取消'
      confirmDom.confirmText = settingContent.confirmText || '确认'
      typeof(settingContent.showCancel) == 'boolean' && !settingContent.showCancel? confirmDom.showCancel = false: confirmDom.showCancel = true
    }
    confirmDom.confirmBtn = function () { //确认
      res()
      confirmDom.showMask = false
      // typeof settingContent.confirmBtn == 'function'? settingContent.confirmBtn(): ''
    }
    confirmDom.cancelBtn = function () { //取消
      rej()
      confirmDom.showMask = false
      // typeof settingContent.cancelBtn == 'function'? settingContent.cancelBtn(): ''
    }

  })
}

export default dialog;

