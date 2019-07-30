import login from '@/api/user'

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
  getNowTime(){
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
}

export default utils
