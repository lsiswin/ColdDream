<template>
  <div v-if="tourRoute" class="bg-white min-h-screen">
    <!-- Header Image -->
    <div class="relative h-96">
      <img class="w-full h-full object-cover" :src="tourRoute.imageUrl || 'https://via.placeholder.com/1200x600'" :alt="tourRoute.title">
      <div class="absolute inset-0 bg-black opacity-30"></div>
      <div class="absolute bottom-0 left-0 p-8 text-white max-w-7xl mx-auto w-full">
        <h1 class="text-4xl font-bold">{{ tourRoute.title }}</h1>
        <p class="mt-2 text-xl opacity-90">{{ tourRoute.tags }}</p>
      </div>
    </div>

    <div class="max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:px-8 grid grid-cols-1 lg:grid-cols-3 gap-8">
      <!-- Main Content -->
      <div class="lg:col-span-2">
        <!-- Tabs -->
        <div class="border-b border-gray-200 mb-8">
          <nav class="-mb-px flex space-x-8" aria-label="Tabs">
            <button 
              @click="activeTab = 'description'"
              :class="[activeTab === 'description' ? 'border-primary text-primary' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300', 'whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm']"
            >
              产品特色
            </button>
            <button 
              @click="activeTab = 'itinerary'"
              :class="[activeTab === 'itinerary' ? 'border-primary text-primary' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300', 'whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm']"
            >
              参考行程
            </button>
            <button 
              @click="activeTab = 'notice'"
              :class="[activeTab === 'notice' ? 'border-primary text-primary' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300', 'whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm']"
            >
              预订须知
            </button>
          </nav>
        </div>

        <div v-if="activeTab === 'description'" class="prose max-w-none text-gray-600">
          <h3 class="text-xl font-bold text-gray-900 mb-4">产品介绍</h3>
          <p class="leading-relaxed whitespace-pre-line">{{ tourRoute.description }}</p>
        </div>

        <div v-if="activeTab === 'itinerary'">
          <h3 class="text-xl font-bold text-gray-900 mb-4">行程安排</h3>
          <div v-if="tourRoute.itinerary" class="bg-gray-50 p-6 rounded-lg border border-gray-100">
             <!-- Simple rendering for now -->
            <pre class="whitespace-pre-wrap font-sans text-gray-600">{{ tourRoute.itinerary }}</pre>
          </div>
          <div v-else class="text-gray-500 italic">暂无详细行程信息</div>
        </div>

        <div v-if="activeTab === 'notice'" class="text-gray-600 space-y-4">
          <h3 class="text-xl font-bold text-gray-900 mb-4">费用与须知</h3>
          <div class="bg-blue-50 p-4 rounded-md">
            <h4 class="font-bold text-blue-800 mb-2">费用包含</h4>
            <ul class="list-disc list-inside text-sm text-blue-700">
              <li>行程中所列景点首道大门票</li>
              <li>全程空调旅游巴士</li>
              <li>优秀中文导游服务</li>
              <li>旅行社责任险</li>
            </ul>
          </div>
          <div class="bg-yellow-50 p-4 rounded-md">
            <h4 class="font-bold text-yellow-800 mb-2">退改规则</h4>
            <p class="text-sm text-yellow-700">出行前 7 天可免费取消；出行前 3 天取消收取 50% 违约金；出行当天不可取消。</p>
          </div>
        </div>
      </div>

      <!-- Sidebar -->
      <div class="lg:col-span-1">
        <div class="bg-white shadow-lg rounded-xl p-6 sticky top-24 border border-gray-100">
          <div class="flex justify-between items-baseline mb-6">
            <span class="text-gray-500">人均价格</span>
            <div class="text-right">
              <span class="text-3xl font-bold text-red-500">¥{{ tourRoute.price }}</span>
              <span class="text-gray-400 text-sm"> /人起</span>
            </div>
          </div>
          
          <div class="space-y-4 border-t border-gray-100 pt-6">
            <div class="flex items-center text-gray-600">
              <svg class="w-5 h-5 mr-3 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
              <span>行程天数: 5 天 (演示)</span>
            </div>
            <div class="flex items-center text-gray-600">
              <svg class="w-5 h-5 mr-3 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"></path></svg>
              <span>成团人数: 20 人封顶</span>
            </div>
            <div class="flex items-center text-gray-600">
              <svg class="w-5 h-5 mr-3 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
              <span>服务承诺: 无强制购物</span>
            </div>
          </div>

          <div class="mt-8">
            <router-link 
              :to="`/booking/${tourRoute.id}`"
              class="w-full flex justify-center py-4 px-4 border border-transparent rounded-lg shadow-sm text-base font-medium text-white bg-primary hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary transition-colors"
            >
              立即预订
            </router-link>
            <p class="mt-4 text-center text-xs text-gray-400">预订即代表同意《旅游服务协议》</p>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div v-else class="min-h-screen flex items-center justify-center">
    <div class="flex flex-col items-center">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mb-4"></div>
      <div class="text-xl text-gray-500">加载中...</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { getRouteDetails, type TourRoute } from '@/api/routes';

const route = useRoute();
const tourRoute = ref<TourRoute | null>(null);
const activeTab = ref('description');

onMounted(async () => {
  const id = route.params.id as string;
  if (id) {
    try {
      const res = await getRouteDetails(id);
      if (res.success) {
        tourRoute.value = res.data;
        // Mask the URL to hide the ID and show the title instead
        if (tourRoute.value?.title) {
          const newPath = `/tours/${encodeURIComponent(tourRoute.value.title)}`;
          // Use replaceState to change URL without triggering a route update or page reload
          window.history.replaceState({ ...window.history.state }, '', newPath);
        }
      }
    } catch (error) {
      console.error(error);
    }
  }
});
</script>
<script lang="ts">
// Fix template variable name issue by re-exporting or using different name
export default {
  // eslint-disable-next-line vue/no-reserved-component-names
  name: 'TourDetail'
}
</script>
