<template>
  <view class="container">
    <view class="list">
      <view class="item">
        <text>系统通知</text>
        <switch checked color="#007aff" />
      </view>
      <view class="item">
        <text>活动推送</text>
        <switch checked color="#007aff" />
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
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
    align-items: center;
    padding: 30rpx;
    border-bottom: 1px solid #f5f5f5;
    
    &:last-child {
      border-bottom: none;
    }
  }
}
</style>
