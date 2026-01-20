import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { UserProfile } from '@/api/auth';

export const useAuthStore = defineStore('auth', () => {
    const token = ref<string | null>(uni.getStorageSync('token') || null);
    const user = ref<UserProfile | null>(null);

    const setToken = (newToken: string) => {
        token.value = newToken;
        uni.setStorageSync('token', newToken);
    };

    const setUser = (newUser: UserProfile) => {
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
