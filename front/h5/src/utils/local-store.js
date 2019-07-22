import Cookies from 'js-cookie'

const setObject = function (key, value) {
  const json = JSON.stringify(value)
  localStorage.setItem(key, json)
}
const getObject = function (key) {
  const json = localStorage.getItem(key)
  return JSON.parse(json)
}

const TOKEN_KEY = 'token'
const URL_KEY = 'url'
const USER_KEY = 'user'

export default {
  // TODO: 将 expires 转换为 js-cookie 所需数据格式要求
  setToken(token, expires) {
    const opts = !!expires ? { expires } : {}
    Cookies.set(TOKEN_KEY, token, opts)
  },

  getToken() {
    return Cookies.get(TOKEN_KEY)
  },

  setUrl(url) {
    localStorage.setItem(URL_KEY, url)
  },

  getUrl() {
    return localStorage.getItem(URL_KEY)
  },

  removeUrl() {
    localStorage.removeItem(URL_KEY)
  },

  setUser(user) {
    setObject(USER_KEY, user)
  },

  getUser() {
    return getObject(USER_KEY)
  }
}
