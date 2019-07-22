const tokenKey = '06f0d34cf875a244b9725dbc19c827dd'
const userKey = '58dc37da9e552908e7b0e08f230d3277'

export default {
  getToken() {
    return localStorage.getItem(tokenKey)
  },

  setToken(token) {
    localStorage.setItem(tokenKey, token)
  },

  removeToken() {
    localStorage.removeItem(tokenKey)
  },

  getUser() {
    const user = localStorage.getItem(userKey)
    try {
      return JSON.parse(user)
    } catch(err) {
      console.error('user info did not find in local storage', err)
      return null
    }
  },

  setUser(user) {
    localStorage.setItem(userKey, JSON.stringify(user))
  },

  removeUser() {
    localStorage.removeItem(userKey)
  },

}
