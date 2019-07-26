<template>
  <div class="publishInformation">
    <ul class="pa_content">
      <li class="pa_item row ac jb" alt="" @click="goChange">
        <div class="pa_item_txt1">币       种</div>
        <div class="pa_item_r row ac">
          <div class="pa_item_txt2 wd">111</div>
          <img src="http://static.pinlala.com/bxb/ic_nav_back.png"   class="next_btn">
        </div>
      </li>
      <li class="pa_item row ac jb" >
        <div class="pa_item_txt1">出售单价/个</div>
        <div class="pa_item_r row ac">
          <div class="pa_item_txt2 wd row_r"><span>元</span><input type="text" placeholder="请输入出售单价/个" class="pa_item_input"></div>
        </div>
      </li>
      <li class="pa_item row ac jb"  >
        <div class="pa_item_txt1">出售数量</div>
        <div class="pa_item_r row ac">
          <div class="pa_item_txt2 wd row_r"> <span>个</span><input type="text" placeholder="请输入出售数量" class="pa_item_input"></div>
        </div>
      </li>
        <li class="pi_btn btn_type2" @click="goSend">发布</li>
    </ul>
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
      <!--填写钱包地址-->
      <div class="tH_add_address" v-if="temp==1">
        <div class="tH_address_tit">购买前前请完善您的XXXX钱包地址</div>
        <div class="tH_address_box row jb "><span class="tH_address_span">XXX钱包地址：</span>
          <div class="tH_address_input"><input type="text" class="tH_address_inputs" placeholder="请输入您的XXX钱包地址"></div>
        </div>
      </div>
      <div class="tH_add_success" v-if="temp==2">
        <div class="tH_success_box row jc "><span class="tH_success_span">交易码：KHGF7HY</span><img src="http://static.pinlala.com/bxb/copy_btn.png" alt="" @click="copyCode(txt)" class="customer_box_btn">
        </div>
      </div>
    </dialogTemplete>
    <!--pick组件-->
    <van-popup v-model="pickShow" position="bottom">
      <van-picker
        show-toolbar
        position="bottom"
        :columns="columns"
        @cancel="onCancel"
        @confirm="onConfirm"
      />
    </van-popup>
  </div>

</template>

<script>
  import store from '@/utils/local-store'
  import dialogTemplete from '@/components/dialogTemplete/dialogTemplete.vue'
  export default {
    components: {
      dialogTemplete,
    },
    created() {
      // this.temp = 2
      // this.dialog = {
      //   title:'发布成功',
      //   showCancel: false,
      //   confirmText: '查看消息列表',
      //   dialogShow: true,
      //   position: 'center',
      //   confirm() {
      //   }
      // }
    },

    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        addressArr: [
          {
            name: '币       种',
            address: '小犀牛'
          },{
            name: '出售单价/个',
            address: '18348093356'
          },{
            name: '出售数量',
            address: ''
          },],
        txt:111,
        temp:'',
        pickShow:false,
        moneyType:'',
        columns: ['杭州', '宁波', '温州', '嘉兴', '湖州'],
        dialog: {
          dialogShow: false,
        }
      }
    },

    methods: {
      goChange() {
        this.pickShow = true
      },
      goSend() {
        this.temp = 1
        this.dialog = {
          showCancel: false,
          confirmText: '保存',
          dialogShow: true,
          position: 'bottom',
          confirm() {
          }
        }
      },
      onConfirm(value, index) {
        this.moneyType = value
        this.pickShow =false
      },
      onCancel() {
        this.pickShow =false

      },
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
  .publishInformation {
    .pa_content {
      box-sizing: border-box;
      padding-left: 0.4rem;
      padding-right: 0.2rem;
    }

    .pa_item {
      padding-top: 0.7rem;
      padding-bottom: 0.14rem;
      border-bottom: 1px solid rgba(151, 151, 151, 0.51);
    }

    .pa_item_txt1 {
      width: 2rem;
      font-size: 0.28rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(102, 102, 102, 1)
    }

    .pa_item_txt2 {
      width: 4.7rem;
      margin-right: 0.2rem;
      text-align: right;
      font-size: 0.28rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(51, 51, 51, 1);
    }

    .pa_tit {
      font-size: 0.24rem;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(255, 0, 0, 1);
      line-height: 0.33rem;
      margin-top: 0.2rem;

    }

    .next_btn {
      width: 0.18rem;
      height: 0.32rem;
    }
    .pi_btn{
      width: 3.64rem;
      background:linear-gradient(62deg,rgba(118,119,123,1) 0%,rgba(200,200,200,1) 100%);
      box-shadow:0px 0.07rem 0.28rem 0px rgba(184,184,184,1);
      margin: 0.93rem auto;
    }
    .pa_item_input{
      width: 70%;
      padding-right: 0.1rem;
      text-align: right;
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

    /*发布成功弹窗*/
    .tH_success_span{
      padding-right: 0.05rem;
      font-size:0.3rem;
      font-family:PingFangSC-Medium;
      font-weight:500;
      color:rgba(51,51,51,1);
    }

  }

</style>
