<template>
  <view class="container">
    <view class="waterfall">
      <view class="column">
        <view class="item" v-for="item in leftCol" :key="item.id">
          <image :src="item.imageUrl" mode="widthFix" class="img" lazy-load @click="previewImage(item.imageUrl)" />
          <text class="desc">{{ item.description }}</text>
          <view class="footer">
            <view class="user">
              <image :src="item.userAvatar || '/static/avatar.png'" class="avatar" />
              <text class="name">{{ item.userName }}</text>
            </view>
            <view class="actions">
              <view class="action" @click="handleLike(item)">
                <text class="icon" :class="{ active: item.isLiked }">❤</text>
                <text class="count">{{ item.likes }}</text>
              </view>
              <view class="action" @click="handleCollect(item)">
                <text class="icon" :class="{ active: item.isCollected }">★</text>
                <text class="count">{{ item.collects }}</text>
              </view>
            </view>
          </view>
        </view>
      </view>
      <view class="column">
        <view class="item" v-for="item in rightCol" :key="item.id">
          <image :src="item.imageUrl" mode="widthFix" class="img" lazy-load @click="previewImage(item.imageUrl)" />
          <text class="desc">{{ item.description }}</text>
          <view class="footer">
            <view class="user">
              <image :src="item.userAvatar || '/static/avatar.png'" class="avatar" />
              <text class="name">{{ item.userName }}</text>
            </view>
            <view class="actions">
              <view class="action" @click="handleLike(item)">
                <text class="icon" :class="{ active: item.isLiked }">❤</text>
                <text class="count">{{ item.likes }}</text>
              </view>
              <view class="action" @click="handleCollect(item)">
                <text class="icon" :class="{ active: item.isCollected }">★</text>
                <text class="count">{{ item.collects }}</text>
              </view>
            </view>
          </view>
        </view>
      </view>
    </view>
    
    <view v-if="loading" class="loading">加载中...</view>
    <view v-if="!loading && list.length === 0" class="empty">暂无收藏内容</view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { getCollectedInspirations, likeInspiration, collectInspiration, type Inspiration } from '@/api/inspiration';

const list = ref<Inspiration[]>([]);
const loading = ref(false);

const leftCol = computed(() => list.value.filter((_, i) => i % 2 === 0));
const rightCol = computed(() => list.value.filter((_, i) => i % 2 === 1));

onMounted(() => {
  loadData();
});

const loadData = async () => {
  loading.value = true;
  try {
    const res = await getCollectedInspirations();
    list.value = res;
  } catch (error) {
    console.error(error);
  } finally {
    loading.value = false;
  }
};

const handleLike = async (item: Inspiration) => {
  try {
    const res = await likeInspiration(item.id);
    item.likes = res.likes;
    item.isLiked = res.isLiked;
  } catch (error) {
    uni.showToast({ title: '操作失败', icon: 'none' });
  }
};

const handleCollect = async (item: Inspiration) => {
  try {
    const res = await collectInspiration(item.id);
    item.collects = res.collects;
    item.isCollected = res.isCollected;
    // If uncollected, remove from list
    if (!res.isCollected) {
        list.value = list.value.filter(i => i.id !== item.id);
    }
  } catch (error) {
    uni.showToast({ title: '操作失败', icon: 'none' });
  }
};

const previewImage = (url: string) => {
  uni.previewImage({ urls: [url] });
};
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  background: #f8f9fa;
  min-height: 100vh;
}

.waterfall {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  
  .column {
    width: 48%;
    
    .item {
      background: #fff;
      border-radius: 16rpx;
      overflow: hidden;
      margin-bottom: 20rpx;
      box-shadow: 0 4rpx 10rpx rgba(0,0,0,0.05);
      
      .img {
        width: 100%;
        display: block;
      }
      
      .desc {
        font-size: 26rpx;
        color: #333;
        padding: 16rpx;
        display: block;
        line-height: 1.4;
      }
      
      .footer {
        padding: 0 16rpx 16rpx;
        display: flex;
        justify-content: space-between;
        align-items: center;
        
        .user {
          display: flex;
          align-items: center;
          
          .avatar {
            width: 32rpx;
            height: 32rpx;
            border-radius: 50%;
            margin-right: 8rpx;
          }
          
          .name {
            font-size: 20rpx;
            color: #999;
            max-width: 100rpx;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
          }
        }
        
        .actions {
          display: flex;
          gap: 16rpx;
          
          .action {
            display: flex;
            align-items: center;
            
            .icon {
              font-size: 24rpx;
              color: #ccc;
              margin-right: 4rpx;
              
              &.active {
                color: #ff385c;
              }
            }
            
            .count {
              font-size: 20rpx;
              color: #999;
            }
          }
        }
      }
    }
  }
}

.loading, .empty {
  text-align: center;
  padding: 40rpx;
  color: #999;
  font-size: 28rpx;
}
</style>
