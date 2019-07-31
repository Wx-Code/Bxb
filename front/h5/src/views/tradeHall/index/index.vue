<template>

  <van-pull-refresh v-model="isLoading" @refresh="onRefresh" class="integral">

    <van-list
      v-model="loading"
      :finished="finished"
      :immediate-check = 'false'
      finished-text="没有更多了"
      @load="getTradeHallLsit()"
    >
      <div class="tradeHall">
        <div class="tH_content">
          <div class="tH_item" v-for="(item,index) in list" @click="goDetail()">
            <div class="tH_item_box row jb">
              <img :src="item.avatar" alt="" class="tH_user_logo">
              <div class="tH_item_r">
                <div class="tH_item_title">{{item.nickname}}</div>
                <div class="tH_item_time">{{item.releaseTime}}</div>
                <div class="tH_item_info row spa">
                  <div class="tH_item_info1"><span class="tH_info1_txt">可售数量</span><span class="tH_info1_num">{{item.amount}}个</span>
                  </div>
                  <div class="tH_item_info2"><span class="tH_info1_txt">单价</span><span class="tH_info1_num">{{item.price}}元</span>
                  </div>
                  <div class="tH_item_info3"><span class="tH_info1_txt">币种</span><span class="tH_info1_num">{{item.bTypeText}}</span>
                  </div>
                </div>
                <div class="tH_item_btnBox row_r ">
                  <div class="tH_btn_sty btn_type3" @click="goBuy(item)">购买</div>
                  <div class="tH_btn_sty btn_type4" @click="goLink(item.wxCodePhoto)">去沟通</div>
                </div>
              </div>

            </div>
          </div>

        </div>
        <div class="tH_go_send">
          <img src="http://static.pinlala.com/bxb/goSend.png" alt="" @click="goSend" class="tH_go_send">
        </div>
        <!--提示框组件-->
        <dialogTemplete v-model="dialog.dialogShow"
                        :dialogShow="dialog.dialogShow"
                        :showTitle="dialog.showTitle"
                        :showBtn="dialog.showBtn"
                        :title="dialog.title"
                        :position="dialog.position"
                        :confirmText="dialog.confirmText"
                        :showCancel="dialog.showCancel"
                        :cancelText="dialog.cancelText"
                        @cancel="typeof dialog.cancel === 'function' ? typeof dialog.cancel() : ''"
                        @confirm="typeof dialog.confirm === 'function' ? typeof dialog.confirm() :''">
          <!--联系人微信-->
          <div class="tH_link" v-if="temp==1">
            <img :src="wxCodePhoto" alt="" class="tH_link_code">
            <div class="tH_link_tip">长按扫描二维码添加联系人</div>
          </div>
          <!--填写钱包地址-->
          <div class="tH_add_address" v-if="temp==2">
            <div class="tH_address_tit">购买前前请完善您的XXXX钱包地址</div>
            <div class="tH_address_box row jb "><span class="tH_address_span">XXX钱包地址：</span>
              <div class="tH_address_input"><input type="text" class="tH_address_inputs" placeholder="请输入您的XXX钱包地址"></div>
            </div>
          </div>
          <!--提交订单 -->
          <div class="tH_submit" v-if="temp==3">
            <div class="tH_submit_boxs">
              <div class="tH_submit_box row jb "><div class="tH_submit_span txt_j">购买数量：</div>
                <div class="tH_submit_input"><input type="text" v-model="buyNum" class="tH_address_inputs" :placeholder="'当前剩余数量'+ amount"></div>
              </div>
              <div class="tH_submit_box row jb "><div class="tH_submit_span txt_j">交易码：</div>
                <div class="tH_submit_input"><input type="text" v-model="sellCode" class="tH_address_inputs" placeholder="请输入交易码"></div>
              </div>
            </div>

            <div class="tH_tip">*购买数量不能超过xx个，请重新输入</div>
          </div>
        </dialogTemplete>
      </div>

    </van-list>
  </van-pull-refresh>

</template>

<script>
  import store from '@/utils/local-store'
  import dialogTemplete from '@/components/dialogTemplete/dialogTemplete.vue'
  import pageServe from '@/api/page'
  import userServe from '@/api/user'
  export default {
    components: {
      dialogTemplete,
    },
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        isHasAddress:false,
        buyNum:'',//购买数量
        sellCode:'',//交易吗
        amount:'',//交易吗
        query: {
          page: 1,
          size: 10
        },
        isLoading: false,
        list:[], //数组
        loading: false,
        finished: false,
        temp: false,
        wxCodePhoto:'',//联系人的二维码图片
        dialog: {
          dialogShow: false,
        }
      }
    },
    async created(){
      await  this.getUserInfo()
      await  this.judgeIsHasAddress()
      await  this.getTradeHallLsit()
    },
    methods: {
     async getTradeHallLsit(){
       const that = this
       this.loading = true
       const { success, data, message } = await pageServe.getTradeHallList(this.query)
       const { list } = data
       this.loading = false
       this.query.page++
       if (success == false) {
         that.$toast(message)
         return
       }
       this.list = this.list.concat(list)
       if (!list || list.length < this.query.size) {
         that.finished = true
       }
      },
      // 下拉刷新
      async onRefresh() {
        this.query.page = 1
        this.list = []
        await this.getUserInfo()
        await this.getTradeHallLsit()
        this.isLoading = false
      },
      goCustomer() {
        this.$router.push({name: 'customer'})
      },
      goSend() {
        this.$router.push({name: 'publishInformation'})
      },
      goLink(src) {
        this.temp = 1
        this.wxCodePhoto = src
        this.dialog = {
          showBtn: false,
          dialogShow: true,
          showTitle: false,
          position: 'center',
          confirm() {
          }
        }
      },
      goBuy(itemData){
        if(!this.isHasAddress){
          this.showAddAress(itemData)
          return
        }
        this.showOrderCommit(itemData)

      },
      showOrderCommit(itemData){
       const  that =this
        this.amount = itemData.amount
        this.temp = 3
        this.dialog = {
          title:'提交订单',
          showCancel: false,
          confirmText: '提交订单',
          dialogShow: true,
          confirm() {
            that.orderCommitRequest(itemData)
          }
        }
      },
      showAddAress(itemData){
        this.temp = 2
        this.dialog = {
          showCancel: false,
          confirmText: '保存',
          dialogShow: true,
          position: 'bottom',
          confirm() {
          }
        }

      },
      orderCommitRequest(itemData){

      },
      async getUserInfo() {
        const { data,errorCode } = await userServe.getUserInfo()
        if (!data) return false
        if (errorCode == '0000') {
          this.userInfo = data
          store.setUser(this.userInfo)
        }
      },
      judgeIsHasAddress() {
        if (this.userInfo.walletAddress && this.userInfo.walletAddress.length > 0) {
          this.isHasAddress = true

        }
      }



    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .tradeHall {
    background: #F9F9F9;
    min-height: 100%;

    .tH_content {


    }

    .tH_item {
      margin-bottom: 0.2rem;
      background: #fff;
      padding-left: 0.25rem;
      padding-top: 0.27rem;
      padding-bottom: 0.22rem;
      border-top: 1px solid #ddd;
      border-bottom: 1px solid #ddd;
    }

    .tH_item_box {
      width: 100%;
    }

    .tH_user_logo {
      width: 1rem;
      height: 1rem;
      border-radius: 50%;
    }

    .tH_item_r {
      width: 5.8rem;


    }

    .tH_item_title {
      font-size: 0.3rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
      line-height: 0.42rem;
    }

    .tH_item_time {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(153, 153, 153, 1);
      line-height: 0.33rem;
    }

    .tH_item_info {
      padding-bottom: 0.3rem;
      margin-top: 0.21rem;
      border-bottom: 1px solid #d0d0d0;
    }

    .tH_item_info1 {
      width: 40%;
    }

    .tH_item_info2 {
      width: 35%;
    }

    .tH_item_info3 {
      width: 25%;
    }

    .tH_info1_txt {
      font-size: 0.26rem;
      box-sizing: border-box;
      padding-right: 0.1rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(102, 102, 102, 1);
    }

    .tH_info1_num {
      font-size: 0.26rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(85, 133, 228, 1);
    }

    .tH_item_btnBox {
      padding-top: 0.2rem;
    }

    .tH_btn_sty {
      margin-right: 0.2rem;
      width: 1.31rem;
    }

    .tH_go_send {
      width: 1.34rem;
      height: 1.34rem;
      position: fixed;
      right: 0.13rem;
      bottom: 2rem;
    }

    .tH_link_code {
      width: 4.2rem;
      height: 4.2rem;
      margin: 0 auto;
    }

    .tH_link_tip {
      width: 100%;
      box-sizing: border-box;
      font-size: 0.26rem;
      text-align: center;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      box-sizing: border-box;
      margin-top: 0.45rem;
      color: rgba(51, 51, 51, 1);
    }

    /*添加钱包地址弹窗*/
    .tH_address_tit {
      font-size: 0.26rem;
      text-align: center;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(102, 102, 102, 1);
    }

    .tH_address_span {
      font-size: 0.28rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
    }

    .tH_address_box {
      padding: 0.63rem 0 0.8rem;
    }
    .tH_address_input{
      width: 4.3rem;
      height: 0.56rem;
      padding-bottom: 0.1rem;
      border-bottom: 1px solid #ddd;
    }
    .tH_address_inputs{
      width: 100%;
      height: 0.4rem;
      color: #333;
      font-size: 0.28rem;
      line-height: 0.4rem;
    }
    input::-webkit-input-placeholder {
      font-size: 0.28rem;
      color: #CCC;
    }

    /*提交订单弹窗*/
    .tH_submit{
        width: 4.8rem;
      margin: 0 auto;
      padding-top: 0.36rem;
      box-sizing: border-box;
    }
    .tH_submit_span{
      width: 1.6rem;
      font-size:0.3rem;
      font-family:PingFangSC-Medium;
      font-weight:500;
      color:rgba(51,51,51,1);
    }
    .tH_submit_box{
      margin-bottom: 0.57rem;
    }
    .tH_submit_box:last-child{
      margin-bottom: 0.1rem;
    }
    .tH_submit_input{
      width: 2.96rem;
      height: 0.56rem;
      padding-bottom: 0.1rem;
      border-bottom: 1px solid #ddd;
    }
    .tH_tip{
      font-size:0.24rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(222,0,0,1);
      line-height:0.35rem;
    }

  }
</style>
