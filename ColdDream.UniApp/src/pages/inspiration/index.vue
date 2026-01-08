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
    <view v-if="!loading && list.length === 0" class="empty">暂无灵感，快来发布吧</view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { getInspirations, likeInspiration, collectInspiration, type Inspiration } from '@/api/inspiration';

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
    const res = await getInspirations();
    list.value = res;
    
    // Fallback data for demo if empty
    if (list.value.length === 0) {
      list.value = [
        { id: '1', imageUrl: 'https://picsum.photos/300/400?random=1', description: '京都的秋天真是太美了！', userId: 'u1', userName: '旅行家小A', userAvatar: '', likes: 120, collects: 45, isLiked: false, isCollected: false },
        { id: '2', imageUrl: 'https://picsum.photos/300/500?random=2', description: '北海道滑雪初体验', userId: 'u2', userName: '滑雪爱好者', userAvatar: '', likes: 88, collects: 20, isLiked: false, isCollected: false },
        { id: '3', imageUrl: 'https://picsum.photos/300/350?random=3', description: '冲绳潜水，见到了海龟', userId: 'u3', userName: '深蓝', userAvatar: '', likes: 230, collects: 100, isLiked: true, isCollected: false },
        { id: '4', imageUrl: 'https://picsum.photos/300/450?random=4', description: '奈良的小鹿很可爱，但是会咬人', userId: 'u4', userName: '鹿鹿', userAvatar: '', likes: 56, collects: 12, isLiked: false, isCollected: true },
      ];
    }
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
    // If 401, prompt login
    uni.showToast({ title: '请先登录', icon: 'none' });
  }
};

const handleCollect = async (item: Inspiration) => {
  try {
    const res = await collectInspiration(item.id);
    item.collects = res.collects;
    item.isCollected = res.isCollected;
  } catch (error) {
    uni.showToast({ title: '请先登录', icon: 'none' });
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
