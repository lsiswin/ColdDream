import { request, type ApiResponse } from '@/utils/request';

export interface UserProfile {
    id: string;
    username: string;
    email: string;
    nickName?: string;
    avatarUrl?: string;
    points: number;
}

export const getUserProfile = () => {
    return request<ApiResponse<UserProfile>>({
        url: '/auth/me',
        method: 'GET'
    });
};

export const updateProfile = (data: { nickName: string; avatarUrl?: string }) => {
    return request<ApiResponse<any>>({
        url: '/auth/profile',
        method: 'PUT',
        data
    });
};

export interface LoginResponse {
    token: string;
    username: string;
    email: string;
    points: number;
    isNewUser?: boolean;
}

export const wechatLogin = (data: { loginCode: string; phoneCode: string }) => {
    return request<ApiResponse<LoginResponse>>({
        url: '/auth/wechat-login',
        method: 'POST',
        data
    });
};

export const guestLogin = () => {
    return request<ApiResponse<LoginResponse>>({
        url: '/auth/guest-login',
        method: 'POST'
    });
};
