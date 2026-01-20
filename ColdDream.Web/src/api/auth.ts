import api, { type ApiResponse } from './index';

export interface LoginResponse {
  token: string;
  username: string;
  email: string;
  points: number;
  isNewUser?: boolean;
}

export const guestLogin = () => {
  return api.post<any, ApiResponse<LoginResponse>>('/auth/guest-login');
};

export const register = (data: any) => {
  return api.post<any, ApiResponse<LoginResponse>>('/auth/register', data);
};

export const login = (data: any) => {
  return api.post<any, ApiResponse<LoginResponse>>('/auth/login', data);
};

export const updateProfile = (data: { nickName: string; avatarUrl?: string }) => {
  return api.post<any, ApiResponse<any>>('/auth/profile', data);
};
