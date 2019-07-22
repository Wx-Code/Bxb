'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  BASE_API: '"http://testapi.pinlala.com/activity_app"',
  FRONT_HOST: '"http://localhost:8080/app"',
  RELOAD_ON_CLICK_MENU: true,
  WECHAT_APP_ID: '"wx378dc130adbcf3e3"',
})
