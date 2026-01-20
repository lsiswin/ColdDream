<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-3xl mx-auto bg-white rounded-lg shadow-lg overflow-hidden" v-if="booking">
      <div class="border-b border-gray-200 px-6 py-4 flex justify-between items-center bg-gray-50">
        <div>
          <h1 class="text-xl font-bold text-gray-900">订单详情</h1>
          <p class="text-sm text-gray-500 mt-1">下单时间: {{ new Date(booking.createdAt).toLocaleString() }}</p>
        </div>
        <span :class="getStatusClass(booking.status)" class="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium">
          {{ getStatusText(booking.status) }}
        </span>
      </div>

      <div class="p-6 space-y-8">
        <!-- Route Info -->
        <section class="flex flex-col md:flex-row gap-6">
          <img :src="booking.tourRoute?.imageUrl || 'https://via.placeholder.com/300x200'" class="w-full md:w-1/3 h-40 object-cover rounded-lg" />
          <div class="flex-1">
            <h2 class="text-2xl font-bold text-gray-900 mb-2">{{ booking.tourRoute?.title }}</h2>
            <div class="space-y-2 text-gray-600">
              <p><span class="font-medium">出行日期:</span> {{ new Date(booking.bookingDate).toLocaleDateString() }}</p>
              <p><span class="font-medium">出行人数:</span> {{ booking.travelers }} 人</p>
              <p><span class="font-medium">路线标签:</span> {{ booking.tourRoute?.tags || '无' }}</p>
            </div>
          </div>
        </section>

        <!-- Cost Breakdown -->
        <section class="border-t border-gray-100 pt-6">
          <h3 class="text-lg font-bold text-gray-900 mb-4">费用明细</h3>
          <div class="bg-gray-50 p-4 rounded-md space-y-2">
            <div class="flex justify-between text-gray-600">
              <span>路线费用</span>
              <span>¥{{ (booking.tourRoute?.price || 0) * booking.travelers }}</span>
            </div>
            <div v-if="booking.discountAmount && booking.discountAmount > 0" class="flex justify-between text-red-500">
              <span>优惠抵扣</span>
              <span>-¥{{ booking.discountAmount }}</span>
            </div>
            <div class="flex justify-between items-center text-lg font-bold border-t border-gray-200 pt-2 mt-2">
              <span>实付金额</span>
              <span class="text-primary">¥{{ booking.totalPrice }}</span>
            </div>
          </div>
        </section>

        <!-- Contact Info -->
        <section class="border-t border-gray-100 pt-6">
          <h3 class="text-lg font-bold text-gray-900 mb-4">联系人信息</h3>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="block text-sm text-gray-500">姓名</label>
              <div class="font-medium">{{ booking.contactName }}</div>
            </div>
            <div>
              <label class="block text-sm text-gray-500">电话</label>
              <div class="font-medium">{{ booking.contactPhone }}</div>
            </div>
          </div>
        </section>

        <!-- Actions -->
        <div class="border-t border-gray-100 pt-6 flex justify-end space-x-4">
          <router-link to="/dashboard?tab=orders" class="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50">
            返回列表
          </router-link>
          <button 
            v-if="booking.status === 'Pending' || booking.status === 'Confirmed'"
            @click="handleCancel"
            class="px-4 py-2 border border-transparent rounded-md text-white bg-red-500 hover:bg-red-600"
          >
            取消订单
          </button>
        </div>
      </div>
    </div>
    <div v-else class="min-h-screen flex items-center justify-center">
      <div class="text-xl text-gray-500">加载中...</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getBookingById, cancelBooking, type Booking } from '@/api/booking';

const route = useRoute();
const router = useRouter();
const bookingId = route.params.id as string;
const booking = ref<Booking | null>(null);

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

const loadBooking = async () => {
  try {
    const res = await getBookingById(bookingId);
    if (res.success) {
      booking.value = res.data;
    } else {
      alert('订单加载失败');
      router.push('/dashboard');
    }
  } catch (error) {
    console.error(error);
    alert('网络错误');
  }
};

const handleCancel = async () => {
  if (!confirm('确定要取消该订单吗？')) return;
  try {
    const res = await cancelBooking(bookingId);
    if (res.success) {
      alert('订单已取消');
      loadBooking();
    } else {
      alert(res.message);
    }
  } catch (error) {
    console.error(error);
  }
};

onMounted(() => {
  if (bookingId) {
    loadBooking();
  }
});
</script>
