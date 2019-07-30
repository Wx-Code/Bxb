<template>
  <div class="app-loading">
    <div class="spinner_box p_c">
      <div class="spinner ">
        <div class="spinner-container container1">
          <div class="circle1"></div>
          <div class="circle2"></div>
          <div class="circle3"></div>
          <div class="circle4"></div>
        </div>
        <div class="spinner-container container2">
          <div class="circle1"></div>
          <div class="circle2"></div>
          <div class="circle3"></div>
          <div class="circle4"></div>
        </div>
        <div class="spinner-container container3">
          <div class="circle1"></div>
          <div class="circle2"></div>
          <div class="circle3"></div>
          <div class="circle4"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import store from '@/utils/local-store'
import userService from '@/api/user'
import { addPrefix } from '@/utils/path'

export default {
  created() {
    const action = this.$route.query.action
    if (action === 'login') {
      this.login()
    } else {
      this.redirect()
    }
  },

  data() {
    return {
      host: process.env.FRONT_HOST,
      appId: process.env.WECHAT_APP_ID
    }
  },

  methods: {
    login() {
      const redirectUrl = this.$route.query.redirect
      store.setUrl(redirectUrl)
      const backUrl = encodeURIComponent(this.host + '/login')
      window.location.href = `https://open.weixin.qq.com/connect/oauth2/authorize?appid=${this.appId}&redirect_uri=${backUrl}&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect`
    },

    async redirect() {
      const code = this.$route.query.code
      const { data } = await userService.login(code)
      const { token, user } = data
      store.setToken(token)
      store.setUser(user)

      const redirectUrl = store.getUrl()
      store.removeUrl()
      window.location.href = redirectUrl
    },
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .app-loading {
    height: 100%;
    overflow: hidden;
  }

  .spinner {
    margin: 100px auto;
    width: 0.5rem;
    height: 0.5rem;
    position: relative;
  }

  .spinner_box {
    top: 40%;

  }

  .container1 > div, .container2 > div, .container3 > div {
    width: 0.1rem;
    height: 0.1rem;
    background-color: #333;

    border-radius: 100%;
    position: absolute;
    -webkit-animation: bouncedelay 1.2s infinite ease-in-out;
    animation: bouncedelay 1.2s infinite ease-in-out;
    -webkit-animation-fill-mode: both;
    animation-fill-mode: both;
  }

  .spinner .spinner-container {
    position: absolute;
    width: 100%;
    height: 100%;
  }

  .container2 {
    -webkit-transform: rotateZ(45deg);
    transform: rotateZ(45deg);
  }

  .container3 {
    -webkit-transform: rotateZ(90deg);
    transform: rotateZ(90deg);
  }

  .circle1 {
    top: 0;
    left: 0;
  }

  .circle2 {
    top: 0;
    right: 0;
  }

  .circle3 {
    right: 0;
    bottom: 0;
  }

  .circle4 {
    left: 0;
    bottom: 0;
  }

  .container2 .circle1 {
    -webkit-animation-delay: -1.1s;
    animation-delay: -1.1s;
  }

  .container3 .circle1 {
    -webkit-animation-delay: -1.0s;
    animation-delay: -1.0s;
  }

  .container1 .circle2 {
    -webkit-animation-delay: -0.9s;
    animation-delay: -0.9s;
  }

  .container2 .circle2 {
    -webkit-animation-delay: -0.8s;
    animation-delay: -0.8s;
  }

  .container3 .circle2 {
    -webkit-animation-delay: -0.7s;
    animation-delay: -0.7s;
  }

  .container1 .circle3 {
    -webkit-animation-delay: -0.6s;
    animation-delay: -0.6s;
  }

  .container2 .circle3 {
    -webkit-animation-delay: -0.5s;
    animation-delay: -0.5s;
  }

  .container3 .circle3 {
    -webkit-animation-delay: -0.4s;
    animation-delay: -0.4s;
  }

  .container1 .circle4 {
    -webkit-animation-delay: -0.3s;
    animation-delay: -0.3s;
  }

  .container2 .circle4 {
    -webkit-animation-delay: -0.2s;
    animation-delay: -0.2s;
  }

  .container3 .circle4 {
    -webkit-animation-delay: -0.1s;
    animation-delay: -0.1s;
  }

  @-webkit-keyframes bouncedelay {
    0%, 80%, 100% {
      -webkit-transform: scale(0.0)
    }
    40% {
      -webkit-transform: scale(1.0)
    }
  }

  @keyframes bouncedelay {
    0%, 80%, 100% {
      transform: scale(0.0);
      -webkit-transform: scale(0.0);
    }
    40% {
      transform: scale(1.0);
      -webkit-transform: scale(1.0);
    }
  }
</style>
