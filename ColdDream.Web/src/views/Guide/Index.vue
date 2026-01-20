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
          <input type="text" v-model="searchQuery" class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:border-primary focus:ring-1 focus:ring-primary sm:text-sm" placeholder="搜索攻略..." />
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

      <div v-else-if="filteredGuides.length === 0" class="text-center py-20 text-gray-500">
        暂无相关攻略，快来发布第一篇吧！
      </div>

      <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <div v-for="guide in filteredGuides" :key="guide.id" class="bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow overflow-hidden flex flex-col">
          <div class="h-48 w-full bg-gray-200 relative">
            <img :src="guide.imageUrl || 'https://via.placeholder.com/400x300'" class="w-full h-full object-cover" />
            <div class="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black to-transparent p-4">
              <h3 class="text-white font-bold text-lg truncate">{{ guide.title }}</h3>
            </div>
          </div>
          <div class="p-4 flex-1 flex flex-col">
            <p class="text-gray-600 text-sm line-clamp-2 mb-4 flex-1">{{ guide.description }}</p>
            <div class="flex items-center justify-between text-xs text-gray-500 border-t border-gray-50 pt-3">
              <div class="flex items-center space-x-2">
                <span v-if="guide.budget" class="bg-green-50 text-green-700 px-2 py-0.5 rounded">¥{{ guide.budget }}</span>
                <span v-if="guide.duration" class="bg-blue-50 text-blue-700 px-2 py-0.5 rounded">{{ guide.duration }}</span>
              </div>
              <span>{{ new Date(guide.createdAt).toLocaleDateString() }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { getGuides, type Guide } from '@/api/guide';

const guides = ref<Guide[]>([]);
const loading = ref(true);
const searchQuery = ref('');

const filteredGuides = computed(() => {
  if (!searchQuery.value) return guides.value;
  const query = searchQuery.value.toLowerCase();
  return guides.value.filter(g => 
    g.title.toLowerCase().includes(query) || 
    g.description.toLowerCase().includes(query)
  );
});

onMounted(async () => {
  try {
    const res = await getGuides();
    if (res.success) {
      guides.value = res.data;
    }
  } catch (error) {
    console.error(error);
  } finally {
    loading.value = false;
  }
});
</script>
