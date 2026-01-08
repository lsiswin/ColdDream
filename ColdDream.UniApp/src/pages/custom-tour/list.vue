<template>
  <view class="container">
    <view class="list" v-if="list.length > 0">
      <view class="card" v-for="item in list" :key="item.id">
        <view class="card-header">
          <text class="destination">{{ item.destination }}</text>
          <text class="status" :class="getStatusClass(item.status)">{{ getStatusText(item.status) }}</text>
        </view>
        
        <view class="card-body">
          <view class="info-row">
            <text class="label">出发日期</text>
            <text class="value">{{ formatDate(item.startDate) }}</text>
          </view>
          <view class="info-row">
            <text class="label">行程概况</text>
            <text class="value">{{ item.days }}天 · {{ item.peopleCount }}人 · {{ item.budget }}</text>
          </view>
          <view class="info-row" v-if="item.requirements">
            <text class="label">备注需求</text>
            <text class="value">{{ item.requirements }}</text>
          </view>
        </view>
        
        <view class="card-footer">
          <text class="time">提交时间: {{ formatDateTime(item.createdAt) }}</text>
          <view class="actions">
            <button 
              class="btn-mini btn-cancel" 
              v-if="['Pending', 'Processing'].includes(item.status)"
              @click.stop="handleCancel(item)"
            >取消</button>
            <button 
              class="btn-mini btn-delete" 
              v-if="['Completed', 'Cancelled'].includes(item.status)"
              @click.stop="handleDelete(item)"
            >删除</button>
          </view>
        </view>
      </view>
    </view>
    
    <view class="empty-state" v-else>
      <text class="empty-text">您还没有定制记录</text>
      <button class="btn-create" @click="goToCreate">去定制</button>
    </view>
    
    <view class="fab" @click="goToCreate" v-if="list.length > 0">+</view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getMyCustomTours, cancelCustomTour, deleteCustomTour, type CustomTour } from '@/api/custom-tour';

const list = ref<CustomTour[]>([]);

onMounted(() => {
  loadData();
});

const loadData = async () => {
  try {
    const res = await getMyCustomTours();
    list.value = res;
  } catch (error) {
    console.error(error);
  }
};

const handleCancel = (item: CustomTour) => {
  uni.showModal({
    title: '提示',
    content: '确定要取消这个定制需求吗？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await cancelCustomTour(item.id);
          uni.showToast({ title: '已取消' });
          loadData();
        } catch (error) {
          console.error(error);
        }
      }
    }
  });
};

const handleDelete = (item: CustomTour) => {
  uni.showModal({
    title: '提示',
    content: '确定要删除这条记录吗？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await deleteCustomTour(item.id);
          uni.showToast({ title: '已删除' });
          loadData();
        } catch (error) {
          console.error(error);
        }
      }
    }
  });
};

const getStatusText = (status: string) => {
  const map: Record<string, string> = {
    'Pending': '处理中',
    'Processing': '方案生成中',
    'Completed': '方案已出',
    'Cancelled': '已取消'
  };
  return map[status] || status;
};

const getStatusClass = (status: string) => {
  return status.toLowerCase();
};

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString();
};

const formatDateTime = (dateStr: string) => {
  return new Date(dateStr).toLocaleString();
};

const goToCreate = () => {
  uni.navigateTo({ url: '/pages/custom-tour/create' });
};
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  background: #f8f9fa;
  min-height: 100vh;
}

.card {
  background: #fff;
  border-radius: 16rpx;
  padding: 30rpx;
  margin-bottom: 20rpx;
  box-shadow: 0 2rpx 8rpx rgba(0,0,0,0.05);
  
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20rpx;
    padding-bottom: 20rpx;
    border-bottom: 1px solid #f5f5f5;
    
    .destination {
      font-size: 32rpx;
      font-weight: bold;
      color: #333;
    }
    
    .status {
      font-size: 24rpx;
      padding: 4rpx 12rpx;
      border-radius: 8rpx;
      
      &.pending { background: #fff3cd; color: #856404; }
      &.processing { background: #e3f2fd; color: #0d47a1; }
      &.completed { background: #d4edda; color: #155724; }
    }
  }
  
  .card-body {
    .info-row {
      display: flex;
      margin-bottom: 16rpx;
      font-size: 28rpx;
      
      .label {
        color: #999;
        width: 140rpx;
      }
      
      .value {
        color: #333;
        flex: 1;
      }
    }
  }
  
  .card-footer {
    margin-top: 20rpx;
    display: flex;
    justify-content: space-between;
    align-items: center;
    
    .time {
      font-size: 22rpx;
      color: #ccc;
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
        
        &.btn-cancel {
          background: #fff;
          border: 1px solid #ccc;
          color: #666;
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

.empty-state {
  text-align: center;
  padding-top: 200rpx;
  
  .empty-text {
    color: #999;
    font-size: 28rpx;
    display: block;
    margin-bottom: 40rpx;
  }
  
  .btn-create {
    width: 240rpx;
    height: 80rpx;
    line-height: 80rpx;
    border-radius: 40rpx;
    background: #007aff;
    color: #fff;
    font-size: 28rpx;
  }
}

.fab {
  position: fixed;
  bottom: 60rpx;
  right: 40rpx;
  width: 100rpx;
  height: 100rpx;
  background: #007aff;
  color: #fff;
  border-radius: 50%;
  text-align: center;
  line-height: 90rpx;
  font-size: 60rpx;
  box-shadow: 0 8rpx 20rpx rgba(0, 122, 255, 0.3);
}
</style>
