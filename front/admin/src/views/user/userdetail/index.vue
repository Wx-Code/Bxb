<template>
  <section class="app-container">
    <div class="header">
      <h2>用户详情</h2>
      <el-button type="primary" size="small" icon="el-icon-arrow-left" @click="back" class="back">返回</el-button>
    </div>
    <hr>
    <div class="tab_box">
      <div class="tab_box_item">
        <div class="tab_box_itemIn">
          <div class="item_left">微信昵称：</div>
          <div class="item_right">{{userInfoData.nickname}}</div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">微信二维码图片：</div>
          <div class="item_right">
            <img :src="userInfoData.wxCodePhoto" class="imgheight50">
          </div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">头像：</div>
          <div class="item_right">
            <img :src="userInfoData.avatar" class="imgheight50">
          </div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">用户姓名：</div>
          <div class="item_right">{{userInfoData.realname}}</div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">手机号：</div>
          <div class="item_right">{{userInfoData.phone}}</div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">钱包地址：</div>
          <div class="item_right">{{userInfoData.walletAddress}}</div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">累计出售：</div>
          <div class="item_right">{{userInfoData.outTotalAmount}}</div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">累计购买：</div>
          <div class="item_right">{{userInfoData.inTotalAmount}}</div>
        </div>
        <div class="tab_box_itemIn">
          <div class="item_left">注册时间：</div>
          <div class="item_right">{{userInfoData.createdTime}}</div>
        </div>
      </div>
    </div>

  </section>
</template>
<script>
import userApi from "@/api/user";
import "@/styles/demo.css";
export default {
  data() {
    return {
      puserId: this.$route.query.puserId,
      listLoading: false,
      userInfoData:{}
    };
  },
  methods: {
   
    loadList() {
      this.listLoading = true;
      this.$progress.start();
      userApi.getUserDetail(this.puserId).then(res => {
        this.$log("[info] in loadList----", res);
        this.$progress.done();
        this.listLoading = false;
        if(res.success==true){
          this.userInfoData = res.data;
        }
      });
    },

    back() {
      this.$router.go(-1);
    },
  },

  // 初次加载
  async mounted() {
    this.loadList();
  }
};
</script>
<style lang="scss" scoped>
.back {
  float: right;
  margin-top: -35px;
}
.dtitle {
  background-color: #eaeaea;
  padding: 10px 30px;
  font-size: 16px;
  font-weight: bold;
}
.mt50 {
  margin-top: 50px;
}
.input200 {
  width: 200px;
}
.formmargin {
  margin: 20px 30% 0px 30%;
}
.el-form-item {
  margin-bottom: 0px;
}
.imgheight50 {
  height: 50px;
}
</style>
