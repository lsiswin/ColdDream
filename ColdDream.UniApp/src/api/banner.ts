import { request } from '@/utils/request';

export interface Banner {
    id: string;
    imageUrl: string;
    title: string;
    tag: string;
    targetUrl: string;
    sortOrder: number;
}

export const getBanners = () => {
    return request<Banner[]>({
        url: '/banner',
        method: 'GET',
    });
};
