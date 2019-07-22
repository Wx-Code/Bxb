<template>
  <div v-if="!item.hidden" class="menu-wrapper">

    <!-- 只有一个子级时，不显示父级，只显示子级 -->
    <!-- <template v-if="hasOneShowingChild(item.children,item) && (!onlyOneChild.children||onlyOneChild.noShowingChildren)&&!item.alwaysShow">
      <app-link :to="resolvePath(onlyOneChild.path)">
        <el-menu-item :index="resolvePath(onlyOneChild.path)" :class="{'submenu-title-noDropdown':!isNest}">
          <item :meta="Object.assign({},item.meta,onlyOneChild.meta)" />
        </el-menu-item>
      </app-link>
    </template> -->

    <el-menu-item
      v-if="!item.children || item.children.length === 0"
      :index="basePath"
      :class="{'submenu-title-noDropdown' : !isNest}"
      @click.native="load(basePath)">
      <item :meta="Object.assign({},item.meta)" />
    </el-menu-item>

    <el-submenu v-else ref="subMenu" :index="resolvePath(item.path)" popper-append-to-body>
      <template slot="title">
        <item :meta="item.meta" />
      </template>
      <sidebar-item
        v-for="child in item.children"
        :is-nest="true"
        :item="child"
        :key="child.path"
        :base-path="resolvePath(child.path)"
        class="nest-menu" />
    </el-submenu>

  </div>
</template>

<script>
import path from 'path'
import { isExternal } from '@/utils/validate'
import Item from './Item'
import AppLink from './Link'
import { mapGetters } from 'vuex'

export default {
  name: 'SidebarItem',
  components: { Item, AppLink },
  props: {
    // route object
    item: {
      type: Object,
      required: true
    },
    isNest: {
      type: Boolean,
      default: false
    },
    basePath: {
      type: String,
      default: ''
    }
  },

  data() {
    // To fix https://github.com/PanJiaChen/vue-admin-template/issues/237
    // TODO: refactor with render function
    this.onlyOneChild = null
    return {}
  },

  computed: {
    ...mapGetters([
      'currentRoute'
    ])
  },

  methods: {
    hasOneShowingChild(children = [], parent) {
      const showingChildren = children.filter(item => {
        if (item.hidden) {
          return false
        } else {
          // Temp set(will be used if only has one showing child)
          this.onlyOneChild = item
          return true
        }
      })

      // When there is only one child router, the child router is displayed by default
      if (showingChildren.length === 1) {
        return true
      }

      // Show parent if there are no child router to display
      if (showingChildren.length === 0) {
        this.onlyOneChild = { ... parent, path: '', noShowingChildren: true }
        return true
      }

      return false
    },
    resolvePath(routePath) {
      if (isExternal(routePath)) {
        return routePath
      }
      return path.resolve(this.basePath, routePath)
    },
    isExternalLink(path) {
      return isExternal(path)
    },

    /**
     * 点击菜单时刷新页面
     */
    load(path) {
      if (this.isExternalLink(path)) {
        return
      }

      if (!process.env.RELOAD_ON_CLICK_MENU) {
        this.$router.push({ path })
        return
      }

      let url = path
      if (this.$router.mode !== 'history') {

        // VUE-ROUTER 默认路由方式是通过 url 的哈希值来判断路由的变化的
        // 因此在这种情况下改变 location.href 是不会触发页面刷新的，所以需要手动刷新

        url = `/#${path}`
        location.href = url
        // fix: 从 QQ 等软件跳转过来的链接，会被添加一些参数导致路由无法正常解析
        if (!location.search) {
          location.reload()
        }

      } else {
        location.href = `${url}?${+new Date()}`
      }
    },

  },
}
</script>
