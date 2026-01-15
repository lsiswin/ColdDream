<template>
  <view class="container" v-if="guide">
    <image :src="guide.imageUrl || '/static/placeholder.png'" class="cover-image" mode="aspectFill" />
    
    <view class="content-wrapper">
      <view class="header">
        <text class="title">{{ guide.title }}</text>
        <view class="meta-row">
          <view class="tags" v-if="guide.tags">
            <text class="tag" v-for="(tag, idx) in parseTags(guide.tags)" :key="idx">{{ tag }}</text>
          </view>
          <text class="date">{{ formatDateTime(guide.createdAt) }}</text>
        </view>
        <view class="info-grid">
          <view class="info-item" v-if="guide.duration">
            <text class="label">时长</text>
            <text class="value">{{ guide.duration }}</text>
          </view>
          <view class="info-item" v-if="guide.budget">
            <text class="label">人均</text>
            <text class="value">{{ guide.budget }}</text>
          </view>
        </view>
      </view>

      <view class="section">
        <text class="section-title">简介</text>
        <text class="description">{{ guide.description }}</text>
      </view>

      <view class="section" v-if="itineraryList.length > 0">
        <text class="section-title">详细行程</text>
        <view class="timeline">
          <view class="timeline-item" v-for="(day, index) in itineraryList" :key="index">
            <view class="timeline-dot"></view>
            <view class="timeline-line" v-if="index < itineraryList.length - 1"></view>
            <view class="timeline-content">
              <text class="day-title">Day {{ day.day }}: {{ day.title }}</text>
              <text class="day-desc">{{ day.content }}</text>
            </view>
          </view>
        </view>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { getGuideById, type Guide } from '@/api/guide';

const guide = ref<Guide | null>(null);

onLoad(async (options: any) => {
  if (options.id) {
    await loadData(options.id);
  }
});

const loadData = async (id: string) => {
  try {
    const res = await getGuideById(id);
    if (res.success) {
      guide.value = res.data;
    }
  } catch (error) {
    console.error(error);
  }
};

const itineraryList = computed(() => {
  if (!guide.value?.itinerary) return [];
  try {
    return JSON.parse(guide.value.itinerary);
  } catch (e) {
    return [];
  }
});

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
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;
};
</script>

<style lang="scss">
.container {
  background-color: #fff;
  min-height: 100vh;
  padding-bottom: 40rpx;
}

.cover-image {
  width: 100%;
  height: 500rpx;
}

.content-wrapper {
  padding: 30rpx;
  margin-top: -40rpx;
  background: #fff;
  border-radius: 40rpx 40rpx 0 0;
  position: relative;
  z-index: 1;
}

.header {
  margin-bottom: 40rpx;
  
  .title {
    font-size: 40rpx;
    font-weight: bold;
    color: #333;
    margin-bottom: 20rpx;
    display: block;
  }
  
  .meta-row {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 30rpx;
    
    .tags {
      display: flex;
      gap: 10rpx;
      
      .tag {
        font-size: 22rpx;
        color: #007aff;
        background: #f0f9ff;
        padding: 4rpx 12rpx;
        border-radius: 8rpx;
      }
    }
    
    .date {
      font-size: 24rpx;
      color: #999;
    }
  }
  
  .info-grid {
    display: flex;
    background: #f8f9fa;
    padding: 20rpx;
    border-radius: 16rpx;
    
    .info-item {
      flex: 1;
      text-align: center;
      
      .label {
        font-size: 24rpx;
        color: #999;
        display: block;
        margin-bottom: 8rpx;
      }
      
      .value {
        font-size: 28rpx;
        font-weight: bold;
        color: #333;
      }
      
      &:first-child {
        border-right: 1px solid #eee;
      }
    }
  }
}

.section {
  margin-bottom: 40rpx;
  
  .section-title {
    font-size: 32rpx;
    font-weight: bold;
    color: #333;
    margin-bottom: 20rpx;
    display: block;
    padding-left: 16rpx;
    border-left: 6rpx solid #007aff;
  }
  
  .description {
    font-size: 28rpx;
    color: #666;
    line-height: 1.6;
  }
}

.timeline {
  padding-left: 20rpx;
  
  .timeline-item {
    position: relative;
    padding-left: 40rpx;
    padding-bottom: 40rpx;
    
    &:last-child {
      padding-bottom: 0;
    }
    
    .timeline-dot {
      position: absolute;
      left: 0;
      top: 10rpx;
      width: 16rpx;
      height: 16rpx;
      border-radius: 50%;
      background: #007aff;
      border: 4rpx solid #cce5ff;
    }
    
    .timeline-line {
      position: absolute;
      left: 10rpx;
      top: 30rpx;
      bottom: -10rpx;
      width: 2rpx;
      background: #eee;
    }
    
    .timeline-content {
      .day-title {
        font-size: 30rpx;
        font-weight: bold;
        color: #333;
        margin-bottom: 10rpx;
        display: block;
      }
      
      .day-desc {
        font-size: 26rpx;
        color: #666;
        line-height: 1.5;
      }
    }
  }
}
</style>
