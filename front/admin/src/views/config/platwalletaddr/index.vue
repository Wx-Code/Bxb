<template>
  <section class="app-container">
    <div class="tool-bar mb10">
    <div class="header-platwallet">
      <h2 class="platwallet-css">平台钱包地址配置</h2>
      <el-button type="primary" class="addaddr" icon="el-icon-plus" @click="addPlatWalletAddr()" >添加地址</el-button>
    </div>
    </div>

    <el-table :data="list" :v-loading="loading" border class="mt10">
      <el-table-column label="ID" prop="platWalletAddrId"></el-table-column>
      <el-table-column label="地址名称" prop="platWalletAddrName"></el-table-column>
      <el-table-column label="钱包地址" prop="platWalletAddr"></el-table-column>
      <el-table-column label="TOKEN" prop="token"></el-table-column>
      <el-table-column label="用途" prop="purpost">
        <template slot-scope="scope">
        <el-tag v-if="scope.row.purpost == 1">转币</el-tag>
        <el-tag v-else>手续费</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="状态" prop="state">
        <template slot-scope="scope">
        <el-tag v-if="scope.row.state == 1" type="danger">已停用</el-tag>
        <el-tag v-else type="success">使用中</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" min-width="150px">
        <template slot-scope="scope"> 
          <el-button  type="text" icon="el-icon-edit-outline" @click="handleEditPlatWalletAddr(scope.row)">编辑</el-button>
          <el-button  type="text" v-if="scope.row.state==1" icon="el-icon-edit-outline" @click="handleEditState(scope.row)">启用</el-button>
          <el-button  type="text" v-if="scope.row.state==0" icon="el-icon-edit-outline" @click="handleEditState(scope.row)">停用</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog title="平台钱包地址信息" :model="PlatWalletData" :visible.sync="PlatWalletInfoFormVisible">
        <el-form>
          <el-form-item label="地址名称：" >
              <el-input  class="input-100" v-model="PlatWalletData.platWalletAddrName" placeholder="地址名称" type="text"></el-input>
          </el-form-item>
          <el-form-item  label="钱包地址：">
              <el-input class="input-100" v-model="PlatWalletData.platWalletAddr" placeholder="钱包地址" type="text"></el-input>
          </el-form-item>
          <el-form-item  label="Toekn:">
              <el-input class="input-101" v-model="PlatWalletData.token" placeholder="Token" type="text"></el-input>
          </el-form-item>
          <el-form-item  label="地址状态：">
              <el-radio-group v-model="PlatWalletData.state">
                  <el-radio :label="0">启用</el-radio>
                  <el-radio :label="1">停用</el-radio>
              </el-radio-group>
          </el-form-item>
          <el-form-item label="地址用途：">
              <el-radio-group v-model="PlatWalletData.purpost">
                  <el-radio :label="1">转币</el-radio>
                  <el-radio :label="2">手续费</el-radio>
              </el-radio-group>
          </el-form-item>
        </el-form>
        <div slot="footer">
        <el-button type="default" @click="PlatWalletInfoFormVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSubmit" >提交</el-button>
        </div>
    </el-dialog>

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
      PlatWalletInfoFormVisible:false,
      PlatWalletData:{
        platWalletAddrId:null,
        platWalletAddrName:null,
        platWalletAddr:null,
        state:null,
        purpost:null,
        token:null
      },
      editData:[],
      platData:[],
      itemCount:0,
      strMsg:null
    }
    
  },    


  methods: {

    async loadPlatWalletAddrConfig() {
        configApi.get(this.query.configName).then(res => {
        this.$log("loadPlatWalletAddrConfig Response Result is :", res);
        if (res.success == true) {
          this.list = JSON.parse(res.data.configValue);
          this.itemCount=this.list.length
          this.$log("this list is ", this.list)
        } else {
          this.$error(res.message);
        }
      });

    },

    addPlatWalletAddr(value)
    {
        this.PlatWalletInfoFormVisible=true
        this.PlatWalletData.platWalletAddrId=null
        this.PlatWalletData.platWalletAddrName=null
        this.PlatWalletData.platWalletAddr=null
        this.PlatWalletData.state=null
        this.PlatWalletData.purpost=null
        this.PlatWalletData.token=null
    },


    handleEditPlatWalletAddr(row)
    {
      this.PlatWalletInfoFormVisible=true
      this.PlatWalletData.platWalletAddrId=row.platWalletAddrId
      this.PlatWalletData.platWalletAddrName=row.platWalletAddrName
      this.PlatWalletData.platWalletAddr=row.platWalletAddr
      this.PlatWalletData.state=row.state
      this.PlatWalletData.purpost=row.purpost
      this.PlatWalletData.token=row.token
    },

    //停用| 启用
    handleEditState(row)
    {
      this.$log("row" ,row) 
      this.platData=this.list;
      this.$log("this list is", this.platData)
      this.$confirm('是否确定执行该操作？','提示',{
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
      }).then(()=>{

         if(row.state==1)
          {
            //需要启用
            this.$set(this.platData[row.platWalletAddrId-1],'state',0)
          }
          else
          {
            //需要停用
            this.$set(this.platData[row.platWalletAddrId-1],'state',1)
          }
          this.query.configValue = JSON.stringify(this.platData);    

          configApi.addOrUpdate(this.query).then(res=>{
              if(res.success=true)
              {
                  this.$message({
                      type: 'success',
                      message: '操作成功!'
                  });
                  this.loadPlatWalletAddrConfig()
              }
              else
              {
                  this.$error(res.message);
              }
          })
      }).catch(() => {})
    },

    handleSubmit()
    {
        this.editData=this.list
        if(this.PlatWalletData.platWalletAddrName==null)
        {
          this.$error("钱包名称不能为空值， 请重新输入")
          return false
        }
        if(this.PlatWalletData.platWalletAddr==null)
        {
          this.$error("钱包地址不能为空值， 请重新输入")
          return false
        }
        if(this.PlatWalletData.token==null)
        {
          this.$error("token不能为空值， 请重新输入")
          return false
        }
        if(this.PlatWalletData.state==null)
        {
          this.$error("请选择地址状态")
          return false
        }
        if(this.PlatWalletData.purpost==null)
        {
          this.$error("请选择地址用途")
          return false
        }

        if(this.PlatWalletData.platWalletAddrId==null||this.PlatWalletData.platWalletAddrId==""||this.PlatWalletData.platWalletAddrId==undefined)
        {
           this.PlatWalletData.platWalletAddrId=this.itemCount+1
           this.editData.push(this.PlatWalletData)
        }
        else
        {
          this.PlatWalletData.platWalletAddrId=this.PlatWalletData.platWalletAddrId
          this.$set(this.editData,this.PlatWalletData.platWalletAddrId-1,this.PlatWalletData)
        }

        this.$log("postdata",this.PlatWalletData.platWalletAddrId-1)
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
    .input-101{
     width: 400px;
     margin-left:25px;
   }
</style>
