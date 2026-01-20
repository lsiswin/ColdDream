<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-3xl mx-auto bg-white rounded-lg shadow px-8 py-10">
      <h2 class="text-2xl font-bold text-gray-900 mb-8 text-center">{{ isEditMode ? '编辑旅游攻略' : '撰写旅游攻略' }}</h2>
      
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
            <label class="block text-sm font-medium text-gray-700 mb-1">预计花费 (元)</label>
            <input v-model="form.price" type="number" min="0" placeholder="例如: 3000" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
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
            {{ submitting ? '提交中...' : (isEditMode ? '保存修改' : '发布攻略') }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { createRoute, updateRoute, getRouteDetails } from '@/api/routes';

const router = useRouter();
const route = useRoute();
const submitting = ref(false);
const isEditMode = ref(false);
const routeId = ref('');

const form = reactive({
  title: '',
  description: '',
  imageUrl: '',
  itinerary: '',
  price: 0, 
  duration: '',
  tags: '',
  isPrivate: false 
});

onMounted(async () => {
  // Check if we are in edit mode
  if (route.query.id) {
    isEditMode.value = true;
    routeId.value = route.query.id as string;
    await loadRouteData(routeId.value);
  }
});

const loadRouteData = async (id: string) => {
  try {
    const res = await getRouteDetails(id);
    if (res.success && res.data) {
      const data = res.data;
      form.title = data.title;
      form.description = data.description;
      form.imageUrl = data.imageUrl || '';
      form.itinerary = data.itinerary || '';
      form.price = data.price;
      form.duration = data.duration || '';
      form.tags = data.tags || '';
      form.isPrivate = data.isPrivate;
    }
  } catch (error) {
    console.error('Failed to load route details', error);
    alert('加载路线信息失败');
  }
};

const handleSubmit = async () => {
  submitting.value = true;
  try {
    const payload = {
      ...form,
      price: Number(form.price)
    };
    
    let res;
    if (isEditMode.value) {
      res = await updateRoute(routeId.value, payload);
    } else {
      res = await createRoute(payload);
    }

    if (res.success) {
      alert(isEditMode.value ? '攻略更新成功！' : '攻略发布成功！');
      router.push('/guide');
    } else {
      alert(res.message || (isEditMode.value ? '更新失败' : '发布失败'));
    }
  } catch (error) {
    console.error(error);
    alert('操作失败，请重试');
  } finally {
    submitting.value = false;
  }
};
</script>
