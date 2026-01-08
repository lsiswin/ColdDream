<template>
  <view class="container">
    <!-- Custom Header -->
    <view class="custom-header" :style="{ paddingTop: statusBarHeight + 'px' }">
      <view class="header-content">
        <text class="app-title">个人中心</text>
        <view class="header-icons">
          <text class="icon">•••</text>
          <text class="icon">◎</text>
        </view>
      </view>
    </view>

    <scroll-view scroll-y class="main-scroll" :style="{ paddingTop: (statusBarHeight + 44) + 'px' }">
      <!-- User Info Card -->
      <view class="user-card">
        <view class="user-info" v-if="user">
          <image :src="user.avatarUrl || '/static/avatar.png'" class="avatar" mode="aspectFill" />
          <view class="info-text">
            <text class="username">{{ user.nickName || user.username }}</text>
            <view class="user-meta">
              <text class="level-tag">金牌会员</text>
              <text class="points">{{ user.points || 0 }} 积分</text>
            </view>
          </view>
        </view>
        <view class="user-info" v-else @click="goToLogin">
          <image src="/static/avatar.png" class="avatar" mode="aspectFill" />
          <view class="info-text">
            <text class="username">点击登录</text>
            <text class="sub-text">登录后享受更多权益</text>
          </view>
        </view>
      </view>

      <!-- Dashboard Cards -->
      <view class="dashboard-section">
        <!-- Points Mall (Large) -->
        <view class="mall-card" @click="goToMall">
          <view class="card-content">
            <text class="card-title">🎁 积分商城</text>
            <text class="card-arrow">></text>
          </view>
        </view>
        
        <!-- Small Cards -->
        <view class="small-cards">
          <view class="small-card coupon" @click="goToCoupons">
            <text class="card-label">旅行券</text>
            <text class="card-sub">享超值优惠</text>
            <text class="card-icon">🎫</text>
          </view>
          <view class="small-card souvenir">
            <text class="card-label">纪念品</text>
            <text class="card-sub">尊享优选</text>
            <text class="card-icon">🎁</text>
          </view>
        </view>
      </view>

      <!-- Menu List -->
      <view class="menu-list">
        <view class="menu-item" v-for="(item, index) in menuItems" :key="index" @click="handleMenuClick(item)">
          <view class="menu-left">
            <text class="menu-icon">{{ item.icon }}</text>
            <text class="menu-text">{{ item.text }}</text>
          </view>
          <text class="menu-arrow">></text>
        </view>
      </view>
    </scroll-view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useAuthStore } from '@/stores/auth';

import { storeToRefs } from 'pinia';

const statusBarHeight = ref(20);
const authStore = useAuthStore();
const { user } = storeToRefs(authStore);
const hasLogin = computed(() => !!user.value);

const menuItems = [
  { text: '我的订单', icon: '📄', path: '/pages/bookings/my' },
  { text: '我的定制', icon: '✈️', path: '/pages/custom-tour/list' },
  { text: '优惠券', icon: '🎟️', path: '/pages/coupon/index' },
  { text: '收藏夹', icon: '❤️', path: '/pages/inspiration/favorites' },
  { text: '我的攻略', icon: '📖', path: '/pages/guide/my' },
  { text: '系统设置', icon: '⚙️', path: '/pages/settings/index' },
  { text: '联系客服', icon: '🎧', path: '/pages/service/contact' }
];

onMounted(() => {
  const sysInfo = uni.getSystemInfoSync();
  statusBarHeight.value = sysInfo.statusBarHeight || 20;
});

const goToLogin = () => {
  uni.navigateTo({ url: '/pages/auth/login' });
};

const goToMall = () => {
  uni.navigateTo({ url: '/pages/products/index' });
};

const goToCoupons = () => {
  uni.navigateTo({ url: '/pages/coupon/index' });
};

const goToUserInfo = () => {
  uni.navigateTo({ url: '/pages/user/info' });
};

const goToSettings = () => {
  uni.navigateTo({ url: '/pages/settings/index' });
};

const handleMenuClick = (item: any) => {
  if (item.path) {
    uni.navigateTo({ url: item.path });
  } else {
    uni.showToast({ title: '功能开发中', icon: 'none' });
  }
};
</script>

<style lang="scss">
.container {
  background-color: #f8f9fa;
  min-height: 100vh;
}

.custom-header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  background: transparent; /* Transparent for gradient background effect if needed, or white */
  z-index: 100;
  padding-bottom: 20rpx;
  
  .header-content {
    height: 44px;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    
    .app-title {
      font-size: 36rpx;
      font-weight: bold;
      color: #333;
    }
    
    .header-icons {
      position: absolute;
      right: 30rpx;
      display: flex;
      gap: 20rpx;
      
      .icon {
        font-size: 32rpx;
        color: #333;
        border: 1px solid #eee;
        border-radius: 50%;
        width: 60rpx;
        height: 60rpx;
        line-height: 56rpx;
        text-align: center;
        background: rgba(255,255,255,0.8);
      }
    }
  }
}

.main-scroll {
  /* Background gradient for the top part */
  background: linear-gradient(180deg, #e0f7fa 0%, #f8f9fa 30%);
}

.user-card {
  padding: 40rpx 30rpx;
  
  .user-info {
    display: flex;
    align-items: center;
    
    .avatar {
      width: 120rpx;
      height: 120rpx;
      border-radius: 50%;
      margin-right: 30rpx;
      border: 4rpx solid #fff;
      box-shadow: 0 4rpx 10rpx rgba(0,0,0,0.1);
    }
    
    .info-text {
      .username {
        font-size: 40rpx;
        font-weight: bold;
        color: #333;
        display: block;
        margin-bottom: 10rpx;
      }
      
      .sub-text {
        font-size: 28rpx;
        color: #666;
      }
      
      .user-meta {
        display: flex;
        align-items: center;
        gap: 20rpx;
        
        .level-tag {
          background: #ffd700;
          color: #856404;
          font-size: 22rpx;
          padding: 4rpx 12rpx;
          border-radius: 8rpx;
          font-weight: bold;
        }
        
        .points {
          font-size: 28rpx;
          color: #333;
          font-weight: 500;
        }
      }
    }
  }
}

.dashboard-section {
  padding: 0 30rpx;
  margin-bottom: 30rpx;
  
  .mall-card {
    background: linear-gradient(135deg, #fce38a, #f38181);
    border-radius: 24rpx;
    padding: 40rpx;
    margin-bottom: 20rpx;
    box-shadow: 0 8rpx 20rpx rgba(243, 129, 129, 0.2);
    
    .card-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
      
      .card-title {
        font-size: 36rpx;
        font-weight: bold;
        color: #856404;
      }
      
      .card-arrow {
        font-size: 32rpx;
        color: #856404;
        opacity: 0.6;
      }
    }
  }
  
  .small-cards {
    display: flex;
    justify-content: space-between;
    gap: 20rpx;
    
    .small-card {
      flex: 1;
      background: #fff;
      border-radius: 24rpx;
      padding: 30rpx;
      position: relative;
      overflow: hidden;
      box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.05);
      
      &.coupon { background: linear-gradient(135deg, #fff5e6, #fff); }
      &.souvenir { background: linear-gradient(135deg, #f3e5f5, #fff); }
      
      .card-label {
        font-size: 30rpx;
        font-weight: bold;
        color: #333;
        display: block;
        margin-bottom: 8rpx;
      }
      
      .card-sub {
        font-size: 22rpx;
        color: #999;
      }
      
      .card-icon {
        position: absolute;
        bottom: 10rpx;
        right: 20rpx;
        font-size: 48rpx;
        opacity: 0.8;
      }
    }
  }
}

.menu-list {
  background: #fff;
  border-radius: 24rpx;
  margin: 0 30rpx 30rpx;
  padding: 10rpx 0;
  box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.05);
  
  .menu-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 30rpx 40rpx;
    border-bottom: 1rpx solid #f5f5f5;
    
    &:last-child {
      border-bottom: none;
    }
    
    &:active {
      background: #f9f9f9;
    }
    
    .menu-left {
      display: flex;
      align-items: center;
      
      .menu-icon {
        font-size: 36rpx;
        margin-right: 24rpx;
        width: 40rpx;
        text-align: center;
      }
      
      .menu-text {
        font-size: 30rpx;
        color: #333;
      }
    }
    
    .menu-arrow {
      font-size: 28rpx;
      color: #ccc;
    }
  }
}
</style>
