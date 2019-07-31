<template>
  <div class="platformAddress">
    <ul class="pa_content">
      <!--<li class="pa_item row ac jb" v-for="(item,index) in addressArr" @click="goChangeMyAddress">-->
        <!--<div class="pa_item_txt1">{{item.name}}钱包地址</div>-->
        <!--<div class="pa_item_r row ac">-->
          <!--<div class="pa_item_txt2 wd">{{item.address}}</div>-->
          <!--<img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="next_btn">-->
        <!--</div>-->

      <!--</li>-->
      <li class="pa_item row ac jb"  @click="goChangeMyAddress">
        <div class="pa_item_txt1">GRT钱包地址</div>
        <div class="pa_item_r row ac">
          <div class="pa_item_txt2 wd">{{walletAddress || '去填写'}}</div>
          <img src="http://static.pinlala.com/bxb/ic_nav_back.png" alt="" class="next_btn">
        </div>

      </li>

    </ul>
  </div>

</template>

<script>
  import store from '@/utils/local-store'

  export default {
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        addressArr:[{
          name:'GDT',
          address:'SHBV123452JFHN34'
        }],
        walletAddress:''
      }
    },
    created() {
      const userData = store.getUser()
      if(!userData)return false
      this.walletAddress = userData.walletAddress
    },

    methods: {
      goChangeMyAddress(){
        this.$router.push({ name: 'changeMyAddress' })
      },

    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .platformAddress {
    .pa_content{
      box-sizing: border-box;
      padding-left: 0.4rem ;
      padding-right: 0.2rem;
    }
    .pa_item{
      padding-top: 0.7rem;
      padding-bottom: 0.14rem;
      border-bottom: 1px solid rgba(151, 151, 151, 0.51);
    }
    .pa_item_txt1{
      width: 2rem;
      font-size:0.28rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(102,102,102,1)
    }

    .pa_item_txt2{
      width: 4.7rem;
      margin-right: 0.2rem;
      text-align: right;
      font-size:0.28rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(51,51,51,1);
    }
    .pa_tit{
      font-size:0.24rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(255,0,0,1);
      line-height:0.33rem;
      margin-top: 0.2rem;

    }
    .next_btn{
      width: 0.18rem;
      height: 0.32rem;
    }

  }

</style>
