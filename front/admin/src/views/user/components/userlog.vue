<template>
  <section>
    <div class="header">
      <h2>用户操作日志</h2>
      <el-button type="primary" size="small" icon="el-icon-arrow-left" @click="back" class="back">返回</el-button>
    </div>
    <hr>
    <div class="tool-bar">
      <el-form :model="query" ref="queryForm" inline>
        <el-form-item prop="logType">
          <el-select v-model="query.logType" placeholder="操作类型" clearable>
            <el-option
              v-for="item in userTypeMap"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-form-item>
        <el-form-item prop="startTime">
          <el-date-picker
            v-model="query.startTime"
            type="date"
            placeholder="开始日期"
            format="yyyy-MM-dd HH:mm:ss"
            clearable
          ></el-date-picker>
        </el-form-item>
        <el-form-item prop="endTime">
          <el-date-picker
            v-model="query.endTime"
            type="date"
            placeholder="结束日期"
            format="yyyy-MM-dd HH:mm:ss"
            clearable
          ></el-date-picker>
        </el-form-item>
        <el-button type="primary" icon="el-icon-search" @click="loadList" :loading="listLoading">查询</el-button>
        <el-button
          type="info"
          icon="el-icon-refresh"
          @click="handleQueryReset"
          :loading="listLoading"
          class="mb20"
        >重置</el-button>
      </el-form>
    </div>
    <el-table :data="tableData" border style="width: 100%">
      <el-table-column label="操作类型">
        <template slot-scope="scope">{{ mapUserTypeText(scope.row.logType)}}</template>
      </el-table-column>
      <el-table-column prop="logContent" label="操作信息"></el-table-column>
      <el-table-column prop="createdTime" label="操作时间"></el-table-column>
      <el-table-column prop="operator" label="操作人"></el-table-column>
    </el-table>
    <div v-if="total!=0">
      <el-pagination
        class="mt10"
        :total="total"
        :current-page="query.page"
        :page-size="query.size"
        :page-sizes="[10, 20, 50, 100]"
        @current-change="handlePageChange"
        @size-change="handleSizeChange"
        layout="total, sizes, prev, pager, next"
      ></el-pagination>
    </div>
  </section>
</template>
<script>
import userApi from "@/api/user";
import commonApi from "@/api/common";
export default {
  name: "UserLog",
  props: {
    puserId: {
      type: Number | String,
      required: true
    }
  },
  data() {
    return {
      listLoading: false,
      userTypeMap: [],
      query: {
        userId: this.puserId,
        logType: null,
        startTime: null,
        endTime: null,
        page: 1,
        size: 20
      },
      total: 0,
      tableData: []
    };
  },
  methods: {
    async loadUserTypeMap() {
      let res = await commonApi.getUserLogType();
      this.$log("[info] api getUserLogType's result is ", res);
      this.userTypeMap = res.data || [];
    },
    loadList() {
      this.listLoading = true;
      this.$progress.start();
      userApi.getUserLogList(this.query).then(res => {
        this.$log("[info] in loadList----", res);
        this.$progress.done();
        this.listLoading = false;
        const data = res.data.list || [];
        data.forEach(v => (v.loading = false));
        this.tableData = data;
        this.total = res.data.total;
      });
    },
    handlePageChange(page) {
      this.query.page = page;
      this.loadList();
    },
    handleSizeChange(size) {
      this.query.size = size;
      this.query.page = 1;
      this.loadList();
    },
    handleQueryReset() {
      this.$refs.queryForm.resetFields();
      this.loadList();
    },
    mapUserTypeText(status) {
      let s = this.userTypeMap.find(item => item.value == status);
      if (s == undefined || s == null) {
        return "";
      } else {
        return s && s.label;
      }
    },
    back() {
      this.$emit("close");
    }
  },
  // 初次加载
  async mounted() {
    await this.loadUserTypeMap();
    this.loadList();
  }
};
</script>
<style lang="scss" scoped>
.back {
  float: right;
  margin-top: -35px;
}
</style>
