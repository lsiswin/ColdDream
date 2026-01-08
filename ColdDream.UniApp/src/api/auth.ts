import { request } from '@/utils/request';

export interface UserProfile {
    id: string;
    username: string;
    email: string;
    nickName?: string;
    avatarUrl?: string;
    points: number;
}

export const getUserProfile = () => {
    return request<UserProfile>({
        url: '/auth/me',
        method: 'GET'
    });
};

export const updateProfile = (data: { nickName: string; avatarUrl?: string }) => {
    return request({
        url: '/auth/profile',
        method: 'PUT',
        data
    });
};
