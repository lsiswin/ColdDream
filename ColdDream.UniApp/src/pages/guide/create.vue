<template>
  <view class="container">
    <view class="form-section">
      <view class="section-title">基本信息</view>
      <view class="form-group">
        <text class="label">标题</text>
        <input class="input" v-model="form.title" placeholder="给你的攻略起个好听的名字" />
      </view>
      
      <view class="form-group">
        <text class="label">简介</text>
        <textarea class="textarea" v-model="form.description" placeholder="简单介绍一下这次旅行的亮点..." />
      </view>
      
      <view class="form-group">
        <text class="label">封面图片</text>
        <view class="upload-box" @click="chooseImage">
          <image v-if="form.imageUrl" :src="form.imageUrl" mode="aspectFill" class="preview-image" />
          <view v-else class="upload-placeholder">
            <text class="icon">+</text>
            <text class="text">点击上传封面</text>
          </view>
        </view>
      </view>

      <view class="form-row">
        <view class="form-group half">
          <text class="label">预计花费</text>
          <input class="input" v-model="form.budget" placeholder="例如: 3000-5000" />
        </view>
        <view class="form-group half">
          <text class="label">游玩时长</text>
          <input class="input" v-model="form.duration" placeholder="例如: 3天2晚" />
        </view>
      </view>
      
      <view class="form-group">
        <text class="label">标签 (用逗号分隔)</text>
        <input class="input" v-model="tagsInput" placeholder="例如: 亲子, 海边, 美食" />
      </view>
    </view>
    
    <view class="form-section">
      <view class="section-title">详细行程</view>
      <view class="itinerary-list">
        <view class="day-item" v-for="(day, index) in itineraryList" :key="index">
          <view class="day-header">
            <text class="day-num">Day {{ index + 1 }}</text>
            <text class="btn-delete-day" @click="removeDay(index)">删除</text>
          </view>
          <view class="form-group">
            <input class="input" v-model="day.title" placeholder="今日主题 (例如: 抵达京都)" />
          </view>
          <view class="form-group">
            <textarea class="textarea small" v-model="day.content" placeholder="详细安排..." />
          </view>
        </view>
      </view>
      <button class="btn-add-day" @click="addDay">+ 添加一天</button>
    </view>

    <button class="btn-submit" @click="submit">发布攻略 (奖励10积分)</button>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { createGuide, updateGuide, getGuideById, type Guide } from '@/api/guide';

const isEdit = ref(false);
const guideId = ref('');
const tagsInput = ref('');

interface ItineraryItem {
  day: number;
  title: string;
  content: string;
}

const itineraryList = ref<ItineraryItem[]>([
  { day: 1, title: '', content: '' }
]);

const form = ref<Partial<Guide>>({
  title: '',
  description: '',
  imageUrl: '',
  budget: '',
  duration: '',
  tags: '',
  itinerary: ''
});

onLoad(async (options: any) => {
  if (options.id) {
    isEdit.value = true;
    guideId.value = options.id;
    uni.setNavigationBarTitle({ title: '编辑攻略' });
    await loadDetail(options.id);
  } else {
    uni.setNavigationBarTitle({ title: '新建攻略' });
  }
});

const loadDetail = async (id: string) => {
  try {
    const res = await getGuideById(id);
    form.value = {
      id: res.id,
      title: res.title,
      description: res.description,
      imageUrl: res.imageUrl,
      budget: res.budget,
      duration: res.duration
    };
    
    if (res.tags) {
      try {
        const tags = JSON.parse(res.tags);
        tagsInput.value = Array.isArray(tags) ? tags.join(', ') : res.tags;
      } catch {
        tagsInput.value = res.tags;
      }
    }

    if (res.itinerary) {
      try {
        itineraryList.value = JSON.parse(res.itinerary);
      } catch {
        itineraryList.value = [{ day: 1, title: '', content: '' }];
      }
    }
  } catch (error) {
    console.error(error);
  }
};

const chooseImage = () => {
  uni.chooseImage({
    count: 1,
    success: (res) => {
      const tempFilePath = res.tempFilePaths[0];
      uploadImage(tempFilePath);
    }
  });
};

const uploadImage = (filePath: string) => {
  uni.showLoading({ title: '上传中...' });
  uni.uploadFile({
    url: 'https://localhost:7282/api/upload', 
    filePath: filePath,
    name: 'file',
    success: (uploadRes) => {
      console.log('Upload result:', uploadRes);
      if (uploadRes.statusCode !== 200) {
        uni.hideLoading();
        uni.showToast({ title: '上传失败: ' + uploadRes.statusCode, icon: 'none' });
        console.error('Upload failed:', uploadRes.data);
        return;
      }
      
      try {
        const data = JSON.parse(uploadRes.data);
        form.value.imageUrl = 'https://localhost:7282' + data.url;
        uni.hideLoading();
      } catch (e) {
        console.error('JSON Parse Error:', e);
        uni.hideLoading();
        uni.showToast({ title: '响应格式错误', icon: 'none' });
      }
    },
    fail: (err) => {
      console.error(err);
      uni.hideLoading();
      uni.showToast({ title: '上传失败', icon: 'none' });
    }
  });
};

const addDay = () => {
  itineraryList.value.push({
    day: itineraryList.value.length + 1,
    title: '',
    content: ''
  });
};

const removeDay = (index: number) => {
  itineraryList.value.splice(index, 1);
  // Re-index days
  itineraryList.value.forEach((item, idx) => {
    item.day = idx + 1;
  });
};

const submit = async () => {
  if (!form.value.title || !form.value.description) {
    uni.showToast({ title: '请填写标题和简介', icon: 'none' });
    return;
  }

  // Process tags
  const tags = tagsInput.value.split(/[,，]/).map(t => t.trim()).filter(t => t);
  form.value.tags = JSON.stringify(tags);
  
  // Process itinerary
  form.value.itinerary = JSON.stringify(itineraryList.value);

  try {
    if (isEdit.value) {
      await updateGuide(guideId.value, form.value);
      uni.showToast({ title: '更新成功' });
    } else {
      await createGuide(form.value);
      uni.showToast({ title: '创建成功' });
    }
    setTimeout(() => {
      uni.navigateBack();
    }, 1500);
  } catch (error) {
    console.error(error);
  }
};
</script>

<style lang="scss">
.container {
  padding: 30rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
  padding-bottom: 100rpx;
}

.form-section {
  background: #fff;
  border-radius: 20rpx;
  padding: 30rpx;
  margin-bottom: 30rpx;
  
  .section-title {
    font-size: 32rpx;
    font-weight: bold;
    margin-bottom: 30rpx;
    padding-left: 20rpx;
    border-left: 8rpx solid #007aff;
  }
}

.form-group {
  margin-bottom: 30rpx;
  
  &.half {
    width: 48%;
  }
  
  .label {
    font-size: 28rpx;
    font-weight: bold;
    color: #333;
    margin-bottom: 16rpx;
    display: block;
  }
  
  .input {
    border: 1px solid #eee;
    padding: 20rpx;
    border-radius: 10rpx;
    font-size: 28rpx;
    background: #f9f9f9;
  }
  
  .textarea {
    border: 1px solid #eee;
    padding: 20rpx;
    border-radius: 10rpx;
    font-size: 28rpx;
    width: 100%;
    height: 160rpx;
    box-sizing: border-box;
    background: #f9f9f9;
    
    &.small {
      height: 120rpx;
    }
  }
  
  .upload-box {
    width: 100%;
    height: 300rpx;
    background: #f9f9f9;
    border: 1px dashed #ccc;
    border-radius: 10rpx;
    display: flex;
    justify-content: center;
    align-items: center;
    overflow: hidden;
    
    .preview-image {
      width: 100%;
      height: 100%;
    }
    
    .upload-placeholder {
      display: flex;
      flex-direction: column;
      align-items: center;
      color: #999;
      
      .icon {
        font-size: 60rpx;
        margin-bottom: 10rpx;
      }
      
      .text {
        font-size: 28rpx;
      }
    }
  }
}

.form-row {
  display: flex;
  justify-content: space-between;
}

.day-item {
  background: #f8f9fa;
  padding: 20rpx;
  border-radius: 10rpx;
  margin-bottom: 20rpx;
  border: 1px solid #eee;
  
  .day-header {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20rpx;
    
    .day-num {
      font-weight: bold;
      color: #007aff;
    }
    
    .btn-delete-day {
      color: #ff3b30;
      font-size: 24rpx;
    }
  }
}

.btn-add-day {
  background: #fff;
  border: 1px solid #007aff;
  color: #007aff;
  font-size: 28rpx;
  margin-top: 20rpx;
}

.btn-submit {
  background: #007aff;
  color: #fff;
  border-radius: 50rpx;
  margin-top: 40rpx;
  font-weight: bold;
  box-shadow: 0 8rpx 20rpx rgba(0, 122, 255, 0.3);
}
</style>
