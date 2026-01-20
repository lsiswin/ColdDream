import axios from 'axios';

export interface ApiResponse<T = any> {
  success: boolean;
  message: string;
  data: T;
}

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5116/api',
  timeout: 10000,
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response Interceptor
api.interceptors.response.use(
  (response) => {
    return response.data;
  },
  (error) => {
    const authStore = useAuthStore();
    
    if (error.response) {
      if (error.response.status === 401) {
        authStore.logout();
        // Redirect to login handled by router guard or here if router instance available
        window.location.href = '/login';
      } else {
        // Global error alert for now, can be replaced with Toast
        const message = error.response.data?.message || '请求失败，请稍后重试';
        // Avoid alerting for 401 as we redirect
        if (error.response.status !== 401) {
           alert(message);
        }
      }
    } else {
      alert('网络连接异常，请检查您的网络设置');
    }
    
    return Promise.reject(error);
  }
);

export const uploadFile = async (file: File): Promise<string> => {
  const formData = new FormData();
  formData.append('file', file);
  
  // Note: Assuming backend returns { url: string } or similar
  const res = await api.post<any, { url: string }>('/upload', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
  
  return res.url;
};

export default api;
