<template>
  <view class="container" v-if="booking">
    <view class="status-card" :class="booking.status.toLowerCase()">
      <text class="status-text">{{ getStatusText(booking.status) }}</text>
      <text class="status-desc">{{ getStatusDesc(booking.status) }}</text>
    </view>

    <view class="section">
      <text class="section-title">路线信息</text>
      <view class="route-info" @click="goToRoute(booking.tourRouteId)">
        <image :src="booking.tourRoute?.imageUrl || '/static/placeholder.png'" class="route-image" mode="aspectFill" />
        <view class="info">
          <text class="title">{{ booking.tourRoute?.title }}</text>
          <text class="price">¥{{ booking.tourRoute?.price }}/人</text>
        </view>
      </view>
    </view>

    <view class="section">
      <text class="section-title">预约信息</text>
      <view class="info-item">
        <text class="label">出行日期</text>
        <text class="value">{{ formatDate(booking.bookingDate) }}</text>
      </view>
      <view class="info-item">
        <text class="label">出行人数</text>
        <text class="value">{{ booking.travelers }}人</text>
      </view>
      <view class="info-item">
        <text class="label">联系人</text>
        <text class="value">{{ booking.contactName }}</text>
      </view>
      <view class="info-item">
        <text class="label">联系电话</text>
        <text class="value">{{ booking.contactPhone }}</text>
      </view>
    </view>

    <view class="section" v-if="booking.butler">
      <text class="section-title">私人管家</text>
      <view class="butler-info">
        <image :src="booking.butler.avatarUrl || '/static/avatar.png'" class="butler-avatar" />
        <view class="butler-detail">
          <text class="name">{{ booking.butler.name }}</text>
          <text class="tags" v-if="booking.butler.tags">{{ parseTags(booking.butler.tags) }}</text>
        </view>
        <text class="price">+¥{{ booking.butler.price }}</text>
      </view>
    </view>

    <view class="section price-section">
      <view class="price-row">
        <text>路线费用</text>
        <text>¥{{ (booking.tourRoute?.price || 0) * booking.travelers }}</text>
      </view>
      <view class="price-row" v-if="booking.butler">
        <text>管家服务费</text>
        <text>¥{{ booking.butler.price }}</text>
      </view>
      <view class="total-row">
        <text>实付金额</text>
        <text class="total-price">¥{{ booking.totalPrice }}</text>
      </view>
    </view>

    <view class="footer-actions">
      <button 
        class="btn btn-delete" 
        v-if="booking.status === 'Cancelled' || booking.status === 'Completed'" 
        @click="handleDelete"
      >删除订单</button>
      <button 
        class="btn btn-cancel" 
        v-if="booking.status === 'Pending' || booking.status === 'Confirmed'" 
        @click="handleCancel"
      >取消订单</button>
      <button 
        class="btn btn-pay" 
        v-if="booking.status === 'Pending'" 
        @click="handlePay"
      >去支付</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { getBookingById, cancelBooking, deleteBooking, type Booking } from '@/api/bookings';

const booking = ref<Booking | null>(null);

onLoad((options: any) => {
  if (options.id) {
    loadDetail(options.id);
  }
});

const loadDetail = async (id: string) => {
  try {
    booking.value = await getBookingById(id);
  } catch (error) {
    console.error(error);
    uni.showToast({ title: '获取详情失败', icon: 'none' });
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

const getStatusDesc = (status: string) => {
  const map: Record<string, string> = {
    'Pending': '请在15分钟内完成支付',
    'Confirmed': '您的预约已确认，管家将尽快联系您',
    'Completed': '期待您的下次光临',
    'Cancelled': '订单已取消'
  };
  return map[status] || '';
};

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString();
};

const parseTags = (tagsStr: string) => {
  try {
    const tags = JSON.parse(tagsStr);
    return tags.join(' | ');
  } catch {
    return tagsStr;
  }
};

const goToRoute = (id: string) => {
  uni.navigateTo({ url: `/pages/routes/detail?id=${id}` });
};

const handleCancel = async () => {
  if (!booking.value) return;
  uni.showModal({
    title: '提示',
    content: '确定要取消该订单吗？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await cancelBooking(booking.value!.id);
          uni.showToast({ title: '取消成功' });
          loadDetail(booking.value!.id);
        } catch (error) {
          console.error(error);
        }
      }
    }
  });
};

const handleDelete = async () => {
  if (!booking.value) return;
  uni.showModal({
    title: '提示',
    content: '确定要删除该订单吗？删除后不可恢复。',
    success: async (res) => {
      if (res.confirm) {
        try {
          await deleteBooking(booking.value!.id);
          uni.showToast({ title: '删除成功' });
          setTimeout(() => {
            uni.navigateBack();
          }, 1500);
        } catch (error) {
          console.error(error);
        }
      }
    }
  });
};

const handlePay = () => {
  uni.showToast({ title: '支付功能开发中', icon: 'none' });
};
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  padding-bottom: 140rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.status-card {
  background: #fff;
  padding: 40rpx;
  border-radius: 16rpx;
  margin-bottom: 20rpx;
  text-align: center;
  
  .status-text {
    font-size: 36rpx;
    font-weight: bold;
    display: block;
    margin-bottom: 10rpx;
  }
  
  .status-desc {
    font-size: 24rpx;
    color: #666;
  }
  
  &.pending .status-text { color: #ff9800; }
  &.confirmed .status-text { color: #007aff; }
  &.completed .status-text { color: #4caf50; }
  &.cancelled .status-text { color: #999; }
}

.section {
  background: #fff;
  padding: 30rpx;
  border-radius: 16rpx;
  margin-bottom: 20rpx;
  
  .section-title {
    font-size: 30rpx;
    font-weight: bold;
    margin-bottom: 24rpx;
    display: block;
    border-left: 6rpx solid #007aff;
    padding-left: 16rpx;
  }
}

.route-info {
  display: flex;
  background: #f9f9f9;
  padding: 20rpx;
  border-radius: 12rpx;
  
  .route-image {
    width: 120rpx;
    height: 120rpx;
    border-radius: 8rpx;
    margin-right: 20rpx;
  }
  
  .info {
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    
    .title {
      font-size: 28rpx;
      font-weight: bold;
      margin-bottom: 10rpx;
    }
    
    .price {
      color: #ff5a5f;
      font-size: 26rpx;
    }
  }
}

.info-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20rpx;
  font-size: 28rpx;
  
  .label { color: #666; }
  .value { color: #333; }
  
  &:last-child { margin-bottom: 0; }
}

.butler-info {
  display: flex;
  align-items: center;
  
  .butler-avatar {
    width: 80rpx;
    height: 80rpx;
    border-radius: 50%;
    margin-right: 20rpx;
  }
  
  .butler-detail {
    flex: 1;
    
    .name {
      font-size: 28rpx;
      font-weight: bold;
      display: block;
    }
    
    .tags {
      font-size: 22rpx;
      color: #999;
    }
  }
  
  .price {
    color: #ff5a5f;
    font-weight: bold;
  }
}

.price-section {
  .price-row {
    display: flex;
    justify-content: space-between;
    font-size: 26rpx;
    color: #666;
    margin-bottom: 16rpx;
  }
  
  .total-row {
    display: flex;
    justify-content: space-between;
    font-size: 30rpx;
    font-weight: bold;
    border-top: 1rpx solid #eee;
    padding-top: 20rpx;
    margin-top: 20rpx;
    
    .total-price { color: #ff5a5f; font-size: 36rpx; }
  }
}

.footer-actions {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: #fff;
  padding: 20rpx 40rpx;
  display: flex;
  justify-content: flex-end;
  gap: 20rpx;
  box-shadow: 0 -2rpx 10rpx rgba(0,0,0,0.05);
  
  .btn {
    margin: 0;
    font-size: 28rpx;
    border-radius: 40rpx;
    padding: 0 40rpx;
    
    &.btn-delete {
      background: #fff;
      color: #999;
      border: 1rpx solid #ddd;
    }
    
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
</style>
