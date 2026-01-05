import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useAuthStore = defineStore('auth', () => {
    const token = ref<string | null>(uni.getStorageSync('token') || null);
    const user = ref<any | null>(null);

    const setToken = (newToken: string) => {
        token.value = newToken;
        uni.setStorageSync('token', newToken);
    };

    const setUser = (newUser: any) => {
        user.value = newUser;
    };

    const logout = () => {
        token.value = null;
        user.value = null;
        uni.removeStorageSync('token');
    };

    return {
        token,
        user,
        setToken,
        setUser,
        logout,
    };
});
