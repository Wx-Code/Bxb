<template>
  <section class="app-container">
    <div class="tool-bar mb10">
    <div class="header-platwallet">
      <h2 class="platwallet-css">平台钱包地址配置</h2>
      <el-button type="primary" class="addaddr" icon="el-icon-plus" @click="addPlatWalletAddr()" >添加地址</el-button>
    </div>
    </div>

    <el-table :data="list" :v-loading="loading" border class="mt10">
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
          <el-button  type="text" v-if="scope.row.state==1" icon="el-icon-edit-outline" @click="handleEditState(scope.row)">停用</el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- <el-dialog title="钱包地址信息" :visible.sync="rejectDrawFormVisible">
        <el-form>
        <el-form-item prop="auditRemarkText" label="审核备注">
            <el-input
            v-model="auditRemarkText"
            placeholder="审核备注"
            type="text"
            ></el-input>
            <span><p style="color:red">以上是默认内容， 可重新编写</p></span>
        </el-form-item>

        <el-form-item prop="drawRemarkDesc" label="打款备注">
            <el-input
            v-model="drawRemarkDesc"
            placeholder="填写打款备注，最多 100 个字"
            type="textarea"
            ></el-input>
        </el-form-item>
        </el-form>
        <div slot="footer">
        <el-button type="default" @click="rejectDrawFormVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSubmitRejectDraw" :loading="acceptButtonLoding">提交</el-button>
        </div>
    </el-dialog> -->

  </section>
</template>

<script>
import configApi from '@/api/systemconfig'
export default {
  data() {
    return {
      list: [],
      loading: false,
    }
    
  },    


  methods: {

    async loadPlatWalletAddrConfig() {
      this.$progress.start()
      this.loading = true
      const res = await configApi.getPlatWalletConfigList
      this.$progress.done()

      this.$log('[info] loaded getPlatWalletConfigList', res)
      this.loading = false
      this.list = res.data
    },

    // addPlatWalletAddr()
    // {

    // },

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
</style>
