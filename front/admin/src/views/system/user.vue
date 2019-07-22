<template>
  <section class="app-container">

    <div class="tool-bar mb10">
      <el-form ref="queryForm" :model="query" inline>
        <el-form-item prop="username">
          <el-input  v-model="query.username" placeholder="用户名" clearable></el-input>
        </el-form-item>
        <el-form-item prop="state">
          <el-select v-model="query.state" placeholder="用户状态" clearable >
            <el-option v-for="item in userStates" :key="item.value" :label="item.label" :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item prop="startTime">
          <el-date-picker v-model="query.startTime" value-format="yyyy-MM-dd" type="date" placeholder="开始时间"></el-date-picker>
        </el-form-item>
        <el-form-item prop="endTime">
          <el-date-picker v-model="query.endTime" value-format="yyyy-MM-dd" type="date" placeholder="结束时间"></el-date-picker>
        </el-form-item>
        <el-button type="primary" icon="el-icon-search" @click="loadUsers">查询</el-button>
        <el-button type="info" icon="el-icon-refresh" @click="handleQueryReset">重置</el-button>
      </el-form>
      <el-button v-if="isSuperAdmin()" type="primary" icon="el-icon-plus" @click="handleAddUser">创建用户</el-button>
    </div>

    <el-table :data="list" :v-loading="loading" border class="mt10">
      <el-table-column label="用户ID" prop="adminUserId"></el-table-column>
      <el-table-column label="用户名" prop="username"></el-table-column>
      <el-table-column label="用户状态">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.state == -1" type="danger">禁用</el-tag>
          <el-tag v-else type="success">启用</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="联系方式" prop="contact"></el-table-column>
      <el-table-column label="创建时间" prop="createdTime"></el-table-column>
      <el-table-column label="操作" min-width="150px">
        <template slot-scope="scope">
          <el-button v-if="isSuperAdmin()" type="text" icon="el-icon-edit-outline" @click="handleEditUser(scope.row)">编辑信息</el-button>
          <!-- 超级管理员不允许禁用 -->
          <el-button v-if="isSuperAdmin()" type="text" @click="handleDisable(scope.row)"
            :icon="scope.row.state == 0 ? 'el-icon-circle-close' : 'el-icon-circle-check'">
              {{ scope.row.state == 0 ? '停用' : '启用' }}
          </el-button>

          <!-- 超级管理员的密码，只有自己登录后才能修改，不列入权限控制范围内 -->
          <el-button v-if="isSuperAdmin()" type="text" @click="handleChangePwd(scope.row)"> <svg-icon icon-class="password" /> 修改密码</el-button>

        </template>
      </el-table-column>
    </el-table>

    <el-pagination v-if="total > 0"
      class="mt10"
      :total="total"
      :current-page.sync="query.page"
      :page-size.sync="query.size"
      :page-sizes="[10, 20, 50, 100]"
      @current-change="loadUsers"
      @size-change="handleSizeChanged"
      layout="total, sizes, prev, pager, next">
    </el-pagination>

    <el-dialog :title="editFormTitle" :visible.sync="editFormVisible">
        <el-alert
          v-if="editModel.adminUserId == 0"
          title="新创建的账号，其默认密码是：123456"
          type="success"
          :closable="false"
          class="pwd-alert"
        ></el-alert>
      <el-form ref="editForm" :model="editModel" :rules="rules" label-position="right" label-width="120px">
        <el-form-item label="用户名：" prop="username">
          <el-input v-model="editModel.username" placeholder="不超过 15 个字" :readonly="editModel.adminUserId > 0"></el-input>
        </el-form-item>
        <el-form-item label="联系方式：" prop="contact">
          <el-input v-model="editModel.contact"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer">
        <el-button :loading="saving" type="default" @click="editFormVisible = false">取消</el-button>
        <el-button :loading="saving" type="primary" @click="handleSave">确定</el-button>
      </div>
    </el-dialog>

    <!-- 修改密码 -->
    <el-dialog :title="`修改用户 ${userOfChanging.username} 的密码`" :visible.sync="changePwdDialogVisible">

      <el-form ref="changePwdForm" :model="pwdModel" :rules="pwdRules" label-position="right" label-width="120px">
        <el-form-item label="新密码：" prop="password">
          <el-input v-model="pwdModel.password" placeholder="请输入 6~20 位密码" type="password"></el-input>
        </el-form-item>
        <el-form-item label="确认密码：" prop="confirm">
          <el-input v-model="pwdModel.confirm" placeholder="请输入 6~20 位密码" type="password"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer">
        <el-button type="default" :loading="pwdSaving" @click="changePwdDialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="pwdSaving" @click="handleSavePwd">确定</el-button>
      </div>
    </el-dialog>

  </section>
</template>

<script>
import commonApi from '@/api/common'
import userApi from '@/api/user'
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
  components: { svgIcon },
  data() {
    return {
      list: [],
      userStates: [],
      loading: false,
      total: 0,
      query: {
        page: 1,
        size: 10,
        username: '',
        state: '',
        startTime: '',
        endTime: '',
      },

      editFormVisible: false,
      saving: false,
      editFormTitle: '',
      editModel: Object.assign({}, emptyUser),
      rules: rules,
      editingUser: null,

      changePwdDialogVisible: false,
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
      }
    }
  },

  computed: {
    ...mapGetters([
      'user'
    ])
  },

  methods: {
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
      return q
    },

    async loadUsers() {
      this.$progress.start()
      this.loading = true
      const query = this.getQueryParams()
      const res = await userApi.getList(query)
      this.$progress.done()

      this.$log('[info] loaded users', res)
      this.loading = false
      this.total = res.data.total
      this.list = res.data.list
    },

    handleQueryReset() {
      this.$refs.queryForm.resetFields()
      this.loadUsers()
    },

    handleSizeChanged() {
      this.query.page = 1
      this.loadUsers()
    },

    handleAddUser() {
      const m = Object.assign({}, emptyUser)
      this.editModel = m
      this.editFormTitle = '创建用户'
      this.editFormVisible = true
      this.$refs.editForm.clearValidate()
      this.$log('[info] in handleAddUser-----', m, this.editModel, emptyUser)
    },

    handleEditUser(user) {
      this.editingUser = user
      this.editModel = Object.assign({}, user)
      this.editFormTitle = '修改用户信息'
      this.editFormVisible = true
      this.$refs.editForm.clearValidate()
      this.$log('[info] in handleEditUser-----', user, this.editModel, emptyUser)
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

    handleDisable(user) {
      let data = Object.assign({}, user)
      data.state = data.state == 0 ? -1 : 0
      userApi.updateUser(data).then(res => {
        if (res.success) {
          this.$success('操作成功')
          user.state = data.state
        } else {
          this.$error('操作失败，请联系管理员')
        }
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
