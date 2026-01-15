import { request, type ApiResponse } from '@/utils/request';
import type { TourRoute } from './routes';

export const getTopRoutes = (count: number = 10) => {
    return request<ApiResponse<TourRoute[]>>({
        url: `/leaderboard/top-routes?count=${count}`,
        method: 'GET',
    });
};

export const recordClick = (routeId: string) => {
    return request<ApiResponse<any>>({
        url: `/leaderboard/click/${routeId}`,
        method: 'POST',
    });
};
