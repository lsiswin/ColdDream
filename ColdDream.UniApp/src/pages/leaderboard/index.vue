<template>
  <view class="container">
    <view class="header">
      <text class="title">热门路线排行榜</text>
    </view>
    
    <view class="rank-list">
      <view 
        class="rank-item" 
        v-for="(route, index) in routes" 
        :key="route.id"
        @click="goToDetail(route.id)"
      >
        <view class="rank-num" :class="'rank-' + (index + 1)">{{ index + 1 }}</view>
        <image :src="route.imageUrl || '/static/placeholder.png'" class="route-image" mode="aspectFill" />
        <view class="route-info">
          <text class="route-title">{{ route.title }}</text>
          <view class="meta">
            <text class="route-price">¥{{ route.price }}</text>
            <text class="route-sales">已售 {{ route.sales || 0 }}</text>
          </view>
        </view>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getTopRoutes, type TourRoute } from '@/api/routes';

const routes = ref<TourRoute[]>([]);

const loadData = async () => {
  try {
    const res = await getTopRoutes(20);
    if (res.success) {
      routes.value = res.data;
    }
  } catch (error) {
    console.error(error);
  }
};

const goToDetail = (id: string) => {
  uni.navigateTo({
    url: `/pages/routes/detail?id=${id}`
  });
};

onMounted(() => {
  loadData();
});
</script>

<style lang="scss">
.container {
  padding: 30rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.header {
  margin-bottom: 40rpx;
  text-align: center;
  padding: 20rpx 0;
  
  .title {
    font-size: 40rpx;
    font-weight: 800;
    color: #1a1a1a;
    position: relative;
    display: inline-block;
    
    &::after {
      content: '';
      position: absolute;
      bottom: -10rpx;
      left: 50%;
      transform: translateX(-50%);
      width: 60rpx;
      height: 6rpx;
      background: #ff385c;
      border-radius: 4rpx;
    }
  }
}

.rank-list {
  display: flex;
  flex-direction: column;
  gap: 24rpx;
}

.rank-item {
  display: flex;
  align-items: center;
  background: #fff;
  padding: 24rpx;
  border-radius: 20rpx;
  box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.04);
  transition: transform 0.2s;
  
  &:active {
    transform: scale(0.99);
  }
  
  .rank-num {
    width: 60rpx;
    height: 60rpx;
    line-height: 60rpx;
    text-align: center;
    font-size: 32rpx;
    font-weight: 700;
    margin-right: 24rpx;
    color: #999;
    font-style: italic;
    
    &.rank-1 { 
      color: #FFD700; 
      font-size: 44rpx; 
      text-shadow: 0 2rpx 4rpx rgba(255, 215, 0, 0.3);
    }
    &.rank-2 { 
      color: #C0C0C0; 
      font-size: 40rpx; 
      text-shadow: 0 2rpx 4rpx rgba(192, 192, 192, 0.3);
    }
    &.rank-3 { 
      color: #CD7F32; 
      font-size: 38rpx; 
      text-shadow: 0 2rpx 4rpx rgba(205, 127, 50, 0.3);
    }
  }
  
  .route-image {
    width: 140rpx;
    height: 140rpx;
    border-radius: 16rpx;
    margin-right: 24rpx;
    object-fit: cover;
  }
  
  .route-info {
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    
    .route-title {
      font-size: 32rpx;
      font-weight: 600;
      color: #1a1a1a;
      display: block;
      margin-bottom: 12rpx;
      line-height: 1.4;
    }
    
    .meta {
      display: flex;
      justify-content: space-between;
      align-items: center;
      
      .route-price {
        color: #ff385c;
        font-size: 30rpx;
        font-weight: 700;
      }
      
      .route-sales {
        font-size: 24rpx;
        color: #999;
      }
    }
  }
}
</style>
