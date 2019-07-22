import login from '@/api/login'
import Cookies from 'js-cookie'

const utils =  {
  //获取链接中的参数
  getUrlData(name){
    return (
      decodeURIComponent(
        (new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(
          location.href
        ) || [, ''])[1].replace(/\+/g, '%20')
      ) || null
    )

  },
  //获取链接中的参数
  getLinkData(name,link){
    return (
      decodeURIComponent(
        (new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(
          link
        ) || [, ''])[1].replace(/\+/g, '%20')
      ) || null
    )

  },
  getToken(){
    return window.localStorage.getItem('token')
  },
  //设置本地缓存
  setLocalData(k,v){

    if(!window.localStorage.getItem('global')){
      return
    }
    var globalObj = JSON.parse(window.localStorage.getItem('global'))
    globalObj[k] = v
    window.localStorage.setItem('global',JSON.stringify(globalObj))

  },
  //取本地缓存
  getLocalData(k){
    if(!window.localStorage.getItem('global')){
      return
    }
    var globalObj = JSON.parse(window.localStorage.getItem('global'))
    console.log(globalObj);
    return k ? globalObj[k] : globalObj

  },
  //获取用户信息
  getUserInfo(_s){
    login.getUserInfo().then(res => {

      if (res.success) {
        _s.userInof=res.data


      } else {
        _s.$toast(res.message)





      }
    })

  },
  // 活动规则处理
  ruleChange(ruleContent,_s){
    if(ruleContent && ruleContent.length>0){
      var arr = ruleContent.split('\n')
      var ruleContent = []
      for (let i = 0; i < arr.length; i++) {
        const o ={}
        if(arr[i].length>0){
          o.txt =arr[i]
          ruleContent.push(o)
        }



      }
      return  ruleContent

    }


  },
  //获取本地token值
  getTokenNew(){
    return Cookies.get('token') || window.localStorage.getItem('token')
  },
  getTime(){

      var date = new Date();
      var seperator1 = "-";
      var seperator2 = ":";
      var month = date.getMonth() + 1;
      var strDate = date.getDate();
      if (month >= 1 && month <= 9) {
        month = "0" + month;
      }
      if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
      }
      var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
        + " " + date.getHours() + seperator2 + date.getMinutes()
        + seperator2 + date.getSeconds();
      return currentdate;

  },
  clearCache(){
    Cookies.set('token', '')


  }





}

export default utils
