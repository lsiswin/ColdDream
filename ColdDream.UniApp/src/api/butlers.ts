import { request, type ApiResponse } from '@/utils/request';

export interface Butler {
    id: string;
    name: string;
    avatarUrl: string;
    price: number;
    tags: string;
    rating: number;
}

export const getButlersByRoute = (routeId: string) => {
    return request<ApiResponse<Butler[]>>({
        url: `/butlers/route/${routeId}`,
        method: 'GET',
    });
};
