<template>
  <div class="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-3xl mx-auto bg-white rounded-lg shadow px-8 py-10">
      <div class="text-center mb-10">
        <h2 class="text-3xl font-extrabold text-gray-900">私人定制旅行</h2>
        <p class="mt-4 text-lg text-gray-500">告诉我们您的需求，专业的旅行管家为您量身打造专属行程</p>
      </div>

      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">目的地</label>
            <div class="flex space-x-2">
              <select v-model="selectedProvince" @change="handleProvinceChange" class="block w-1/2 border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm">
                <option value="">选择省份</option>
                <option v-for="province in provinces" :key="province" :value="province">{{ province }}</option>
              </select>
              <select v-model="selectedCity" class="block w-1/2 border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" :disabled="!selectedProvince">
                <option value="">选择城市</option>
                <option v-for="city in availableCities" :key="city" :value="city">{{ city }}</option>
              </select>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">出发日期</label>
            <input type="date" required v-model="form.startDate" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">游玩天数</label>
            <input type="number" min="1" required v-model="form.days" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">出行人数</label>
            <input type="number" min="1" required v-model="form.peopleCount" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>
          
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">人均预算</label>
            <input type="text" required v-model="form.budget" placeholder="例如：5000-8000" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">其他需求</label>
          <textarea v-model="form.requirements" rows="4" placeholder="例如：酒店星级、特殊饮食、特定景点..." class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm"></textarea>
        </div>

        <div class="border-t border-gray-200 pt-6 grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">联系人</label>
            <input type="text" required v-model="form.contactName" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">联系电话</label>
            <input type="tel" required v-model="form.contactPhone" class="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-primary focus:border-primary sm:text-sm" />
          </div>
        </div>

        <div class="flex justify-end pt-4">
          <button type="submit" :disabled="submitting" class="w-full md:w-auto inline-flex justify-center py-3 px-8 border border-transparent shadow-sm text-base font-medium rounded-md text-white bg-primary hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary disabled:opacity-50 transition-colors">
            {{ submitting ? '提交中...' : '提交定制需求' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import { createCustomTour } from '@/api/custom-tour';
import { chinaCities } from '@/assets/china_cities';

const router = useRouter();
const submitting = ref(false);

const provinces = Object.keys(chinaCities);
const selectedProvince = ref('');
const selectedCity = ref('');

const availableCities = computed(() => {
  return selectedProvince.value ? chinaCities[selectedProvince.value] : [];
});

const handleProvinceChange = () => {
  selectedCity.value = '';
};

const form = ref({
  startDate: '',
  days: 3,
  peopleCount: 2,
  budget: '',
  requirements: '',
  contactName: '',
  contactPhone: ''
});

const handleSubmit = async () => {
  if (!selectedProvince.value || !selectedCity.value) {
    alert('请选择目的地省份和城市');
    return;
  }

  submitting.value = true;
  try {
    const payload = {
      ...form.value,
      destination: `${selectedProvince.value} - ${selectedCity.value}`
    };
    
    const res = await createCustomTour(payload);
    if (res.success) {
      alert('需求提交成功！我们的管家会尽快联系您。');
      router.push('/dashboard?tab=custom-tours');
    } else {
      alert(res.message || '提交失败');
    }
  } catch (error) {
    console.error(error);
    alert('提交失败，请重试');
  } finally {
    submitting.value = false;
  }
};
</script>
