<template>
  <div class="bg-gray-50 min-h-screen py-8">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex flex-col md:flex-row gap-8">
        <!-- Left Sidebar Navigation -->
        <div class="w-full md:w-64 flex-shrink-0">
          <div class="bg-white shadow rounded-lg overflow-hidden sticky top-24">
            <div class="p-6 border-b border-gray-100 flex flex-col items-center">
              <div class="h-20 w-20 rounded-full bg-gray-200 flex items-center justify-center text-2xl text-gray-500 font-bold mb-3">
                {{ authStore.user?.username.charAt(0).toUpperCase() }}
              </div>
              <h3 class="font-bold text-gray-900">{{ authStore.user?.username }}</h3>
              <p class="text-xs text-gray-500 mt-1">积分: {{ authStore.user?.points }}</p>
            </div>
            <nav class="p-2 space-y-1">
              <button 
                @click="activeTab = 'profile'"
                :class="[activeTab === 'profile' ? 'bg-blue-50 text-primary font-medium' : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900', 'group w-full flex items-center px-4 py-3 text-sm rounded-md transition-colors']"
              >
                <svg class="mr-3 h-5 w-5 text-gray-400 group-hover:text-gray-500" :class="{'text-primary': activeTab === 'profile'}" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
                个人资料
              </button>
              <button 
                @click="activeTab = 'orders'"
                :class="[activeTab === 'orders' ? 'bg-blue-50 text-primary font-medium' : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900', 'group w-full flex items-center px-4 py-3 text-sm rounded-md transition-colors']"
              >
                <svg class="mr-3 h-5 w-5 text-gray-400 group-hover:text-gray-500" :class="{'text-primary': activeTab === 'orders'}" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
                我的订单
              </button>
              <button 
                @click="activeTab = 'custom-tours'"
                :class="[activeTab === 'custom-tours' ? 'bg-blue-50 text-primary font-medium' : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900', 'group w-full flex items-center px-4 py-3 text-sm rounded-md transition-colors']"
              >
                <svg class="mr-3 h-5 w-5 text-gray-400 group-hover:text-gray-500" :class="{'text-primary': activeTab === 'custom-tours'}" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                定制需求
              </button>
              <button 
                @click="activeTab = 'coupons'"
                :class="[activeTab === 'coupons' ? 'bg-blue-50 text-primary font-medium' : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900', 'group w-full flex items-center px-4 py-3 text-sm rounded-md transition-colors']"
              >
                <svg class="mr-3 h-5 w-5 text-gray-400 group-hover:text-gray-500" :class="{'text-primary': activeTab === 'coupons'}" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 5v2m0 4v2m0 4v2M5 5a2 2 0 00-2 2v3a2 2 0 110 4v3a2 2 0 002 2h14a2 2 0 002-2v-3a2 2 0 110-4V7a2 2 0 00-2-2H5z" /></svg>
                优惠券
              </button>
              <button 
                @click="activeTab = 'favorites'"
                :class="[activeTab === 'favorites' ? 'bg-blue-50 text-primary font-medium' : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900', 'group w-full flex items-center px-4 py-3 text-sm rounded-md transition-colors']"
              >
                <svg class="mr-3 h-5 w-5 text-gray-400 group-hover:text-gray-500" :class="{'text-primary': activeTab === 'favorites'}" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" /></svg>
                我的收藏
              </button>
            </nav>
          </div>
        </div>

        <!-- Right Content Area -->
        <div class="flex-1 bg-white shadow rounded-lg p-6 min-h-[500px]">
          <!-- Profile Tab -->
          <div v-if="activeTab === 'profile'">
            <div class="flex justify-between items-center mb-6">
              <h2 class="text-xl font-bold text-gray-900">个人资料</h2>
              <button @click="editingProfile = !editingProfile" class="text-sm text-primary hover:underline">
                {{ editingProfile ? '取消编辑' : '编辑资料' }}
              </button>
            </div>
            
            <form v-if="editingProfile" @submit.prevent="handleUpdateProfile" class="space-y-4 max-w-lg">
              <div>
                <label class="block text-sm font-medium text-gray-700">昵称</label>
                <input v-model="profileForm.nickName" type="text" class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700">头像链接</label>
                <input v-model="profileForm.avatarUrl" type="text" class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" placeholder="https://..." />
              </div>
              <div class="flex justify-end pt-4">
                <button type="submit" class="bg-primary text-white px-4 py-2 rounded-md text-sm hover:bg-blue-600">保存修改</button>
              </div>
            </form>

            <div v-else class="space-y-6">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div class="bg-gray-50 p-4 rounded-md">
                  <span class="block text-sm text-gray-500">用户名</span>
                  <span class="block text-lg font-medium text-gray-900 mt-1">{{ authStore.user?.username }}</span>
                </div>
                <div class="bg-gray-50 p-4 rounded-md">
                  <span class="block text-sm text-gray-500">邮箱</span>
                  <span class="block text-lg font-medium text-gray-900 mt-1">{{ authStore.user?.email }}</span>
                </div>
                <div class="bg-gray-50 p-4 rounded-md">
                  <span class="block text-sm text-gray-500">昵称</span>
                  <span class="block text-lg font-medium text-gray-900 mt-1">{{ authStore.user?.username || '未设置' }}</span>
                </div>
                 <div class="bg-gray-50 p-4 rounded-md">
                  <span class="block text-sm text-gray-500">会员积分</span>
                  <span class="block text-lg font-medium text-primary mt-1">{{ authStore.user?.points }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Orders Tab -->
          <div v-if="activeTab === 'orders'">
            <h2 class="text-xl font-bold text-gray-900 mb-6">我的订单</h2>
            <div v-if="bookings.length === 0" class="text-center py-10 text-gray-500">
              暂无订单，快去<router-link to="/tours" class="text-primary hover:underline">探索世界</router-link>吧！
            </div>
            <div v-else class="grid gap-6">
              <div v-for="booking in bookings" :key="booking.id" class="bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md transition-shadow overflow-hidden flex flex-col md:flex-row">
                <!-- Image Section -->
                <div class="w-full md:w-48 h-32 md:h-auto flex-shrink-0 bg-gray-200">
                   <img :src="booking.tourRoute?.imageUrl || 'https://via.placeholder.com/200'" class="w-full h-full object-cover" />
                </div>
                
                <!-- Content Section -->
                <div class="p-4 flex-1 flex flex-col justify-between">
                  <div>
                    <div class="flex justify-between items-start">
                      <h3 class="text-lg font-bold text-gray-900 line-clamp-1">{{ booking.tourRoute?.title || '未知路线' }}</h3>
                      <span :class="getStatusClass(booking.status)" class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium whitespace-nowrap ml-2">
                        {{ getStatusText(booking.status) }}
                      </span>
                    </div>
                    <div class="mt-2 text-sm text-gray-500 space-y-1">
                      <p>出行日期: {{ new Date(booking.bookingDate).toLocaleDateString() }}</p>
                      <p>出行人数: {{ booking.travelers }} 人</p>
                    </div>
                  </div>
                  
                  <div class="mt-4 flex items-center justify-between border-t border-gray-50 pt-3">
                    <span class="text-lg font-bold text-red-500">¥{{ booking.totalPrice }}</span>
                    <div class="space-x-3">
                      <button 
                        v-if="booking.status === 'Pending' || booking.status === 'Confirmed'"
                        @click="handleCancelBooking(booking.id)"
                        class="text-sm text-gray-500 hover:text-red-600"
                      >
                        取消订单
                      </button>
                      <router-link :to="`/order/${booking.id}`" class="text-sm text-primary hover:text-blue-700 font-medium">
                        查看详情
                      </router-link>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Custom Tours Tab -->
          <div v-if="activeTab === 'custom-tours'">
            <h2 class="text-xl font-bold text-gray-900 mb-6">我的定制</h2>
            <div v-if="customTours.length === 0" class="text-center py-10 text-gray-500">
              暂无定制需求
              <div class="mt-4">
                <router-link to="/custom-tour" class="bg-primary text-white px-4 py-2 rounded-md hover:bg-blue-600">发起定制</router-link>
              </div>
            </div>
            <div v-else class="space-y-4">
              <div v-for="ct in customTours" :key="ct.id" class="border border-gray-200 rounded-lg p-4 hover:bg-gray-50 transition-colors">
                <div class="flex justify-between">
                  <h4 class="font-bold text-gray-900">{{ ct.destination }} ({{ ct.days }}天)</h4>
                  <span class="text-sm text-gray-500">{{ new Date(ct.createdAt).toLocaleDateString() }}</span>
                </div>
                <p class="text-sm text-gray-600 mt-2">预算: {{ ct.budget }} | 人数: {{ ct.peopleCount }}</p>
                <p class="text-sm text-gray-600 mt-1">需求: {{ ct.requirements || '无特殊需求' }}</p>
                <div class="mt-2 text-right">
                  <span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                    {{ ct.status }}
                  </span>
                </div>
              </div>
            </div>
          </div>

          <!-- Coupons Tab -->
          <div v-if="activeTab === 'coupons'">
            <h2 class="text-xl font-bold text-gray-900 mb-6">我的优惠券</h2>
            <div v-if="coupons.length === 0" class="text-center py-10 text-gray-500">
              暂无优惠券
              <div class="mt-4">
                <button @click="handleGetTestCoupon" class="bg-primary text-white px-4 py-2 rounded-md hover:bg-blue-600">领取测试红包</button>
              </div>
            </div>
            <div v-else class="grid gap-4 md:grid-cols-2">
              <div v-for="coupon in coupons" :key="coupon.id" :class="[coupon.isUsed ? 'bg-gray-100 opacity-60' : 'bg-white border-red-200 shadow-sm', 'border rounded-lg p-4 flex justify-between items-center relative overflow-hidden']">
                <div v-if="coupon.isUsed" class="absolute right-0 top-0 bg-gray-400 text-white text-xs px-2 py-1">已使用</div>
                <div>
                  <h4 :class="[coupon.isUsed ? 'text-gray-500' : 'text-red-800', 'font-bold text-lg']">{{ coupon.name }}</h4>
                  <p :class="[coupon.isUsed ? 'text-gray-400' : 'text-red-600', 'text-sm mt-1']">满 {{ coupon.minSpend }} 减 {{ coupon.amount }}</p>
                  <p class="text-xs text-gray-400 mt-2">有效期至: {{ new Date(coupon.expiryDate).toLocaleDateString() }}</p>
                </div>
                <div class="text-3xl font-bold" :class="[coupon.isUsed ? 'text-gray-400' : 'text-red-500']">
                  <span class="text-sm align-top">¥</span>{{ coupon.amount }}
                </div>
              </div>
            </div>
          </div>
          
          <!-- Favorites Tab -->
          <div v-if="activeTab === 'favorites'">
            <h2 class="text-xl font-bold text-gray-900 mb-6">我的收藏</h2>
             <div v-if="favorites.length === 0" class="text-center py-10 text-gray-500">
              暂无收藏内容
              <div class="mt-4">
                <router-link to="/inspiration" class="text-primary hover:underline">去社区逛逛</router-link>
              </div>
            </div>
            <div v-else class="grid grid-cols-1 sm:grid-cols-2 gap-6">
               <div v-for="item in favorites" :key="item.id" class="group relative rounded-lg overflow-hidden bg-gray-100 shadow-sm hover:shadow-md transition-all">
                <img :src="item.imageUrl" class="w-full h-48 object-cover group-hover:opacity-90 transition-opacity" />
                <div class="p-4 bg-white">
                   <p class="text-sm text-gray-800 line-clamp-2">{{ item.description }}</p>
                   <div class="mt-3 flex items-center justify-between">
                     <div class="flex items-center text-xs text-gray-500">
                       <img :src="item.userAvatar" class="w-5 h-5 rounded-full mr-2" />
                       {{ item.userName }}
                     </div>
                     <span class="text-xs text-red-500" v-if="!item.isCollected">已收藏</span>
                     <button 
                       v-else 
                       @click="handleCancelFavorite(item.id)" 
                       class="text-xs text-gray-400 hover:text-red-500 transition-colors"
                     >
                       取消收藏
                     </button>
                   </div>
                </div>
               </div>
            </div>
          </div>

        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, reactive } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';
import { getMyBookings, cancelBooking, type Booking } from '@/api/booking';
import { getMyCustomTours, type CustomTour } from '@/api/custom-tour';
import { getMyCoupons, issueTestCoupon, type Coupon } from '@/api/coupon';
import { getCollectedInspirations, collectInspiration, type Inspiration } from '@/api/inspiration';
import { updateProfile } from '@/api/auth';

const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();

const activeTab = ref('profile');
const bookings = ref<Booking[]>([]);
const customTours = ref<CustomTour[]>([]);
const coupons = ref<Coupon[]>([]);
const favorites = ref<Inspiration[]>([]);

const editingProfile = ref(false);
const profileForm = reactive({
  nickName: authStore.user?.username || '',
  avatarUrl: ''
});

const statusMap: Record<string, string> = {
  'Pending': '待确认',
  'Confirmed': '已确认',
  'Paid': '已支付',
  'Completed': '已完成',
  'Cancelled': '已取消'
};

const getStatusText = (status: string) => statusMap[status] || status;

const getStatusClass = (status: string) => {
  switch (status) {
    case 'Confirmed':
    case 'Paid':
      return 'bg-green-100 text-green-800';
    case 'Pending':
      return 'bg-yellow-100 text-yellow-800';
    case 'Cancelled':
      return 'bg-gray-100 text-gray-800';
    default:
      return 'bg-blue-100 text-blue-800';
  }
};

const loadData = async () => {
  if (activeTab.value === 'orders') {
    try {
      const res = await getMyBookings();
      if (res.success) bookings.value = res.data;
    } catch (e) { console.error(e); }
  } else if (activeTab.value === 'custom-tours') {
    try {
      const res = await getMyCustomTours();
      if (res.success) customTours.value = res.data;
    } catch (e) { console.error(e); }
  } else if (activeTab.value === 'coupons') {
    try {
      const res = await getMyCoupons();
      if (res.success) coupons.value = res.data;
    } catch (e) { console.error(e); }
  } else if (activeTab.value === 'favorites') {
    try {
      const res = await getCollectedInspirations();
      if (res.success) favorites.value = res.data;
    } catch (e) { console.error(e); }
  }
};

const handleCancelBooking = async (id: string) => {
  if (!confirm('确定要取消该订单吗？')) return;
  try {
    const res = await cancelBooking(id);
    if (res.success) {
      alert('订单已取消');
      loadData();
    } else {
      alert(res.message);
    }
  } catch (e) { console.error(e); }
};

const handleGetTestCoupon = async () => {
  try {
    const res = await issueTestCoupon();
    if (res.success) {
      alert('领取成功！');
      loadData();
    }
  } catch (e) { console.error(e); }
};

const handleUpdateProfile = async () => {
  try {
    const res = await updateProfile(profileForm);
    if (res.success) {
      alert('资料更新成功');
      editingProfile.value = false;
      // Ideally update store user data here
    } else {
      alert(res.message);
    }
  } catch (e) { console.error(e); }
};

const handleCancelFavorite = async (id: string) => {
  if (!confirm('确定要取消收藏吗？')) return;
  try {
    const res = await collectInspiration(id);
    if (res.success) {
      // Remove locally
      favorites.value = favorites.value.filter(item => item.id !== id);
    } else {
      alert(res.message);
    }
  } catch (e) { console.error(e); }
};

watch(activeTab, () => {
  loadData();
  router.replace({ query: { ...route.query, tab: activeTab.value } });
});

onMounted(() => {
  if (route.query.tab) {
    activeTab.value = route.query.tab as string;
  }
  loadData();
});
</script>
