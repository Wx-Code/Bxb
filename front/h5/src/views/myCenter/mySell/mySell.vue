<template>
  <div class="mySell">
    <van-tabs v-model="active" class="fs14" animated swipeable color="#5585E4" :lazy-render="true" :sticky="true"
              @change="itemChange"
              title-inactive-color="#333333"
              title-active-color="#5585E4">
      <van-tab v-for="(item ,inx)  in arr" :title="item.title" :key="inx">
        <div class="ms_box_items">
          <van-list
            v-model="item.loading"
            :finished="item.finished"
            :immediate-check='false'
            class="h_100"
            v-show="inx==active"
            finished-text="没有更多了"
            @load="getMyBuyLsit()"
          >
            <div class="ms_content">
              <div class="ms_content_item" v-for="(itemData,index) in item.list" :key="itemData.orderId">
                <div class="mySell_item_header row jb ac">
                  <div class="mySell_header_l row"><span class="mySell_l_time">订单编号</span> <span class="mySell_l_times">{{itemData.orderId}}</span>
                  </div>
                </div>
                <div class="mySell_item_content">
                  <div class="mySell_item_content1 row">
                    <div class="mySell_item_box1"><span class="mySell_box1_txt1">买方:</span><span
                      class="mySell_box1_txt2">{{itemData.nickname}}</span>
                    </div>
                    <div class="mySell_item_box3 row_r"><span
                      class="mySell_box1_txt2">{{itemData.priceTxt}}</span><span
                      class="mySell_box1_txt1">购买数量：</span></div>
                  </div>
                  <div class="mySell_item_content2 row"><span class="mySell_box1_txt1">交易时间：</span><span
                    class="mySell_box1_txt2">{{itemData.createTime}}</span></div>
                </div>
                <div class="mySell_item_footer row">
                  <div class="mySell_time_show row">
                    <div class="mySell_time_txt1" v-if="inx==1 && itemData.surplusTime>0">剩余收款时间：</div>
                    <div class="mySell_time_txt2" v-if="inx==1 && itemData.surplusTime>0">
                      <!--<van-count-down :time="itemData.surplusTime" @finish="timeFinished(index)" />-->
                      <van-count-down :time="itemData.surplusTime" v-if="inx==1 && itemData.surplusTime>0" @finish="timeFinished(index)"/>
                    </div>
                  </div>
                  <div class="mySell_btn_box row_r">
                    <div class="tH_btn_sty btn_type3" v-if="inx==3" @click="goCustomer">订单异常</div>
                    <div class="tH_btn_sty btn_type3" v-if="inx==1" @click="confirmReceipts(itemData.orderId,index)">确认收款
                    </div>
                    <div class="tH_btn_sty btn_type4" @click="goDetail(itemData)">详情</div>
                  </div>
                </div>
              </div>
            </div>
          </van-list>
        </div>
      </van-tab>
    </van-tabs>

  </div>

</template>

<script>
  import pageServe from '@/api/page'
  import globalData from '@/store/globalData'

  export default {
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        active: 0,
        arr: [
          {
            title: '待转币',
            query: {
              page: 1,
              size: 10,
              Status: '0'
            },
            isLoading: false,
            list: [], //数组
            loading: false,
            finished: false,
          },
          {
            title: '待收款',
            query: {
              page: 1,
              size: 10,
              Status: '10'

            },
            isLoading: false,
            list: [], //数组
            loading: false,
            finished: false,
          },
          {
            title: '已完成',
            query: {
              page: 1,
              size: 10,
              Status: '30'
            },
            isLoading: false,
            list: [], //数组
            loading: false,
            finished: false,
          },
          {
            title: '交易关闭',
            query: {
              page: 1,
              size: 10,
              Status: '-1'
            },
            isLoading: false,
            list: [], //数组
            loading: false,
            finished: false,
          },
        ]
      }
    },
    created() {
      // console.log();
      this.active = globalData.pageState
      this.getMyBuyLsit(globalData.pageState)
    },
    methods: {
      async getMyBuyLsit() {
        const that = this
        this.arr[this.active].loading = true
        const {success, data, message} = await pageServe.mySellList(this.arr[this.active].query)
        const {list} = data
        this.arr[this.active].loading = false
        this.arr[this.active].query.page++
        if (success == false) {
          that.$toast(message)
          return
        }
        this.arr[this.active].list = this.arr[this.active].list.concat(list)
        // console.log(this.arr);
        if (!list || list.length < this.arr[this.active].query.size) {
          this.arr[this.active].finished = true
        }

      },
      confirmReceipts(orderId, index) {
        const that = this
        this.$dialog({
          title: '确认收款',
          content_txt:'请您确保收款金额与您沟通的金额一致，确认收款后平台将向买方转币。',
          cancelText: '再想想',
          confirmText: '提交订单',
        }).then(res => {
            that.confirmReceiptsRequest(orderId, index)
          },
          err => {

          })
      },
      async confirmReceiptsRequest(orderId, index) {
        const {data, errorCode, message} = await pageServe.confirmReceipts(orderId)
        console.log('确认收款成功数据', data);
        if (errorCode == '0000') {
          this.$toast({message: '确认收款成功，订单完成', duration: '1500'})
          this.arr[this.active].list.splice(index, 1)
        } else {
          this.$toast({message: message, duration: '1500'})
        }
      },
      itemChange(index) {
        // console.log(document.body.offsetHeight);
        this.arr[index].query.page = 1
        this.arr[index].isLoading = false
        this.arr[index].loading = false
        this.arr[index].list = []
        this.arr[index].finished = false
        this.getMyBuyLsit(index)
      },
      timeFinished(index) {
        this.arr[this.active].list.splice(index, 1)
      },
      goCustomer() {
        const  that =this
        globalData.pageState = that.active
        that.$router.push({name: 'customer'})
      },
      goDetail(itemData) {  // pageType 1.为我买到的2. 为我卖出的    pageStatus： 为页面内的几种状态值
        const  that =this
        globalData.pageState = that.active
        console.log(itemData);
        that.$router.push({name: 'orderDetail', query: { pageType: 2, pageData: JSON.stringify(itemData)}})
      },
    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>

  .mySell {
    height: 100%;
    /*overflow: scroll;*/

    .ms_content {
      height: 100%;
      /*overflow: scroll;*/
    }

    .van-count-down {
      font-size: 0.24rem;
      line-height: 0.58rem;
      font-family: PingFangSC-Medium;
      /*font-weight: bold;*/
      color: rgba(255, 0, 0, 1);
    }


    .ms_box_items {
      min-height: 11rem;
    }

    .ms_content_item {
      border-top: 1px solid #ddd;
      border-bottom: 1px solid #ddd;
      background: #fff;
      padding-left: 0.2rem;
      margin-bottom: 0.22rem;
    }

    .mySell_l_time {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: #666666;
    }

    .mySell_header_l {
      height: 0.6rem;
      line-height: 0.6rem;
    }

    .mySell_l_times {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      padding-left: 0.1rem;
      color: #333300;
    }

    .mySell_item_header {
      padding-right: 0.1rem;
      border-bottom: 1px solid #ddd;
    }

    .mySell_header_r1 {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(215, 2, 2, 1);
    }

    .mySell_header_r2 {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(153, 153, 153, 1);
    }

    .mySell_item_content {
      padding: 0.3rem 0.2rem 0.2rem 0;
      box-sizing: border-box;
    }

    .mySell_item_box1 {
      width: 45%;

    }

    .mySell_item_box3 {
      width: 50%;
    }

    .mySell_box1_txt1 {
      font-size: 0.25rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(102, 102, 102, 1);
    }

    .mySell_box1_txt2 {
      font-size: 0.25rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
    }

    .tH_btn_sty {
      margin-right: 0.2rem;
      width: 1.31rem;
    }

    .mySell_item_footer {
      border-top: 1px solid #ddd;
      padding: 0.2rem 0;
    }

    .mySell_item_content2 {
      margin-top: 0.2rem;
    }

    .mySell_time_show {
      width: 40%;
    }

    .mySell_btn_box {
      width: 60%;
    }

    .mySell_time_txt1 {
      font-size: 0.26rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(102, 102, 102, 1);
      line-height: 0.58rem;
    }

    .mySell_time_txt2 {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: #FF0000;
      line-height: 0.58rem;
    }


  }

</style>
