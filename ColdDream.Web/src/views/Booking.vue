<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-3xl mx-auto bg-white rounded-xl shadow-lg px-8 py-10">
      <div class="flex items-center justify-between mb-8 border-b border-gray-100 pb-4">
        <h2 class="text-2xl font-bold text-gray-900">预订行程</h2>
        <router-link to="/tours" class="text-sm text-primary hover:text-blue-600">返回路线列表</router-link>
      </div>
      
      <div v-if="tourRoute" class="mb-8 p-4 bg-blue-50 rounded-lg flex items-start border border-blue-100">
        <img :src="tourRoute.imageUrl || 'https://via.placeholder.com/100'" class="w-24 h-24 object-cover rounded-md mr-4" />
        <div>
          <h3 class="text-lg font-bold text-gray-900">{{ tourRoute.title }}</h3>
          <p class="text-gray-500 mt-1">单价: <span class="text-red-500 font-medium">¥{{ tourRoute.price }}</span> / 人</p>
          <p class="text-xs text-gray-400 mt-2">{{ tourRoute.tags }}</p>
        </div>
      </div>

      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">出行日期</label>
            <input type="date" required v-model="form.bookingDate" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">出行人数</label>
            <input type="number" min="1" max="50" required v-model="form.travelers" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">联系人姓名</label>
            <input type="text" required v-model="form.contactName" placeholder="请输入真实姓名" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">联系电话</label>
            <input type="tel" required v-model="form.contactPhone" placeholder="请输入手机号码" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>
        </div>

        <!-- Coupon Section -->
        <div class="border-t border-gray-100 pt-6">
          <label class="block text-sm font-medium text-gray-700 mb-2">优惠券</label>
          <select v-model="form.couponId" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm">
            <option value="">不使用优惠券</option>
            <option v-for="coupon in availableCoupons" :key="coupon.id" :value="coupon.id">
              {{ coupon.name }} (立减 ¥{{ coupon.amount }})
            </option>
          </select>
          <p v-if="availableCoupons.length === 0" class="mt-1 text-xs text-gray-400">暂无可用优惠券</p>
        </div>

        <!-- Price Calculation -->
        <div class="bg-gray-50 p-4 rounded-md mt-6">
          <div class="flex justify-between text-sm text-gray-600 mb-2">
            <span>行程费用</span>
            <span>¥{{ routePrice }}</span>
          </div>
          <div v-if="discountAmount > 0" class="flex justify-between text-sm text-red-500 mb-2">
            <span>优惠抵扣</span>
            <span>-¥{{ discountAmount }}</span>
          </div>
          <div class="flex justify-between items-center text-lg font-bold border-t border-gray-200 pt-2 mt-2">
            <span>总计应付</span>
            <span class="text-primary">¥{{ finalPrice }}</span>
          </div>
        </div>

        <div class="flex justify-end pt-4">
          <button type="submit" :disabled="submitting" class="w-full md:w-auto inline-flex justify-center py-3 px-8 border border-transparent shadow-sm text-base font-medium rounded-md text-white bg-primary hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary disabled:opacity-50 disabled:cursor-not-allowed transition-colors">
            {{ submitting ? '提交中...' : '确认并支付' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getRouteDetails, type TourRoute } from '@/api/routes';
import { createBooking } from '@/api/booking';
import { getMyCoupons, type Coupon } from '@/api/coupon';

const route = useRoute();
const router = useRouter();
const tourRouteId = route.params.id as string;

const tourRoute = ref<TourRoute | null>(null);
const coupons = ref<Coupon[]>([]);
const submitting = ref(false);

const form = ref({
  tourRouteId: tourRouteId,
  bookingDate: new Date().toISOString().split('T')[0],
  travelers: 1,
  contactName: '',
  contactPhone: '',
  couponId: ''
});

onMounted(async () => {
  if (tourRouteId) {
    try {
      const [routeRes, couponRes] = await Promise.all([
        getRouteDetails(tourRouteId),
        getMyCoupons()
      ]);

      if (routeRes.success) {
        tourRoute.value = routeRes.data;
      }
      if (couponRes.success) {
        coupons.value = couponRes.data;
      }
    } catch (error) {
      console.error(error);
    }
  }
});

const routePrice = computed(() => {
  if (!tourRoute.value) return 0;
  return tourRoute.value.price * form.value.travelers;
});

const availableCoupons = computed(() => {
  return coupons.value.filter(c => !c.isUsed && routePrice.value >= c.minSpend);
});

// Reset coupon if no longer valid
watch(routePrice, (newVal) => {
  const currentCoupon = coupons.value.find(c => c.id === form.value.couponId);
  if (currentCoupon && newVal < currentCoupon.minSpend) {
    form.value.couponId = '';
  }
});

const discountAmount = computed(() => {
  if (!form.value.couponId) return 0;
  const coupon = coupons.value.find(c => c.id === form.value.couponId);
  return coupon ? coupon.amount : 0;
});

const finalPrice = computed(() => {
  return Math.max(0, routePrice.value - discountAmount.value);
});

const handleSubmit = async () => {
  submitting.value = true;
  try {
    const payload = {
      ...form.value,
      couponId: form.value.couponId || undefined // Send undefined if empty string
    };
    const res = await createBooking(payload);
    if (res.success) {
      alert('预订成功！');
      router.push('/dashboard?tab=orders');
    } else {
      alert(res.message || '预订失败');
    }
  } catch (error) {
    console.error(error);
    alert('预订失败，请重试');
  } finally {
    submitting.value = false;
  }
};
</script>
