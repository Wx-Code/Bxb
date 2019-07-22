<template>
  <div class="navbar">
    <hamburger :toggle-click="toggleSideBar" :is-active="sidebar.opened" class="hamburger-container"/>
    <breadcrumb />
    <el-dropdown class="avatar-container" trigger="click">
      <div class="avatar-wrapper">
        <img :src="avatar || defaultAvatar" class="user-avatar">
        <span class="user-name">{{ user.username }}</span>
        <i class="el-icon-caret-bottom"/>
      </div>
      <el-dropdown-menu slot="dropdown" class="user-dropdown">
        <router-link class="inlineBlock" to="/">
          <el-dropdown-item>
            首页
          </el-dropdown-item>
        </router-link>
        <el-dropdown-item @click.native="handleChangePwdClicked">修改密码</el-dropdown-item>
        <el-dropdown-item divided>
          <span style="display:block;" @click="logout">退出登录</span>
        </el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>

    <!-- 修改密码 -->
    <el-dialog title="修改密码" :visible.sync="changePwdDialogVisible">
      <el-form ref="changePwdForm" :model="pwdModel" :rules="pwdRules" label-position="right" label-width="120px">
        <el-form-item label="原密码：" prop="oldPassword">
          <el-input v-model="pwdModel.oldPassword" placeholder="请输入 6~20 位密码" type="password"></el-input>
        </el-form-item>
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
  </div>
</template>

<script>
import defaultAvatar  from '@/assets/logo.png'
import { mapGetters } from 'vuex'
import Breadcrumb from '@/components/Breadcrumb'
import Hamburger from '@/components/Hamburger'
import userApi from '@/api/user'

export default {
  components: {
    Breadcrumb,
    Hamburger
  },
  computed: {
    ...mapGetters([
      'sidebar',
      'avatar',
      'user'
    ])
  },

  data() {
    const pwdRuleArr = [
      { required: true, message: '密码不能为空' },
      { min: 6, message: '密码长度不能低于 6 位' },
      { max: 20, message: '密码长度不能多于 20 位' },
    ]
    return {
      defaultAvatar: defaultAvatar,
      changePwdDialogVisible: false,
      pwdSaving: false,
      pwdModel: {
        userId: 0,
        password: '',
        oldPassword: '',
        confirm: '',
      },
      pwdRules: {
        oldPassword: pwdRuleArr,
        password: pwdRuleArr,
        confirm: pwdRuleArr
      }
    }
  },

  methods: {
    toggleSideBar() {
      this.$store.dispatch('ToggleSideBar')
    },

    async logout() {
      await this.$store.dispatch('LogOut')
      this.$router.push({ path: '/login' })
    },

    handleChangePwdClicked() {
      this.changePwdDialogVisible = true
      setTimeout(() => {
        this.$refs.changePwdForm.clearValidate()
      }, 0);
    },

    handleSavePwd() {
      this.$refs.changePwdForm.validate(valid => {
        if (valid) {
          let m = this.pwdModel
          if (m.password !== m.confirm) {
            this.$error('新密码与确认密码不一致')
            return false
          }

          m.userId = this.user.userId
          const { userId, password, oldPassword } = m
          this.pwdSaving = true
          this.$progress.start()
          userApi.updatePwd(userId, password, oldPassword).then(res => {
            this.$progress.done()
            this.pwdSaving = false
            if (res.success) {
              this.$success('密码修改成功')
              this.logout()
            } else {
              this.$error(res.message || '修改密码失败，请联系管理员')
            }
          })
        }
      })
    },

  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.navbar {
  height: 50px;
  line-height: 50px;
  border-radius: 0px !important;
  .hamburger-container {
    line-height: 58px;
    height: 50px;
    float: left;
    padding: 0 10px;
  }
  .screenfull {
    position: absolute;
    right: 90px;
    top: 16px;
    color: red;
  }
  .avatar-container {
    height: 50px;
    display: inline-block;
    position: absolute;
    right: 35px;
    .avatar-wrapper {
      cursor: pointer;
      position: relative;
      .user-name {
        display: inline-block;
        height: 40px;
        line-height: 40px;
        font-size: 16px;
        vertical-align: middle;
      }
      .user-avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        vertical-align: middle;
      }
      .el-icon-caret-bottom {
        position: absolute;
        right: -20px;
        top: 17px;
        font-size: 16px;
      }
    }
  }
}
</style>

