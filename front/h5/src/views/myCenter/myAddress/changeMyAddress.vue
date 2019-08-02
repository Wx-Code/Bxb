<template>
  <div class="changeMyAddress">
    <div class="cma_content">
      <div class="cma_input_box row jb">
        <input type="text" class="cma_input" placeholder="请输入新地址" v-model="address">
        <img src="http://static.pinlala.com/bxb/guanbi-2.png" alt="" @click="clearInput" v-show="address.length>0"
             class="clear_input">
      </div>
      <div class="cma_btn btn_type2" @click="completeChange">完成</div>
    </div>
  </div>

</template>

<script>
  import store from '@/utils/local-store'
  import userServe from '@/api/user'

  export default {


    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        address: '',
        userInfo: '',
        canClick: true

      }
    },
    created() {
      const userData = store.getUser()
      if (!userData) return false
      this.userInfo = userData
    },

    methods: {
      goChange(txt) {


      },
      clearInput() {
        this.address = ''
      },
      async completeChange() {
        const that = this
        if(!that.validateRequestData()) return
        if (!this.canClick) return
        this.canClick = false
        this.$toast.loading({
          duration: 0,       // 持续展示 toast
          forbidClick: true, // 禁用背景点击
          loadingType: 'spinner',
          message: '修改中'
        })
        const {data, errorCode} = await userServe.editwalletaddr({
          WalletAddress: this.address,
          UserId: this.userInfo.userId
        })
          that.canClick = true
          that.$toast.clear()
        if (errorCode == '0000') {
          this.$toast({message: '修改成功', duration: '1500',forbidClick:true})
          const userData = store.getUser()
          userData.walletAddress = this.address
          store.setUser(userData)

          const that = this
          setTimeout(function () {
            that.$router.go(-1)
          }, 1500)
        } else {
          this.$toast({message: '修改失败', duration: '1500'})
        }
      },
      validateRequestData(){
        if (!this.address || this.address.length <= 0) {
          this.$toast({message: '请填写钱包地址', duration: '1500'})
          return false
        }  else {
          return true
        }
      }

    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .changeMyAddress {
    padding-top: 0.5rem;

    .cma_content {
      padding-left: 0.4rem;
      padding-right: 0.2rem;
      box-sizing: border-box;

    }

    .cma_input_box {
      border-bottom: 1px solid #C8C8C8;
      padding-bottom: 0.13rem;
    }

    .clear_input {
      width: 0.4rem;
      height: 0.4rem;
    }

    .cma_input {
      width: 100%;
      height: 0.45rem;
      line-height: 0.45rem;
      font-size: 0.28rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
    }

    .cma_btn {
      width: 3.64rem;
      margin: 0.89rem auto 0;
    }
  }

</style>
