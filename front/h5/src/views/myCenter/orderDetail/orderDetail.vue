<template>
  <div class="orderDetail">
    <div class="od_content">
      <div class="od_time_header row ac">
        <img class="od_time_log" src="http://static.pinlala.com/bxb/time_icon.png" alt="">
        <span class="od_time_txt">剩余收款时间：</span>
        <div class="od_time_num">
          <van-count-down :time="time"/>
        </div>


      </div>
      <div class="od_body">
        <ul>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">订单编号</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">234567897654</span>
              <img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(111)"
                   class="customer_box_btn ml20">
            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">买方</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">小犀牛</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">手机号码</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">13467895432</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">购买数量</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">1000个</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">单价</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">10元/个</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">币种</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">GRT</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">交易码</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">FGHJKL</span>
              <img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(111)"
                   class="customer_box_btn ml20">
            </div>
          </li>
        </ul>
      </div>
      <div class="od_tip">温馨提示：请在规定的时间内向平台钱包地址转币，转币时请备注
        订单号，超时未转币交易将自动关闭。</div>
      <div class="od_tip_btnBox">
        <div class="od_tip_btn btn_type2">确认收款</div>
      </div>
    </div>
  </div>

</template>

<script>
  export default {
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        time: 30 * 60 * 60 * 1000
      }
    },
    created() {
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


      }
    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>


  .orderDetail {
    background: #F9F9F9;
    min-height: 100%;
    .od_content {


    }
    .od_time_header {
      background: #fff;
      padding: 0.28rem;
      border-bottom: 1px solid #ddd;
    }

    .van-count-down {
      font-size: 0.3rem;
      font-family: PingFangSC-Medium;
      font-weight: bold;
      color: rgba(255, 0, 0, 1);
    }

    .od_time_txt {
      font-size: 0.26rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      padding-left: 0.1rem;
      color: rgba(102, 102, 102, 1);
    }

    .od_time_log {
      width: 0.3rem;
      height: 0.3rem;
    }

    .od_body {
      background: #fff;
      padding-left: 0.2rem;
      border-bottom: 1px solid #ddd;
    }
    .od_li_r{
      padding-right: 0.2rem;
    }

    .od_body_li {
      padding: 0.2rem 0;
      border-top: 1px solid #ddd;
    }
    .od_body_li:first-child{
      border-top: none;
    }
    .od_li_title{
      font-size:0.28rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(102,102,102,1);
    }
    .od_li_txt{
      font-size:0.28rem;
      font-family:PingFangSC-Medium;
      font-weight:500;
      color:rgba(51,51,51,1);
    }
    .od_tip{
      padding: 0.2rem;
      font-size:0.24rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(255,0,0,1);
      line-height:0.36rem;
    }
    .od_tip_btn{
      width: 3.64rem;
      margin: 0.57rem auto;
    }


  }

</style>
