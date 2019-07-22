import { login } from '@/api/login'
import local from '@/utils/auth'

const setLoginStatus = (token, user) => {
  local.setToken(token)
  local.setUser(user)
}
const clearLoginStatus = () => {
  local.removeToken()
  local.removeUser()
}

const user = {
  state: {
    token: local.getToken(),
    name: '',
    avatar: '',
    user: {},
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_USER: (state, user) => {
      state.user = user
    }
  },

  actions: {
    // 登录
    Login({ commit }, userInfo) {
      return new Promise((resolve, reject) => {
        login(userInfo.username, userInfo.password).then(response => {
          if (response.success) {
            const data = response.data
            const user = { userId: data.userId, username: data.username }
            setLoginStatus(data.token, user)
            commit('SET_TOKEN', data.token)
            commit('SET_USER', user)
          }
          resolve(response)
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 获取用户信息
    GetInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        const user = local.getUser()
        commit('SET_USER', user)
        resolve(user)
      })
    },

    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        commit('SET_TOKEN', '')
        commit('SET_USER', {})
        clearLoginStatus()
        resolve()
      })
    },

    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        commit('SET_USER', {})
        clearLoginStatus()
        resolve()
      })
    }
  }
}

export default user
