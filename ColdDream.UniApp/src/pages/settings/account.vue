<template>
  <view class="container">
    <view class="list">
      <view class="item">
        <text>修改密码</text>
        <text class="arrow">></text>
      </view>
      <view class="item">
        <text>绑定手机</text>
        <text class="arrow">138****8888</text>
      </view>
      <view class="item">
        <text>注销账号</text>
        <text class="arrow">></text>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
// Logic for changing password etc. would go here
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
    
    .arrow {
      color: #999;
    }
  }
}
</style>
