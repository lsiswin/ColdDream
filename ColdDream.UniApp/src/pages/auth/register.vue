<template>
  <view class="container">
    <view class="form-item">
      <text class="label">用户名</text>
      <input class="input" v-model="username" placeholder="请输入用户名" />
    </view>
    <view class="form-item">
      <text class="label">邮箱</text>
      <input class="input" v-model="email" placeholder="请输入邮箱" />
    </view>
    <view class="form-item">
      <text class="label">密码</text>
      <input class="input" v-model="password" password placeholder="请输入密码" />
    </view>
    <view class="form-item">
      <text class="label">昵称</text>
      <input class="input" v-model="nickname" placeholder="请输入昵称" />
    </view>
    
    <button class="btn-register" @click="handleRegister">注册</button>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '@/stores/auth';
import { request } from '@/utils/request';

const username = ref('');
const email = ref('');
const password = ref('');
const nickname = ref('');
const authStore = useAuthStore();

const handleRegister = async () => {
  if (!username.value || !password.value || !email.value) {
    uni.showToast({ title: '请填写完整信息', icon: 'none' });
    return;
  }
  
  try {
    const res = await request<any>({
      url: '/auth/register',
      method: 'POST',
      data: {
        username: username.value,
        email: email.value,
        password: password.value,
        nickName: nickname.value
      }
    });
    
    if (res && res.token) {
      authStore.setToken(res.token);
      authStore.setUser({ username: res.username, email: res.email });
      uni.showToast({ title: '注册成功' });
      setTimeout(() => {
        uni.navigateBack(); // Or go to home
      }, 1500);
    }
  } catch (error) {
    console.error(error);
  }
};
</script>

<style lang="scss">
.container {
  padding: 40rpx;
}

.form-item {
  margin-bottom: 30rpx;
  
  .label {
    display: block;
    margin-bottom: 10rpx;
    font-size: 28rpx;
  }
  
  .input {
    border: 1px solid #ddd;
    height: 80rpx;
    padding: 0 20rpx;
    border-radius: 8rpx;
  }
}

.btn-register {
  background: #007aff;
  color: #fff;
  margin-top: 60rpx;
}
</style>
