<template>

  <van-pull-refresh v-model="isLoading" @refresh="onRefresh" class="integral">

    <van-list
      v-model="loading"
      :finished="finished"
      :immediate-check='false'
      finished-text="没有更多了"
      @load="getMySendLsitRequest()"
    >
      <div class="mySend">
        <div class="mySend_content">
          <div class="mySend_item" v-for="(item,index) in list">
            <div class="mySend_item_header row jb ac">
              <div class="mySend_header_l row"><span class="mySend_l_time">发布时间</span> <span class="mySend_l_times">{{item.releaseTime}}</span>
              </div>
              <span class="mySend_header_r1">{{item.statusText}}</span>
              <!--<span class="mySend_header_r2">已下架</span>-->

            </div>
            <div class="mySend_item_content">
              <div class="mySend_item_content1 row">
                <div class="mySend_item_box1"><span class="mySend_box1_txt1">可售数量:</span><span class="mySend_box1_txt2">{{item.amount}}个</span>
                </div>
                <div class="mySend_item_box2"><span class="mySend_box1_txt1">单价/个:</span><span class="mySend_box1_txt2">{{item.price}}</span>
                </div>
                <div class="mySend_item_box3 row_r"><span class="mySend_box1_txt2">{{item.bTypeText}}</span><span
                  class="mySend_box1_txt1">币种:</span></div>
              </div>
              <div class="mySend_item_content2 row"><span class="mySend_box1_txt1">交易码：</span><span
                class="mySend_box1_txt2">{{item.tradeCode}}</span><img src="http://static.pinlala.com/bxb/copy_btn.png"
                                                                       alt="" @click="copyCode(item.tradeCode)"
                                                                       class="customer_box_btn ml20"></div>
            </div>
            <div class="mySend_item_footer row_r" v-if="item.status==1">
              <div class="tH_btn_sty btn_type5" @click="goDown(item.tradeId)">下架</div>
              <div class="tH_btn_sty btn_type4" @click="goSend(item)">编辑</div>

            </div>
          </div>
        </div>
      </div>
    </van-list>
  </van-pull-refresh>


</template>

<script>
  import store from '@/utils/local-store'
  import pageServe from '@/api/page'

  export default {
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        query: {
          page: 1,
          size: 10
        },
        isLoading: false,
        list: [], //数组
        loading: false,
        finished: false,
      }
    },
    async created() {
      await this.init()
    },
    methods: {
      async init() {
        this.query.page = 1
        this.list = []
        await this.getMySendLsitRequest()
        this.isLoading = false
      },
      copyCode(txt) {
        const  that =this
        this.$copyText(txt).then(
          res => {
            that.$toast({message: '复制成功', duration: '1500'})
          },
          err => {
            that.$toast({message: '复制成功', duration: '复制失败'})
          }
        )
      },
      goSend(itemData) {
        this.$router.push({name: 'changeMySend',query: { pageName: 'mySend', itemData:JSON.stringify(itemData)  }})
      },
      goDown(arg) {
        const that = this
        this.$dialog({
          title: '确认取消该订单？'
        }).then(async function () {
          await that.putOffMySendRequest(arg)
          await that.init()
        })

      },
      async putOffMySendRequest(tradeId) {
        const that = this
        this.loading = true
        const {success, data, errorCode, message} = await pageServe.putOffMySend({tradeId: tradeId})
        if (errorCode == '0000') {
          that.$toast({message: '下架成功', duration: '1500'})
        } else {
          that.$toast({message: '下架失败', duration: '1500'})
        }
      },
      async getMySendLsitRequest() {
        const that = this
        this.loading = true
        const {success, data, message} = await pageServe.getMySendLsit(this.query)
        const {list} = data
        this.loading = false
        this.query.page++
        if (success == false) {
          // that.$toast(message)
          return
        }
        this.list = this.list.concat(list)
        if (!list || list.length < this.query.size) {
          that.finished = true
        }
      },
      // 下拉刷新
      async onRefresh() {
        this.init()
      },
    },

  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .mySend {
    background: #F9F9F9;
    min-height: 100%;

    .mySend_item {
      border-top: 1px solid #ddd;
      border-bottom: 1px solid #ddd;
      background: #fff;
      padding-left: 0.2rem;
      margin-bottom: 0.22rem;
    }

    .mySend_l_time {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: #666666;
    }

    .mySend_header_l {
      height: 0.6rem;
      line-height: 0.6rem;
    }

    .mySend_l_times {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      padding-left: 0.1rem;
      color: #333300;
    }

    .mySend_item_header {
      padding-right: 0.1rem;
      border-bottom: 1px solid #ddd;
    }

    .mySend_header_r1 {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(215, 2, 2, 1);
    }

    .mySend_header_r2 {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(153, 153, 153, 1);
    }

    .mySend_item_content {
      padding: 0.3rem 0.2rem 0.2rem 0;
      box-sizing: border-box;
    }

    .mySend_item_box1 {
      width: 42%;

    }

    .mySend_item_box2 {
      width: 33%;
    }

    .mySend_item_box3 {
      width: 25%;
    }

    .mySend_box1_txt1 {
      font-size: 0.28rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(102, 102, 102, 1);
    }

    .mySend_box1_txt2 {
      font-size: 0.28rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
    }

    .tH_btn_sty {
      margin-right: 0.2rem;
      width: 1.31rem;
    }

    .mySend_item_footer {
      border-top: 1px solid #ddd;
      padding: 0.2rem 0;
    }

    .mySend_item_content2 {
      margin-top: 0.2rem;
    }


  }

</style>
