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
      
      <!-- Test Only -->
      <button class="btn-guest" @click="handleGuestLogin">我是测试员 (跳过微信登录)</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { wechatLogin, guestLogin } from '@/api/auth';

const authStore = useAuthStore();

const handleGetPhoneNumber = (e: any) => {
  if (e.detail.errMsg === "getPhoneNumber:ok" || e.detail.errMsg === "getPhoneNumber:ok") {
    const phoneCode = e.detail.code;
    
    uni.showLoading({ title: '登录中...' });
    
    uni.login({
      provider: 'weixin',
      success: async (loginRes) => {
        if (loginRes.code) {
          try {
            const res = await wechatLogin({
              loginCode: loginRes.code,
              phoneCode: phoneCode
            });
            
            if (res.success) {
              const data = res.data;
              authStore.setToken(data.token);
              
              if (data.isNewUser) {
                uni.hideLoading();
                uni.navigateTo({ url: '/pages/auth/profile' });
              } else {
                authStore.setUser({
                  id: '', // UserProfile needs ID, but login might not return it fully structured yet. 
                  // Ideally backend login response should match UserProfile or we fetch profile after login.
                  // For now, let's map what we have.
                  username: data.username,
                  email: data.email,
                  points: data.points
                } as any); // Temporary cast until LoginResponse fully matches UserProfile or we fetch me
                
                uni.hideLoading();
                uni.showToast({ title: '登录成功' });
                setTimeout(() => {
                  uni.navigateBack();
                }, 1500);
              }
            } else {
              uni.hideLoading();
              uni.showToast({ title: res.message || '登录失败', icon: 'none' });
            }
          } catch (error) {
            console.error(error);
            uni.hideLoading();
            uni.showToast({ title: '登录失败', icon: 'none' });
          }
        }
      },
      fail: () => {
        uni.hideLoading();
        uni.showToast({ title: '获取登录凭证失败', icon: 'none' });
      }
    });
    
  } else {
    console.error('getPhoneNumber failed:', e.detail);
    uni.showToast({ title: `授权失败: ${e.detail.errMsg}`, icon: 'none' });
  }
};

const handleGuestLogin = async () => {
  uni.showLoading({ title: '登录中...' });
  try {
    const res = await guestLogin();
    
    if (res.success) {
      const data = res.data;
      authStore.setToken(data.token);
      authStore.setUser({
        id: '',
        username: data.username,
        email: data.email,
        nickName: '游客测试员',
        points: data.points
      } as any);
      
      uni.hideLoading();
      uni.showToast({ title: '登录成功' });
      setTimeout(() => {
        uni.navigateBack();
      }, 1500);
    } else {
      uni.hideLoading();
      uni.showToast({ title: res.message || '登录失败', icon: 'none' });
    }
  } catch (error) {
    console.error(error);
    uni.hideLoading();
    uni.showToast({ title: '登录失败', icon: 'none' });
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
  
  .btn-guest {
    margin-top: 40rpx;
    background: #f8f9fa;
    color: #666;
    font-size: 28rpx;
    border: 1px solid #eee;
    border-radius: 45rpx;
    
    &:active {
      background: #eee;
    }
  }
}
</style>
