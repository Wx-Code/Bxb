<template>
  <div class="mySell">
    <van-tabs  v-model="active" animated swipeable color="#5585E4"  title-inactive-color="#333333" title-active-color="#5585E4">
      <van-tab v-for="(item ,inx)  in arr" :title=" item.title" :key="inx" >
        <div class="ms_content">
          <div class="ms_content_item">
              <div class="mySell_item_header row jb ac">
                <div class="mySell_header_l row"> <span class="mySell_l_time">订单编号</span> <span  class="mySell_l_times">2345678987654</span></div>
              </div>
              <div class="mySell_item_content">
                <div class="mySell_item_content1 row">
                  <div class="mySell_item_box1"><span class="mySell_box1_txt1">卖方:</span><span class="mySell_box1_txt2">小犀牛兜兜</span></div>
                  <div class="mySell_item_box3 row_r"><span class="mySell_box1_txt2">1234个</span><span class="mySell_box1_txt1">购买数量：</span></div>
                </div>
                <div class="mySell_item_content2 row"><span class="mySell_box1_txt1">交易时间：</span><span class="mySell_box1_txt2">2019-02-15 12:00:00</span></div>
              </div>
              <div class="mySell_item_footer row">
                <div class="mySell_time_show row">
                  <span class="mySell_time_txt1">剩余转币时间：</span>
                  <span class="mySell_time_txt2">12:00:00</span>
                </div>
                <div class="mySell_btn_box row_r">

                  <div class="tH_btn_sty btn_type3" >确认收款</div>
                  <div class="tH_btn_sty btn_type4" @click="goDetail" >详情</div>
                </div>



              </div>
          </div>
        </div>

      </van-tab>
    </van-tabs>

  </div>

</template>

<script>
  export default {


    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        active: 1,
        arr: [
          {
            title: '待转币'
          },
          {
            title: '待收款'
          },
          {
            title: '已完成'
          },
          {
            title: '交易关闭'
          },
        ]

      }
    },
    created() {
    },
    methods: {
      goDetail(){
        this.$router.push({name: 'orderDetail'})
      }
    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>


.mySell{
  .ms_content_item{
    border-top: 1px solid #ddd;
    border-bottom: 1px solid #ddd;
    background: #fff;
    padding-left: 0.2rem;
    margin-bottom: 0.22rem;
  }
  .mySell_l_time{
    font-size:0.24rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    color:#666666;
  }
  .mySell_header_l{
    height: 0.6rem;
    line-height: 0.6rem;
  }
  .mySell_l_times{
    font-size:0.24rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    padding-left: 0.1rem;
    color:#333300;
  }
  .mySell_item_header{
    padding-right: 0.1rem;
    border-bottom: 1px solid #ddd;
  }
  .mySell_header_r1{
    font-size:0.24rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    color:rgba(215,2,2,1);
  }
  .mySell_header_r2{
    font-size:0.24rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    color:rgba(153,153,153,1);
  }
  .mySell_item_content{
    padding: 0.3rem 0.2rem 0.2rem 0;
    box-sizing: border-box;
  }
  .mySell_item_box1{
    width: 45%;

  }
  .mySell_item_box3{
    width: 50%;
  }
  .mySell_box1_txt1{
    font-size:0.28rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    color:rgba(102,102,102,1);
  }
  .mySell_box1_txt2{
    font-size:0.28rem;
    font-family:PingFangSC-Medium;
    font-weight:500;
    color:rgba(51,51,51,1);
  }
  .tH_btn_sty {
    margin-right: 0.2rem;
    width: 1.31rem;
  }
  .mySell_item_footer{
    border-top: 1px solid #ddd;
    padding: 0.2rem 0;
  }
  .mySell_item_content2{
    margin-top: 0.2rem;
  }
  .mySell_time_show{
    width: 40%;
  }
  .mySell_btn_box{
    width: 60%;
  }
  .mySell_time_txt1{
    font-size:0.26rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    color:rgba(102,102,102,1);
    line-height:0.58rem;
  }
  .mySell_time_txt2{
    font-size:0.24rem;
    font-family:PingFangSC-Regular;
    font-weight:400;
    color:#FF0000;
    line-height:0.58rem;
  }


}

</style>
