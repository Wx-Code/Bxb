<template>
  <section>
    <div class="header-ae">
      <h2 class="h2-css">交易规则配置</h2>
    </div>
    <hr>
    <br>
    <br>
    <el-form :model="transactionRules" label-width="300px" class="formpadding">
      <el-form-item  label="交易规则：">
        <el-input class="input-txt" autosize type="textarea" v-model="transactionRules" placeholder="请输入交易规则">
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
      transactionRules:null,
      query: {
        configName: "TradeRules",
        configValue: "",
        remark: "交易规则配置",
      }
    };
  },
  methods: {
    lnitialloading() {
      configApi.get(this.query.configName).then(res => {
        this.$log("LoadConfig Response Result is :", res);
        if (res.success == true) {
          this.transactionRules = res.data.configValue;
        } else {
          this.$error(res.message);
        }
      });
    },

    onSubmit() {

      if(this.transactionRules==null||this.transactionRules==''||this.transactionRules==undefined)
      {
        this.$error("请输入交易规则")
        return false
      }

      this.query.configValue = this.transactionRules
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
  height: 70%;
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

</style>
