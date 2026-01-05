<template>
  <view class="container">
    <!-- Custom Header -->
    <view class="custom-header" :style="{ paddingTop: statusBarHeight + 'px' }">
      <view class="header-content">
        <text class="app-title">西游纪行</text>
        <view class="header-icons">
          <text class="icon">•••</text>
          <text class="icon">◎</text>
        </view>
      </view>
    </view>

    <scroll-view scroll-y class="main-scroll" :style="{ paddingTop: (statusBarHeight + 44) + 'px' }">
      <!-- Banner Swiper -->
      <swiper class="banner-swiper" circular autoplay interval="3000" indicator-dots indicator-active-color="#fff">
        <swiper-item v-for="(item, index) in banners" :key="index">
          <view class="banner-item">
            <image :src="item.image" mode="aspectFill" class="banner-image" />
            <view class="banner-text">
              <text class="banner-title">{{ item.title }}</text>
              <text class="banner-tag">{{ item.tag }}</text>
            </view>
          </view>
        </swiper-item>
      </swiper>

      <!-- Grid Navigation -->
      <view class="nav-grid">
        <view class="nav-item" v-for="(nav, index) in navs" :key="index">
          <view class="nav-icon" :style="{ background: nav.color }">
            <image :src="nav.icon" mode="aspectFit" class="icon-img" v-if="nav.icon" />
            <text v-else>{{ nav.text[0] }}</text>
          </view>
          <text class="nav-text">{{ nav.text }}</text>
        </view>
      </view>

      <!-- Leaderboard Section -->
      <view class="section">
        <view class="section-header">
          <text class="section-title">🔥 热门路线排行榜</text>
          <text class="more" @click="goToLeaderboard">></text>
        </view>
        <scroll-view scroll-x class="leaderboard-scroll" show-scrollbar="false">
          <view class="leaderboard-list">
            <view 
              class="leaderboard-card" 
              v-for="(route, index) in topRoutes" 
              :key="route.id"
              @click="goToDetail(route.id)"
            >
              <view class="rank-badge" :class="'rank-' + (index + 1)">NO.{{ index + 1 }}</view>
              <image :src="route.imageUrl || '/static/placeholder.png'" mode="aspectFill" class="card-image" />
              <view class="card-info">
                <text class="card-title">{{ route.title }}</text>
                <view class="card-meta">
                  <text class="rating">★ 4.9分</text>
                  <text class="sales">1.2w人收藏</text>
                </view>
              </view>
            </view>
          </view>
        </scroll-view>
      </view>

      <!-- All Routes List -->
      <view class="section">
        <view class="section-header">
          <text class="section-title">🗺️ 所有路线列表</text>
          <text class="more">></text>
        </view>
        <view class="route-list">
          <view 
            class="route-item" 
            v-for="route in routes" 
            :key="route.id"
            @click="goToDetail(route.id)"
          >
            <image :src="route.imageUrl || '/static/placeholder.png'" class="route-image" mode="aspectFill" />
            <view class="route-info">
              <text class="route-title">{{ route.title }}</text>
              <view class="route-bottom">
                <text class="route-price">¥{{ route.price }}</text>
                <text class="route-sales">已售 300+</text>
              </view>
            </view>
          </view>
        </view>
      </view>
    </scroll-view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getRoutes, type TourRoute } from '@/api/routes';
import { getTopRoutes } from '@/api/leaderboard';

const statusBarHeight = ref(20);
const routes = ref<TourRoute[]>([]);
const topRoutes = ref<TourRoute[]>([]);

const banners = [
  { title: '春日京都：赏樱限定路线', tag: '1/5', image: 'https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e' },
  { title: '夏日海岛：冲绳潜水之旅', tag: '2/5', image: 'https://images.unsplash.com/photo-1540541338287-41700207dee6' },
  { title: '秋日北海道：红叶狩猎', tag: '3/5', image: 'https://images.unsplash.com/photo-1506744038136-46273834b3fb' }
];

const navs = [
  { text: '定制路线', color: 'linear-gradient(135deg, #6a11cb, #2575fc)' },
  { text: '我的行程', color: 'linear-gradient(135deg, #ff9a9e, #fecfef)' },
  { text: '攻略指南', color: 'linear-gradient(135deg, #84fab0, #8fd3f4)' },
  { text: '发现灵感', color: 'linear-gradient(135deg, #fa709a, #fee140)' }
];

onMounted(async () => {
  const sysInfo = uni.getSystemInfoSync();
  statusBarHeight.value = sysInfo.statusBarHeight || 20;
  
  loadData();
});

const loadData = async () => {
  try {
    const [routesRes, topRoutesRes] = await Promise.all([
      getRoutes(),
      getTopRoutes(5)
    ]);
    routes.value = routesRes;
    topRoutes.value = topRoutesRes;
  } catch (error) {
    console.error(error);
  }
};

const goToDetail = (id: string) => {
  uni.navigateTo({ url: `/pages/routes/detail?id=${id}` });
};

const goToLeaderboard = () => {
  uni.switchTab({ url: '/pages/leaderboard/index' });
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
  background: #fff;
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
      }
    }
  }
}

.banner-swiper {
  height: 400rpx;
  margin: 20rpx 30rpx;
  border-radius: 30rpx;
  overflow: hidden;
  box-shadow: 0 10rpx 30rpx rgba(0,0,0,0.1);
  
  .banner-item {
    position: relative;
    width: 100%;
    height: 100%;
    
    .banner-image {
      width: 100%;
      height: 100%;
    }
    
    .banner-text {
      position: absolute;
      bottom: 30rpx;
      left: 30rpx;
      right: 30rpx;
      color: #fff;
      display: flex;
      justify-content: space-between;
      align-items: flex-end;
      
      .banner-title {
        font-size: 36rpx;
        font-weight: bold;
        text-shadow: 0 2rpx 4rpx rgba(0,0,0,0.5);
      }
      
      .banner-tag {
        background: rgba(0,0,0,0.5);
        padding: 4rpx 16rpx;
        border-radius: 20rpx;
        font-size: 24rpx;
      }
    }
  }
}

.nav-grid {
  display: flex;
  justify-content: space-between;
  padding: 30rpx;
  
  .nav-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    
    .nav-icon {
      width: 100rpx;
      height: 100rpx;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: 16rpx;
      box-shadow: 0 8rpx 20rpx rgba(0,0,0,0.1);
      color: #fff;
      font-size: 40rpx;
      font-weight: bold;
    }
    
    .nav-text {
      font-size: 26rpx;
      color: #333;
      font-weight: 500;
    }
  }
}

.section {
  margin-top: 20rpx;
  
  .section-header {
    padding: 0 30rpx;
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20rpx;
    
    .section-title {
      font-size: 34rpx;
      font-weight: bold;
      color: #333;
    }
    
    .more {
      color: #999;
      font-size: 28rpx;
    }
  }
}

.leaderboard-scroll {
  white-space: nowrap;
  padding-left: 30rpx;
  
  .leaderboard-list {
    display: flex;
    padding-right: 30rpx;
    padding-bottom: 30rpx;
    
    .leaderboard-card {
      display: inline-block;
      width: 320rpx;
      background: #fff;
      border-radius: 20rpx;
      overflow: hidden;
      margin-right: 20rpx;
      box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.05);
      position: relative;
      
      .rank-badge {
        position: absolute;
        top: 20rpx;
        left: 20rpx;
        padding: 4rpx 12rpx;
        border-radius: 8rpx;
        color: #fff;
        font-size: 22rpx;
        font-weight: bold;
        z-index: 1;
        
        &.rank-1 { background: #FFD700; }
        &.rank-2 { background: #C0C0C0; }
        &.rank-3 { background: #CD7F32; }
        &.rank-4, &.rank-5 { background: #999; }
      }
      
      .card-image {
        width: 320rpx;
        height: 200rpx;
      }
      
      .card-info {
        padding: 20rpx;
        
        .card-title {
          font-size: 28rpx;
          font-weight: bold;
          color: #333;
          display: block;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          margin-bottom: 10rpx;
        }
        
        .card-meta {
          display: flex;
          justify-content: space-between;
          font-size: 22rpx;
          color: #999;
          
          .rating { color: #ff9800; }
        }
      }
    }
  }
}

.route-list {
  padding: 0 30rpx 30rpx;
  
  .route-item {
    display: flex;
    background: #fff;
    border-radius: 20rpx;
    overflow: hidden;
    margin-bottom: 20rpx;
    box-shadow: 0 2rpx 10rpx rgba(0,0,0,0.03);
    
    .route-image {
      width: 200rpx;
      height: 200rpx;
    }
    
    .route-info {
      flex: 1;
      padding: 20rpx;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      
      .route-title {
        font-size: 30rpx;
        font-weight: bold;
        color: #333;
        line-height: 1.4;
      }
      
      .route-bottom {
        display: flex;
        justify-content: space-between;
        align-items: flex-end;
        
        .route-price {
          color: #ff385c;
          font-size: 32rpx;
          font-weight: bold;
        }
        
        .route-sales {
          font-size: 24rpx;
          color: #999;
        }
      }
    }
  }
}
</style>
