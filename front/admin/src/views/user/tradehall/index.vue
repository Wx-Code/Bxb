<template>
  <section class="app-container">
    <div class="tool-bar mb10">
      <el-form ref="queryForm" :model="query" inline>
        <el-form-item prop="userId">
          <el-input  v-model="query.userId" placeholder="用户ID" clearable></el-input>
        </el-form-item>
        <el-form-item prop="bType">
          <el-select v-model="query.bType" placeholder="币种" clearable>
            <el-option v-for="item in bTypes" :key="item.value" :label="item.label" :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item prop="status">
          <el-select v-model="query.status" placeholder="状态" clearable>
            <el-option v-for="item in status" :key="item.value" :label="item.label" :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item prop="startTime">
          <el-date-picker v-model="query.startTime" value-format="yyyy-MM-dd" type="date" placeholder="开始时间"></el-date-picker>
        </el-form-item>
        <el-form-item prop="endTime">
          <el-date-picker v-model="query.endTime" value-format="yyyy-MM-dd" type="date" placeholder="结束时间"></el-date-picker>
        </el-form-item>
        <el-button type="primary" icon="el-icon-search" @click="loadTrades" :loading="loading">查询</el-button>
        <el-button type="info" icon="el-icon-refresh" @click="handleQueryReset">重置</el-button>
      </el-form>
    </div>

    <el-table :data="list" :v-loading="loading" border class="mt10">
      <el-table-column label="消息ID" prop="tradeId"></el-table-column>
      <el-table-column label="用户ID" prop="releaseUserId"></el-table-column>
      <el-table-column label="币种" prop="bTypeText"></el-table-column>
      <el-table-column label="单价" prop="price"></el-table-column>
      <el-table-column label="可售数量" prop="amount"></el-table-column>
      <el-table-column label="状态" prop="statusText"></el-table-column>
      <el-table-column label="发布时间" prop="releaseTime"></el-table-column>
      <el-table-column label="操作" min-width="150px">
        <template slot-scope="scope">
          <el-button type="text" @click="handleDisable(scope.row)"
            :icon="scope.row.status == 1 ? 'el-icon-circle-close' : ''">
              {{ scope.row.status == 1 ? '下架' : '' }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-pagination v-if="total > 0"
      class="mt10"
      :total="total"
      :current-page.sync="query.page"
      :page-size.sync="query.size"
      :page-sizes="[10, 20, 50, 100]"
      @current-change="loadTrades"
      @size-change="handleSizeChanged"
      layout="total, sizes, prev, pager, next">
    </el-pagination>
  </section>
</template>

<script>
import commonApi from '@/api/common'
import tradeApi from '@/api/tradehall'
import svgIcon from '@/components/SvgIcon/index'

export default {
  components: { svgIcon },
  data() {
    return {
      list: [],
      bTypes: [],
      status: [],
      loading: false,
      total: 0,
      query: {
        page: 1,
        size: 10,
        userId: null,
        bType: null,
        status: null,
        startTime: null,
        endTime: null,
      }
    }
  },

  methods: {
    getQueryParams() {
      var q = Object.assign({}, this.query)
      if (q.startTime) {
        q.startTime = q.startTime + ' 00:00:00'
      }
      if (q.endTime) {
        q.endTime = q.endTime + ' 23:59:59'
      }
      return q
    },

    async loadTrades() {
      this.$progress.start();
      this.loading = true;
      const query = this.getQueryParams();
      const res = await tradeApi.getList(query);
      this.$progress.done();

      this.$log('[info] loaded trades', res);
      this.loading = false;
      this.total = res.data.total;
      this.list = res.data.list;
    },

    handleQueryReset() {
      this.$refs.queryForm.resetFields();
      this.loadTrades();
    },

    handleSizeChanged() {
      this.query.page = 1;
      this.loadTrades();
    },

    handleDisable(trade) {
      tradeApi.putOff(trade.tradeId).then(res => {
        if (res.success) {
          this.$success('操作成功')
          trade.status = 0;
        } else {
          this.$error('操作失败，请联系管理员')
        }
      })
    }
  },

  async mounted() {
    var res = await commonApi.getCurrencyType()
    if(res.success){
        this.$log('[info] currency type is :', res)
        this.bTypes = res.data;
    }

    res = await commonApi.getTradeHallShelfStatus();
    if(res.success){
        this.$log('[info] shelf status is :', res);
        this.status = res.data;
    }
    
    this.loadTrades();
  }
}
</script>

