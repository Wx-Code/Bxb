<template>
  <section>
    <div class="header-ae">
      <h2 class="h2-css">客服信息配置</h2>
    </div>
    <hr>
    <br>
    <br>
    <el-form :model="model" label-width="300px" class="formpadding">
      <el-form-item label="微信客服号：">
        <el-radio-group  v-model="radioNum">
          <el-radio :label="item.wxCustomerId" :key="item.wxCustomerId"  v-for="item in model.weixinCustomerList">{{item.wxCustomerNumber}}</el-radio>
        </el-radio-group> 
        <el-button class="baocun-btn" type="primary" @click="insertCustomerNumber()">添加客服号</el-button>

      </el-form-item>


      <el-form-item  label="客服电话：">
        <el-input class="input-txt" autosize type="text" v-model="model.customerTel" placeholder="请输入客服电话">
        </el-input>
      </el-form-item>

      <el-form-item>
        <el-button class="baocun-btn" type="primary" @click="onSubmit()">保存</el-button>
      </el-form-item>
    </el-form>


    
    <el-dialog title="微信客服号信息" :model="customerData" :visible.sync="CustomerFormVisible">
        <el-form>
          <el-form-item label="微信客服号：" >
              <el-input  class="input-100" v-model="customerData.wxCustomerNumber"  placeholder="微信客服号码" type="text"></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer">
        <el-button type="default" @click="CustomerFormVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSubmit" >提交</el-button>
        </div>
    </el-dialog>
  </section>
</template>
<script>
import configApi from "@/api/systemconfig"; //用户相关

export default {
  data() {
    return {
      model: {
        weixinCustomerList:[],
        customerTel:null
        
      },
      customerData:{
        wxCustomerId:null,
        wxCustomerNumber:null,
        isChecked:false
      },
      query: {
        configName: "CustomerService",
        configValue: "",
        remark: "客服信息配置",
      },
      CustomerFormVisible:false,
      handleSubData:[],
      radioNum:null
    };
  },
  methods: {
    lnitialloading() {
      configApi.get(this.query.configName).then(res => {
        this.$log("LoadConfig Response Result is :", res);
        if (res.success == true) {
          this.model = JSON.parse(res.data.configValue);
          this.model.weixinCustomerList.forEach(v => {
            if(v.isChecked==true)
            {
              this.radioNum=v.wxCustomerId
            }
          });
        } else {
          this.$error(res.message);
        }
      });
    },

    insertCustomerNumber()
    {
      this.CustomerFormVisible=true
      this.customerData.wxCustomerId=null
      this.customerData.wxCustomerNumber=null
    },

    handleSubmit()
    {
      if(this.customerData.wxCustomerNumber==null||this.customerData.wxCustomerNumber==''||this.customerData.wxCustomerNumber==undefined)
      {
        this.$error("请输入微信客服号")
        return false
      }

      this.handleSubData=this.model

      this.$log("this.HandleSubData", this.handleSubData);
      this.$log("this.HandleSubData length", this.handleSubData.length);


      this.customerData.wxCustomerId=this.handleSubData.weixinCustomerList.length+1
    
      this.handleSubData.weixinCustomerList.push(this.customerData)
      this.query.configValue = JSON.stringify(this.handleSubData);
      this.$log("this query  is:",this.query)
      configApi.addOrUpdate(this.query).then(res => {
        this.$log("Insert CustomerNumber： ",res)
        if (res.success) {
          this.lnitialloading()
          this.CustomerFormVisible=false
          
        } else {
          this.$error("操作失败，请联系管理员");
        }
      });
      
    },

    onSubmit() {

      const postData = Object.assign({}, this.model);

      postData.weixinCustomerList.forEach(v => {
        if(v.wxCustomerId==this.radioNum)
        {
          v.isChecked=true
        }
        else
        {
          v.isChecked=false
        }
      });

      this.$log("选中之后的值:", postData.weixinCustomerList)
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
