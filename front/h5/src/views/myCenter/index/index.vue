<template>
  <van-pull-refresh v-model="isLoading" @refresh="onRefresh" class="myCenter_box">
    <div class="myCenter">
      <div class="mc_header  col ac">
        <div class="my_user_logo">
          <img :src="userInfo.avatar" alt="" class="my_user_logoImg" v-if="userInfo.avatar">
        </div>
        <div class="mc_user_name tc">{{userInfo.nickname }}</div>
      </div>
      <div class="mc_content col ac">
        <div class="mc_num_box row jb ac">
          <!--<div class="mc_num_num col ac" @click="goRecord">-->
          <div class="mc_num_num col ac">
            <div class="mc_num tc">{{userInfo.outTotalAmount || 0}}</div>
            <div class="mc_num_txt tc">累计售出</div>
          </div>
          <div class="mc_num_line"></div>
          <div class="mc_num_num col ac ">
            <!--<div class="mc_num_num col ac " @click="goRecord">-->
            <div class="mc_num tc">{{userInfo.inTotalAmount || 0}}</div>
            <div class="mc_num_txt tc">累计购买</div>
          </div>

        </div>
        <div class="mc_nav">
          <div class="mc_nav_content col ">
            <div class="mc_nav_item row jb ac " @click="goMySend">
              <div class="mc_nav_item1 row ac ">
                <img src="http://static.pinlala.com/bxb/send.png" alt="" class="mc_nav_icon1">
                <div class="mc_nav_item2">我发布的</div>
              </div>
              <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="mc_nav_next">
            </div>
            <div class="mc_nav_item row jb ac " @click="goMySell">
              <div class="mc_nav_item1 row ac ">
                <img src="http://static.pinlala.com/bxb/sell.png" alt="" class="mc_nav_icon2">
                <div class="mc_nav_item2">我卖出的</div>
              </div>
              <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="mc_nav_next">
            </div>
            <div class="mc_nav_item row jb ac " @click="goMyBuy">
              <div class="mc_nav_item1 row ac ">
                <img src="http://static.pinlala.com/bxb/mai-.png" alt="" class="mc_nav_icon3">
                <div class="mc_nav_item2">我买到的</div>
              </div>
              <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="mc_nav_next">
            </div>
            <div class="mc_nav_item row jb ac " @click="goMyAddress">
              <div class="mc_nav_item1 row ac ">
                <img src="http://static.pinlala.com/bxb/money.png" alt="" class="mc_nav_icon4">
                <div class="mc_nav_item2">我的钱包地址</div>
              </div>
              <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="mc_nav_next">
            </div>
            <div class="mc_nav_item row jb ac " @click="goPlatformAddress">
              <div class="mc_nav_item1 row ac ">
                <img src="http://static.pinlala.com/bxb/money.png" alt="" class="mc_nav_icon4">
                <div class="mc_nav_item2">平台钱包地址</div>
              </div>
              <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="mc_nav_next">
            </div>
            <div class="mc_nav_item row jb ac " @click="goMyInformation">
              <div class="mc_nav_item1 row ac ">
                <img src="http://static.pinlala.com/bxb/my.png" alt="" class="mc_nav_icon5">
                <div class="mc_nav_item2">我的资料</div>
              </div>
              <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="mc_nav_next">
            </div>
            <div class="mc_nav_item row jb ac " @click="goCustomer">
              <div class="mc_nav_item1 row ac ">
                <img src="http://static.pinlala.com/bxb/weixinkefu.png" alt="" class="mc_nav_icon6">
                <div class="mc_nav_item2">微信客服</div>
              </div>
              <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="mc_nav_next">
            </div>
          </div>

        </div>

      </div>
    </div>
  </van-pull-refresh>


</template>

<script>
  import store from '@/utils/local-store'
  import user from '@/api/user'
  import globalData from '@/store/globalData'

  export default {
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        userInfo: '',
        isLoading: false,


      }
    },
    created() {
      this.getMyCenterData()
    },

    methods: {
      goCustomer() {
        this.$router.push({name: 'customer'})
      },
      goPlatformAddress() {
        this.$router.push({name: 'platformAddress'})
      },
      goMyAddress() {
        this.$router.push({name: 'myAddress', params: {walletAddress: this.userInfo.walletAddress}})
      },
      goMyInformation() {
        this.$router.push({name: 'myInformation'})
      },
      goRecord() {
        this.$router.push({name: 'record'})

      },
      goMySend() {
        this.$router.push({name: 'mySend'})

      },
      goMySell() {
        this.initPageState()
        this.$router.push({name: 'mySell'})

      },
      goMyBuy() {
        this.initPageState()
        this.$router.push({name: 'myBuy'})

      },
      async getMyCenterData() {
        const {data, errorCode} = await user.getUserInfo()
        if (!data) return false
        if (errorCode == '0000') {
          this.userInfo = data
          store.setUser(this.userInfo)
        }
      },
      initPageState() { //进入我买的和我买的列表时初始化页面类型
        globalData.pageState = 0
      },
      async onRefresh() {
        await this.getMyCenterData()
        this.isLoading = false
      },

    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .myCenter_box{
    height: 100%;
  }
  .myCenter {

    .mc_header {
      height: 4.2rem;
      overflow: hidden;
      background: linear-gradient(135deg, rgba(74, 114, 255, 1) 0%, rgba(74, 116, 255, 1) 50%, rgba(58, 98, 246, 1) 100%);

    }

    .my_user_logo {
      width: 1.56rem;
      height: 1.56rem;
      margin-top: 0.65rem;
      overflow: hidden;
      border-radius: 50%;
      /*background: pink;*/
    }

    .my_user_logoImg {
      width: 100%;
      height: 100%;
    }

    .mc_user_name {
      margin-top: 0.15rem;
      font-size: 0.3rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(255, 255, 255, 1);
    }

    .mc_content {
      margin-top: -0.87rem;
    }

    .mc_num_box {
      width: 94.67%;
      height: 1.74rem;
      background: rgba(255, 255, 255, 1);
      box-shadow: 0px 0.02rem 0.04rem 0 rgba(0, 0, 0, 0.24);
      border-radius: 0.08rem;
    }

    .mc_num_num {
      width: 3.4rem;
    }

    .mc_num {
      font-size: 0.36rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(85, 133, 228, 1);
    }

    .mc_num_txt {
      margin-top: 0.1rem;
      font-size: 0.24rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(102, 102, 102, 1);
    }

    .mc_num_line {
      width: 1px;
      height: 0.84rem;
      background: #f2f2f2;
    }

    .mc_nav {
      width: 100%;
      border-top: 1px solid #C8C8C8;
      padding-left: 0.22rem;
      margin-top: 0.2rem;
      border-bottom: 1px solid #C8C8C8;
    }

    .mc_nav_item {
      width: 100%;
      height: 0.8rem;
      border-bottom: 1px solid #C8C8C8;
    }

    .mc_nav_item:last-child {
      border-bottom: none;
    }

    .mc_nav_icon1 {
      width: 0.35rem;
      height: 0.31rem;

    }

    .mc_nav_icon2 {
      width: 0.33rem;
      height: 0.31rem;

    }

    .mc_nav_icon3 {
      width: 0.34rem;
      height: 0.3rem;

    }

    .mc_nav_icon4 {
      width: 0.29rem;
      height: 0.29rem;

    }

    .mc_nav_icon5 {
      width: 0.28rem;
      height: 0.24rem;

    }

    .mc_nav_icon6 {
      width: 0.28rem;
      height: 0.3rem;

    }

    .mc_nav_next {
      width: 0.18rem;
      height: 0.32rem;
      margin-right: 0.2rem;
    }

    .mc_nav_item2 {
      font-size: 0.28rem;
      padding-left: 0.1rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(102, 102, 102, 1);
    }


  }
</style>
