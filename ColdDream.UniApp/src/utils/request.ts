const BASE_URL = 'http://localhost:5116/api'; // Updated port

interface RequestOptions {
  url: string;
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
  data?: any;
  header?: any;
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
    }

    uni.request({
      url: `${BASE_URL}${options.url}`,
      method: options.method || 'GET',
      data: options.data,
      header: header,
      success: (res: any) => {
        if (res.statusCode >= 200 && res.statusCode < 300) {
          resolve(res.data as T);
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
