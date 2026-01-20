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

        <!-- Butler Selection -->
        <div class="border-t border-gray-100 pt-6" v-if="butlers.length > 0">
          <label class="block text-sm font-medium text-gray-700 mb-4">选择旅行管家 (可选)</label>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div 
              v-for="butler in butlers" 
              :key="butler.id"
              @click="form.butlerId = (form.butlerId === butler.id ? '' : butler.id)"
              :class="[
                'relative border rounded-lg p-4 cursor-pointer transition-all flex items-center space-x-4',
                form.butlerId === butler.id ? 'border-primary ring-1 ring-primary bg-blue-50' : 'border-gray-200 hover:border-blue-300'
              ]"
            >
              <img :src="butler.avatarUrl || 'https://via.placeholder.com/64'" class="w-12 h-12 rounded-full object-cover" />
              <div class="flex-1">
                <div class="flex justify-between items-center">
                  <h4 class="font-medium text-gray-900">{{ butler.name }}</h4>
                  <span class="text-xs font-bold text-orange-500 flex items-center">
                    {{ butler.rating }} <svg class="w-3 h-3 ml-0.5" fill="currentColor" viewBox="0 0 20 20"><path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"/></svg>
                  </span>
                </div>
                <p class="text-sm text-gray-500">服务费: ¥{{ butler.price }}</p>
              </div>
              <button @click.stop="openButlerModal(butler)" class="text-xs text-primary hover:underline absolute bottom-2 right-4">
                查看详情
              </button>
            </div>
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
          <div v-if="selectedButlerPrice > 0" class="flex justify-between text-sm text-gray-600 mb-2">
            <span>管家服务费</span>
            <span>+¥{{ selectedButlerPrice }}</span>
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

    <!-- Butler Modal -->
    <div v-if="showButlerModal" class="fixed inset-0 z-50 overflow-y-auto" aria-labelledby="modal-title" role="dialog" aria-modal="true">
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" aria-hidden="true" @click="showButlerModal = false"></div>
        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>
        <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4" v-if="currentButler">
            <div class="sm:flex sm:items-start">
              <div class="mx-auto flex-shrink-0 flex items-center justify-center h-16 w-16 rounded-full bg-blue-100 sm:mx-0 sm:h-20 sm:w-20">
                <img :src="currentButler.avatarUrl || 'https://via.placeholder.com/80'" class="h-16 w-16 sm:h-20 sm:w-20 rounded-full object-cover" />
              </div>
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left flex-1">
                <h3 class="text-lg leading-6 font-medium text-gray-900" id="modal-title">{{ currentButler.name }}</h3>
                <div class="mt-2">
                  <p class="text-sm text-gray-500 flex items-center justify-center sm:justify-start">
                    <span class="font-bold text-orange-500 mr-2">{{ currentButler.rating }} 分</span>
                    <span class="bg-blue-100 text-blue-800 text-xs px-2 py-0.5 rounded-full">{{ currentButler.tags }}</span>
                  </p>
                  <p class="text-sm text-gray-500 mt-2">
                    专业资深旅行管家，为您提供全方位的行程规划与服务，让您的旅途无忧无虑。
                  </p>
                  <p class="text-base font-bold text-primary mt-4">服务费: ¥{{ currentButler.price }}</p>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button @click="selectButlerFromModal" type="button" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-primary text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary sm:ml-3 sm:w-auto sm:text-sm">
              选择此管家
            </button>
            <button @click="showButlerModal = false" type="button" class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm">
              关闭
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getRouteDetails, type TourRoute } from '@/api/routes';
import { createBooking } from '@/api/booking';
import { getMyCoupons, type Coupon } from '@/api/coupon';
import { getButlersByRoute, type Butler } from '@/api/butlers';

const route = useRoute();
const router = useRouter();
const tourRouteId = route.params.id as string;

const tourRoute = ref<TourRoute | null>(null);
const coupons = ref<Coupon[]>([]);
const butlers = ref<Butler[]>([]);
const submitting = ref(false);

// Modal state
const showButlerModal = ref(false);
const currentButler = ref<Butler | null>(null);

const form = ref({
  tourRouteId: tourRouteId,
  bookingDate: new Date().toISOString().split('T')[0],
  travelers: 1,
  contactName: '',
  contactPhone: '',
  couponId: '',
  butlerId: ''
});

onMounted(async () => {
  if (tourRouteId) {
    try {
      const [routeRes, couponRes, butlerRes] = await Promise.all([
        getRouteDetails(tourRouteId),
        getMyCoupons(),
        getButlersByRoute(tourRouteId)
      ]);

      if (routeRes.success) {
        tourRoute.value = routeRes.data;
      }
      if (couponRes.success) {
        coupons.value = couponRes.data;
      }
      if (butlerRes.success) {
        butlers.value = butlerRes.data;
      }
    } catch (error) {
      console.error(error);
    }
  }
});

const openButlerModal = (butler: Butler) => {
  currentButler.value = butler;
  showButlerModal.value = true;
};

const selectButlerFromModal = () => {
  if (currentButler.value) {
    form.value.butlerId = currentButler.value.id;
  }
  showButlerModal.value = false;
};

const routePrice = computed(() => {
  if (!tourRoute.value) return 0;
  return tourRoute.value.price * form.value.travelers;
});

const selectedButlerPrice = computed(() => {
  if (!form.value.butlerId) return 0;
  const butler = butlers.value.find(b => b.id === form.value.butlerId);
  return butler ? butler.price : 0;
});

const availableCoupons = computed(() => {
  const totalBeforeDiscount = routePrice.value + selectedButlerPrice.value;
  return coupons.value.filter(c => !c.isUsed && totalBeforeDiscount >= c.minSpend);
});

// Reset coupon if no longer valid
watch([routePrice, selectedButlerPrice], () => {
  const totalBeforeDiscount = routePrice.value + selectedButlerPrice.value;
  const currentCoupon = coupons.value.find(c => c.id === form.value.couponId);
  if (currentCoupon && totalBeforeDiscount < currentCoupon.minSpend) {
    form.value.couponId = '';
  }
});

const discountAmount = computed(() => {
  if (!form.value.couponId) return 0;
  const coupon = coupons.value.find(c => c.id === form.value.couponId);
  return coupon ? coupon.amount : 0;
});

const finalPrice = computed(() => {
  return Math.max(0, routePrice.value + selectedButlerPrice.value - discountAmount.value);
});

const handleSubmit = async () => {
  submitting.value = true;
  try {
    const payload = {
      ...form.value,
      couponId: form.value.couponId || undefined,
      butlerId: form.value.butlerId || undefined
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
