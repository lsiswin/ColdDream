<template>
  <view class="container">
    <view class="header">
      <text class="title">完善个人信息</text>
      <text class="subtitle">请填写您的基本信息以便我们提供更好的服务</text>
    </view>
    
    <view class="form">
      <view class="form-item">
        <text class="label">头像</text>
        <button class="avatar-wrapper" open-type="chooseAvatar" @chooseavatar="onChooseAvatar">
          <image class="avatar" :src="avatarUrl || '/static/avatar.png'" mode="aspectFill"></image>
        </button>
      </view>
      
      <view class="form-item">
        <text class="label">昵称</text>
        <input type="nickname" class="input" v-model="nickName" placeholder="请输入昵称" @blur="onNickNameBlur" />
      </view>
      
      <button class="btn-submit" @click="handleSubmit">保存并进入</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { request } from '@/utils/request';
import { useAuthStore } from '@/stores/auth';

const avatarUrl = ref('');
const nickName = ref('');
const authStore = useAuthStore();

const onChooseAvatar = (e: any) => {
  avatarUrl.value = e.detail.avatarUrl;
  // In a real app, you should upload this image to your server here
  // and get a remote URL to save in the profile.
  // For now, we just use the local path (which works in mini-program temporarily).
};

const onNickNameBlur = (e: any) => {
  nickName.value = e.detail.value;
};

const handleSubmit = async () => {
  if (!nickName.value) {
    uni.showToast({ title: '请输入昵称', icon: 'none' });
    return;
  }
  
  try {
    await request({
      url: '/auth/profile',
      method: 'POST',
      data: {
        nickName: nickName.value,
        avatarUrl: avatarUrl.value // Note: This should be a remote URL in production
      }
    });
    
    // Update local store
    authStore.setUser({
      username: '微信用户',
      nickName: nickName.value,
      avatarUrl: avatarUrl.value
    });
    
    uni.showToast({ title: '保存成功' });
    setTimeout(() => {
      uni.switchTab({ url: '/pages/index/index' });
    }, 1500);
    
  } catch (error) {
    console.error(error);
    uni.showToast({ title: '保存失败', icon: 'none' });
  }
};
</script>

<style lang="scss">
.container {
  padding: 40rpx;
}

.header {
  margin-bottom: 60rpx;
  
  .title {
    font-size: 48rpx;
    font-weight: bold;
    display: block;
    margin-bottom: 20rpx;
  }
  
  .subtitle {
    font-size: 28rpx;
    color: #666;
  }
}

.form-item {
  margin-bottom: 40rpx;
  
  .label {
    font-size: 30rpx;
    font-weight: bold;
    margin-bottom: 20rpx;
    display: block;
  }
  
  .avatar-wrapper {
    width: 160rpx;
    height: 160rpx;
    padding: 0;
    border-radius: 50%;
    background: #f0f0f0;
    display: flex;
    align-items: center;
    justify-content: center;
    
    &::after {
      border: none;
    }
    
    .avatar {
      width: 100%;
      height: 100%;
      border-radius: 50%;
    }
  }
  
  .input {
    height: 90rpx;
    border-bottom: 1px solid #eee;
    font-size: 32rpx;
  }
}

.btn-submit {
  margin-top: 80rpx;
  background: #007aff;
  color: #fff;
  border-radius: 45rpx;
  height: 90rpx;
  line-height: 90rpx;
  font-size: 32rpx;
  font-weight: bold;
}
</style>
