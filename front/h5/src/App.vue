<template>
  <div id="app">
    <!--<transition :name="animate">-->
      <!--<router-view class="Router" v-wechat-title="$route.meta.title"></router-view>-->
      <router-view  v-wechat-title="$route.meta.title"></router-view>
    <!--</transition>-->
  </div>
</template>

<script>
  export default {
    name: 'App',
    data(){
      return {
        animate:''
      }
    },
    // watch: {
    //   $route(to, from) {
    //     if (to.meta.index > from.meta.index) {
    //       //设置动画名称
    //       this.animate = 'slide-left';
    //     } else {
    //       this.animate = 'slide-right';
    //     }
    //   }
    // }
  }
</script>

<style >
  #app {
    min-height: 100%;
  }


  *,
  ::before,
  ::after {
    padding: 0;
    margin: 0;
    -webkit-box-sizing: border-box;
    /*兼容移动端主流浏览器*/
    box-sizing: border-box;
    /*运用了非固定像素布局*/
    -webkit-tap-highlight-color: transparent;
    /*轻击*/
  }

  span {
    margin: 0;
    padding: 0;
  }

  /*清除浮动*/
  .clearfix::before,
  .clearfix::after {
    content: "";
    display: block;
    height: 0;
    line-height: 0;
    visibility: hidden;
    /*隐藏但是占位*/
    clear: both;
    /*<div class="clear"></div>*/
  }

  /*html,*/
  html {
    width: 100%;
    min-height: 100%;
    /*max-width: 750px;*/
    /*overflow: hidden;*/
    margin: 0 auto;
    height: 100%;
  }
  #app{
    height: 100%;
  }

  body {
    width: 100%;
    /*max-width: 750px;*/
    min-height: 100% !important;
    height: 100%;
    margin: 0 auto;
    overflow: scroll;
    -webkit-overflow-scrolling: touch;
    overflow-scrolling: touch;
    font-size: 0.16rem;
  }

  body::-webkit-scrollbar {
    width: 0px;
  }

  body::-webkit-scrollbar {
    display: none;
  }


  a,
  input {
    border: 0;
    outline: 0;
    text-decoration: none;
    color: #333;
    background-color: transparent;
  }

  textarea {
    resize: none !important;
    border: none;
    outline: none;
  }

  ol,
  ul,
  dl,
  li,
  dt,
  dd {
    list-style: none;
  }

  i,
  b,
  s {
    font-style: normal !important;
    font-weight: 400;
    text-decoration: none;
  }

  select,
  button,
  button:hover,
  button:active,
  button:focus {
    border: 0;
    outline: 0;
    background-color: transparent;
  }
  .wd{
    word-wrap:break-word
  }

  /* img {
      border: 0 !important;
  } */

  .f_left {
    float: left;
  }

  .f_right {
    float: right !important;
  }

  .p_r {
    position: relative;
  }

  .cur {
    cursor: pointer;
  }

  .p_c {
    position: absolute;
    left: 50%;
    top: 50%;
    -webkit-transform: translate(-50%, -50%) !important;
    -moz-transform: translate(-50%, -50%) !important;
    -ms-transform: translate(-50%, -50%) !important;
    -o-transform: translate(-50%, -50%) !important;
    transform: translate(-50%, -50%) !important;
  }

  .v_hide {
    visibility: hidden;

  }

  .m0 {
    margin: 0 !important;
  }

  .m_t0 {
    margin-top: 0 !important;
  }

  .m_b0 {
    margin-bottom: 0 !important;
  }

  .m_l0 {
    margin-left: 0;
  }

  .m_r0 {
    margin-right: 0;
  }


  .pt0 {
    padding-top: 0 !important;
  }

  .dbl {
    display: block !important;
  }

  .p0 {
    padding: 0 !important;
  }

  .bbt0 {
    border-bottom: 0 !important;
  }

  .bb1 {
    border-bottom: 1px solid #C8C8C8 !important;
  }

  .bt1 {
    border-top: 1px solid #C8C8C8 !important;
  }

  .bl1 {
    border-left: 1px solid #C8C8C8 !important;
  }

  .br1 {
    border-right: 1px solid #C8C8C8 !important;
  }

  .po_top {
    position: absolute;
    left: 0;
    top: 50%;
    -webkit-transform: translateY(-50%);
    -moz-transform: translateY(-50%);
    -ms-transform: translateY(-50%);
    -o-transform: translateY(-50%);
    transform: translateY(-50%);
  }

  .hide {
    display: none;
  }

  .show {
    display: block;
    opacity: 1 !important;
  }


  .w_100 {
    width: 100% !important;

  }

  .h_100 {
    height: 100% !important;
  }


  .o_hidden {
    overflow: hidden;
  }

  .cur {
    cursor: pointer;
  }

  .border_raidus {
    -webkit-border-radius: 50%;
    -moz-border-radius: 50%;
    border-radius: 50%;
  }


  input[type="submit"],
  input[type="reset"],
  input[type="button"],
  button {
    -webkit-appearance: none;
  }


  img {
    display: flex;
    /* border: none!important; */
  }


  /* 伸缩盒子 */
  .flex {
    display: flex;
  }

  .row {
    display: flex;
    flex-direction: row;
  }
  .row_r{
    display: flex;
    flex-direction: row-reverse;
  }

  .col {
    display: flex;
    flex-direction: column;
  }
  .pb0{
    padding-bottom: 0 !important;

  }
  .pt0{
    padding-bottom: 0 !important;

  }


  .ac {
    align-items: center;
  }


  .tc {
    text-align: center;
  }

  .ja {
    justify-content: space-around;
  }

  .jb {
    justify-content: space-between;
  }

  .jc {
    justify-content: center;
  }

  .fw {
    flex-wrap: wrap;
  }

  .no_fw {
    flex-wrap: nowrap;
  }


  ::-webkit-scrollbar {
    width: 0;
    height: 1px
  }

  ::-webkit-scrollbar-thumb {
    border-radius: 5px;
    -webkit-box-shadow: inset 0 0 5px rgba(0, 0, 0, .2);
    background: rgba(0, 0, 0, .2)
  }

  .ovr {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
  }

  .ovr2 {
    display: -webkit-box;
    overflow: hidden;
    text-overflow: ellipsis;
    word-wrap: break-word;
    white-space: normal !important;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
  }
  .txt_j{
    text-align-last: justify;
  }

  .noBg {
    background: none !important;
  }
  .btn_type1{
    min-width:1.76rem;
    height:0.68rem;
    background:rgba(85,133,228,1);
    color: #fff;
    font-size:0.24rem;
    text-align: center;
    line-height: 0.68rem;
    border-radius:0.08rem;

  }
  .btn_type2{
    height:0.88rem;
    background:linear-gradient(62deg,rgba(64,101,218,1) 0%,rgba(102,149,238,1) 100%);
    box-shadow:0px 0.07rem 0.28rem 0px rgba(85,133,228,0.53);
    border-radius:0.44rem;
    color: #fff;
    font-size:0.34rem;
    text-align: center;
    line-height: 0.88rem;
  }
  .btn_type3{
    height:0.58rem;
    color: #fff;
    font-size:0.28rem;
    font-weight:500;
    text-align: center;
    line-height: 0.58rem;
    background:rgba(215,2,2,1);
    border-radius:0.11rem;
  }
  .btn_type4{
    height:0.58rem;
    color: #fff;
    font-size:0.28rem;
    font-weight:500;
    text-align: center;
    line-height: 0.58rem;
    background:rgba(85,133,228,1);
    border-radius:0.11rem;
  }
  .btn_type5{
    height:0.58rem;
    color: #fff;
    font-size:0.28rem;
    font-weight:500;
    text-align: center;
    line-height: 0.58rem;
    background:rgba(197,197,197,1);
    border-radius:0.11rem;
  }
  .btn_type6{
    height:0.88rem;
    background:linear-gradient(62deg,rgba(215,2,2,1) 0%,rgba(251,120,120,1) 100%);
    box-shadow:0px 0.07rem 0.28rem 0px rgba(223,28,28,0.47);
    border-radius:0.44rem;
    color: #fff;
    font-size:0.34rem;
    text-align: center;
    line-height: 0.88rem;
  }
  .btn_type7{
    height:0.88rem;
    background: linear-gradient(62deg, rgba(118, 119, 123, 1) 0%, rgba(200, 200, 200, 1) 100%);
    box-shadow: 0px 0.07rem 0.28rem 0px rgba(184, 184, 184, 1);
    border-radius:0.44rem;
    color: #fff;
    font-size:0.34rem;
    text-align: center;
    line-height: 0.88rem;
  }


 /*路由动画*/

  .Router {
    position: absolute;
    left: 0;
    right: 0;
  }
  .slide-right-enter-active,
  .slide-right-leave-active,
  .slide-left-enter-active,
  .slide-left-leave-active {
    will-change: transform;
    transition: all 500ms;
    position: absolute;
  }

  .slide-right-enter {
    opacity: 0;
    transform: translate3d(-100%, 0, 0);
  }

  .slide-right-leave-active {
    opacity: 0;
    transform: translate3d(100%, 0, 0);
  }

  .slide-left-enter {
    opacity: 0;
    transform: translate3d(100%, 0, 0);
  }

  .slide-left-leave-active {
    opacity: 0;
    transform: translate3d(-100%, 0, 0);
  }

  /*复制按钮样式*/
  .customer_box_btn{
    width: 0.29rem;
    height: 0.33rem;

  }

  .mr20{
    margin-right: 0.2rem;
  }
  .ml20{
    margin-left: 0.2rem;

  }

  .van-tab{
    font-size: 0.3rem !important;
  }




</style>
