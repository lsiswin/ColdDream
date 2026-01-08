<template>
  <view class="container">
    <view class="logo-section">
      <image src="/static/logo.png" class="logo" mode="aspectFit" />
      <text class="app-name">ColdDream Travel</text>
      <text class="version">Version 1.0.0</text>
    </view>
    
    <view class="list">
      <view class="item">
        <text>用户协议</text>
        <text class="arrow">></text>
      </view>
      <view class="item">
        <text>隐私政策</text>
        <text class="arrow">></text>
      </view>
      <view class="item">
        <text>联系我们</text>
        <text class="arrow">></text>
      </view>
    </view>
    
    <text class="copyright">Copyright © 2026 ColdDream. All Rights Reserved.</text>
  </view>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.logo-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 60rpx 0;
  
  .logo {
    width: 160rpx;
    height: 160rpx;
    margin-bottom: 20rpx;
  }
  
  .app-name {
    font-size: 36rpx;
    font-weight: bold;
    color: #333;
    margin-bottom: 10rpx;
  }
  
  .version {
    font-size: 24rpx;
    color: #999;
  }
}

.list {
  background: #fff;
  border-radius: 12rpx;
  
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

.copyright {
  text-align: center;
  font-size: 20rpx;
  color: #ccc;
  margin-top: auto;
  padding-bottom: 40rpx;
}
</style>
