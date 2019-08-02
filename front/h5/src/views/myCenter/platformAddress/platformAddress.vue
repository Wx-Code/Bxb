<template>
  <div class="platformAddress">
    <ul class="pa_content">
      <li class="pa_item row ac jb" v-for="(item,index) in addressArr" v-if="item.state==0">
        <div class="pa_item_txt1">{{item.platWalletAddrName}}</div>
        <div class="pa_item_txt2 wd">{{item.platWalletAddr}}</div>
        <img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(item.platWalletAddr)" class="customer_box_btn">
      </li>
      <p class="pa_tit" v-if="addressArr.length>0">温馨提示：<br>转币时请确保钱包地址的正确，以免影响您的正常交易。</p>
    </ul>
  </div>
</template>

<script>
  import store from '@/utils/local-store'
  import pageServe from '@/api/page'
  export default {
    created() {
      this.getPlatformAddress()

    },

    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        addressArr:[]
      }
    },

    methods: {
      copyCode(txt) {
        this.$copyText(txt).then(
          res => {
            console.log(res)
            this.$toast("复制成功");
          },
          err => {
            this.$toast("复制失败");
          }
        )



      },
      async getPlatformAddress(){
        const  { data } =  await pageServe.getPlatformAddress()
        if(!data) return
        this.addressArr = data


      }

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
      padding-top: 0.46rem;
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
      width: 4.2rem;
      margin-right: 0.1rem;
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

  }

</style>
