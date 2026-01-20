<template>
  <div class="bg-white">
    <!-- Hero Section -->
    <div class="relative bg-gray-900">
      <div class="absolute inset-0">
        <img class="w-full h-full object-cover" src="https://images.unsplash.com/photo-1469854523086-cc02fe5d8800?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1920&q=80" alt="Travel Banner">
        <div class="absolute inset-0 bg-gray-900 opacity-60"></div>
      </div>
      <div class="relative max-w-7xl mx-auto py-24 px-4 sm:py-32 sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl font-extrabold tracking-tight text-white sm:text-5xl lg:text-6xl">开启您的下一段梦幻旅程</h1>
        <p class="mt-6 text-xl text-gray-300 max-w-3xl mx-auto">
          ColdDream 冷梦旅行，为您提供精心策划的专属路线、独家体验，让每一次旅行都成为一生的回忆。
        </p>
        <div class="mt-10 flex justify-center gap-4">
          <router-link to="/tours" class="inline-flex items-center justify-center px-8 py-3 border border-transparent text-base font-medium rounded-md text-white bg-primary hover:bg-blue-600 transition-colors">
            探索路线
          </router-link>
          <router-link to="/custom-tour" class="inline-flex items-center justify-center px-8 py-3 border border-white text-base font-medium rounded-md text-white hover:bg-white hover:text-gray-900 transition-colors">
            私人定制
          </router-link>
        </div>
      </div>
    </div>

    <!-- Features Section -->
    <div class="py-12 bg-white">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="text-center">
          <p class="mt-2 text-3xl leading-8 font-extrabold tracking-tight text-gray-900 sm:text-4xl">为什么选择我们</p>
        </div>

        <div class="mt-10">
          <div class="grid grid-cols-1 gap-10 sm:grid-cols-2 lg:grid-cols-3">
            <div class="flex flex-col items-center">
              <div class="flex items-center justify-center h-12 w-12 rounded-md bg-primary text-white mb-4">
                <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3.055 11H5a2 2 0 012 2v1a2 2 0 002 2 2 2 0 012 2v2.945M8 3.935V5.5A2.5 2.5 0 0010.5 8h.5a2 2 0 012 2 2 2 0 104 0 2 2 0 012-2h1.064M15 20.488V18a2 2 0 012-2h3.064M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
              </div>
              <h3 class="text-lg leading-6 font-medium text-gray-900">全球精选目的地</h3>
              <p class="mt-2 text-base text-gray-500 text-center">覆盖全球热门及小众目的地，深入当地文化体验。</p>
            </div>
            <div class="flex flex-col items-center">
              <div class="flex items-center justify-center h-12 w-12 rounded-md bg-primary text-white mb-4">
                <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
              </div>
              <h3 class="text-lg leading-6 font-medium text-gray-900">金牌管家服务</h3>
              <p class="mt-2 text-base text-gray-500 text-center">1对1 专属旅行管家，24小时在线解决您的问题。</p>
            </div>
            <div class="flex flex-col items-center">
              <div class="flex items-center justify-center h-12 w-12 rounded-md bg-primary text-white mb-4">
                <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
              </div>
              <h3 class="text-lg leading-6 font-medium text-gray-900">无忧出行保障</h3>
              <p class="mt-2 text-base text-gray-500 text-center">价格透明，无强制购物，先行赔付保障。</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Featured Tours -->
    <div class="max-w-7xl mx-auto py-16 px-4 sm:py-24 sm:px-6 lg:px-8">
      <div class="text-center">
        <h2 class="text-base font-semibold text-primary tracking-wide uppercase">热门推荐</h2>
        <p class="mt-1 text-4xl font-extrabold text-gray-900 sm:text-5xl sm:tracking-tight lg:text-6xl">本季最热路线</p>
        <p class="max-w-xl mt-5 mx-auto text-xl text-gray-500">为您挑选当下最值得去的旅行地</p>
      </div>

      <div class="mt-12 grid gap-8 md:grid-cols-2 lg:grid-cols-3">
        <div v-for="route in topRoutes" :key="route.id" class="flex flex-col rounded-lg shadow-lg overflow-hidden transition-transform duration-300 hover:scale-105">
          <div class="flex-shrink-0 relative h-48">
            <img class="h-full w-full object-cover" :src="route.imageUrl || 'https://via.placeholder.com/400x300'" :alt="route.title">
            <div class="absolute top-2 left-2 bg-primary text-white text-xs px-2 py-1 rounded">
              TOP {{ topRoutes.indexOf(route) + 1 }}
            </div>
          </div>
          <div class="flex-1 bg-white p-6 flex flex-col justify-between">
            <div class="flex-1">
              <p class="text-sm font-medium text-primary">
                {{ route.tags || '精选' }}
              </p>
              <router-link :to="`/tours/${route.id}`" class="block mt-2">
                <p class="text-xl font-semibold text-gray-900">{{ route.title }}</p>
                <p class="mt-3 text-base text-gray-500 line-clamp-3">{{ route.description }}</p>
              </router-link>
            </div>
            <div class="mt-6 flex items-center justify-between">
              <div class="text-lg font-bold text-red-500">¥{{ route.price }} <span class="text-xs text-gray-400 font-normal">起</span></div>
              <router-link :to="`/tours/${route.id}`" class="text-primary hover:text-blue-600 font-medium">
                查看详情 &rarr;
              </router-link>
            </div>
          </div>
        </div>
      </div>
      
      <div class="mt-12 text-center">
        <router-link to="/tours" class="text-base font-medium text-primary hover:text-blue-600">
          查看所有路线 <span aria-hidden="true">&rarr;</span>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getTopRoutes, type TourRoute } from '@/api/routes';

const topRoutes = ref<TourRoute[]>([]);

onMounted(async () => {
  try {
    const res = await getTopRoutes(3);
    if (res.success) {
      topRoutes.value = res.data;
    }
  } catch (error) {
    console.error('Failed to load top routes', error);
  }
});
</script>
