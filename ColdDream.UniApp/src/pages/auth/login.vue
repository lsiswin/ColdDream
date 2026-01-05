<template>
  <view class="container">
    <view class="logo-area">
      <image src="/static/logo.png" class="logo" mode="aspectFit" />
      <text class="app-name">西游纪行</text>
    </view>
    
    <view class="login-area">
      <!-- #ifdef MP-WEIXIN -->
      <button 
        class="btn-login wechat" 
        open-type="getPhoneNumber" 
        @getphonenumber="handleGetPhoneNumber"
      >
        <text class="icon">📱</text>
        微信手机号一键登录
      </button>
      <!-- #endif -->
      
      <!-- #ifndef MP-WEIXIN -->
      <view class="tips">
        <text>请在微信小程序中打开以使用一键登录</text>
      </view>
      <!-- #endif -->
      
      <view class="agreement">
        <text class="text">登录即代表同意</text>
        <text class="link">《用户协议》</text>
        <text class="text">和</text>
        <text class="link">《隐私政策》</text>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();

const handleGetPhoneNumber = (e: any) => {
  if (e.detail.errMsg === "getPhoneNumber:ok") {
    // User allowed getting phone number
    const { code, encryptedData, iv } = e.detail;
    
    uni.showLoading({ title: '登录中...' });
    
    // In a real app, send 'code' to backend to exchange for session_key,
    // then decrypt 'encryptedData' with 'iv' to get phone number.
    // Here we simulate a successful login for demonstration.
    
    console.log('WeChat Code:', code);
    console.log('Encrypted Data:', encryptedData);
    console.log('IV:', iv);
    
    setTimeout(() => {
      // Mock login success
      authStore.setToken('mock-wechat-token');
      authStore.setUser({
        username: '微信用户',
        nickName: '微信用户',
        avatarUrl: '', // Can be fetched via getUserProfile if needed
        points: 100
      });
      
      uni.hideLoading();
      uni.showToast({ title: '登录成功' });
      
      setTimeout(() => {
        uni.navigateBack();
      }, 1500);
    }, 1000);
    
  } else {
    // User denied
    uni.showToast({ title: '您取消了授权', icon: 'none' });
  }
};
</script>

<style lang="scss">
.container {
  padding: 60rpx;
  height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  background: #fff;
}

.logo-area {
  margin-top: 100rpx;
  display: flex;
  flex-direction: column;
  align-items: center;
  
  .logo {
    width: 160rpx;
    height: 160rpx;
    margin-bottom: 30rpx;
    border-radius: 30rpx;
    background: #f8f9fa;
  }
  
  .app-name {
    font-size: 40rpx;
    font-weight: bold;
    color: #333;
  }
}

.login-area {
  margin-bottom: 100rpx;
  
  .btn-login {
    height: 90rpx;
    line-height: 90rpx;
    border-radius: 45rpx;
    font-size: 32rpx;
    font-weight: bold;
    display: flex;
    align-items: center;
    justify-content: center;
    
    &.wechat {
      background: #07c160;
      color: #fff;
      box-shadow: 0 8rpx 20rpx rgba(7, 193, 96, 0.3);
      
      .icon {
        margin-right: 16rpx;
        font-size: 36rpx;
      }
      
      &:active {
        transform: scale(0.98);
      }
    }
  }
  
  .tips {
    text-align: center;
    color: #999;
    font-size: 28rpx;
  }
  
  .agreement {
    margin-top: 40rpx;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 24rpx;
    
    .text {
      color: #999;
    }
    
    .link {
      color: #007aff;
    }
  }
}
</style>
