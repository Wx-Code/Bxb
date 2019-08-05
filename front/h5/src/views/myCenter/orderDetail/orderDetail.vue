<template>
  <div class="orderDetail">
    <div class="od_content">
      <div class="od_time_header row ac" v-if="pageData.surplusTime>0">
        <img class="od_time_log" src="http://static.pinlala.com/bxb/time_icon.png" alt="">
        <span class="od_time_txt">剩余收款时间：</span>
        <div class="od_time_num">
          <van-count-down :time="pageData.pageData.surplusTime"/>
        </div>
      </div>
      <div class="od_body">
        <ul>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">订单编号</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">{{pageData.orderId}}</span>
              <img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(pageData.orderId)"
                   class="customer_box_btn ml20">
            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">买方</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">{{pageData.nickname}}</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">手机号码</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">{{pageData.phone}}</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">购买数量</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">{{pageData.priceTxt}}</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">单价</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">{{pageData.amountTxt}}</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">币种</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">{{pageData.btypeTxt}}</span>

            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">交易码</span>
            <div class="od_li_r row ac">
              <span lass="od_li_txt">{{pageData.tradeCode}}</span>
              <img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(pageData.tradeCode)"
                   class="customer_box_btn ml20">
            </div>
          </li>
          <li class="od_body_li row jb ac">
            <span class="od_li_title">关闭原因</span>
            <div class="od_li_r row ac">
              <!--<span lass="od_li_txt">{{pageData.tradeCode}}</span>-->
              <span lass="od_li_txt">暂无</span>
            </div>
          </li>
        </ul>
      </div>

      <!--提示信息模块-->
      <div class="od_tip_box" v-if="pageType==1">
        <div class="od_tips" v-if="pageData.state!=30">温馨提示：</div>
        <div class="od_tip" v-if=" pageData.state== 0">{{tipTxt[0]}}</div>
        <div class="od_tip" v-if="pageData.state== 10">{{tipTxt[1]}}</div>
        <div class="od_tip" v-if="pageData.state== -1">{{tipTxt[2]}}</div>
      </div>
      <div class="od_tip_box" v-if=" pageType==2">
        <div class="od_tips" v-if="pageData.state!=30 && pageData.status!=-1">温馨提示：</div>
        <div class="od_tip" v-if=" pageData.state== 0">{{tipTxt[3]}}</div>
        <div class="od_tip" v-if="pageData.state== 10">{{tipTxt[4]}}</div>
      </div>
      <div class="od_tip_btnBox">
        <div class="od_tip_btn btn_type6" v-if="pageType==1 && pageData.state==0"
             @click="cancelOrder(pageData.orderId)">{{benTxt[0]}}
        </div>
        <div class="od_tip_btn btn_type2" v-if="pageType==2 && pageData.state==10"
             @click="confirmReceipts(pageData.orderId)">{{benTxt[1]}}
        </div>
        <div class="od_tip_btn btn_type6" v-if="pageType==1 && pageData.state==-1" @click="goCustomer">{{benTxt[2]}}
        </div>
      </div>
    </div>
  </div>

</template>

<script>
  import pageServe from '@/api/page'

  export default {
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        time: 30 * 60 * 60 * 1000,
        pageData: {},
        pageType: '', // 1.我买的跳转进入 2.我卖的跳转进入
        benTxt: ['取消订单', '确认收款', '订单异常'],
        tipTxt: [
          '如卖家在规定的时间内没有向平台转币，交易将自动关闭。',
          '如在规定的付款时间内卖家没有确认收款，订单将自动取消，请您及时转款并督促卖家收款后及时收款。',
          '如已向卖家转款但卖家未确认收款，请您及时联系平台客服。',
          '请在规定的时间内向平台钱包地址转币，转币时请备注订单号，超时未转币交易将自动关闭。',
          '1.如在规定时间内买家未付款，请及时联系买家；2.如已收到转款请及时确认收款']
      }
    },
    created() {
      this.pageData = JSON.parse(this.$route.query.pageData)
      this.pageType = this.$route.query.pageType
      console.log(this.pageData);
      console.log(this.pageType);

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
      cancelOrder(orderId) {
        const that = this
        this.$dialog({
          title: '确认取消该订单？'
        }).then(res => {
            that.cancelOrderRequest(orderId)
          },
          err => {

          })
      },
      async cancelOrderRequest(orderId, index) {
        const {data, errorCode, message} = await pageServe.cancelOrder(orderId)
        console.log('取消订单', data);
        if (errorCode == '0000') {
          const that = this
          this.$toast({message: '取消已取消', duration: '1500'})
          setTimeout(function () {
            that.$router.go(-1)
          }, 1500)
        } else {
          this.$toast({message: message, duration: '1500'})
        }
      },
      confirmReceipts(orderId) {
        const that = this
        this.$dialog({
          title: '确认取消该订单？'
        }).then(res => {
            that.confirmReceiptsRequest(orderId, index)
          },
          err => {

          })
      },
      async confirmReceiptsRequest(orderId) {
        const {data, errorCode, message} = await pageServe.confirmReceipts(orderId)
        console.log('确认收款成功数据', data);
        if (errorCode == '0000') {
          this.$toast({message: '确认收款成功，订单完成', duration: '1500'})
        } else {
          this.$toast({message: message, duration: '1500'})
        }
      },
      goCustomer() {
        this.$router.push({name: 'customer'})
      },
    },

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

    .od_li_r {
      font-size: 0.28rem;
      color: #333;
      padding-right: 0.2rem;
    }

    .od_body_li {
      padding: 0.2rem 0;
      border-top: 1px solid #ddd;
    }

    .od_body_li:first-child {
      border-top: none;
    }

    .od_li_title {
      font-size: 0.28rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(102, 102, 102, 1);
    }

    .od_li_txt {
      font-size: 0.28rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
    }

    .od_tip {
      padding: 0 0.2rem 0.2rem;
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(255, 0, 0, 1);
      line-height: 0.36rem;
    }

    .od_tips {
      padding: 0.2rem 0.2rem 0;
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(255, 0, 0, 1);
      /*line-height:0.36rem;*/
    }

    .od_tip_btn {
      width: 3.64rem;
      margin: 0.57rem auto;
    }


  }

</style>
