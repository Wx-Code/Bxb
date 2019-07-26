<template>
  <div class="userLogin">
    <div class="userLogin_body">
      <ul class="userLogin_ul">
        <phone-verify @changePhone="changePhone" @changeCode="changeCode"></phone-verify>
        <li class="row userLogin_li jb m_b0">
          <div class="userLogin_li_box row ac  bb1 ul_sty1 w_100  register_pb">
            <img src="http://static.pinlala.com/bxb/weixinerweima.png" alt="" class="ul_icon3">
            <div class="row jb w_100">
              <span class="ul_code_text2" v-if="imgUploadSuccess">上传成功</span>
              <span class="ul_code_text1" v-if="!imgUploadSuccess">请上传图片二维码</span>
              <img src="http://static.pinlala.com/bxb/shangchuan.png" alt="" class="ul_icon4">
            </div>
          </div>

        </li>
      </ul>
      <div class="ul_agree_box row ac">
        <van-checkbox v-model="checked" class="register_checked" icon-size="14px" checked-color="#5585E4">我已仔细阅读
        </van-checkbox>
        <span class="register_txt" @click="showRule()">《币小保平台交易规则》</span>
      </div>
      <div class="ul_login_btnBox">
        <div class="ul_login_btn btn_type2" @click="register()">立即注册</div>
      </div>
    </div>
  </div>

</template>

<script>
  import store from '@/utils/local-store'
  import userService from '@/api/user'
  import { Checkbox } from 'vant';
  import  phoneVerify  from '@/components/phoneVerify/phoneVerify';

  export default {
    components: {
      'van-checkbox': Checkbox, //自定义弹框组组件
      'phone-verify':phoneVerify
    },
    data() {
      return {
        phone:'',
        code:''
      }
    },
    created() {
      this.weChatLogin()
    },

    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        imgUploadSuccess: false,
        checked: false,
        phone: '',
        code:''
      }
    },

    methods: {
      showRule() {
        this.$dialog({
          showCancel: false,
          confirmText: '同意',
          content_txt: '好多卡好地方回归到付款更好看电饭锅和待付款各个好地方开个店的活动风华高科的复合弓大骨饭开的会待付款峰会上开发好多卡好地方回归到付款更好看电饭锅和待付款各个好地方开个店的活动风华高科的复合弓大骨饭开的会待付款峰会上开发好多卡好地方回归到付款更好看电饭锅和待付款 各个好地方开个店的活动风华高科的复合弓大骨饭开的会待付款峰会上开发好多卡好地方回归到付款更好看电饭锅和待付款 各个好地方开个店的活动风华高科的复合弓大骨饭开的会待付款峰会上开发好多卡好地方回归到付款更好看电饭锅和待付款各个好地方开个店的活动风华高科的复合弓大骨饭开的会待付款峰会上开发\n' +
            '       好多卡好地方回归到付款更好看电饭锅和待付款各个好地方开个店的活动风华高科的复合弓大骨饭开的会待付款峰会上开发。',
          confirmBtn() {
            console.log(111);
          }
        })
      },
      changePhone(phone) {
        // console.log(phone);
        this.phone = phone

      },
      changeCode(code) {
        this.code = code

      },
      register(){
        console.log(this.phone);
      },
      weChatLogin(){
        const code = this.$route.query.code
        if(code){
          this.code=code
        }else{
          const backUrl = encodeURIComponent(this.host + '/register')
          window.location.href = `https://open.weixin.qq.com/connect/oauth2/authorize?appid=${this.appId}&redirect_uri=${backUrl}&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect`
        }

      }
    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>

  .userLogin {

  }

  .userLogin_li_box {
    width: 100%;
  }

  .ul_sty1 {
    width: 4.74rem;
  }

  .userLogin_body {
    padding: 1.31rem 0.4rem 0;
  }

  .userLogin_li {
    margin-bottom: 0.53rem;

  }

  .ul_icon1 {
    width: 0.34rem;
    height: 0.46rem;
  }

  .ul_icon2 {
    width: 0.41rem;
    height: 0.34rem;
  }

  .ul_input {
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

  .ul_login_btnBox {
    padding-top: 0.98rem;
  }

  .ul_login_btn {
    width: 3.64rem;
    margin: 0 auto;
  }

  .ul_icon3 {
    width: 0.34rem;
    height: 0.34rem;
  }

  .ul_icon4 {
    width: 0.38rem;
    height: 0.37rem;
  }

  .ul_code_text1 {
    padding-left: 0.17rem;
    color: #CCCCCC;
    font-size: 0.28rem;
  }

  .ul_code_text2 {
    padding-left: 0.17rem;
    color: #5585E4;
    font-size: 0.28rem;
  }

  .register_pb {
    padding-bottom: 0.15rem;
  }

  .ul_agree_box {
    margin-top: 0.31rem;
  }

  .register_txt {
    font-size: 0.24rem;
    font-family: PingFangSC-Regular;
    font-weight: 400;
    color: rgba(51, 51, 51, 1);
    color: #4884E9;
  }

  /*.register_checked{*/
  /*width: 31%;*/
  /*}*/
</style>
