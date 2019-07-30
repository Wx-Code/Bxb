<template>
  <div class="customer col ac">
    <img src="http://static.pinlala.com/bxb/customer.png" alt="" class="customer_img">
    <div class="customer_content" v-for="item in weiXinCustomerList" v-if="item.isChecked">
      <div class="customer_box1 row ac">
        <p class="customer_box_txt1 txt_j">客服微信号</p>
        <span class="customer_box_txt2 wd">{{item.wxCustomerNumber}}</span>
        <img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(item.wxCustomerNumber)" class="customer_box_btn">

      </div>


    </div>
    <div class="customer_box1 row ac">
      <p class="customer_box_txt1 txt_j">客服电话</p>
      <a class="customer_box_txt2 wd" :href="'tel:'+ phone">{{phone}}</a>
    </div>




  </div>

</template>

<script>
  import store from '@/utils/local-store'
  import pageServe from '@/api/page'

  export default {
    created() {
      // this.getCustomerInfo()
    },
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        weiXinCustomerList:[],
        wxNumber:'',
        phone:''

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
      async getCustomerInfo(){
        const  { data } =  await pageServe.getCustomerInfo()
        if(!data) return
        this.weiXinCustomerList = data.weiXinCustomerList
        this.phone = data.phone
      }
    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .customer {
    overflow: hidden;
    .customer_img {
      width: 2.74rem;
      height: 4.7rem;
      margin-top: 1.06rem;

    }
    .customer_content{
      margin-top: 1rem;
    }
    .customer_box_txt1{
      font-size:0.3rem;
      margin-right: 0.13rem;
      width: 1.6rem;
      min-width: 1.6rem;
      font-family:PingFangSC-Medium;
      font-weight:500;
      color:rgba(102,102,102,1);
    }
    .customer_box_txt2{
      font-size:0.3rem;
      /*width: 4rem;*/
      max-width: 4rem;
      color:rgba(51,51,51,1);

    }
    .customer_box1{
      max-width: 5.7rem;
      margin-bottom: 0.32rem;
    }
    .customer_box_btn{
      width: 0.29rem;
      height: 0.33rem;
      margin-left: 0.1rem;
    }


  }

</style>
