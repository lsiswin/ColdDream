<template>
  <view class="container">
    <view class="list">
      <view class="item" @click="clearCache">
        <text>清除缓存</text>
        <text class="value">12.5MB</text>
      </view>
      <view class="item">
        <text>字体大小</text>
        <text class="value">标准</text>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
const clearCache = () => {
  uni.showLoading({ title: '清理中' });
  setTimeout(() => {
    uni.hideLoading();
    uni.showToast({ title: '清理完成' });
  }, 1000);
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
  
  .item {
    display: flex;
    justify-content: space-between;
    padding: 30rpx;
    border-bottom: 1px solid #f5f5f5;
    
    &:last-child {
      border-bottom: none;
    }
    
    .value {
      color: #999;
    }
  }
}
</style>
