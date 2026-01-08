<template>
  <view class="container" v-if="route">
    <image :src="route.imageUrl || '/static/placeholder.png'" class="detail-image" mode="aspectFill" />
    <view class="content">
      <text class="title">{{ route.title }}</text>
      <text class="price">¥{{ route.price }}</text>
      <view class="tags" v-if="route.tags">
        <text class="tag" v-for="tag in JSON.parse(route.tags)" :key="tag">{{ tag }}</text>
      </view>
      <view class="description">
        <text class="section-title">简介</text>
        <text class="desc-text">{{ route.description }}</text>
      </view>

      <view class="section" v-if="route.routeMapUrl">
        <text class="section-title">路线地图</text>
        <image :src="route.routeMapUrl" mode="widthFix" class="map-image" @click="previewImage(route.routeMapUrl)" />
      </view>

      <view class="section" v-if="route.itinerary">
        <text class="section-title">行程介绍</text>
        <view class="timeline">
          <view class="timeline-item" v-for="(day, index) in itineraryList" :key="index">
            <view class="timeline-dot"></view>
            <view class="timeline-line" v-if="Number(index) < itineraryList.length - 1"></view>
            <view class="timeline-content">
              <text class="day-title">Day {{ day.day }}: {{ day.title }}</text>
              <text class="day-desc">{{ day.content }}</text>
            </view>
          </view>
        </view>
      </view>
      
      <view class="actions">
        <button class="btn-book" @click="goToBook">立即预约</button>
      </view>
    </view>
  </view>
  <view v-else class="loading">
    <text>加载中...</text>
  </view>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { getRouteDetails, type TourRoute } from '@/api/routes';

const route = ref<TourRoute | null>(null);
const routeId = ref<string>('');

const itineraryList = computed(() => {
  if (!route.value?.itinerary) return [];
  try {
    return JSON.parse(route.value.itinerary);
  } catch (e) {
    return [];
  }
});

onLoad((options: any) => {
  if (options.id) {
    routeId.value = options.id;
    loadDetail(options.id);
  }
});

const loadDetail = async (id: string) => {
  try {
    route.value = await getRouteDetails(id);
  } catch (error) {
    console.error('Failed to load detail', error);
    uni.showToast({
      title: '无法获取详情，可能需要登录',
      icon: 'none'
    });
  }
};

const goToBook = () => {
  // Check login status first
  const token = uni.getStorageSync('token');
  if (!token) {
    uni.navigateTo({ url: '/pages/auth/login' });
    return;
  }
  
  uni.navigateTo({
    url: `/pages/booking/create?routeId=${routeId.value}`
  });
};

const previewImage = (url: string) => {
  uni.previewImage({
    urls: [url]
  });
};
</script>

<style lang="scss">
.container {
  padding-bottom: 140rpx;
  background-color: #fff;
  min-height: 100vh;
}

.detail-image {
  width: 100%;
  height: 600rpx;
  object-fit: cover;
}

.content {
  padding: 40rpx;
  border-radius: 40rpx 40rpx 0 0;
  margin-top: -40rpx;
  background: #fff;
  position: relative;
  z-index: 1;
  
  .title {
    font-size: 48rpx;
    font-weight: 800;
    color: #1a1a1a;
    display: block;
    margin-bottom: 20rpx;
    line-height: 1.3;
  }
  
  .price {
    color: #ff385c;
    font-size: 44rpx;
    font-weight: 800;
    display: block;
    margin-bottom: 30rpx;
  }
  
  .tags {
    display: flex;
    flex-wrap: wrap;
    gap: 16rpx;
    margin-bottom: 40rpx;
    
    .tag {
      font-size: 24rpx;
      color: #4a4a4a;
      background: #f1f3f5;
      padding: 10rpx 24rpx;
      border-radius: 30rpx;
      font-weight: 500;
    }
  }
  
  .section {
    margin-top: 50rpx;
    padding-top: 40rpx;
    border-top: 1rpx solid #f0f0f0;
    
    .section-title {
      font-size: 36rpx;
      font-weight: 700;
      margin-bottom: 24rpx;
      display: block;
      color: #1a1a1a;
    }
    
    .desc-text {
      font-size: 30rpx;
      color: #4a4a4a;
      line-height: 1.8;
      text-align: justify;
    }

    .map-image {
      width: 100%;
      border-radius: 20rpx;
      box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.1);
    }

    .timeline {
      padding-left: 20rpx;
      
      .timeline-item {
        position: relative;
        padding-bottom: 40rpx;
        padding-left: 40rpx;
        
        &:last-child {
          padding-bottom: 0;
        }
        
        .timeline-dot {
          position: absolute;
          left: 0;
          top: 10rpx;
          width: 20rpx;
          height: 20rpx;
          background: #ff385c;
          border-radius: 50%;
          z-index: 2;
          border: 4rpx solid #fff;
          box-shadow: 0 0 0 4rpx #ff385c;
        }
        
        .timeline-line {
          position: absolute;
          left: 10rpx;
          top: 30rpx;
          bottom: -10rpx;
          width: 2rpx;
          background: #e0e0e0;
          z-index: 1;
        }
        
        .timeline-content {
          .day-title {
            font-size: 32rpx;
            font-weight: bold;
            color: #333;
            display: block;
            margin-bottom: 10rpx;
          }
          
          .day-desc {
            font-size: 28rpx;
            color: #666;
            line-height: 1.6;
          }
        }
      }
    }
  }
}

.actions {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 20rpx 40rpx 40rpx;
  background: #fff;
  box-shadow: 0 -4rpx 20rpx rgba(0,0,0,0.05);
  z-index: 100;
  
  .btn-book {
    background: linear-gradient(135deg, #ff385c, #e61e4d);
    color: #fff;
    border-radius: 60rpx;
    height: 100rpx;
    line-height: 100rpx;
    font-size: 32rpx;
    font-weight: 600;
    box-shadow: 0 8rpx 20rpx rgba(255, 56, 92, 0.3);
    
    &:active {
      transform: scale(0.98);
      box-shadow: 0 4rpx 10rpx rgba(255, 56, 92, 0.2);
    }
  }
}

.loading {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  color: #999;
  font-size: 28rpx;
}
</style>
