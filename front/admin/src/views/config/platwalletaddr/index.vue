<template>
  <section class="app-container">
    <div class="tool-bar mb10">
      <div class="header-platwallet">
        <h2 class="platwallet-css">平台钱包地址配置</h2>
      </div>
    </div>

    <div class="turnconin-div">
      <div class="header-platwallet">
        <h3 class="platwallet-css">用于转币</h3>
      </div>
      <el-form :model="PlatWalletTrunCoinModel">
        <el-form-item label="地址名称：" >
            <el-input  class="input-100" v-model.trim="PlatWalletTrunCoinModel.PlatWalletAddrName" placeholder="地址名称" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="钱包地址：">
            <el-input class="input-100" v-model.trim="PlatWalletTrunCoinModel.PlatWalletAddr" placeholder="钱包地址" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="Cookie:">
            <el-input class="input-100" v-model.trim="PlatWalletTrunCoinModel.Cookie" placeholder="Cookie" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="IET钱包ID:">
            <el-input class="input-100" v-model.trim="PlatWalletTrunCoinModel.WalletId" placeholder="IET钱包ID" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="交易密码:">
            <el-input class="input-100" v-model.trim="PlatWalletTrunCoinModel.Password" placeholder="交易密码" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="电话:">
            <el-input class="input-100" v-model.trim="PlatWalletTrunCoinModel.Phone" placeholder="电话" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="地址状态：">
            <el-radio-group v-model="PlatWalletTrunCoinModel.State">
                <el-radio :label="0">启用</el-radio>
                <el-radio :label="1">停用</el-radio>
            </el-radio-group> 
        </el-form-item>

        <el-form-item>
          <el-button class="baocun-btn" type="primary" @click="handleSubmit(1)">保存</el-button>
        </el-form-item>

      </el-form>      
    </div>

     <div class="commissionservice-div">
      <div class="header-platwallet">
        <h3 class="platwallet-css">用于手续费</h3>
      </div>
      <el-form :model="PlatWalletCommissionServicModel">
        <el-form-item label="地址名称：" >
            <el-input  class="input-100" v-model.trim="PlatWalletCommissionServicModel.PlatWalletAddrName" placeholder="地址名称" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="钱包地址：">
            <el-input class="input-100" v-model.trim="PlatWalletCommissionServicModel.PlatWalletAddr" placeholder="钱包地址" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="Cookie:">
            <el-input class="input-100" v-model.trim="PlatWalletCommissionServicModel.Cookie" placeholder="Cookie" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="IET钱包ID:">
            <el-input class="input-100" v-model.trim="PlatWalletCommissionServicModel.WalletId" placeholder="IET钱包ID" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="交易密码:">
            <el-input class="input-100" v-model.trim="PlatWalletCommissionServicModel.Password" placeholder="交易密码" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="电话:">
            <el-input class="input-100" v-model.trim="PlatWalletCommissionServicModel.Phone" placeholder="电话" type="text"></el-input>
        </el-form-item>
        <el-form-item  label="地址状态：">
            <el-radio-group v-model="PlatWalletCommissionServicModel.State">
                <el-radio :label="0">启用</el-radio>
                <el-radio :label="1">停用</el-radio>
            </el-radio-group> 
        </el-form-item>

        <el-form-item>
          <el-button class="baocun-btn" type="primary" @click="handleSubmit(2)">保存</el-button>
        </el-form-item>
      </el-form>      
    </div>

  </section>
</template>

<script>
import configApi from '@/api/systemconfig'

export default {
  data() {
    return {
      list: [],
      loading: false,
      query: {
        configName: "PlatformWalletAddress",
        configValue: "",
        remark: "平台钱包地址配置",
      },
      PlatWalletTrunCoinModel:{},
      PlatWalletCommissionServicModel:{},
      editData:[],
      itemCount:0,
    }
    
  },    


  methods: {

    async loadPlatWalletAddrConfig() {
        configApi.get(this.query.configName).then(res => {
        this.$log("loadPlatWalletAddrConfig Response Result is :", res);
        if (res.success == true) {
          this.list = JSON.parse(res.data.configValue);
          this.list.forEach(v => {
            if(v.Purpost==1)
            {
              // 转币使用
              this.PlatWalletTrunCoinModel=v
            }
            else{
              //手续费使用
              this.PlatWalletCommissionServicModel=v
            }
          });
          this.itemCount=this.list.length

        } else {
          this.$error(res.message);
        }
      });

    },

    handleSubmit(purpost)
    {
        this.editData=this.list

        if(purpost==1)
        {
          //转币

          if(this.PlatWalletTrunCoinModel!=null&&
           (this.PlatWalletTrunCoinModel.PlatWalletAddrName==null||this.PlatWalletTrunCoinModel.PlatWalletAddrName==''
          ||this.PlatWalletTrunCoinModel.PlatWalletAddr==null||this.PlatWalletTrunCoinModel.PlatWalletAddr==''
          ||this.PlatWalletTrunCoinModel.Cookie==null||this.PlatWalletTrunCoinModel.Cookie==''
          ||this.PlatWalletTrunCoinModel.WalletId==null||this.PlatWalletTrunCoinModel.WalletId==''
          ||this.PlatWalletTrunCoinModel.Password==null||this.PlatWalletTrunCoinModel.Password==''
          ||this.PlatWalletTrunCoinModel.Phone==null||this.PlatWalletTrunCoinModel.Phone==''
          ||this.PlatWalletTrunCoinModel.State==null))

          {
            this.$error("信息填写不完整， 请完善信息")
            return false
          }

          this.PlatWalletTrunCoinModel.purpost=1
          if(this.itemCount==0)
          {
            this.PlatWalletTrunCoinModel.PlatWalletAddrId=this.itemCount+1
            this.editData.push(this.PlatWalletTrunCoinModel)
          }
          else
          {
            if(this.PlatWalletTrunCoinModel.PlatWalletAddrId==null||this.PlatWalletTrunCoinModel.PlatWalletAddrId==''||this.PlatWalletTrunCoinModel.PlatWalletAddrId==undefined)
            {

              this.PlatWalletTrunCoinModel.PlatWalletAddrId=this.itemCount+1 
              this.editData.push(this.PlatWalletTrunCoinModel)
            }
            else
            {
              this.PlatWalletTrunCoinModel.PlatWalletAddrId=this.PlatWalletTrunCoinModel.PlatWalletAddrId
              this.$set(this.editData,this.PlatWalletTrunCoinModel.PlatWalletAddrId-1,this.PlatWalletTrunCoinModel)

            }
          }

        }
        else
        {
          //服务费
          if(this.PlatWalletCommissionServicModel!=null
          && (this.PlatWalletCommissionServicModel.PlatWalletAddrName.trim()===null||this.PlatWalletCommissionServicModel.PlatWalletAddrName==''
          ||this.PlatWalletCommissionServicModel.PlatWalletAddr===null||this.PlatWalletCommissionServicModel.PlatWalletAddr==''
          ||this.PlatWalletCommissionServicModel.Cookie===null||this.PlatWalletCommissionServicModel.Cookie==''
          ||this.PlatWalletCommissionServicModel.WalletId===null||this.PlatWalletCommissionServicModel.WalletId==''
          ||this.PlatWalletCommissionServicModel.Password===null||this.PlatWalletCommissionServicModel.Password==''
          ||this.PlatWalletCommissionServicModel.Phone===null||this.PlatWalletCommissionServicModel.Phone==''
          ||this.PlatWalletCommissionServicModel.State==null))

          {
            this.$error("信息填写不完整， 请完善信息")
            return false
          }

          this.PlatWalletCommissionServicModel.purpost=2

           if(this.itemCount==0)
          {
            this.PlatWalletCommissionServicModel.PlatWalletAddrId=this.itemCount+1
            this.editData.push(this.PlatWalletCommissionServicModel)

          }
          else
          {
            if(this.PlatWalletCommissionServicModel.PlatWalletAddrId==null||this.PlatWalletCommissionServicModel.PlatWalletAddrId==''||this.PlatWalletCommissionServicModel.PlatWalletAddrId==undefined)
            {
              this.PlatWalletCommissionServicModel.PlatWalletAddrId=this.itemCount+1
              this.editData.push(this.PlatWalletCommissionServicModel)

            }
            else
            {
              this.PlatWalletCommissionServicModel.PlatWalletAddrId=this.PlatWalletCommissionServicModel.PlatWalletAddrId
              this.$set(this.editData,this.PlatWalletCommissionServicModel.PlatWalletAddrId-1,this.PlatWalletCommissionServicModel)

            }
          }

        }

        this.query.configValue = JSON.stringify(this.editData);
        this.$log("this query is:",this.query)
        configApi.addOrUpdate(this.query).then(res => {
          this.$log("AddOrUpdate Result Is： ",res)
          if (res.success) {
            this.loadPlatWalletAddrConfig()
            this.$success("操作成功")
            this.PlatWalletInfoFormVisible=false
          } else {
            this.$error("操作失败，请联系管理员");
          }
        });
    }

  }, 
  async mounted() {
    this.loadPlatWalletAddrConfig()
  }
}
</script>

<style lang="scss">
  .platwallet-css {
  position: relative;
  top: 10px;
  display: inline;
  margin-right: 65%;
  }
  .header-platwallet {
  background-color: #f2f2f2;
  height: 50px;
  margin-bottom: 20px;
   }

  .addaddr{
  position: relative;
  top: 5px;
  display: inline;
   }

   .input-100{
     width: 400px;
   }
  .turnconin-div{
    width: 50%;
    float: left;
  }

  .commissionservice-div{
    width: 50%;
    float: left;
  }
</style>
