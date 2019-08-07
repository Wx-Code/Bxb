
<template>
  <section class="app-container">
    <!-- 查询条件 -->
    <div class="tool-bar mb10">
      <el-form ref="queryForm" :model="query" inline>
        <el-form-item prop="SellerNickname">
          <el-input  v-model="query.SellerNickname" placeholder="卖家名称" clearable></el-input>
        </el-form-item>
        <el-form-item prop="BuyerNickname">
          <el-input  v-model="query.BuyerNickname" placeholder="买家名称" clearable></el-input>
        </el-form-item>
        <el-form-item prop="SellerPhone">
          <el-input  v-model="query.SellerPhone" placeholder="卖家手机号" clearable></el-input>
        </el-form-item>
        <el-form-item prop="BuyerPhone">
          <el-input  v-model="query.BuyerPhone" placeholder="买家手机号" clearable></el-input>
        </el-form-item>
          <el-form-item prop="SellerWalletAddress">
          <el-input  v-model="query.SellerWalletAddress" placeholder="卖家钱包地址" clearable></el-input>
        </el-form-item>
        <el-form-item prop="BuyerWalletAddress">
          <el-input  v-model="query.BuyerWalletAddress" placeholder="买家钱包地址" clearable></el-input>
        </el-form-item>
        <el-form-item prop="TradeCode">
          <el-input  v-model="query.TradeCode" placeholder="交易码" clearable></el-input>
        </el-form-item>
        <el-form-item prop="Status">
          <!-- <el-select v-model="query.state" placeholder="订单状态" clearable >
            <el-option v-for="item in userStates" :key="item.value" :label="item.label" :value="item.value"></el-option>
          </el-select> -->

            <el-select ref="addRequestState" @change="changetable($event)" v-model="couponSelected" >
            <el-option
              v-for="item in options"
              :key="item.value"
              :label="item.label"
              :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item prop="startTime">
          <el-date-picker v-model="query.startTime" value-format="yyyy-MM-dd" type="date" placeholder="开始时间"></el-date-picker>
        </el-form-item>
        <el-form-item prop="endTime">
          <el-date-picker v-model="query.endTime" value-format="yyyy-MM-dd" type="date" placeholder="结束时间"></el-date-picker>
        </el-form-item>
        <el-button type="primary" icon="el-icon-search" @click="loadUsers('')">查询</el-button>
        <el-button type="info" icon="el-icon-refresh" @click="handleQueryReset">重置</el-button>
      </el-form>
    </div>

    <!-- 待转币订单列表 -->
    <div id="table1" name="tables" style="display:" >
      <el-table :data="list" :v-loading="loading" border class="mt10">
        <el-table-column label="订单号" prop="orderId"></el-table-column>
        <el-table-column label="卖家名称" prop="sellerNickname"></el-table-column>
        <el-table-column label="卖家手机号码" prop="sellerPhone"></el-table-column>
        <el-table-column label="卖家钱包地址" prop="sellerWalletAddress"></el-table-column>
        <el-table-column label="交易币种" prop="btypeTxt"></el-table-column>
        <el-table-column label="交易码" prop="tradeCode"></el-table-column>
        <el-table-column label="购买数量" prop="amount"></el-table-column>
        <el-table-column label="卖家设置单价" prop="price"></el-table-column>
        <el-table-column label="买家名称" prop="buyerNickname"></el-table-column>
        <el-table-column label="买家手机号码" prop="buyerPhone"></el-table-column>
        <el-table-column label="交易时间" prop="createTime"></el-table-column>
        <el-table-column label="买家钱包地址" prop="buyerWalletAddress"></el-table-column>
        <el-table-column label="转币倒计时" prop="surplusTime"></el-table-column>
        <el-table-column label="操作" min-width="150px">
          <template slot-scope="scope">
            <div v-if="scope.row.state == 0">
              <el-button v-if="isSuperAdmin()" type="text"   @click="ShouBi(scope.row)">确认收币</el-button>
            </div>
            <el-button v-if="isSuperAdmin()" type="text" @click="handleLogsClick(scope.row)"> 操作日志</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!-- 订单完成（状态：20，30） -->
    <div id="table2"  name="tables" style="display:none" >
      <el-table :data="list" :v-loading="loading" border class="mt10">
        <el-table-column label="已完成" prop="orderId"></el-table-column>
        <el-table-column label="卖家名称" prop="sellerNickname"></el-table-column>
        <el-table-column label="卖家手机号码" prop="sellerPhone"></el-table-column>
        <el-table-column label="卖家钱包地址" prop="sellerWalletAddress"></el-table-column>
        <el-table-column label="交易币种" prop="btypeTxt"></el-table-column>
        <el-table-column label="交易码" prop="tradeCode"></el-table-column>
        <el-table-column label="购买数量" prop="amount"></el-table-column>
        <el-table-column label="卖家设置单价" prop="price"></el-table-column>
        <el-table-column label="买家名称" prop="buyerNickname"></el-table-column>
        <el-table-column label="买家手机号码" prop="buyerPhone"></el-table-column>
        <el-table-column label="交易时间" prop="createTime"></el-table-column>
        <el-table-column label="买家钱包地址" prop="buyerWalletAddress"></el-table-column>
        <el-table-column label="状态" prop="stateTxt"></el-table-column>
        <el-table-column label="操作" min-width="150px">
          <template slot-scope="scope">
            <div v-if=" scope.row.state == 20 ">
            <el-button v-if="isSuperAdmin()" type="text"   @click="ZhuanBi(scope.row)">确认转币</el-button>
            </div>
            <el-button v-if="isSuperAdmin()" type="text" @click="handleLogsClick(scope.row)"> 操作日志</el-button>
            
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!-- 交易关闭订单列表 -->
    <div id="table3"  name="tables" style="display:none" >
      <el-table :data="list" :v-loading="loading" border class="mt10">
        <el-table-column label="交易关闭" prop="orderId"></el-table-column>
        <el-table-column label="卖家名称" prop="sellerNickname"></el-table-column>
        <el-table-column label="卖家手机号码" prop="sellerPhone"></el-table-column>
        <el-table-column label="卖家钱包地址" prop="sellerWalletAddress"></el-table-column>
        <el-table-column label="交易币种" prop="btypeTxt"></el-table-column>
        <el-table-column label="交易码" prop="tradeCode"></el-table-column>
        <el-table-column label="购买数量" prop="amount"></el-table-column>
        <el-table-column label="卖家设置单价" prop="price"></el-table-column>
        <el-table-column label="买家名称" prop="buyerNickname"></el-table-column>
        <el-table-column label="买家手机号码" prop="buyerPhone"></el-table-column>
        <el-table-column label="交易时间" prop="createTime"></el-table-column>
        <el-table-column label="买家钱包地址" prop="buyerWalletAddress"></el-table-column>
        <el-table-column label="关闭原因" prop="CloseReason"></el-table-column>
        <el-table-column label="操作" min-width="150px">
          <template slot-scope="scope">
            <div v-if="scope.row.state == 0">
              <el-button v-if="isSuperAdmin()" type="text"   @click="ShouBi(scope.row)">确认收币</el-button>
            </div>
            <div v-if=" scope.row.state == 20 ">
            <el-button v-if="isSuperAdmin()" type="text"   @click="ZhuanBi(scope.row)">确认转币</el-button>
            </div>
            <div v-if=" scope.row.state == -1 ">
          发起维权
            </div>
            <el-button v-if="isSuperAdmin()" type="text" @click="handleLogsClick(scope.row)"> 操作日志</el-button>
            
          </template>
        </el-table-column>
      </el-table>
    </div>
    <el-pagination v-if="total > 0"
      class="mt10"
      :total="total"
      :current-page.sync="query.page"
      :page-size.sync="query.size"
      :page-sizes="[10, 20, 50, 100]"
      @current-change="loadUsers('')"
      @size-change="handleSizeChanged"
      layout="total, sizes, prev, pager, next">
    </el-pagination>

    <!-- 操作日志列表 -->
    <el-dialog :title="editFormTitle" :visible.sync="LogsTableVisible" :fullscreen="false">
      <el-table :data="logList" border style="width: 100%">
        <el-table-column
          prop="createTime"
          label="时间"
          width="180">
        </el-table-column>
         <el-table-column
          prop="operateName"
          label="操作人"
          width="180">
        </el-table-column> 
        <el-table-column
          prop="operateLog"
          label="操作内容">
        </el-table-column>
        </el-table>
    </el-dialog>
  </section>
</template>

 <script>
import commonApi from '@/api/common'
import orderApi from '@/api/order'
import svgIcon from '@/components/SvgIcon/index'
import { mapGetters } from 'vuex'

const emptyUser = {
  adminUserId: 0,
  username: '',
  contact: '',
  password: '',
  state: 0,
}
const rules = {
  username: [
    { required: true, message: '用户名不能为空' },
    { pattern: /^[\da-zA-Z_\u4e00-\u9fa5]{1,20}$/, message: '用户名只能用 1-20 位字母、数字或汉字组成' }
  ]
}

export default {
  // components: { svgIcon },
  data() {
    return {
       options: [{
          value: '0',
          label: '待转币订单'
        }, {
          value: '10',
          label: '待收款订单'
        }, {
          value: '30',
          label: '已完成订单'
        }, {
          value: '-1',
          label: '已取消订单'
        }],
      couponSelected : '',
      value: '',
      list: [],
      userStates: [],
      loading: false,
      total: 0,
      query: {
        page: 1,
        size: 10,
        username: '',
        Status: '',
        startTime: '',
        endTime: '',  
        OrderId: 0,
      },

      editFormVisible: false, 
      saving: false,
      editFormTitle: '',
      editModel: Object.assign({}, emptyUser),
      rules: rules,
      editingUser: null,

      changePwdDialogVisible: false,
      LogsTableVisible:false, // log日志列表
      pwdSaving: false,
      userOfChanging: {},
      pwdModel: {
        userId: 0,
        password: '',
        confirm: '',
      },
      pwdRules: {
        password: [
          { required: true, message: '密码不能为空' },
          { min: 6, message: '密码长度不能低于 6 位' },
          { max: 20, message: '密码长度不能多于 20 位' },
        ],
        confirm: [
          { required: true, message: '密码不能为空' },
          { min: 6, message: '密码长度不能低于 6 位' },
          { max: 20, message: '密码长度不能多于 20 位' },
        ]
      }, 
       logList:[]
    }
  },

  computed: {
    ...mapGetters([
      'user'
    ])
  },
  created(){
      //如果没有这句代码，select中初始化会是空白的，默认选中就无法实现
      this.couponSelected = this.options[0].value;
  },
  methods: { 
    changetable(e){
        var len = document.getElementsByName("tables").length
        for(var i=1; i<=len;i++){
            document.getElementById("table" + i).style.display = "none"
        }
        if(e == 0 || e == 10){
          document.getElementById("table1").style.display="block"
        }
        if(e == -1){
          document.getElementById("table3").style.display="block"
        }
        if(e == 30){
          document.getElementById("table2").style.display="block"
        }

        this.loadUsers(e)
    },
    async handleLogsClick(u){
        
      this.$progress.start()
      this.loading = true
      this.editFormTitle = "订单号：" + u.orderId
      this.LogsTableVisible = true
      const query = this.getQueryParams()
      query.OrderId = u.orderId
      const res = await orderApi.getOrderLogList(query)
      console.log(res)
 
      this.$progress.done()
       
      this.$log('[info] loaded users', res)
      this.loading = false
      // this.total = res.data.total
      this.logList = res.data.list

    },
    selfLogin(u) {
      return u.adminUserId == this.user.adminUserId
    },

    isSuperAdmin() {
      this.$log('[info] logon user', this.user)
      return this.user.username === 'admin'
    },

    getQueryParams() {
      var q = Object.assign({}, this.query)
      if (q.startTime) {
        q.startTime = q.startTime + ' 00:00:00'
      }
      if (q.endTime) {
        q.endTime = q.endTime + ' 23:59:59'
      }
      q.Status = this.$refs.addRequestState.value;  
      return q
    },

    async loadUsers(e) { 
      this.$progress.start()
      this.loading = true
      const query = this.getQueryParams()
      if(e != "" && e!="undefined" && e != null){
        query.Status = e
      }
      const res = await orderApi.getList(query)
      this.$progress.done()

      this.$log('[info] loaded users', res)
      this.loading = false
      this.total = res.data.total
      this.list = res.data.list
    },

    handleQueryReset() {
      this.$refs.queryForm.resetFields()
      this.loadUsers("")
    },

    handleSizeChanged() {
      this.query.page = 1
      this.loadUsers("")
    },
 
    //收币
    ShouBi(user) {
      let data = Object.assign({}, user)
      data.state = data.state = 10
      orderApi.confirmReceipt(data).then(res => {
        if (res.success) {
          this.$success('操作成功')
          user.state = data.state
        } else {
          this.$error('操作失败，请联系管理员')
        }
      })
    },

    //转币  
    ZhuanBi(user) {
      let data = Object.assign({}, user)
      data.state = data.state = 30
      orderApi.confirmReceipt(data).then(res => {
        if (res.success) {
          this.$success('操作成功')
          user.state = data.state
        } else {
          this.$error('操作失败，请联系管理员')
        }
      })
    },

    handleEditRole(user) {
      this.selectedUser = user
      // 设置角色时，需要实时获取当前所有角色
      // 列表，和当前用户所具有的角色列表
      this.rolesLoading = true
      this.$progress.start()
      Promise.all([this.loadAllRoles(), userApi.getRoles(user.adminUserId)])
        .then(arr => {
          this.$progress.done()
          this.rolesLoading = false

          this.roles = arr[0].data || []
          this.userRoles = arr[1].data || []
          this.selectedRoleIds = this.userRoles.map(v => v.adminRoleId)
          this.editRoleDialogVisible = true
          this.$log('[info] in handleEditRole Promise.all-----', this.userRoles, this.roles, this.selectedRoleIds)
        })
    },  
    handleChangePwd(user) {
      this.userOfChanging = user
      this.pwdModel.userId = user.adminUserId
      this.pwdModel.password = ''
      this.pwdModel.confirm = ''
      this.changePwdDialogVisible = true
      setTimeout(() => {
        this.$refs.changePwdForm.clearValidate()
      }, 0);
    },

    handleSave() {
      this.$refs.editForm.validate(valid => {
        if (valid) {
          const postData = Object.assign(this.editModel)
          const request = postData.adminUserId > 0 ? userApi.updateUser : userApi.addUser
          this.saving = true
          this.$progress.start()
          request(postData).then(res => {
            this.$progress.done()
            this.saving = false
            if (res.success) {
              this.editFormVisible = false
              this.$success('提交成功')
              this.loadUsers()
            } else {
              this.$error(res.message || '提交失败，请联系管理员')
            }
          })
        }
      })
    },

    handleSavePwd() {
      this.$refs.changePwdForm.validate(async (valid) => {
        if (valid) {
          const postData = Object.assign({}, this.pwdModel)
          this.$log('[info] in handleSavePwd-----', postData)
          if (postData.password !== postData.confirm) {
            this.$error('两次输入的密码不一致')
          } else {
            this.pwdSaving = true
            this.$progress.start()
            const res = await userApi.updatePwd(postData.userId, postData.password)

            this.$progress.done()
            this.pwdSaving = false
            if (res.success) {
              this.$success('密码修改成功')
              this.changePwdDialogVisible = false
            } else {
              this.$error('密码修改失败，请联系管理员')
            }
          }
        }
      })
    },

  }, // end methods

  async mounted() {
    const res = await commonApi.getUserStates()
    this.$log('[info] user states', res)
    this.userStates = res.data

    this.loadUsers()
  }
}
</script>

<style lang="scss">
  .role-item {
    display: inline-block;
    width: 20%;
    margin-left: 0 !important;
    padding: 8px 0;
  }
  .pwd-alert {
    padding-left: 48px;
    margin-bottom: 16px;
  }
</style>
