<template>
  <view class="container">
    <view class="guide-list">
      <view class="guide-item" v-for="item in list" :key="item.id" @click="goToDetail(item.id)">
        <image :src="item.imageUrl || '/static/placeholder.png'" class="guide-image" mode="aspectFill" />
        <view class="guide-info">
          <view class="title-row">
            <text class="guide-title">{{ item.title }}</text>
            <text class="status-badge" :class="'status-' + item.status">
              {{ item.status === 1 ? '已发布' : (item.status === 2 ? '已驳回' : '审核中') }}
            </text>
          </view>
          <text class="guide-desc">{{ item.description }}</text>
          <view class="guide-footer">
            <text class="time">{{ formatDateTime(item.createdAt) }}</text>
            <view class="actions">
              <button class="btn-mini btn-edit" @click.stop="goToEdit(item.id)">编辑</button>
              <button class="btn-mini btn-delete" @click.stop="handleDelete(item)">删除</button>
            </view>
          </view>
        </view>
      </view>
      
      <view class="empty" v-if="list.length === 0">
        <text>暂无攻略，快去创建吧~</text>
      </view>
    </view>
    
    <view class="fab-btn" @click="goToCreate">
      <text class="plus">+</text>
      <text>新建攻略</text>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { onShow } from '@dcloudio/uni-app';
import { getMyGuides, deleteGuide, type Guide } from '@/api/guide';

const list = ref<Guide[]>([]);

onShow(() => {
  loadData();
});

const loadData = async () => {
  try {
    const res = await getMyGuides();
    list.value = res;
  } catch (error) {
    console.error(error);
  }
};

const goToCreate = () => {
  uni.navigateTo({ url: '/pages/guide/create' });
};

const goToEdit = (id: string) => {
  uni.navigateTo({ url: `/pages/guide/create?id=${id}` });
};

const goToDetail = (id: string) => {
  // Reuse create page for viewing detail for now, or create a separate detail page
  // User said "content same as tour route", so maybe a detail page is better.
  // But for now, let's just allow edit.
  goToEdit(id);
};

const handleDelete = (item: Guide) => {
  uni.showModal({
    title: '提示',
    content: '确定要删除这篇攻略吗？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await deleteGuide(item.id);
          uni.showToast({ title: '已删除' });
          loadData();
        } catch (error) {
          console.error(error);
        }
      }
    }
  });
};

const formatDateTime = (dateStr: string) => {
  if (!dateStr) return '';
  const date = new Date(dateStr);
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;
};
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  padding-bottom: 140rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.guide-item {
  background: #fff;
  border-radius: 20rpx;
  overflow: hidden;
  margin-bottom: 20rpx;
  box-shadow: 0 2rpx 10rpx rgba(0,0,0,0.03);
  
  .guide-image {
    width: 100%;
    height: 300rpx;
  }
  
  .guide-info {
    padding: 20rpx;
    
    .title-row {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 10rpx;
      
      .guide-title {
        font-size: 32rpx;
        font-weight: bold;
        color: #333;
        flex: 1;
        margin-right: 20rpx;
      }
      
      .status-badge {
        font-size: 20rpx;
        padding: 4rpx 12rpx;
        border-radius: 8rpx;
        
        &.status-0 { background: #fff3cd; color: #856404; } // Pending
        &.status-1 { background: #d4edda; color: #155724; } // Published
        &.status-2 { background: #f8d7da; color: #721c24; } // Rejected
      }
    }
    
    .guide-desc {
      font-size: 26rpx;
      color: #666;
      margin-bottom: 20rpx;
      display: block;
      line-height: 1.4;
    }
    
    .guide-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-top: 1px solid #f0f0f0;
      padding-top: 20rpx;
      
      .time {
        font-size: 22rpx;
        color: #999;
      }
      
      .actions {
        display: flex;
        gap: 20rpx;
        
        .btn-mini {
          margin: 0;
          padding: 0 24rpx;
          height: 50rpx;
          line-height: 50rpx;
          font-size: 24rpx;
          border-radius: 25rpx;
          
          &.btn-edit {
            background: #fff;
            border: 1px solid #007aff;
            color: #007aff;
          }
          
          &.btn-delete {
            background: #fff;
            border: 1px solid #ff3b30;
            color: #ff3b30;
          }
        }
      }
    }
  }
}

.empty {
  text-align: center;
  padding: 100rpx 0;
  color: #999;
  font-size: 28rpx;
}

.fab-btn {
  position: fixed;
  bottom: 40rpx;
  left: 50%;
  transform: translateX(-50%);
  background: #007aff;
  color: #fff;
  padding: 20rpx 60rpx;
  border-radius: 50rpx;
  display: flex;
  align-items: center;
  gap: 10rpx;
  box-shadow: 0 8rpx 20rpx rgba(0, 122, 255, 0.3);
  z-index: 100;
  
  .plus {
    font-size: 40rpx;
    font-weight: bold;
    line-height: 1;
  }
}
</style>
