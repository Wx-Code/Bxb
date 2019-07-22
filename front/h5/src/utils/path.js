/**
 * 在路由路径前增加前缀，原因是因为服务器域名有 /app 后缀
 * 需要通过 testgame.pinlala.com/app 才能访问到本应用
 * @param {String} path 路由路径
 */
const addPrefix = (path) => `/app${path}`


export {
  addPrefix,
}
