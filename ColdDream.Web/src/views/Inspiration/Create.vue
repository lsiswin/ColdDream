<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-2xl mx-auto bg-white rounded-lg shadow px-8 py-10">
      <h2 class="text-2xl font-bold text-gray-900 mb-6 text-center">发布旅行灵感</h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">图片链接</label>
          <input 
            v-model="form.imageUrl" 
            type="text" 
            required 
            placeholder="请输入图片 URL (例如: https://...)" 
            class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" 
          />
          <!-- Preview -->
          <div v-if="form.imageUrl" class="mt-4 rounded-lg overflow-hidden h-64 bg-gray-100">
            <img :src="form.imageUrl" class="w-full h-full object-cover" @error="handleImageError" />
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">灵感描述</label>
          <textarea 
            v-model="form.description" 
            rows="4" 
            required 
            placeholder="分享此刻的心情和故事..." 
            class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm"
          ></textarea>
        </div>

        <div class="flex justify-end space-x-4 pt-4">
          <router-link to="/inspiration" class="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50">
            取消
          </router-link>
          <button 
            type="submit" 
            :disabled="submitting" 
            class="px-6 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-primary hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary disabled:opacity-50"
          >
            {{ submitting ? '发布中...' : '发布' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { createInspiration } from '@/api/inspiration';

const router = useRouter();
const submitting = ref(false);
const form = reactive({
  imageUrl: '',
  description: ''
});

const handleImageError = (e: Event) => {
  (e.target as HTMLImageElement).src = 'https://via.placeholder.com/400x300?text=Invalid+Image+URL';
};

const handleSubmit = async () => {
  submitting.value = true;
  try {
    const res = await createInspiration(form);
    if (res.success) {
      alert('发布成功！');
      router.push('/inspiration');
    } else {
      alert(res.message || '发布失败');
    }
  } catch (error) {
    console.error(error);
    alert('发布失败，请重试');
  } finally {
    submitting.value = false;
  }
};
</script>
