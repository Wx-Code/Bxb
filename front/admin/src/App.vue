<template>
  <div id="app">
    <router-view/>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style lang="scss">
  @import '~element-ui/packages/theme-chalk/src/common/var.scss';
  .mb10 { margin-bottom: 10px; }
  .mt10 { margin-top: 10px; }
  .mt20 { margin-top: 20px; }
  .mr10 { margin-right: 10px; }
  .ml10 { margin-left: 10px; }
  .w100 { width: 100px; }
  .w150 { width: 150px; }
  .w200 { width: 200px; }

  .color-success { color: $--color-success; }
  .color-info { color: $--color-info; }
  .color-warning { color: $--color-warning; }
  .color-danger { color: $--color-danger; }
  .color-primary { color: $--color-primary; }

  .fr { float: right; }
  .fl { float: left; }
  .header {
    h1,h2,h3,h4,h5,h6 {
      color: #303133;
    }
  }

  ul {
    margin: 0;
    list-style: none;
    padding: 0;
  }

  .remove-shadow { box-shadow: none; }

  .app-header {
    border-bottom: 1px solid #dcdfe6;
    padding-bottom: 8px;

    h2 {
      margin: 0;
      position: relative;
      font-weight: 400;
      color: #1f2f3d;
    }
  }

  .app-body {
    margin-top: 8px;
  }
</style>
