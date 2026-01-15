<template>
  <view class="container">
    <view class="section" v-if="route">
      <text class="section-title">预约路线</text>
      <view class="route-info">
        <text class="route-title">{{ route.title }}</text>
        <text class="route-price">¥{{ route.price }}/人</text>
      </view>
    </view>

    <view class="section">
      <text class="section-title">选择日期与时间</text>
      <picker mode="date" :start="startDate" @change="onDateChange">
        <view class="picker-item">
          <text>日期</text>
          <text>{{ form.travelDate || '请选择日期' }}</text>
        </view>
      </picker>
      <picker mode="selector" :range="['上午', '下午']" @change="onTimeSlotChange">
        <view class="picker-item">
          <text>时间段</text>
          <text>{{ form.timeSlot === 0 ? '上午' : (form.timeSlot === 1 ? '下午' : '请选择时间段') }}</text>
        </view>
      </picker>
    </view>

    <view class="section">
      <text class="section-title">出行人数</text>
      <input class="input" type="number" v-model.number="form.peopleCount" placeholder="请输入人数" />
    </view>

    <view class="section">
      <text class="section-title">选择管家 (可选)</text>
      <view class="butler-list">
        <view 
          class="butler-item" 
          :class="{ active: form.butlerId === butler.id }"
          v-for="butler in butlers" 
          :key="butler.id"
          @click="selectButler(butler)"
        >
          <image :src="butler.avatarUrl || '/static/avatar.png'" class="butler-avatar" />
          <view class="butler-info">
            <text class="butler-name">{{ butler.name }}</text>
            <text class="butler-price">+¥{{ butler.price }}</text>
          </view>
        </view>
      </view>
    </view>

    <view class="section">
      <text class="section-title">联系人信息</text>
      <input class="input" v-model="form.contactName" placeholder="联系人姓名" />
      <input class="input" v-model="form.contactPhone" placeholder="联系电话" />
    </view>

    <view class="section">
      <text class="section-title">优惠券 (仅限路线费用)</text>
      <picker mode="selector" :range="availableCoupons" range-key="name" @change="onCouponChange">
        <view class="picker-item">
          <text>选择优惠券</text>
          <text v-if="selectedCoupon" class="coupon-text">-¥{{ discountAmount }}</text>
          <text v-else class="placeholder">{{ availableCoupons.length > 0 ? '有可用优惠券' : '无可用优惠券' }}</text>
        </view>
      </picker>
    </view>

    <view class="footer">
      <view class="total-price">
        <text>总价: </text>
        <text class="price">¥{{ totalPrice }}</text>
      </view>
      <button class="btn-submit" @click="submitBooking">提交预约</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { getRouteDetails, type TourRoute } from '@/api/routes';
import { getButlersByRoute, type Butler } from '@/api/butlers';
import { createBooking, type BookingRequest } from '@/api/bookings';
import { getMyCoupons, type Coupon } from '@/api/coupon';

const routeId = ref('');
const route = ref<TourRoute | null>(null);
const butlers = ref<Butler[]>([]);
const coupons = ref<Coupon[]>([]);
const startDate = new Date().toISOString().split('T')[0];

const form = ref<BookingRequest & { couponId?: string }>({
  tourRouteId: '',
  peopleCount: 1,
  timeSlot: -1,
  contactName: '',
  contactPhone: '',
  travelDate: '',
  butlerId: undefined,
  couponId: undefined
});

const selectedButlerPrice = ref(0);
const selectedCoupon = ref<Coupon | null>(null);

onLoad(async (options: any) => {
  if (options.routeId) {
    routeId.value = options.routeId;
    form.value.tourRouteId = options.routeId;
    await loadData();
  }
});

const loadData = async () => {
  try {
    const [routeRes, butlersRes, couponsRes] = await Promise.all([
      getRouteDetails(routeId.value),
      getButlersByRoute(routeId.value),
      getMyCoupons()
    ]);
    if (routeRes.success) route.value = routeRes.data;
    if (butlersRes.success) butlers.value = butlersRes.data;
    if (couponsRes.success) coupons.value = couponsRes.data;
  } catch (error) {
    console.error(error);
  }
};

const onDateChange = (e: any) => {
  form.value.travelDate = e.detail.value;
};

const onTimeSlotChange = (e: any) => {
  form.value.timeSlot = e.detail.value;
};

const selectButler = (butler: Butler) => {
  if (form.value.butlerId === butler.id) {
    form.value.butlerId = undefined;
    selectedButlerPrice.value = 0;
  } else {
    form.value.butlerId = butler.id;
    selectedButlerPrice.value = butler.price;
  }
};

const onCouponChange = (e: any) => {
  const index = e.detail.value;
  if (index >= 0) {
    const coupon = availableCoupons.value[index];
    selectedCoupon.value = coupon;
    form.value.couponId = coupon.id;
  } else {
    selectedCoupon.value = null;
    form.value.couponId = undefined;
  }
};

const routeTotalPrice = computed(() => {
  if (!route.value) return 0;
  return route.value.price * form.value.peopleCount;
});

const availableCoupons = computed(() => {
  return coupons.value.filter(c => routeTotalPrice.value >= c.minSpend);
});

const discountAmount = computed(() => {
  if (!selectedCoupon.value) return 0;
  if (routeTotalPrice.value < selectedCoupon.value.minSpend) {
    // If price drops below minSpend (e.g. reduced people), remove coupon
    selectedCoupon.value = null;
    form.value.couponId = undefined;
    return 0;
  }
  return Math.min(selectedCoupon.value.amount, routeTotalPrice.value);
});

const totalPrice = computed(() => {
  return Math.max(0, routeTotalPrice.value + selectedButlerPrice.value - discountAmount.value);
});

const submitBooking = async () => {
  if (!form.value.travelDate || form.value.timeSlot === -1 || !form.value.contactName || !form.value.contactPhone) {
    uni.showToast({ title: '请填写完整信息', icon: 'none' });
    return;
  }

  try {
    const res = await createBooking(form.value);
    
    if (res.success) {
      uni.showToast({ title: '预约成功' });
      setTimeout(() => {
        uni.switchTab({ url: '/pages/index/index' });
      }, 1500);
    } else {
      uni.showToast({ title: res.message || '预约失败', icon: 'none' });
    }
  } catch (error) {
    console.error(error);
  }
};
</script>

<style lang="scss">
.container {
  padding: 20rpx;
  padding-bottom: 120rpx;
}

.section {
  background: #fff;
  padding: 30rpx;
  border-radius: 12rpx;
  margin-bottom: 20rpx;
  
  .section-title {
    font-size: 30rpx;
    font-weight: bold;
    margin-bottom: 20rpx;
    display: block;
  }
}

.route-info {
  display: flex;
  justify-content: space-between;
  
  .route-price {
    color: #ff5a5f;
    font-weight: bold;
  }
}

.picker-item {
  display: flex;
  justify-content: space-between;
  padding: 20rpx 0;
  border-bottom: 1px solid #eee;
}

.input {
  border-bottom: 1px solid #eee;
  padding: 20rpx 0;
  height: 80rpx;
}

.butler-list {
  display: flex;
  flex-wrap: wrap;
  
  .butler-item {
    width: 30%;
    margin-right: 3%;
    margin-bottom: 20rpx;
    border: 1px solid #eee;
    border-radius: 8rpx;
    padding: 10rpx;
    text-align: center;
    
    &.active {
      border-color: #007aff;
      background: #f0f9ff;
    }
    
    .butler-avatar {
      width: 80rpx;
      height: 80rpx;
      border-radius: 50%;
      margin-bottom: 10rpx;
    }
    
    .butler-name {
      font-size: 24rpx;
      display: block;
    }
    
    .butler-price {
      font-size: 22rpx;
      color: #ff5a5f;
    }
  }
}

.footer {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: #fff;
  padding: 20rpx;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 -2rpx 10rpx rgba(0,0,0,0.05);
  
  .total-price {
    .price {
      color: #ff5a5f;
      font-size: 36rpx;
      font-weight: bold;
      margin-left: 10rpx;
    }
  }
  
  .btn-submit {
    background: #007aff;
    color: #fff;
    margin: 0;
    padding: 0 60rpx;
    border-radius: 40rpx;
  }
}

.coupon-text {
  color: #ff5a5f;
  font-weight: bold;
}

.placeholder {
  color: #999;
}
</style>
