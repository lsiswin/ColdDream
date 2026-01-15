const BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5116/api';

interface RequestOptions {
  url: string;
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
  data?: any;
  header?: any;
}

export interface ApiResponse<T = any> {
  success: boolean;
  message: string;
  data: T;
}

export const request = <T>(options: RequestOptions): Promise<T> => {
  return new Promise((resolve, reject) => {
    const token = uni.getStorageSync('token');
    const header = {
      'Content-Type': 'application/json',
      ...options.header,
    };

    if (token) {
      header['Authorization'] = `Bearer ${token}`;
      // console.log('Request with token:', token.substring(0, 10) + '...');
    } else {
      console.warn('Request without token:', options.url);
    }

    uni.request({
      url: `${BASE_URL}${options.url}`,
      method: options.method || 'GET',
      data: options.data,
      header: header,
      success: (res: any) => {
        if (res.statusCode >= 200 && res.statusCode < 300) {
          resolve(res.data as T);
        } else if (res.statusCode === 401) {
          // Handle Unauthorized
          uni.removeStorageSync('token');
          uni.showToast({
            title: 'Session expired, please login again',
            icon: 'none',
          });
          setTimeout(() => {
            uni.reLaunch({
              url: '/pages/auth/login',
            });
          }, 1500);
          reject(res.data);
        } else {
          uni.showToast({
            title: res.data?.message || 'Request failed',
            icon: 'none',
          });
          reject(res.data);
        }
      },
      fail: (err) => {
        uni.showToast({
          title: 'Network error',
          icon: 'none',
        });
        reject(err);
      },
    });
  });
};
