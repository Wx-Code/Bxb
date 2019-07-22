import Vue from 'vue'
import NProgress from 'nprogress'
import { MessageBox } from 'element-ui'

const fn = Vue.prototype
const factory = (type) => (message) => fn.$message({
  type,
  message
})

fn.$success = factory('success')
fn.$warning = factory('warning')
fn.$error = factory('error')
fn.$confirm = (msg) => MessageBox.confirm(msg, '提示', {
  confirmButtonText: '确定',
  cancelButtonText: '取消',
  type: 'warning',
  center: false,
})

fn.$log = function() {
  const env = process.env.NODE_ENV
  if (env === 'development' || env === 'test') {
    console.log.apply(console, arguments)
  }
}
fn.$logError = function() {
  console.error.apply(console, arguments)
}

fn.$progress = NProgress
fn.$ajaxStart = NProgress.start
fn.$ajaxDone = NProgress.done
