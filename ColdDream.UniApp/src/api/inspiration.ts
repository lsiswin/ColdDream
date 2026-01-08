import { request } from '@/utils/request';

export interface Inspiration {
    id: string;
    imageUrl: string;
    description: string;
    userId: string;
    userName: string;
    userAvatar: string;
    likes: number;
    collects: number;
    isLiked?: boolean;
    isCollected?: boolean;
}

export const getInspirations = () => {
    return request<Inspiration[]>({
        url: '/inspiration',
        method: 'GET',
    });
};

export const likeInspiration = (id: string) => {
    return request<{ likes: number, isLiked: boolean }>({
        url: `/inspiration/${id}/like`,
        method: 'POST',
    });
};

export const collectInspiration = (id: string) => {
    return request<{ collects: number, isCollected: boolean }>({
        url: `/inspiration/${id}/collect`,
        method: 'POST',
    });
};

export const getCollectedInspirations = () => {
    return request<Inspiration[]>({
        url: '/inspiration/collected',
        method: 'GET'
    });
};
