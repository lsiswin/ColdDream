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
