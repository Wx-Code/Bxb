<template>
  <div class="changeCode">
    <div class="cc_content ">
      <div class="cc_code_box">
        <img :src="imgData" alt="" class="cc_code_img">
        <p class="cc_code_txt tc">微信二维码</p>
      </div>
      <div class="cc_btns">
        <van-uploader :after-read="afterRead">
          <div class="cc_btn btn_type2">更换</div>
        </van-uploader>
      </div>




    </div>
  </div>

</template>

<script>
  import store from '@/utils/local-store'
  import user from '@/api/user'
  export default {
    data() {
      return {
        host: process.env.FRONT_HOST,
        appId: process.env.WECHAT_APP_ID,
        apiHost:process.env.BASE_API,
        imgDataSave:'',//临时路径
        imgData:'',
        userInfo:'',
        imgUploadSuccess:false

      }
    },
    created() {
      this.getMyCode()
    },
    methods: {
      async afterRead(file) {
        this.$toast.loading({
          duration: 0,       // 持续展示 toast
          forbidClick: true, // 禁用背景点击
          loadingType: 'spinner',
          message: '上传中'
        })
        // 此时可以自行将文件上传至服务器
        let formData = new window.FormData();
        formData.append('file', file.file);//通过append向form对象添加数据
        formData.get("file")
        let config = {
          headers: {'Content-Type': 'multipart/form-data'}
        }; //添加请求头
        const { data } = await this.$axios.post(this.apiHost + '/common/wechat/qrcode', formData, config)
        if (!data) return
        if(data.errorCode == '0000'){
          this.imgDataSave = data.data
          this.editwxcode()
        }else {
          this.$toast.clear()
          this.$toast({message: '图片上传失败', duration: '1500'})
        }
      },
      async getMyCode() {
        const { data,errorCode } = await user.getUserInfo()
        if (!data) return false
        if (errorCode == '0000') {
          this.userInfo = data
          this.imgData = data.wxCodePhoto
        }
      },
      async editwxcode() {
        const { data,errorCode } = await user.editwxcode({UserId:this.userInfo.userId,WxCodePhoto:this.imgDataSave})
        if (!data) return false
        if (errorCode == '0000') {
          this.imgUploadSuccess = true
          this.imgData = this.imgDataSave
          const that = this
          setTimeout(function () {
            that.$toast.clear()
            that.$toast({message: '更换成功', duration: '1500'})
          }, 300)
        }
      },
    }
  }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .changeCode {
    .cc_content {
      box-sizing: border-box;
      padding-top: 1.46rem;
    }

    .cc_code_box {
      width: 6.7rem;
      height: 6.14rem;
      background: rgba(255, 255, 255, 1);
      box-shadow: 0px 0.04rem 0.13rem 0 rgba(0, 0, 0, 0.2);
      border-radius: 0.17rem;
      margin: 0 auto;
      overflow: hidden;

    }
    .cc_code_img{
      width: 3.64rem;
      height: 3.64rem;
      margin: 0.8rem auto 0.6rem;
    }
    .cc_code_txt{
      font-size:0.26rem;
      font-family:PingFangSC-Regular;
      font-weight:400;
      color:rgba(51,51,51,1);

    }
    .cc_btn{
      width: 3.64rem;
      margin: 1.16rem auto 0;
    }
    .cc_btns{
      width: 3.64rem;
      margin: 0 auto 0;

    }


  }

</style>
