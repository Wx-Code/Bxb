import weixin from 'weixin-js-sdk'

/**
 * 微信 JSSDK API 列表
 * 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141115
 */
const jsapis = {
  /**
   * 自定义“分享给朋友”及“分享到QQ”按钮的分享内容
   */
  updateAppMessageShareData: 'updateAppMessageShareData',
  /**
   * 自定义“分享到朋友圈”及“分享到QQ空间”按钮的分享内容
   */
  updateTimelineShareData: 'updateTimelineShareData',
  /**
   * 获取“分享到朋友圈”按钮点击状态及自定义分享内容接口
   */
  onMenuShareTimeline: 'onMenuShareTimeline',
  /**
   * 获取“分享给朋友”按钮点击状态及自定义分享内容接口
   */
  onMenuShareAppMessage: 'onMenuShareAppMessage',
  /**
   * 获取“分享到QQ”按钮点击状态及自定义分享内容接口
   */
  onMenuShareQQ: 'onMenuShareQQ',
  /**
   * 获取“分享到腾讯微博”按钮点击状态及自定义分享内容接口
   */
  onMenuShareWeibo: 'onMenuShareWeibo',
  /**
   * 获取“分享到QQ空间”按钮点击状态及自定义分享内容接口
   */
  onMenuShareQZone: 'onMenuShareQZone',
  /**
   * 开始录音接口
   */
  startRecord: 'startRecord',
  /**
   * 停止录音接口
   */
  stopRecord: 'stopRecord',
  /**
   * 监听录音自动停止接口
   */
  onVoiceRecordEnd: 'onVoiceRecordEnd',
  /**
   * 播放语音接口
   */
  playVoice: 'playVoice',
  /**
   * 暂停播放接口
   */
  pauseVoice: 'pauseVoice',
  /**
   * 停止播放接口
   */
  stopVoice: 'stopVoice',
  /**
   * 监听语音播放完毕接口
   */
  onVoicePlayEnd: 'onVoicePlayEnd',
  /**
   * 上传语音接口
   */
  uploadVoice: 'uploadVoice',
  /**
   * 下载语音接口
   */
  downloadVoice: 'downloadVoice',
  /**
   * 拍照或从手机相册中选图接口
   */
  chooseImage: 'chooseImage',
  /**
   * 预览图片接口
   */
  previewImage: 'previewImage',
  /**
   * 上传图片接口
   */
  uploadImage: 'uploadImage',
  /**
   * 下载图片接口
   */
  downloadImage: 'downloadImage',
  /**
   * 识别音频并返回识别结果接口
   */
  translateVoice: 'translateVoice',
  /**
   * 获取网络状态接口
   */
  getNetworkType: 'getNetworkType',
  /**
   * 使用微信内置地图查看位置接口
   */
  openLocation: 'openLocation',
  /**
   * 获取地理位置接口
   */
  getLocation: 'getLocation',
  /**
   * 隐藏右上角菜单
   */
  hideOptionMenu: 'hideOptionMenu',
  /**
   * 显示右上角菜单
   */
  showOptionMenu: 'showOptionMenu',
  /**
   * 批量隐藏功能按钮接口
   */
  hideMenuItems: 'hideMenuItems',
  /**
   * 批量显示功能按钮接口
   */
  showMenuItems: 'showMenuItems',
  /**
   * 隐藏所有非基础按钮接口
   */
  hideAllNonBaseMenuItem: 'hideAllNonBaseMenuItem',
  /**
   * 显示所有功能按钮接口
   */
  showAllNonBaseMenuItem: 'showAllNonBaseMenuItem',
  /**
   * 关闭当前网页窗口接口
   */
  closeWindow: 'closeWindow',
  /**
   * 调起微信扫一扫接口
   */
  scanQRCode: 'scanQRCode',
  /**
   * 发起一个微信支付请求
   */
  chooseWXPay: 'chooseWXPay',
  /**
   * 跳转微信商品页接口
   */
  openProductSpecificView: 'openProductSpecificView',
  /**
   * 批量添加卡券接口
   */
  addCard: 'addCard',
  /**
   * 拉取适用卡券列表并获取用户选择信息
   */
  chooseCard: 'chooseCard',
  /**
   * 查看微信卡包中的卡券接口
   */
  openCard: 'openCard',
}

/**
 * 配置微信分享事件
 * @param {Object} timeLineConfig 分享到朋友圈的配置信息 { title: '', link: '', imgUrl: '' }
 * @param {Object} appMessageConfig 分享给朋友的配置信息 { title: '', link: '', imgUrl: '', desc: '', dataUrl: '', type: '' }
 */
const registerShareListensers = function (timeLineConfig, appMessageConfig) {
  weixin.ready(function() {
    weixin.hideMenuItems({
      menuList: ['menuItem:openWithQQBrowser','menuItem:copyUrl','menuItem:openWithSafari'] // 要显示的菜单项，所有menu项见附录3
    });
    weixin.onMenuShareTimeline(timeLineConfig)
    weixin.onMenuShareAppMessage(appMessageConfig)
  })
}

export {
  jsapis,
  registerShareListensers
}
