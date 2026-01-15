<template>
  <view class="container">
    <view class="search-bar">
      <input class="search-input" placeholder="搜索攻略..." />
    </view>
    
    <view class="guide-list">
      <view class="guide-card" v-for="item in list" :key="item.id" @click="goToDetail(item.id)">
        <image :src="item.imageUrl || '/static/placeholder.png'" class="guide-image" mode="aspectFill" />
        <view class="guide-content">
          <text class="guide-title">{{ item.title }}</text>
          <view class="guide-tags" v-if="item.tags">
            <text class="tag" v-for="(tag, idx) in parseTags(item.tags)" :key="idx">{{ tag }}</text>
          </view>
          <view class="guide-meta">
            <text class="duration" v-if="item.duration">🕒 {{ item.duration }}</text>
            <text class="budget" v-if="item.budget">💰 {{ item.budget }}</text>
          </view>
          <view class="guide-footer">
            <view class="author">
              <image src="/static/avatar.png" class="avatar" />
              <text class="name">旅行达人</text>
            </view>
            <text class="date">{{ formatDateTime(item.createdAt) }}</text>
          </view>
        </view>
      </view>
    </view>
    
    <view class="empty" v-if="list.length === 0">
      <text>暂无攻略，快去分享你的旅行吧~</text>
    </view>
    
    <view class="fab-btn" @click="goToCreate">
      <text class="plus">+</text>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { onShow } from '@dcloudio/uni-app';
import { request, type ApiResponse } from '@/utils/request';
import type { Guide } from '@/api/guide';

const list = ref<Guide[]>([]);

onShow(() => {
  loadData();
});

const loadData = async () => {
  try {
    const res = await request<ApiResponse<Guide[]>>({
      url: '/guide',
      method: 'GET'
    });
    if (res.success) {
      list.value = res.data;
    }
  } catch (error) {
    console.error(error);
  }
};

const goToDetail = (id: string) => {
  // We need a detail page. For now, let's reuse create page in read-only mode or create a new one.
  // User said "content same as tour route", so let's create a detail page.
  uni.navigateTo({ url: `/pages/guide/detail?id=${id}` });
};

const goToCreate = () => {
  uni.navigateTo({ url: '/pages/guide/create' });
};

const parseTags = (tagsStr: string) => {
  try {
    return JSON.parse(tagsStr);
  } catch {
    return [];
  }
};

const formatDateTime = (dateStr: string) => {
  if (!dateStr) return '';
  const date = new Date(dateStr);
  return `${date.getMonth() + 1}月${date.getDate()}日`;
};
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.search-bar {
  background: #fff;
  padding: 20rpx;
  border-radius: 40rpx;
  margin-bottom: 30rpx;
  
  .search-input {
    font-size: 28rpx;
  }
}

.guide-card {
  background: #fff;
  border-radius: 20rpx;
  overflow: hidden;
  margin-bottom: 30rpx;
  box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.05);
  
  .guide-image {
    width: 100%;
    height: 360rpx;
  }
  
  .guide-content {
    padding: 24rpx;
    
    .guide-title {
      font-size: 34rpx;
      font-weight: bold;
      color: #333;
      margin-bottom: 16rpx;
      display: block;
    }
    
    .guide-tags {
      display: flex;
      flex-wrap: wrap;
      gap: 10rpx;
      margin-bottom: 16rpx;
      
      .tag {
        font-size: 22rpx;
        color: #007aff;
        background: #f0f9ff;
        padding: 4rpx 12rpx;
        border-radius: 8rpx;
      }
    }
    
    .guide-meta {
      display: flex;
      gap: 30rpx;
      margin-bottom: 20rpx;
      font-size: 24rpx;
      color: #666;
    }
    
    .guide-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-top: 1px solid #f5f5f5;
      padding-top: 20rpx;
      
      .author {
        display: flex;
        align-items: center;
        
        .avatar {
          width: 40rpx;
          height: 40rpx;
          border-radius: 50%;
          margin-right: 10rpx;
        }
        
        .name {
          font-size: 24rpx;
          color: #666;
        }
      }
      
      .date {
        font-size: 22rpx;
        color: #999;
      }
    }
  }
}

.fab-btn {
  position: fixed;
  bottom: 60rpx;
  right: 40rpx;
  width: 100rpx;
  height: 100rpx;
  background: #007aff;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 8rpx 20rpx rgba(0, 122, 255, 0.4);
  z-index: 100;
  
  .plus {
    color: #fff;
    font-size: 60rpx;
    font-weight: bold;
    margin-top: -8rpx;
  }
}

.empty {
  text-align: center;
  padding: 100rpx 0;
  color: #999;
}
</style>
