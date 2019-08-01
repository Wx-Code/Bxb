<template>
  <section>
    <div class="header-ae">
      <h2 class="h2-css">订单时间配置</h2>
    </div>
    <hr>
    <br>
    <br>
    <el-form :model="model" label-width="300px" class="formpadding">
      <el-form-item  label="卖家转币有效时间：">
        <el-input class="input-txt" type="number" v-model="model.SellerTrunCoinTime" placeholder="请输入卖家转币有效时间">
          <template slot="append">小时</template>
        </el-input>
      </el-form-item>
      <el-form-item label="卖家确认收款有效时间:">
        <el-input class="input-txt" v-model="model.SellerReceiveMoneyTime" type="number" placeholder="请输入卖家确认收款有效时间">
          <template slot="append">小时</template>
        </el-input>
      </el-form-item>
      <el-form-item>
        <el-button class="baocun-btn" type="primary" @click="onSubmit()">保存</el-button>
      </el-form-item>
    </el-form>
  </section>
</template>
<script>
import configApi from "@/api/systemconfig"; //用户相关

export default {
  data() {
    return {
      model: {
        SellerTrunCoinTime: 0,
        SellerReceiveMoneyTime: 0
      },
      query: {
        configName: "OrderTimeLimit",
        configValue: "",
        remark: "订单时间配置",
      }
    };
  },
  methods: {
    lnitialloading() {
      configApi.get(this.query.configName).then(res => {
        this.$log("LoadConfig Response Result is :", res);
        if (res.success == true) {
          this.model = JSON.parse(res.data.configValue);
          this.model.SellerTrunCoinTime = this.calculateValidHours(
            this.model.SellerTrunCoinTime
          );
          this.model.SellerReceiveMoneyTime = this.calculateValidHours(
            this.model.SellerReceiveMoneyTime
          );
        } else {
          this.$error(res.message);
        }
      });
    },

    calculateValidMilliseconds(hours) {
      if (!hours) return null;
      return +hours *3600000;
    },

    calculateValidHours(milliseconds) {
      if (!milliseconds) return null;
      return +milliseconds / (3600000);
    },

    onSubmit() {
    
      this.$log("This Model: ", this.model)
      if(this.model.SellerTrunCoinTime==null)
      {
          this.$error("请输入卖家转币有效时间")
          return false
      }
     if(this.model.SellerReceiveMoneyTime==null)
      {
          this.$error("请输入卖家确认收款有效时间")
          return false
      }
      const postData = Object.assign({}, this.model);
      postData.SellerTrunCoinTime = this.calculateValidMilliseconds(
        this.model.SellerTrunCoinTime
      );
     postData.SellerReceiveMoneyTime = this.calculateValidMilliseconds(
        this.model.SellerReceiveMoneyTime
      );
      this.query.configValue = JSON.stringify(postData);
      this.$log("this query is:",this.query)
      configApi.addOrUpdate(this.query).then(res => {
        this.$log("AddOrUpdate Result Is： ",res)
        if (res.success) {
          this.lnitialloading()
          this.$success("操作成功");
        } else {
          this.$error("操作失败，请联系管理员");
        }
      });
    }
  },
  mounted() {
    this.lnitialloading();
  }
};
</script>
<style lang="scss" scoped>
.header-ae {
  background-color: #f2f2f2;
  height: 50px;
}
.input-txt {
  width: 50%;
}
.h2-css {
  position: relative;
  top: 10px;
  left: 10px;
}
.baocun-btn {
  margin-left: 20px;
  margin-right: 50px;
}
.tips {
  color: #af1d2a;
}
.formpadding {
  position: relative;
  left: 150px;
  top: 50px;
}
</style>
