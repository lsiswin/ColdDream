<template>
  <view class="container">
    <view class="header">
      <text class="title">定制您的专属旅程</text>
      <text class="subtitle">告诉我们您的需求，专业的旅行顾问将为您规划</text>
    </view>
    
    <view class="form">
      <view class="form-item">
        <text class="label">目的地</text>
        <input class="input" v-model="form.destination" placeholder="您想去哪里？" />
      </view>
      
      <view class="form-item">
        <text class="label">出发日期</text>
        <picker mode="date" :start="startDate" @change="onDateChange">
          <view class="picker-view">
            <text :class="{ placeholder: !form.startDate }">{{ form.startDate || '请选择出发日期' }}</text>
          </view>
        </picker>
      </view>
      
      <view class="row">
        <view class="form-item half">
          <text class="label">游玩天数</text>
          <input class="input" type="number" v-model.number="form.days" placeholder="几天" />
        </view>
        <view class="form-item half">
          <text class="label">出行人数</text>
          <input class="input" type="number" v-model.number="form.peopleCount" placeholder="几人" />
        </view>
      </view>
      
      <view class="form-item">
        <text class="label">人均预算</text>
        <picker mode="selector" :range="budgetOptions" @change="onBudgetChange">
          <view class="picker-view">
            <text :class="{ placeholder: !form.budget }">{{ form.budget || '请选择预算范围' }}</text>
          </view>
        </picker>
      </view>
      
      <view class="form-item">
        <text class="label">其他需求</text>
        <textarea class="textarea" v-model="form.requirements" placeholder="例如：酒店星级、特殊饮食、必去景点等" />
      </view>
      
      <view class="section-title">联系方式</view>
      
      <view class="form-item">
        <text class="label">联系人</text>
        <input class="input" v-model="form.contactName" placeholder="怎么称呼您" />
      </view>
      
      <view class="form-item">
        <text class="label">手机号</text>
        <input class="input" type="number" v-model="form.contactPhone" placeholder="方便联系您的号码" />
      </view>
      
      <button class="btn-submit" @click="submit">提交定制需求</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { createCustomTour, type CustomTourRequest } from '@/api/custom-tour';

const startDate = new Date().toISOString().split('T')[0];
const budgetOptions = ['1000-3000元', '3000-5000元', '5000-8000元', '8000-12000元', '12000元以上'];

const form = ref<CustomTourRequest>({
  destination: '',
  startDate: '',
  days: 3,
  peopleCount: 2,
  budget: '',
  requirements: '',
  contactName: '',
  contactPhone: ''
});

const onDateChange = (e: any) => {
  form.value.startDate = e.detail.value;
};

const onBudgetChange = (e: any) => {
  form.value.budget = budgetOptions[e.detail.value];
};

const submit = async () => {
  if (!form.value.destination || !form.value.startDate || !form.value.contactName || !form.value.contactPhone) {
    uni.showToast({ title: '请填写必要信息', icon: 'none' });
    return;
  }
  
  try {
    const res = await createCustomTour(form.value);
    
    if (res.success) {
      uni.showToast({ title: '提交成功' });
      setTimeout(() => {
        uni.redirectTo({ url: '/pages/custom-tour/list' });
      }, 1500);
    } else {
      uni.showToast({ title: res.message || '提交失败', icon: 'none' });
    }
  } catch (error) {
    console.error(error);
    uni.showToast({ title: '提交失败', icon: 'none' });
  }
};
</script>

<style lang="scss">
.container {
  padding: 40rpx;
  background: #fff;
  min-height: 100vh;
}

.header {
  margin-bottom: 60rpx;
  
  .title {
    font-size: 40rpx;
    font-weight: bold;
    display: block;
    margin-bottom: 16rpx;
  }
  
  .subtitle {
    font-size: 26rpx;
    color: #666;
  }
}

.form-item {
  margin-bottom: 40rpx;
  
  .label {
    font-size: 28rpx;
    font-weight: bold;
    margin-bottom: 20rpx;
    display: block;
    color: #333;
  }
  
  .input, .picker-view {
    height: 80rpx;
    line-height: 80rpx;
    border-bottom: 1px solid #eee;
    font-size: 30rpx;
  }
  
  .textarea {
    width: 100%;
    height: 200rpx;
    background: #f9f9f9;
    padding: 20rpx;
    border-radius: 12rpx;
    font-size: 28rpx;
    box-sizing: border-box;
  }
  
  .placeholder {
    color: #999;
  }
}

.row {
  display: flex;
  justify-content: space-between;
  
  .half {
    width: 48%;
  }
}

.section-title {
  font-size: 32rpx;
  font-weight: bold;
  margin: 60rpx 0 40rpx;
  padding-left: 20rpx;
  border-left: 8rpx solid #007aff;
}

.btn-submit {
  margin-top: 60rpx;
  background: #007aff;
  color: #fff;
  border-radius: 45rpx;
  height: 90rpx;
  line-height: 90rpx;
  font-size: 32rpx;
  font-weight: bold;
  box-shadow: 0 10rpx 20rpx rgba(0, 122, 255, 0.2);
}
</style>
