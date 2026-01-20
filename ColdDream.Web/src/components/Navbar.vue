<template>
  <nav class="bg-white shadow-sm border-b border-gray-100 sticky top-0 z-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16">
        <div class="flex">
          <router-link to="/" class="flex-shrink-0 flex items-center text-xl font-bold text-primary">
            ColdDream 冷梦
          </router-link>
          <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
            <router-link 
              to="/" 
              class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
              active-class="border-primary text-gray-900"
            >
              首页
            </router-link>
            <router-link 
              to="/tours" 
              class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
              active-class="border-primary text-gray-900"
            >
              精选路线
            </router-link>
            <router-link 
              to="/custom-tour" 
              class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
              active-class="border-primary text-gray-900"
            >
              私人定制
            </router-link>
            <router-link 
              to="/inspiration" 
              class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
              active-class="border-primary text-gray-900"
            >
              旅行灵感
            </router-link>
            <router-link 
              to="/guide" 
              class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
              active-class="border-primary text-gray-900"
            >
              攻略社区
            </router-link>
          </div>
        </div>
        <div class="hidden sm:ml-6 sm:flex sm:items-center">
          <template v-if="authStore.token">
            <div class="ml-3 relative group">
              <button class="flex items-center space-x-2 text-gray-700 hover:text-gray-900 focus:outline-none">
                <div class="w-8 h-8 rounded-full bg-gray-200 flex items-center justify-center text-primary font-bold">
                  {{ authStore.user?.username.charAt(0).toUpperCase() }}
                </div>
                <span class="text-sm">{{ authStore.user?.username }}</span>
              </button>
              
              <!-- Dropdown -->
              <div class="absolute right-0 w-48 mt-2 origin-top-right bg-white border border-gray-200 divide-y divide-gray-100 rounded-md shadow-lg opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-all duration-200">
                <div class="py-1">
                  <router-link to="/dashboard" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">个人中心</router-link>
                  <router-link to="/dashboard?tab=orders" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">我的订单</router-link>
                </div>
                <div class="py-1">
                  <button @click="handleLogout" class="block w-full text-left px-4 py-2 text-sm text-red-600 hover:bg-gray-100">退出登录</button>
                </div>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="space-x-4">
              <router-link to="/login" class="text-gray-500 hover:text-gray-900 px-3 py-2 rounded-md text-sm font-medium">登录</router-link>
              <router-link to="/register" class="bg-primary text-white hover:bg-blue-600 px-4 py-2 rounded-md text-sm font-medium">注册</router-link>
            </div>
          </template>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  authStore.logout();
  router.push('/login');
};
</script>
