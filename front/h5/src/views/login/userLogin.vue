<template>
  <div class="userLogin">
    <div class="userLogin_body">
      <ul class="userLogin_ul">
        <!--<li class="row userLogin_li">-->
          <!--<div class="userLogin_li_box row ac bb1">-->
            <!--<img src="http://static.pinlala.com/bxb/shoujihaoma.png"   alt="" class="ul_icon1">-->
            <!--<input type="text"  placeholder="请输入手机号码"     v-model="phone"  class="ul_input">-->
          <!--</div>-->
        <!--</li>-->
        <!--<li class="row userLogin_li jb">-->
          <!--<div class="userLogin_li_box row ac bb1 ul_sty1">-->
            <!--<img src="http://static.pinlala.com/bxb/yanzhengma.png"  alt="" class="ul_icon2">-->
            <!--<input type="text"  placeholder="请输入验证码"    v-model="phone"  class="ul_input">-->
          <!--</div>-->
          <!--<div class="btn_type1">发送验证码</div>-->
        <!--</li>-->
        <phone-verify @changePhone="changePhone" @changeCode="changeCode"></phone-verify>
      </ul>
      <div class="ul_login_btnBox">
          <div class="ul_login_btn btn_type2">登录</div>
      </div>
    </div>
    <div class="ul_register_btnBox row jc">
      <span class="ul_register_btn " @click="goRegisterPage()">立即注册</span>
    </div>
  </div>

</template>

<script>
import store from '@/utils/local-store'
import userService from '@/api/user'
import  phoneVerify  from '@/components/phoneVerify/phoneVerify';

export default {
  components: {
    'phone-verify':phoneVerify
  },

  data(){

    return {
      phone:'',
      code:''
    }

  },
  created() {
    const action = this.$route.query.action
    if (action === 'login') {
      this.redirect()
    } else {
      return false
    }
  },

  data() {
    return {
      host: process.env.FRONT_HOST,
      appId: process.env.WECHAT_APP_ID,
      imgUploadSuccess:false,
      checked:false,
      phone:''
    }
  },

  methods: {
    goRegisterPage(){
      this.$router.push({ name: 'register' })
    },
    async redirect() {
      const redirectUrl = store.getUrl()
      store.removeUrl()
      window.location.href = redirectUrl
    },
    changePhone(phone) {
      // console.log(phone);
      this.phone = phone
    },
    changeCode(code) {
      this.code = code
    },
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>

  .userLogin{

  }
  .userLogin_li_box{
    width: 100%;
  }
  .ul_sty1{
    width: 4.74rem;
  }
  .userLogin_body{
    padding: 1.31rem 0.4rem  0;
  }
  .userLogin_li{
    margin-bottom:0.53rem ;

  }
  .ul_icon1{
    width: 0.34rem;
    height: 0.46rem;
  }
  .ul_icon2{
    width: 0.41rem;
    height: 0.34rem;
  }
  .ul_input{
    width: 4rem;
    height: 0.68rem;
    line-height: 0.68rem;
    padding-left: 0.17rem;
    color: #333333;
    font-size: 0.28rem;
    font-size: 0.28rem;
  }
  .ul_input::placeholder {
    color: #CCCCCC;
    font-size: 0.28rem;
  }
  .ul_login_btnBox{
    padding-top: 0.5rem;
  }
  .ul_login_btn{
    width: 3.64rem;
    margin: 0 auto;
  }
  .ul_register_btnBox{
    width: 100%;
    margin-top: 5.48rem;
    /*position: fixed;*/
    /*bottom: 0.78rem;*/
    /*left: 0;*/
  }
  .ul_register_btn{

    font-size:0.3rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    color:rgba(85,133,228,1);
    line-height:0.42rem;
    border-bottom: 0.01rem solid rgba(85,133,228,1);
  }

</style>
