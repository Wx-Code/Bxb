<template>
  <div class="mySend">
    <div class="mySend_content">
      <div class="mySend_item">
        <div class="mySend_item_header row jb ac">
          <div class="mySend_header_l row"> <span class="mySend_l_time">发布时间</span> <span  class="mySend_l_times">2919-07-12 10:01:00</span></div>
          <span class="mySend_header_r1">发布中</span>
          <!--<span class="mySend_header_r2">已下架</span>-->

        </div>
        <div class="mySend_item_content">
          <div class="mySend_item_content1 row">
            <div class="mySend_item_box1"><span class="mySend_box1_txt1">可售数量:</span><span class="mySend_box1_txt2">1000个</span></div>
            <div class="mySend_item_box2"><span class="mySend_box1_txt1">单价/个:</span><span class="mySend_box1_txt2">10.00</span></div>
            <div class="mySend_item_box3 row_r"><span class="mySend_box1_txt2">GRT</span><span class="mySend_box1_txt1">币种:</span></div>
          </div>
          <div class="mySend_item_content2 row"><span class="mySend_box1_txt1">交易码：</span><span class="mySend_box1_txt2">GHJKJHGHJK</span><img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(item.address)" class="customer_box_btn ml20"></div>
        </div>
        <div class="mySend_item_footer row_r">
            <div class="tH_btn_sty btn_type5" @click="goDown">下架</div>
          <div class="tH_btn_sty btn_type4" @click="goSend">编辑</div>

        </div>
      </div>
      <div class="mySend_item">
        <div class="mySend_item_header row jb ac">
          <div class="mySend_header_l row"> <span class="mySend_l_time">发布时间</span> <span  class="mySend_l_times">2919-07-12 10:01:00</span></div>
          <span class="mySend_header_r1">发布中</span>
          <!--<span class="mySend_header_r2">已下架</span>-->

        </div>
        <div class="mySend_item_content">
          <div class="mySend_item_content1 row">
            <div class="mySend_item_box1"><span class="mySend_box1_txt1">可售数量:</span><span class="mySend_box1_txt2">1000个</span></div>
            <div class="mySend_item_box2"><span class="mySend_box1_txt1">单价/个:</span><span class="mySend_box1_txt2">10.00</span></div>
            <div class="mySend_item_box3 row_r"><span class="mySend_box1_txt2">GRT</span><span class="mySend_box1_txt1">币种:</span></div>
          </div>
          <div class="mySend_item_content2 row"><span class="mySend_box1_txt1">交易码：</span><span class="mySend_box1_txt2">GHJKJHGHJK</span><img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(item.address)" class="customer_box_btn ml20"></div>
        </div>
        <div class="mySend_item_footer row_r">
          <div class="tH_btn_sty btn_type5" @click="goDown">下架</div>
          <div class="tH_btn_sty btn_type4" @click="goSend">编辑</div>

        </div>
      </div>
    </div>

  </div>

</template>

<script>
  import store from '@/utils/local-store'

  export default {
    created() {

    },

    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,

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
      goSend(){
        this.$router.push({name: 'publishInformation'})

      },
      goDown(){
        this.$dialog({
          title:'确认取消该订单？'
        }).then(function () {

        })

      }

    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .mySend {
    background: #F9F9F9;
    min-height: 100%;

    .mySend_item{
      border-top: 1px solid #ddd;
      border-bottom: 1px solid #ddd;
      background: #fff;
      padding-left: 0.2rem;
      margin-bottom: 0.22rem;
    }
    .mySend_l_time{
      font-size:0.24rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:#666666;
    }
    .mySend_header_l{
      height: 0.6rem;
      line-height: 0.6rem;
    }
    .mySend_l_times{
      font-size:0.24rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      padding-left: 0.1rem;
      color:#333300;
    }
    .mySend_item_header{
      padding-right: 0.1rem;
      border-bottom: 1px solid #ddd;
    }
    .mySend_header_r1{
      font-size:0.24rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(215,2,2,1);
    }
    .mySend_header_r2{
      font-size:0.24rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(153,153,153,1);
    }
    .mySend_item_content{
      padding: 0.3rem 0.2rem 0.2rem 0;
      box-sizing: border-box;
    }
    .mySend_item_box1{
      width: 42%;

    }
    .mySend_item_box2{
      width: 33%;
    }
    .mySend_item_box3{
      width: 25%;
    }
    .mySend_box1_txt1{
      font-size:0.28rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(102,102,102,1);
    }
    .mySend_box1_txt2{
      font-size:0.28rem;
      font-family:PingFangSC-Medium;
      font-weight:500;
      color:rgba(51,51,51,1);
    }
     .tH_btn_sty {
      margin-right: 0.2rem;
      width: 1.31rem;
    }
    .mySend_item_footer{
      border-top: 1px solid #ddd;
      padding: 0.2rem 0;
    }
    .mySend_item_content2{
      margin-top: 0.2rem;
    }


  }

</style>
