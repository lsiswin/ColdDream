<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-7xl mx-auto">
      <div class="text-center mb-12">
        <h1 class="text-3xl font-extrabold text-gray-900 sm:text-4xl">全部精选路线</h1>
        <p class="mt-4 text-lg text-gray-500">发现完美假期，从这里开始</p>
      </div>

      <!-- Search/Filter -->
      <div class="mb-8 flex justify-center">
        <div class="relative w-full max-w-lg">
          <input 
            v-model="searchQuery"
            type="text" 
            class="block w-full pl-4 pr-10 py-3 border border-gray-300 rounded-full leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:border-primary focus:ring-1 focus:ring-primary sm:text-sm" 
            placeholder="搜索目的地、路线名称..." 
          />
          <div class="absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none">
            <svg class="h-5 w-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>
          </div>
        </div>
      </div>

      <div v-if="loading" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
      </div>

      <div v-else-if="filteredRoutes.length === 0" class="text-center py-20 text-gray-500">
        没有找到相关路线
      </div>

      <div v-else class="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
        <router-link 
          v-for="route in filteredRoutes" 
          :key="route.id" 
          :to="`/tours/${route.id}`"
          class="bg-white rounded-lg shadow overflow-hidden hover:shadow-lg transition-shadow duration-300 flex flex-col group cursor-pointer"
        >
          <div class="relative h-48 bg-gray-200">
            <img 
              :src="route.imageUrl || 'https://via.placeholder.com/400x300'" 
              :alt="route.title"
              loading="lazy"
              class="h-full w-full object-cover group-hover:scale-105 transition-transform duration-500 opacity-0"
              @load="$event.target.classList.remove('opacity-0')"
            >
            <div class="absolute top-2 right-2 bg-black bg-opacity-50 text-white text-xs px-2 py-1 rounded">
              {{ route.isPrivate ? '私家团' : '跟团游' }}
            </div>
          </div>
          <div class="p-6 flex-1 flex flex-col">
            <div class="flex items-center justify-between mb-2">
              <span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                {{ route.tags || '热门' }}
              </span>
              <span class="text-sm text-gray-500">已售 {{ route.sales || 0 }}</span>
            </div>
            <h3 class="text-lg font-medium text-gray-900 truncate mb-2 group-hover:text-primary transition-colors">{{ route.title }}</h3>
            <p class="text-sm text-gray-500 line-clamp-3 mb-4 flex-1">{{ route.description }}</p>
            <div class="mt-auto flex items-center justify-between border-t border-gray-100 pt-4">
              <div>
                <span class="text-xs text-gray-400">起价</span>
                <span class="text-xl font-bold text-red-500 ml-1">¥{{ route.price }}</span>
              </div>
              <span class="text-sm text-primary group-hover:underline">
                查看详情 &rarr;
              </span>
            </div>
          </div>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { getRoutes, type TourRoute } from '@/api/routes';

const routes = ref<TourRoute[]>([]);
const loading = ref(true);
const searchQuery = ref('');

const filteredRoutes = computed(() => {
  if (!searchQuery.value) return routes.value.slice(0, 20);
  const query = searchQuery.value.toLowerCase();
  return routes.value.filter(r => 
    r.title.toLowerCase().includes(query) || 
    (r.tags && r.tags.toLowerCase().includes(query)) ||
    (r.description && r.description.toLowerCase().includes(query))
  ).slice(0, 20);
});

onMounted(async () => {
  try {
    const res = await getRoutes();
    if (res.success) {
      routes.value = res.data;
    }
  } catch (error) {
    console.error('Failed to load routes', error);
  } finally {
    loading.value = false;
  }
});
</script>
