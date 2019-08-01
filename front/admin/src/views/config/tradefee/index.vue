<template>
  <section>
    <div class="header-ae">
      <h2 class="h2-css">手续费配置</h2>
    </div>
    <hr>
    <br>
    <br>
    <el-form :model="model" label-width="300px" class="formpadding">
      <el-form-item  label="单笔转币手续费：">
        <el-input class="input-txt" type="number" v-model="model.SigleTradeFee" placeholder="请输入单笔转币手续费">
          <template slot="append"></template>
        </el-input>
      </el-form-item>
      <el-form-item label="单笔转币服务费：">
        <el-input class="input-txt" type="number" v-model="model.SigleServiceFee" placeholder="请输入单笔转币服务费">
          <template slot="append">%</template>
        </el-input>
        <br>
        <span class="tips">（允许填写的范围为:0-100）</span>
        <br>
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
        SigleTradeFee: 0,
        SigleServiceFee: 0
      },
      query: {
        configName: "TradeFee",
        configValue: "",
        remark: "手续费配置",
      }
    };
  },
  methods: {
    lnitialloading() {
      configApi.get(this.query.configName).then(res => {
        this.$log("LoadConfig Response Result is :", res);
        if (res.success == true) {
          this.model = JSON.parse(res.data.configValue);
          this.model.SigleServiceFee=this.model.SigleServiceFee*100
        } else {
          this.$error(res.message);
        }
      });
    },

    onSubmit() {
         
      if(this.model.SigleServiceFee!=null&&(this.model.SigleServiceFee>100||this.model.SigleServiceFee<0))
      {
          this.$error("单笔转币服务费百分比输入不合理")
          return false
      }

      const postData = Object.assign({}, this.model);

      postData.SigleServiceFee=postData.SigleServiceFee/100

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
