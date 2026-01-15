<template>
  <view class="container">
    <view class="coupon-list">
      <view class="coupon-card" v-for="item in list" :key="item.id">
        <view class="left">
          <text class="amount">¥<text class="num">{{ item.amount }}</text></text>
          <text class="condition">满{{ item.minSpend }}可用</text>
        </view>
        <view class="right">
          <text class="name">{{ item.name }}</text>
          <text class="desc">仅限旅游路线使用 (不含管家费)</text>
          <text class="date">有效期至 {{ formatDateTime(item.expiryDate) }}</text>
        </view>
        <view class="circle top"></view>
        <view class="circle bottom"></view>
      </view>
    </view>
    
    <view class="empty" v-if="list.length === 0">
      <text>暂无可用优惠券</text>
    </view>
    
    <button class="btn-test" @click="getTestCoupon">领取测试红包</button>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getMyCoupons, issueTestCoupon, type Coupon } from '@/api/coupon';

const list = ref<Coupon[]>([]);

onMounted(() => {
  loadData();
});

const loadData = async () => {
  try {
    const res = await getMyCoupons();
    if (res.success) {
      list.value = res.data;
    }
  } catch (error) {
    console.error(error);
  }
};

const getTestCoupon = async () => {
  try {
    const res = await issueTestCoupon();
    if (res.success) {
      uni.showToast({ title: '领取成功' });
      loadData();
    } else {
      uni.showToast({ title: res.message || '领取失败', icon: 'none' });
    }
  } catch (error) {
    console.error(error);
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
  padding: 30rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.coupon-card {
  display: flex;
  background: #fff;
  border-radius: 16rpx;
  margin-bottom: 24rpx;
  position: relative;
  overflow: hidden;
  box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.05);
  
  .left {
    width: 200rpx;
    background: linear-gradient(135deg, #ff6b6b, #ff4757);
    color: #fff;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 20rpx;
    
    .amount {
      font-size: 32rpx;
      font-weight: bold;
      .num {
        font-size: 60rpx;
      }
    }
    
    .condition {
      font-size: 22rpx;
      opacity: 0.9;
    }
  }
  
  .right {
    flex: 1;
    padding: 24rpx;
    display: flex;
    flex-direction: column;
    justify-content: center;
    
    .name {
      font-size: 30rpx;
      font-weight: bold;
      color: #333;
      margin-bottom: 8rpx;
    }
    
    .desc {
      font-size: 22rpx;
      color: #999;
      margin-bottom: 16rpx;
    }
    
    .date {
      font-size: 20rpx;
      color: #ccc;
    }
  }
  
  .circle {
    position: absolute;
    width: 30rpx;
    height: 30rpx;
    background: #f8f9fa;
    border-radius: 50%;
    left: 185rpx;
    z-index: 1;
    
    &.top { top: -15rpx; }
    &.bottom { bottom: -15rpx; }
  }
}

.btn-test {
  margin-top: 60rpx;
  background: #ff4757;
  color: #fff;
  border-radius: 50rpx;
}

.empty {
  text-align: center;
  padding: 100rpx 0;
  color: #999;
}
</style>
