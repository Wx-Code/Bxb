<template>
  <div
    :class="showMask? 'dialog fadeIn':'dialog fadeOut'"
    v-if="showMask"
    @touchmove.prevent
  >
    <div class="dialog_container">
      <img src="http://static.pinlala.com/bxb/guanbi-2.png" @click="closeMask" alt="" class="closeBtn">
      <!--提示框的标题-->
      <div class="dialog_title_box col">
        <h1 class="dialog_title">{{title}}</h1>
        <div class="dialog_title_icon">
          <img src="http://static.pinlala.com/bxb/title_icon.png" alt="" class="dialog_title_img">
        </div>
      </div>
      <div class="dialog_content">
        <p>
          {{content_txt}}
        </p>

      </div>
      <!--弹框按钮全显示-->
      <div class="btns row jb" v-if="showCancel">
        <!--取消按钮-->
        <div
          class="cancel_btn"
          @click="cancelBtn"
          v-if="showCancel"
        >
          {{cancelText}}
        </div>
        <!--确认按钮-->
        <div
          class="confirm_btn"
          @click="confirmBtn"
        >
          {{confirmText}}
        </div>
      </div>
      <!--只显示确定按钮-->
      <div class="btns row jb" v-if="!showCancel">
        <!--确认按钮-->
        <div
          class=" confirm_btn confirm_btn_one"
          @click="confirmBtn"
        >
          {{confirmText}}
        </div>
      </div>

    </div>
  </div>
</template>
<script>
  export default {
    data() {
      return {
        showMask: true,
        type: '', //1.defalut 默认(有取消按钮和确认按钮) 2.auto1 自定义弹框（关注公众号）
        title: '温馨提示',//title文案
        content: '',   //主要内容
        content_txt: '', //描述内容文案
        cancelText: '取消', //取消按钮（左）文案
        confirmText: '确定', //确认按钮（右）文案
        showCancel: true,// //是否显示取消按钮

      };
    },
    created() {

    },
    methods: {
      //关闭弹窗
      closeMask() {
        this.showMask = false;
      },
      //暴露取消按钮事件
      cancelBtn() {
        this.closeMask();
      },
      //暴露确认按钮事件
      confirmBtn() {
        this.closeMask();
      }
    }

  };
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
  .dialog {
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background: rgba(0, 0, 0, 0.6);
    z-index: 9999;
    .dialog_title_icon{
      margin-top: 0.1rem;
    }
    .dialog_container {
      width: 6.7rem;
      padding: 0.42rem 0.62rem 0.5rem;
      background: #ffffff;
      position: absolute;
      top: 50%;
      left: 50%;
      border-radius: 0.1rem;
      transform: translate(-50%, -50%);
      .dialog_title_img{
        width: 0.93rem;
        height: 0.08rem;
        margin: 0 auto;
      }
      .closeBtn {
        width: 0.42rem;
        height: 0.42rem;
        position: absolute;
        right: 0.2rem;
        top: 0.2rem
      }
      .dialog_content{
        padding-top: 0.27rem;
        font-size:0.26rem;
        font-family:PingFangSC-Regular;
        font-weight:400;
        color:rgba(51,51,51,1);
        text-indent: 2em;
        line-height:0.4rem;
      }
      .dialog_title {
        width: 100%;
        text-align: center;
        font-size: 0.3rem;
        font-family: MicrosoftYaHei;
        font-weight: 400;
        color:rgba(51,51,51,1);
        box-sizing: border-box;
      }
      .content {
        text-align: center;
        color: #666666;
        font-size: 0.34rem;
        line-height: 0.6rem;
        padding: 0 0.1rem;
        margin-top: 0.6rem;
        box-sizing: border-box;
      }

      .content_txt {
        text-align: center;
        color: #666666;
        font-size: 0.28rem;
        line-height: 0.6rem;
        padding: 0 0.3rem;
        box-sizing: border-box;
      }
      .btns {
        width: 100%;
        height: 0.66rem;
        box-sizing: border-box;
        margin-top: 0.5rem;
        .confirm_btn_one{
          min-width: 2.38rem;
          height: 0.66rem;
          line-height: 0.66rem;
          margin: 0 auto;
          font-size: 0.26rem;
          border-radius: 0.33rem;

        }
        .confirm_btn{
          background:linear-gradient(62deg,rgba(64,101,218,1) 0%,rgba(102,149,238,1) 100%);
          box-shadow:0px 7px 28px 0px rgba(85,133,228,0.53);
        }
        .cancel_btn{
          background:linear-gradient(62deg,rgba(118,119,123,1) 0%,rgba(200,200,200,1) 100%);
          box-shadow:0px 7px 28px 0px rgba(184,184,184,1);
        }
        & > div {
          width: 2.38rem;
          text-align: center;
          font-size: 0.26rem;
          font-family: MicrosoftYaHei;
          font-weight: 400;
          color: rgba(255, 255, 255, 1);
          height: 0.66rem;
          line-height: 0.66rem;
          border-radius: 0.33rem;

          cursor: pointer;
        }
        .confirm_btn-sty {
          width: 5.24rem;
          margin: 0 auto;
        }
        .confirm_btn {
          color: #ffffff;

        }

      }

      .close-btn {
        position: absolute;
        top: 16px;
        right: 16px;
        width: 30px;
        height: 30px;
        line-height: 30px;
        text-align: center;
        font-size: 18px;
        cursor: pointer;
      }
    }
  }

  .game_care {
    width: 5.4rem;
    margin: 0 auto;
  }

  .game_care_txt {
    font-size: 0.28rem;
    font-family: MicrosoftYaHei;
    font-weight: 400;
    color: rgba(102, 102, 102, 1);
    line-height: 0.48rem;
    margin-top: 0.5rem
  }

  .game_care_code {
    width: 3.4rem;
    height: 3.4rem;
    margin: 0.68rem auto 0.27rem;
  }

  .game_care_codeTxt {
    text-align: center;
    font-size: 0.2rem;
    font-family: MicrosoftYaHei;
    font-weight: 400;
    color: rgba(153, 153, 153, 1);
  }


  .fadeIn {
    animation: animate_in 0.25s;
  }

  .fadeOut {
    animation: animate_out 0.25s;
    opacity: 0;
  }

  @keyframes animate_in {
    0% {
      opacity: 0;
    }
    100% {
      opacity: 1;
    }
  }

  @keyframes animate_out {
    0% {
      opacity: 1;
    }
    100% {
      opacity: 0;
    }
  }


</style>

