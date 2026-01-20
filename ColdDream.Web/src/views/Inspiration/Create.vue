<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-3xl mx-auto bg-white rounded-lg shadow px-8 py-10">
      <h2 class="text-2xl font-bold text-gray-900 mb-8 text-center">发布旅行灵感</h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">图片上传</label>
          <div class="mt-1 flex justify-center px-6 pt-5 pb-6 border-2 border-gray-300 border-dashed rounded-md relative hover:border-primary transition-colors">
            <div class="space-y-1 text-center" v-if="!imagePreview">
              <svg class="mx-auto h-12 w-12 text-gray-400" stroke="currentColor" fill="none" viewBox="0 0 48 48" aria-hidden="true">
                <path d="M28 8H12a4 4 0 00-4 4v20m32-12v8m0 0v8a4 4 0 01-4 4H12a4 4 0 01-4-4v-4m32-4l-3.172-3.172a4 4 0 00-5.656 0L28 28M8 32l9.172-9.172a4 4 0 015.656 0L28 28m0 0l4 4m4-24h8m-4-4v8m-12 4h.02" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
              </svg>
              <div class="flex text-sm text-gray-600">
                <label for="file-upload" class="relative cursor-pointer bg-white rounded-md font-medium text-primary hover:text-blue-500 focus-within:outline-none">
                  <span>上传图片</span>
                  <input id="file-upload" name="file-upload" type="file" class="sr-only" accept="image/*" @change="handleFileChange" required />
                </label>
                <p class="pl-1">或拖拽文件到此处</p>
              </div>
              <p class="text-xs text-gray-500">PNG, JPG, GIF up to 5MB</p>
            </div>
            <div v-else class="relative w-full h-64">
              <img :src="imagePreview" class="w-full h-full object-contain rounded-md" />
              <button type="button" @click="clearImage" class="absolute top-2 right-2 bg-red-500 text-white rounded-full p-1 hover:bg-red-600">
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </button>
            </div>
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">灵感描述</label>
          <textarea v-model="form.description" required rows="4" placeholder="分享你的旅行见闻..." class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm"></textarea>
        </div>

        <div class="flex justify-end space-x-4 pt-4">
          <router-link to="/inspiration" class="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50">
            取消
          </router-link>
          <button type="submit" :disabled="submitting" class="px-6 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-primary hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary disabled:opacity-50">
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
import { uploadFile } from '@/api/index';

const router = useRouter();
const submitting = ref(false);
const selectedFile = ref<File | null>(null);
const imagePreview = ref<string>('');

const form = reactive({
  imageUrl: '',
  description: ''
});

const handleFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    const file = target.files[0];
    // Basic validation
    if (file.size > 5 * 1024 * 1024) {
      alert('图片大小不能超过 5MB');
      return;
    }
    
    selectedFile.value = file;
    // Create local preview URL
    imagePreview.value = URL.createObjectURL(file);
  }
};

const clearImage = () => {
  selectedFile.value = null;
  imagePreview.value = '';
  // Reset input value if needed via ref
};

const handleSubmit = async () => {
  if (!selectedFile.value) {
    alert('请选择一张图片');
    return;
  }

  submitting.value = true;
  try {
    // 1. Upload Image
    const imageUrl = await uploadFile(selectedFile.value);
    form.imageUrl = imageUrl; // Assuming backend returns full URL or relative path correctly handled by <img>

    // 2. Create Inspiration
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
