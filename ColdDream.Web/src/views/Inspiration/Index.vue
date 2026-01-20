<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-7xl mx-auto">
      <div class="text-center mb-12">
        <h1 class="text-3xl font-extrabold text-gray-900 sm:text-4xl">旅行灵感</h1>
        <p class="mt-4 text-lg text-gray-500">发现旅行灵感，分享你的足迹</p>
      </div>

      <!-- Action Bar -->
      <div class="flex justify-end mb-8">
        <router-link to="/inspiration/create" class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-primary hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary">
          <svg class="-ml-1 mr-2 h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" /></svg>
          发布灵感
        </router-link>
      </div>

      <div v-if="loading" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
      </div>

      <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <div v-for="item in inspirations" :key="item.id" class="bg-white rounded-lg shadow-sm overflow-hidden hover:shadow-lg transition-shadow duration-300 group flex flex-col">
          <div class="relative bg-gray-200 h-64">
            <img 
              :src="item.imageUrl" 
              class="w-full h-full object-cover opacity-0 transition-opacity duration-300" 
              loading="lazy" 
              @load="$event.target.classList.remove('opacity-0')"
            />
            <div class="absolute inset-0 bg-black opacity-0 group-hover:opacity-10 transition-opacity"></div>
          </div>
          <div class="p-4 flex-1 flex flex-col">
            <p class="text-gray-800 text-sm leading-relaxed mb-4 line-clamp-3">{{ item.description }}</p>
            
            <div class="flex items-center justify-between border-t border-gray-50 pt-3">
              <div class="flex items-center">
                <img :src="item.userAvatar || 'https://via.placeholder.com/32'" class="h-8 w-8 rounded-full border border-gray-100" />
                <span class="ml-2 text-xs text-gray-500 truncate max-w-[80px]">{{ item.userName }}</span>
              </div>
              
              <div class="flex items-center space-x-4">
                <button 
                  @click="handleLike(item)"
                  class="flex items-center space-x-1 text-xs transition-colors"
                  :class="item.isLiked ? 'text-red-500' : 'text-gray-400 hover:text-gray-600'"
                >
                  <svg class="h-5 w-5" :fill="item.isLiked ? 'currentColor' : 'none'" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" /></svg>
                  <span>{{ item.likes }}</span>
                </button>
                
                <button 
                  @click="handleCollect(item)"
                  class="flex items-center space-x-1 text-xs transition-colors"
                  :class="item.isCollected ? 'text-yellow-500' : 'text-gray-400 hover:text-gray-600'"
                >
                  <svg class="h-5 w-5" :fill="item.isCollected ? 'currentColor' : 'none'" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 5a2 2 0 012-2h10a2 2 0 012 2v16l-7-3.5L5 21V5z" /></svg>
                  <span>{{ item.collects }}</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getInspirations, likeInspiration, collectInspiration, type Inspiration } from '@/api/inspiration';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const inspirations = ref<Inspiration[]>([]);
const loading = ref(true);

onMounted(async () => {
  try {
    const res = await getInspirations();
    if (res.success) {
      inspirations.value = res.data;
    }
  } catch (error) {
    console.error(error);
  } finally {
    loading.value = false;
  }
});

const handleLike = async (item: Inspiration) => {
  if (!authStore.token) return router.push('/login');
  
  // Optimistic update
  const originalState = item.isLiked;
  item.isLiked = !item.isLiked;
  item.likes += item.isLiked ? 1 : -1;

  try {
    const res = await likeInspiration(item.id);
    if (!res.success) throw new Error();
  } catch (error) {
    // Revert on failure
    item.isLiked = originalState;
    item.likes += item.isLiked ? 1 : -1;
  }
};

const handleCollect = async (item: Inspiration) => {
  if (!authStore.token) return router.push('/login');

  const originalState = item.isCollected;
  item.isCollected = !item.isCollected;
  item.collects += item.isCollected ? 1 : -1;

  try {
    const res = await collectInspiration(item.id);
    if (!res.success) throw new Error();
  } catch (error) {
    item.isCollected = originalState;
    item.collects += item.isCollected ? 1 : -1;
  }
};
</script>
