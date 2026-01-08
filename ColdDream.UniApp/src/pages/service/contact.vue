<template>
  <view class="container">
    <view class="contact-card">
      <view class="header">
        <text class="title">联系客服</text>
        <text class="subtitle">我们将竭诚为您服务</text>
      </view>
      
      <view class="info-list">
        <view class="item">
          <text class="label">客服电话</text>
          <text class="value" @click="makeCall">400-888-8888</text>
        </view>
        <view class="item">
          <text class="label">工作时间</text>
          <text class="value">周一至周日 9:00 - 21:00</text>
        </view>
        <view class="item">
          <text class="label">官方邮箱</text>
          <text class="value">service@colddream.com</text>
        </view>
      </view>
      
      <button class="btn-call" @click="makeCall">立即拨打</button>
      <button class="btn-chat" open-type="contact">在线咨询</button>
    </view>
  </view>
</template>

<script setup lang="ts">
const makeCall = () => {
  uni.makePhoneCall({
    phoneNumber: '400-888-8888'
  });
};
</script>

<style lang="scss">
.container {
  padding: 40rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

.contact-card {
  width: 100%;
  background: #fff;
  border-radius: 24rpx;
  padding: 60rpx 40rpx;
  box-shadow: 0 10rpx 30rpx rgba(0,0,0,0.05);
  
  .header {
    text-align: center;
    margin-bottom: 60rpx;
    
    .title {
      font-size: 40rpx;
      font-weight: bold;
      color: #333;
      display: block;
      margin-bottom: 16rpx;
    }
    
    .subtitle {
      font-size: 28rpx;
      color: #999;
    }
  }
  
  .info-list {
    margin-bottom: 60rpx;
    
    .item {
      display: flex;
      justify-content: space-between;
      padding: 30rpx 0;
      border-bottom: 1px solid #f5f5f5;
      
      .label {
        color: #666;
      }
      
      .value {
        color: #333;
        font-weight: 500;
      }
    }
  }
  
  .btn-call {
    background: #007aff;
    color: #fff;
    border-radius: 50rpx;
    margin-bottom: 30rpx;
    box-shadow: 0 8rpx 20rpx rgba(0, 122, 255, 0.3);
  }
  
  .btn-chat {
    background: #f0f9ff;
    color: #007aff;
    border-radius: 50rpx;
    border: 1px solid #007aff;
  }
}
</style>
