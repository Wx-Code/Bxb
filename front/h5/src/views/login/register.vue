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
              <van-uploader :after-read="afterRead">
                <img src="http://static.pinlala.com/bxb/shangchuan.png" alt="" class="ul_icon4">
              </van-uploader>
            </div>
          </div>

        </li>
      </ul>
      <div class="ul_agree_box row ac">
        <div class="ul_agree_tit">
          <van-checkbox v-model="checked" class="register_checked" icon-size="14px" checked-color="#5585E4">我已仔细阅读并同意
          </van-checkbox>
        </div>
        <span class="register_txt" @click="showRule()">《币小保平台交易规则》</span>
      </div>
      <div class="ul_login_btnBox">
        <div class="ul_login_btn btn_type2" @click="register()">立即注册</div>
      </div>
      <!--<div class="goLoginBtn row ac" ><span class="goLoginTxt" @click="goLogin">< 返回登录</span></div>-->
    </div>
  </div>

</template>

<script>
  import store from '@/utils/local-store'
  import common from '@/api/common'
  import user from '@/api/user'
  import {Checkbox} from 'vant';
  import phoneVerify from '@/components/phoneVerify/phoneVerify';

  export default {
    components: {
      'van-checkbox': Checkbox, //自定义弹框组组件
      'phone-verify': phoneVerify
    },
    created() {
      this.weChatLogin()

    },

    data() {
      return {
        host: process.env.FRONT_HOST,
        apiHost: process.env.BASE_API,
        appId: process.env.WECHAT_APP_ID,
        imgUploadSuccess: false,
        checked: false,
        phone: '',
        wechatCode: '',
        code: '',
        imgData: '',
        canClick: true
      }
    },

    methods: {
      async afterRead(file) {
        this.$toast.loading({
          duration: 0,       // 持续展示 toast
          forbidClick: true, // 禁用背景点击
          loadingType: 'spinner',
          message: '上传中'
        })
        // 此时可以自行将文件上传至服务器
        let formData = new window.FormData();
        formData.append('file', file.file);//通过append向form对象添加数据
        formData.get("file")
        let config = {
          headers: {'Content-Type': 'multipart/form-data'}
        }; //添加请求头
        const { data } = await this.$axios.post(this.apiHost + '/common/wechat/qrcode', formData, config)
        if (!data) return
        this.imgData = data.data
        this.imgUploadSuccess = true
        const that = this
        setTimeout(function () {
          that.$toast.clear()
        }, 300)
      },
      changePhone(phone) {
        // console.log(phone);
        this.phone = phone
      },
      changeCode(code) {
        this.code = code
      },
      async register() {
        console.log(this.phone);
        if (!this.validateRequestData()) return
        if (!this.canClick) return
        this.canClick = true
        const { data ,errorCode ,message} = await user.register({
          wechatCode: this.wechatCode,
          phone: this.phone,
          smsCode: this.code,
          qrCodeUrl: this.imgData
        })
        if (errorCode == '0000') {
          const  that =this
          that.$toast({message: '注册成功', duration: '1500'})
          setTimeout(function () {
            that.redirect()
          },1500)
        } else if (errorCode == '0001') {
          this.$toast({message: message, duration: '1500'})
        } else {
          this.$toast({message: '注册失败', duration: '1500'})
        }

      },
      validateRequestData() {
        //手机号正则式
        let regPho = /^1[0-9]{10}$/
        let regCode = /^\d{6}$/
        if (!regPho.test(this.phone)) {
          this.$toast({message: '请填写正确的手机号码', duration: '1500'})
          return false
        } else if (!regCode.test(this.code)) {
          this.$toast({message: '请填写正确的验证码', duration: '1500'})
          return false
        } else if ( this.imgData.length <= 0 || !this.imgUploadSuccess) {
          this.$toast({message: '请上传您的图片二维码', duration: '1500'})
          return false
        } else if (!this.checked) {
          this.$toast({message: '您还未未同意《币小保平台交易规则》喔~', duration: '1500'})
          return false
        }else{
          return true
          console.log(this.checked);
        }

      },
      weChatLogin() {
        const code = this.$route.query.code
        console.log('wechatCode', code);
        if (code) {
          this.wechatCode = code
        }

      },
      async getRule() {
        const {data} = await common.getRule()
        if (!data) return false
        this.content_txt = data
      },
      async showRule() {
        await this.getRule()
        this.$dialog({
          title:'币小保平台交易规则',
          showBtn: false,
          content_txt: this.content_txt,
        })
      },
    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>

  .userLogin {

  }

  .userLogin_li_box {
    width: 100%;
  }
  /*.goLoginBtn{*/
    /*width: 100%;*/
    /*font-size: 0.5rem;*/
    /*position: fixed;*/
    /*left: 0.2rem;*/
    /*top: 0.2rem;*/
    /*height: 0.8rem ;*/
  /*}*/
  /*.goLoginTxt{*/
    /*font-size: 0.28rem;*/
  /*}*/

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

  .ul_agree_tit {
    font-size: 0.24rem;
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
