<template>
  <view class="container">
    <view class="list">
      <view class="item" @click="navigateTo('/pages/settings/account')">
        <text>账号与安全</text>
        <text class="arrow">></text>
      </view>
      <view class="item" @click="navigateTo('/pages/settings/notifications')">
        <text>消息通知</text>
        <text class="arrow">></text>
      </view>
      <view class="item" @click="navigateTo('/pages/settings/general')">
        <text>通用设置</text>
        <text class="arrow">></text>
      </view>
      <view class="item" @click="navigateTo('/pages/settings/about')">
        <text>关于我们</text>
        <text class="arrow">></text>
      </view>
    </view>
    
    <button class="btn-logout" @click="logout">退出登录</button>
  </view>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();

const navigateTo = (url: string) => {
  uni.navigateTo({ url });
};

const logout = () => {
  uni.showModal({
    title: '提示',
    content: '确定要退出登录吗？',
    success: (res) => {
      if (res.confirm) {
        authStore.logout();
        uni.reLaunch({ url: '/pages/index/index' });
      }
    }
  });
};
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.list {
  background: #fff;
  border-radius: 12rpx;
  margin-bottom: 40rpx;
  
  .item {
    display: flex;
    justify-content: space-between;
    padding: 30rpx;
    border-bottom: 1px solid #f5f5f5;
    
    &:last-child {
      border-bottom: none;
    }
    
    .arrow {
      color: #ccc;
    }
  }
}

.btn-logout {
  background: #fff;
  color: #ff4757;
  border-radius: 12rpx;
  font-size: 32rpx;
}
</style>
