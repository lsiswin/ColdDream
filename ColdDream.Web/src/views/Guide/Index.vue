<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-7xl mx-auto">
      <div class="text-center mb-12">
        <h1 class="text-3xl font-extrabold text-gray-900 sm:text-4xl">攻略社区</h1>
        <p class="mt-4 text-lg text-gray-500">达人指路，让旅行更简单</p>
      </div>

      <!-- Action Bar -->
      <div class="flex justify-between items-center mb-8">
        <div class="relative w-full max-w-xs">
          <input type="text" v-model="searchQuery" class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:border-primary focus:ring-1 focus:ring-primary sm:text-sm" placeholder="搜索路线..." />
          <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
          </div>
        </div>
        <router-link to="/guide/create" class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-primary hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary">
          <svg class="-ml-1 mr-2 h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" /></svg>
          发布攻略
        </router-link>
      </div>

      <div v-if="loading" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
      </div>

        <!-- Tabs and Filter -->
        <div class="flex flex-col sm:flex-row justify-between items-center mb-6">
          <div class="flex space-x-4 mb-4 sm:mb-0">
            <button 
              @click="activeTab = 'all'"
              :class="[activeTab === 'all' ? 'bg-primary text-white' : 'bg-white text-gray-700 hover:bg-gray-50', 'px-4 py-2 rounded-md text-sm font-medium shadow-sm border border-gray-200 transition-colors']"
            >
              全部路线
            </button>
            <button 
              v-if="authStore.user"
              @click="activeTab = 'my'"
              :class="[activeTab === 'my' ? 'bg-primary text-white' : 'bg-white text-gray-700 hover:bg-gray-50', 'px-4 py-2 rounded-md text-sm font-medium shadow-sm border border-gray-200 transition-colors']"
            >
              我的发布
            </button>
          </div>

          <div class="flex items-center space-x-2">
            <span class="text-sm text-gray-500">排序:</span>
            <select v-model="sortBy" class="block pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm rounded-md">
              <option value="newest">最新发布</option>
              <option value="price_asc">价格从低到高</option>
              <option value="price_desc">价格从高到低</option>
              <option value="sales">热度/销量</option>
            </select>
          </div>
        </div>

      <div v-if="loading" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
      </div>

      <div v-else-if="filteredRoutes.length === 0" class="text-center py-20 text-gray-500">
        暂无相关路线
      </div>

      <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <div v-for="route in filteredRoutes" :key="route.id" class="bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow overflow-hidden flex flex-col cursor-pointer group" @click="goToDetail(route.id)">
          <div class="h-48 w-full bg-gray-200 relative">
            <img :src="route.imageUrl || 'https://via.placeholder.com/400x300'" loading="lazy" class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500" />
            <div class="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black to-transparent p-4">
              <h3 class="text-white font-bold text-lg truncate">{{ route.title }}</h3>
            </div>
            <!-- Edit Button for My Routes -->
             <div v-if="activeTab === 'my'" class="absolute top-2 right-2">
               <button @click.stop="handleEdit(route.id)" class="bg-white text-primary text-xs px-2 py-1 rounded shadow hover:bg-gray-100">
                 编辑
               </button>
             </div>
          </div>
          <div class="p-4 flex-1 flex flex-col">
            <p class="text-gray-600 text-sm line-clamp-2 mb-4 flex-1">{{ route.description }}</p>
            <div class="flex items-center justify-between text-xs text-gray-500 border-t border-gray-50 pt-3">
              <div class="flex items-center space-x-2">
                <span class="bg-green-50 text-green-700 px-2 py-0.5 rounded">¥{{ route.price }}</span>
                <span class="bg-blue-50 text-blue-700 px-2 py-0.5 rounded">{{ route.isPrivate ? '私家团' : '跟团游' }}</span>
              </div>
              <span>已售 {{ route.sales || 0 }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import { useRouter } from 'vue-router';
import { getRoutes, getMyRoutes, type TourRoute } from '@/api/routes';
import { useAuthStore } from '@/stores/auth';

const router = useRouter();
const authStore = useAuthStore();
const routes = ref<TourRoute[]>([]);
const myRoutes = ref<TourRoute[]>([]);
const loading = ref(true);
const searchQuery = ref('');
const activeTab = ref('all'); // 'all' or 'my'
const sortBy = ref('newest'); // 'newest', 'price_asc', 'price_desc', 'sales'

const filteredRoutes = computed(() => {
  let result = activeTab.value === 'my' ? myRoutes.value : routes.value;

  // Search Filter
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase();
    result = result.filter(r => 
      r.title.toLowerCase().includes(query) || 
      (r.description && r.description.toLowerCase().includes(query))
    );
  }

  // Sorting
  result = [...result].sort((a, b) => {
    switch (sortBy.value) {
      case 'price_asc':
        return a.price - b.price;
      case 'price_desc':
        return b.price - a.price;
      case 'sales':
        return (b.sales || 0) - (a.sales || 0);
      case 'newest':
      default:
        // Assuming newer items are at the end or we can sort by ID roughly if time based UUID, 
        // or just keep default order from backend which is usually chronological.
        // If we had createdAt, we'd use it.
        return 0;
    }
  });
  
  return result;
});

const loadData = async () => {
  loading.value = true;
  try {
    if (activeTab.value === 'all') {
      const res = await getRoutes();
      if (res.success) routes.value = res.data;
    } else if (activeTab.value === 'my') {
      const res = await getMyRoutes();
      if (res.success) myRoutes.value = res.data;
    }
  } catch (error) {
    console.error('Failed to load routes', error);
  } finally {
    loading.value = false;
  }
};

const goToDetail = (id: string) => {
  router.push(`/tours/${id}`);
};

const handleEdit = (id: string) => {
  router.push(`/guide/create?id=${id}`);
};

watch(activeTab, () => {
  loadData();
});

onMounted(() => {
  loadData();
});
</script>