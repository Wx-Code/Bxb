<template>
  <div class="phoneVerify">
    <ul class="userLogin_ul">
      <li class="row userLogin_li">
        <div class="userLogin_li_box row ac bb1">
          <img src="http://static.pinlala.com/bxb/shoujihaoma.png" alt="" class="ul_icon1">
          <input type="text" placeholder="请输入手机号码" v-model="phone" @input="watchPhone()" class="ul_input">
        </div>

      </li>
      <li class="row userLogin_li jb">
        <div class="userLogin_li_box row ac bb1 ul_sty1">
          <img src="http://static.pinlala.com/bxb/yanzhengma.png" alt="" class="ul_icon2">
          <input type="text" placeholder="请输入验证码" v-model="code" @input="watchCode()" class="ul_input">
        </div>
        <div class="btn_type1" @click="getCode">{{codeText}}</div>
      </li>

    </ul>

  </div>

</template>
<script>
  import userService from '@/api/common'

  export default {
    name: 'phoneVerify',
    props: {
      text: {
        type: Number,
        default: 1
      },
      checkType: {
        type: String
      }
    },
    data() {
      return {
        codeText: '发送验证码',
        disable: 0, // 禁用样式控制 1----禁用
        send: 1, // 是否能点击 1-可点击 0-不可点击
        time: 60, // 默认时间
        code: '',
        phone: ''
      };
    },
    created() {

    },
    methods: {
      //验证手机号
      validate() {
        //手机号正则式
        let regPho = /^1[0-9]{10}$/
        if (!regPho.test(this.phone)) {
          this.$toast('请填写正确的手机号码')
          return false
        } else {
          return true
        }
      },
      async getCode() {
        if (this.validate()) {
          if (this.send === 1) {
            await this.getCodeRequest()
            console.log('发送验证码')
            //倒计时
            this.disable = 1
            this.send = 0
            let that = this
            that.codeText = '60s后重新发送'
            let timer = setInterval(function () {
              if (that.time > 1) {
                that.time--
                that.codeText = that.time + 's后重新发送'
              } else {
                clearInterval(timer)
                timer = null
                that.disable = 0
                that.send = 1
                that.time = 60
                that.codeText = '获取验证码'
              }
            }, 1000)
          }
        }
      },
      async getCodeRequest() {
        const {data, errorCode} = await userService.sendCode({phone: this.phone})
        if (errorCode != '0000') {
          this.$toast({message: '发送成功', duration: '1500'})
        } else {
          this.$toast({message: '发送失败', duration: '1500'})
        }
      },
      watchPhone() {
        // console.log(111);
        this.$emit('changePhone', this.phone)
      },
      watchCode() {
        // console.log(111);
        this.$emit('changeCode', this.code)
      }
    },


  };
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
  .phoneVerify {

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
  }


</style>

