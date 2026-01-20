<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-3xl mx-auto bg-white rounded-lg shadow px-8 py-10">
      <h2 class="text-2xl font-bold text-gray-900 mb-8 text-center">撰写旅游攻略</h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">攻略标题</label>
          <input v-model="form.title" type="text" required placeholder="给你的攻略起个吸引人的标题" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">封面图片</label>
          <input v-model="form.imageUrl" type="text" placeholder="输入图片 URL" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
        </div>

        <div class="grid grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">预计花费</label>
            <input v-model="form.budget" type="text" placeholder="例如: 3000元" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">建议天数</label>
            <input v-model="form.duration" type="text" placeholder="例如: 5天4晚" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">攻略摘要</label>
          <textarea v-model="form.description" rows="3" required placeholder="简要介绍一下这次旅行..." class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm"></textarea>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">详细行程 (Markdown)</label>
          <textarea v-model="form.itinerary" rows="10" placeholder="# 第一天..." class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm font-mono"></textarea>
        </div>

        <div class="flex justify-end space-x-4 pt-4">
          <router-link to="/guide" class="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50">
            取消
          </router-link>
          <button type="submit" :disabled="submitting" class="px-6 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-primary hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary disabled:opacity-50">
            {{ submitting ? '发布中...' : '发布攻略' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { createGuide } from '@/api/guide';

const router = useRouter();
const submitting = ref(false);
const form = reactive({
  title: '',
  description: '',
  imageUrl: '',
  itinerary: '',
  budget: '',
  duration: '',
  tags: ''
});

const handleSubmit = async () => {
  submitting.value = true;
  try {
    const res = await createGuide(form);
    if (res.success) {
      alert('攻略发布成功！');
      router.push('/guide');
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
