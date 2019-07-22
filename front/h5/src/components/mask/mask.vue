<template>
    <div
        class="dialog"
        v-show="showMask"
    >
        <div class="dialog-container">
            <img src="http://pic0.one1tree.com.cn/game/game_close.png" @click="closeMask" alt="" class="closeBtn">
            
            <div class="dialog-title">{{title}}</div>
            <div
                class="content"
                v-html="content"
            ></div>
              <div
                class="content_txt"
                v-html="content_txt"
            ></div>

          
            <div class="btns row jb">
                <div
                    v-if="type != 'confirm'"
                    class="default-btn"
                    @click="closeBtn"
                >
                    {{cancelText}}
                </div>
                <div
                    v-if="type == 'danger'"
                    class="danger-btn"
                    @click="dangerBtn"
                >
                    {{dangerText}}
                </div>
                <div
                    v-if="type == 'confirm'"
                    :class="type == 'confirm' ? 'confirm-btn confirm-btn-sty': 'confirm-btn'"
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
  props: {
    value: {},
    // 类型包括 defalut 默认， danger 危险， confirm 确认，
    type: {
      type: String,
      default: "default"
    },
    content: {
      type: String,
      default: ""
    },
    content_txt: {
      type: String,
      default: ""
    },
    title: {
      type: String,
      default: ""
    },
    cancelText: {
      type: String,
      default: "取消"
    },
    dangerText: {
      type: String,
      default: "删除"
    },
    confirmText: {
      type: String,
      default: "确认"
    }
  },
  data() {
    return {
      showMask: false
    };
  },
  methods: {
    closeMask() {
      this.showMask = false;
    },
    closeBtn() {
      this.$emit("cancel");
      this.closeMask();
    },
    dangerBtn() {
      this.$emit("danger");
      this.closeMask();
    },
    confirmBtn() {
      this.$emit("confirm");
      this.closeMask();
    }
  },
  mounted() {
    this.showMask = this.value;
  },
  watch: {
    value(newVal, oldVal) {
      this.showMask = newVal;
    },
    showMask(val) {
      this.$emit("input", val);
    }
  }
};
</script>
<style rel="stylesheet/scss" lang="scss" scoped >
.dialog {
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
  z-index: 9999;
  .dialog-container {
    width: 6.5rem;
    padding: 0 0.33rem 0.5rem;
    background: #ffffff;
    position: absolute;
    top: 4.27rem;
    left: 50%;
    border-radius: 0.1rem;
    transform: translate(-50%, -50%);
    border-radius: 8px;
    position: relative;
    .closeBtn{
        widows: 0.2rem;
        height: 0.2rem;
        position: absolute;
        right: 0.2rem;
        top: 0.2rem
    }
    .dialog-title {
      width: 100%;
      height: 1.34rem;
      line-height: 1.34rem;
      text-align: center;
      font-size: 0.34rem;
      font-family: MicrosoftYaHei;
      font-weight: 400;
      color: rgba(157, 88, 42, 1);
      box-sizing: border-box;
      border-bottom: 1px solid #afafb1;
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
    .inp {
      margin: 10px 0 0 20px;
      width: 200px;
      height: 40px;
      padding-left: 4px;
      border-radius: 4px;
      border: none;
      background: #efefef;
      outline: none;
      &:focus {
        border: 1px solid #509ee3;
      }
    }
    .btns {
      width: 100%;
      height: 60px;
      box-sizing: border-box;
      margin-top: 0.5rem;
      & > div {
        width: 2.6rem;
        text-align: center;
        font-size:0.36rem;
font-family:MicrosoftYaHei;
font-weight:400;
color:rgba(255,255,255,1);
        height: 0.88rem;
        line-height: 0.88rem;
        background: linear-gradient(
          90deg,
          rgba(250, 122, 23, 1),
          rgba(241, 60, 87, 1)
        );
        border-radius: 0.44rem;

        cursor: pointer;
      }
      .default-btn {
        
        // &:hover {
        //   color: #509ee3;
        // }
      }
      .danger-btn {
        
        &:hover {
          background: rgb(224, 135, 135);
        }
        &:active {
          background: #ef8c8c;
        }
      }
      .confirm-btn-sty{
           width: 5.24rem;
           margin: 0 auto;
      }
      .confirm-btn {
        color: #ffffff;
        // background: #509ee3;
        // &:hover {
        //   background: #6fb0eb;
        // }
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
      &:hover {
        font-weight: 600;
      }
    }
  }
}
</style>
