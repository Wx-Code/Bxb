<template>
  <div class="record">
    <div class="record_content">
      <ul>
        <li class="row record_line_th">
          <div class="record_th1 record_th">数量</div>
          <div class="record_th2 record_th">币种</div>
          <div class="record_th3 record_th">时间</div>
        </li>
        <li class="col " v-for="(item,index) in addressArr">
          <div class="record_line" v-if="index != 0"></div>
          <div class="record_td_box row">
            <div class="record_th1 record_td">20000个</div>
            <div class="record_th2 record_td">GRT</div>
            <div class="record_th3 record_td">2019-02-15 12:00:00</div>
          </div>


        </li>

      </ul>
    </div>

  </div>

</template>

<script>
  import store from '@/utils/local-store'

  export default {
    created() {

    },

    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        addressArr: [{
          num: '20000个',
          type: 'GRT',
          time:'2019-02-15 12:00:00'
        },
          {
            num: '20000个',
            type: 'GRT',
            time:'2019-02-15 12:00:00'
          }]
      }
    },

    methods: {}
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .record {
    .record_table {
      width: 100%;
    }



    .record_th {
      font-size: 0.3rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
      line-height: 1rem;
      height: 1rem;


    }
    .record_line{
      width: 98%;
      margin-left: 2%;
      border-bottom: 1px solid #ddd;
    }
    .record_line_th{
      border-bottom: 1px solid #ddd;
    }
    ul>li:last-child{
      border-bottom:1px solid #ddd;
    }

    .record_td {
      font-size: 0.26rem;
      font-family: PingFangSC-Medium;
      font-weight: 500;
      color: rgba(102, 102, 102, 1);
      line-height: 0.8rem;

    }

    .record_pl {
      padding-left: 0.2rem;

    }
    .record_th1 {
      width: 30%;
      text-align: center;
    }
    .record_th2 {
      width: 30%;
      text-align: center;
    }

    .record_th3 {
      width: 40%;
      text-align: center;
    }


  }

</style>
