import { defineStore } from 'pinia';
import { ref } from 'vue';

export interface User {
  username: string;
  email: string;
  points: number;
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'));
  const user = ref<User | null>(null);

  const setToken = (newToken: string) => {
    token.value = newToken;
    localStorage.setItem('token', newToken);
  };

  const setUser = (newUser: User) => {
    user.value = newUser;
  };

  const logout = () => {
    token.value = null;
    user.value = null;
    localStorage.removeItem('token');
  };

  return {
    token,
    user,
    setToken,
    setUser,
    logout,
  };
});
