<template>
  <view class="container">
    <view class="tabs">
      <view 
        class="tab-item" 
        :class="{ active: currentTab === 'all' }" 
        @click="currentTab = 'all'"
      >全部</view>
      <view 
        class="tab-item" 
        :class="{ active: currentTab === 'Pending' }" 
        @click="currentTab = 'Pending'"
      >待支付</view>
      <view 
        class="tab-item" 
        :class="{ active: currentTab === 'Paid' }" 
        @click="currentTab = 'Paid'"
      >待出行</view>
      <view 
        class="tab-item" 
        :class="{ active: currentTab === 'Completed' }" 
        @click="currentTab = 'Completed'"
      >已完成</view>
    </view>

    <scroll-view scroll-y class="order-list">
      <view class="empty-state" v-if="filteredBookings.length === 0">
        <text>暂无订单</text>
      </view>
      
      <view 
        class="order-card" 
        v-for="booking in filteredBookings" 
        :key="booking.id"
      >
        <view class="card-header">
          <text class="order-no">订单号: {{ booking.orderNumber }}</text>
          <text class="status" :class="booking.status.toLowerCase()">{{ getStatusText(booking.status) }}</text>
        </view>
        
        <view class="card-body" @click="goToDetail(booking.id)">
          <image :src="booking.tourRoute?.imageUrl || '/static/placeholder.png'" class="route-image" mode="aspectFill" />
          <view class="info">
            <text class="title">{{ booking.tourRoute?.title || '未知路线' }}</text>
            <view class="meta">
              <text>出行日期: {{ formatDate(booking.travelDate) }}</text>
              <text>人数: {{ booking.peopleCount }}人</text>
            </view>
            <view class="butler" v-if="booking.butler">
              <text>管家: {{ booking.butler.name }}</text>
            </view>
          </view>
        </view>
        
        <view class="card-footer">
          <text class="amount">总价: ¥{{ booking.totalAmount }}</text>
          <view class="actions">
            <button 
              class="btn btn-cancel" 
              v-if="booking.status === 'Pending'" 
              @click.stop="handleCancel(booking)"
            >取消</button>
            <button 
              class="btn btn-delete" 
              v-if="booking.status === 'Cancelled' || booking.status === 'Completed'" 
              @click.stop="handleDelete(booking)"
            >删除</button>
            <button 
              class="btn btn-pay" 
              v-if="booking.status === 'Pending'" 
              @click.stop="handlePay(booking)"
            >去支付</button>
          </view>
        </view>
      </view>
    </scroll-view>
  </view>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { getMyBookings, cancelBooking, deleteBooking, type Booking } from '@/api/bookings';

const bookings = ref<Booking[]>([]);
const currentTab = ref('all');

const filteredBookings = computed(() => {
  if (currentTab.value === 'all') return bookings.value;
  if (currentTab.value === 'Paid') {
      // Show both Paid and Confirmed in "To Go" tab
      return bookings.value.filter(b => b.status === 'Paid' || b.status === 'Confirmed');
  }
  return bookings.value.filter(b => b.status === currentTab.value);
});

onMounted(() => {
  loadBookings();
});

const loadBookings = async () => {
  try {
    bookings.value = await getMyBookings();
  } catch (error) {
    console.error(error);
  }
};

const getStatusText = (status: string) => {
  const map: Record<string, string> = {
    'Pending': '待支付',
    'Confirmed': '待出行', // Auto-confirmed maps to Paid/Confirmed logic
    'Paid': '待出行',
    'Completed': '已完成',
    'Cancelled': '已取消'
  };
  return map[status] || status;
};

const formatDate = (dateStr?: string) => {
  if (!dateStr) return '';
  return new Date(dateStr).toLocaleDateString();
};

const goToDetail = (id: string) => {
  uni.navigateTo({ url: `/pages/booking/detail?id=${id}` });
};

const handleCancel = async (booking: Booking) => {
  uni.showModal({
    title: '提示',
    content: '确定要取消该订单吗？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await cancelBooking(booking.id);
          uni.showToast({ title: '取消成功' });
          loadBookings();
        } catch (error) {
          console.error(error);
        }
      }
    }
  });
};

const handleDelete = async (booking: Booking) => {
  uni.showModal({
    title: '提示',
    content: '确定要删除该订单吗？删除后不可恢复。',
    success: async (res) => {
      if (res.confirm) {
        try {
          await deleteBooking(booking.id);
          uni.showToast({ title: '删除成功' });
          loadBookings();
        } catch (error) {
          console.error(error);
        }
      }
    }
  });
};

const handlePay = (booking: Booking) => {
  uni.showToast({ title: '支付功能开发中', icon: 'none' });
};
</script>

<style lang="scss">
.container {
  background-color: #f8f9fa;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.tabs {
  display: flex;
  background: #fff;
  padding: 20rpx 0;
  position: sticky;
  top: 0;
  z-index: 10;
  
  .tab-item {
    flex: 1;
    text-align: center;
    font-size: 28rpx;
    color: #666;
    position: relative;
    padding-bottom: 10rpx;
    
    &.active {
      color: #007aff;
      font-weight: bold;
      
      &::after {
        content: '';
        position: absolute;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%);
        width: 40rpx;
        height: 4rpx;
        background: #007aff;
        border-radius: 2rpx;
      }
    }
  }
}

.order-list {
  flex: 1;
  padding: 20rpx;
}

.empty-state {
  text-align: center;
  padding-top: 200rpx;
  color: #999;
}

.order-card {
  background: #fff;
  border-radius: 16rpx;
  padding: 24rpx;
  margin-bottom: 20rpx;
  box-shadow: 0 2rpx 8rpx rgba(0,0,0,0.05);
  
  .card-header {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20rpx;
    font-size: 26rpx;
    
    .order-no {
      color: #666;
    }
    
    .status {
      font-weight: bold;
      
      &.pending { color: #ff9800; }
      &.paid { color: #007aff; }
      &.completed { color: #4caf50; }
      &.cancelled { color: #999; }
    }
  }
  
  .card-body {
    display: flex;
    margin-bottom: 20rpx;
    
    .route-image {
      width: 160rpx;
      height: 160rpx;
      border-radius: 12rpx;
      margin-right: 20rpx;
    }
    
    .info {
      flex: 1;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      
      .title {
        font-size: 30rpx;
        font-weight: bold;
        color: #333;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
      }
      
      .meta {
        font-size: 24rpx;
        color: #999;
        display: flex;
        flex-direction: column;
        gap: 4rpx;
      }
      
      .butler {
        font-size: 24rpx;
        color: #666;
        background: #f5f5f5;
        padding: 4rpx 12rpx;
        border-radius: 8rpx;
        align-self: flex-start;
      }
    }
  }
  
  .card-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-top: 1rpx solid #f5f5f5;
    padding-top: 20rpx;
    
    .amount {
      font-size: 32rpx;
      font-weight: bold;
      color: #333;
    }
    
    .actions {
      display: flex;
      gap: 20rpx;
      
      .btn {
        font-size: 26rpx;
        padding: 0 30rpx;
        height: 60rpx;
        line-height: 60rpx;
        border-radius: 30rpx;
        margin: 0;
        
        &.btn-cancel {
          background: #f5f5f5;
          color: #666;
        }
        
        &.btn-pay {
          background: #007aff;
          color: #fff;
        }
      }
    }
  }
}
</style>
