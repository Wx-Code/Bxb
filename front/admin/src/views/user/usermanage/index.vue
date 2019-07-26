<template>
  <section class="app-container">
      <div class="tool-bar mb10">
        <el-form ref="queryForm" :model="query" inline>
          <el-form-item prop="nickname">
            <el-input v-model="query.nickname" placeholder="用户昵称" clearable></el-input>
          </el-form-item>
          <el-form-item prop="phone">
            <el-input v-model="query.phone" placeholder="手机号码" clearable></el-input>
          </el-form-item>
          <el-form-item prop="walletAddress">
            <el-input v-model="query.walletAddress" placeholder="钱包地址" clearable></el-input>
          </el-form-item>

           <el-form-item prop="startTime">
                     <el-date-picker
                        v-model="query.startTime"
                        type="datetime"
                        placeholder="注册时间">
                    </el-date-picker>
                </el-form-item>
                <el-form-item prop="endTime">
                    <el-date-picker
                        v-model="query.endTime"
                        type="datetime"
                        placeholder="注册时间">
                    </el-date-picker>
                </el-form-item>

          <el-button type="primary" icon="el-icon-search" @click="handleSizeChanged">查询</el-button>
          <el-button type="info" icon="el-icon-refresh" @click="handleQueryReset">重置</el-button>
        </el-form>
      </div>

      <el-table :data="list" :v-loading="loading" border class="mt10">
        <el-table-column label="用户ID" prop="userId"></el-table-column>
        <el-table-column label="微信昵称" prop="nickname"></el-table-column>
        <el-table-column label="微信头像" prop="avatar">
          <template slot-scope="scope">
            <img style="width:50px" :src="scope.row.avatar">
          </template>
        </el-table-column>
        <el-table-column label="手机号码" prop="phone"></el-table-column>
        <el-table-column label="钱包地址" prop="walletAddress"></el-table-column>
        <el-table-column label="注册时间" prop="createdTime"></el-table-column>
        <el-table-column fixed="right" label="操作" width="370">
          <template slot-scope="scope">
            <el-button
              type="text"
              size="small"
              @click="handleQueryDataDetailsClick(scope.row)"
            >用户详情</el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        v-if="total > 0"
        class="mt10"
        :total="total"
        :current-page.sync="query.page"
        :page-size.sync="query.size"
        :page-sizes="[10, 20, 50, 100]"
        @current-change="loadUsers"
        @size-change="handleSizeChanged"
        layout="total, sizes, prev, pager, next"
      ></el-pagination>

  </section>
</template>

<script>
import commonApi from "@/api/common";
import userApi from "@/api/user";
import { mapGetters } from "vuex";
export default {
  data() {
    return {
      list: [],
      loading: false,
      total: 0,
      query: {
        page: 1,
        size: 10,
        nickname: null,
        walletAddress: null,
        phone: null,
        startTime:null,
        endTime:null
      }
    };
  },
  methods: {
    async loadUsers() {
      this.$progress.start();
      this.loading = true;
      this.$log("getUserList query is ", this.query)
      const res = await userApi.getUserList(this.query);
      this.$progress.done();
      this.$log("[info] loaded users", res);
      this.loading = false;
      this.total = res.data.total;
      this.list = res.data.list;
    },

    handleQueryReset() {
      this.$refs.queryForm.resetFields();
      this.loadUsers();
    },

    handleSizeChanged() {
      this.query.page = 1;
      this.loadUsers();
    },
    //列表点击数据详情按钮操作
    handleQueryDataDetailsClick(row) {
      this.$router.push({
        name: "Datadetails",
        query: {
          puserId: row.userId
        }
      });
    },
    onRefresList() {
      this.loadUsers();
      this.componentsMoneyTableVisible = false;
    }
  },
  async mounted() {
    this.loadUsers();
  }
};
</script>

<style lang="scss">
.role-item {
  display: inline-block;
  width: 20%;
  margin-left: 0 !important;
  padding: 8px 0;
}
.pwd-alert {
  padding-left: 48px;
  margin-bottom: 16px;
}
</style>
