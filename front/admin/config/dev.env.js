'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  BASE_API: '"http://localhost:33610"',
  UPLOADIMGAPI: '"http://localhost:33610/common/image/upload"',
  RELOAD_ON_CLICK_MENU: true
})
