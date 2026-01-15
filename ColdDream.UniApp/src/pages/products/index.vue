<template>
  <view class="container">
    <view class="header">
      <text class="title">积分商城</text>
      <view class="user-points" v-if="user">
        <text>我的积分: {{ user.points || 0 }}</text>
      </view>
    </view>
    
    <view class="product-list">
      <view 
        class="product-item" 
        v-for="product in products" 
        :key="product.id"
      >
        <image :src="product.imageUrl || '/static/product.png'" class="product-image" mode="aspectFill" />
        <view class="product-info">
          <text class="product-name">{{ product.name }}</text>
          <text class="product-price">{{ product.pointsPrice }} 积分</text>
          <button class="btn-buy" @click="handleBuy(product)" :disabled="product.stock <= 0">
            {{ product.stock > 0 ? '兑换' : '缺货' }}
          </button>
        </view>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { onShow } from '@dcloudio/uni-app';
import { getProducts, buyProduct, type Product } from '@/api/products';
import { useAuthStore } from '@/stores/auth';
import { storeToRefs } from 'pinia';

const products = ref<Product[]>([]);
const authStore = useAuthStore();
const { user } = storeToRefs(authStore);

import { getUserProfile } from '@/api/auth';

const loadData = async () => {
  try {
    const [productsRes, userRes] = await Promise.all([
      getProducts(),
      authStore.token ? getUserProfile() : Promise.resolve(null)
    ]);
    
    if (productsRes.success) {
      products.value = productsRes.data;
    }
    
    if (userRes && userRes.success) {
      authStore.setUser({ ...authStore.user, ...userRes.data });
    }
  } catch (error) {
    console.error(error);
  }
};

const handleBuy = async (product: Product) => {
  if (!authStore.token) {
    uni.navigateTo({ url: '/pages/auth/login' });
    return;
  }
  
  uni.showModal({
    title: '确认兑换',
    content: `确定消耗 ${product.pointsPrice} 积分兑换 ${product.name} 吗？`,
    success: async (res) => {
      if (res.confirm) {
        try {
          uni.showLoading({ title: '处理中...' });
          const buyRes = await buyProduct(product.id);
          uni.hideLoading();
          
          if (buyRes.success) {
            uni.showToast({ title: '兑换成功' });
            loadData(); // Refresh list and stock
          } else {
            uni.showToast({ title: buyRes.message || '兑换失败', icon: 'none' });
          }
        } catch (error) {
          uni.hideLoading();
          console.error(error);
          uni.showToast({ title: '兑换失败', icon: 'none' });
        }
      }
    }
  });
};

onMounted(() => {
  loadData();
});

onShow(() => {
  loadData();
});
</script>

<style lang="scss">
.container {
  padding: 30rpx;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 40rpx;
  padding: 10rpx 0;
  
  .title {
    font-size: 40rpx;
    font-weight: 800;
    color: #1a1a1a;
  }
  
  .user-points {
    font-size: 28rpx;
    color: #ff385c;
    font-weight: 700;
    background: rgba(255, 56, 92, 0.1);
    padding: 12rpx 24rpx;
    border-radius: 30rpx;
  }
}

.product-list {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  gap: 20rpx;
  
  .product-item {
    width: 48%;
    background: #fff;
    border-radius: 20rpx;
    overflow: hidden;
    margin-bottom: 0; /* Handled by gap */
    box-shadow: 0 4rpx 16rpx rgba(0,0,0,0.04);
    transition: transform 0.2s;
    display: flex;
    flex-direction: column;
    
    &:active {
      transform: scale(0.98);
    }
    
    .product-image {
      width: 100%;
      height: 320rpx;
      object-fit: cover;
    }
    
    .product-info {
      padding: 24rpx;
      flex: 1;
      display: flex;
      flex-direction: column;
      
      .product-name {
        font-size: 30rpx;
        font-weight: 600;
        color: #1a1a1a;
        display: block;
        margin-bottom: 12rpx;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
      }
      
      .product-price {
        color: #ff385c;
        font-size: 28rpx;
        font-weight: 700;
        display: block;
        margin-bottom: 24rpx;
      }
      
      .btn-buy {
        margin-top: auto;
        font-size: 26rpx;
        background: #1a1a1a;
        color: #fff;
        padding: 0;
        height: 64rpx;
        line-height: 64rpx;
        border-radius: 32rpx;
        font-weight: 500;
        
        &[disabled] {
          background: #e0e0e0;
          color: #999;
        }
      }
    }
  }
}
</style>
