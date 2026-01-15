<template>
  <view class="container">
    <view class="form-item">
      <text class="label">头像</text>
      <button class="avatar-wrapper" open-type="chooseAvatar" @chooseavatar="onChooseAvatar">
        <image class="avatar" :src="form.avatarUrl || '/static/avatar.png'" mode="aspectFill"></image>
      </button>
    </view>
    
    <view class="form-item">
      <text class="label">昵称</text>
      <input type="nickname" class="input" v-model="form.nickName" placeholder="请输入昵称" />
    </view>
    
    <button class="btn-save" @click="save">保存</button>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { useAuthStore } from '@/stores/auth';
import { updateProfile } from '@/api/auth';
import { storeToRefs } from 'pinia';

const authStore = useAuthStore();
const { user } = storeToRefs(authStore);

const form = ref({
  nickName: '',
  avatarUrl: ''
});

onLoad(() => {
  if (user.value) {
    form.value.nickName = user.value.nickName || user.value.username || '';
    form.value.avatarUrl = user.value.avatarUrl || '';
  }
});

const onChooseAvatar = (e: any) => {
  const { avatarUrl } = e.detail;
  // In a real app, you would upload this file to your server here.
  // For now, we'll just use the local path or a placeholder if it's a temp file that the backend can't access.
  // Since we don't have file upload implemented yet, we might just store the string if it's a URL, 
  // or warn the user. For WeChat mini-program, this works differently.
  // For H5, we might need a file input.
  // Assuming H5 for now based on "npm run dev:h5".
  form.value.avatarUrl = avatarUrl;
};

const save = async () => {
  if (!form.value.nickName) {
    uni.showToast({ title: '请输入昵称', icon: 'none' });
    return;
  }
  
  try {
    const res = await updateProfile(form.value);
    
    if (res.success) {
      // Update local store
      authStore.setUser({
        ...user.value!,
        nickName: form.value.nickName,
        avatarUrl: form.value.avatarUrl
      });
      
      uni.showToast({ title: '保存成功' });
      setTimeout(() => {
        uni.navigateBack();
      }, 1500);
    } else {
      uni.showToast({ title: res.message || '保存失败', icon: 'none' });
    }
  } catch (error) {
    console.error(error);
  }
};
</script>

<style lang="scss">
.container {
  padding: 30rpx;
  background-color: #fff;
  min-height: 100vh;
}

.form-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 30rpx 0;
  border-bottom: 1px solid #eee;
  
  .label {
    font-size: 30rpx;
    color: #333;
  }
  
  .avatar-wrapper {
    background: none;
    padding: 0;
    margin: 0;
    line-height: normal;
    
    &::after {
      border: none;
    }
  }
  
  .avatar {
    width: 100rpx;
    height: 100rpx;
    border-radius: 50%;
    background: #f0f0f0;
  }
  
  .input {
    text-align: right;
    font-size: 30rpx;
  }
}

.btn-save {
  margin-top: 60rpx;
  background: #007aff;
  color: #fff;
  border-radius: 40rpx;
}
</style>
