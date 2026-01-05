import { request } from '@/utils/request';

export interface TourRoute {
    id: string;
    title: string;
    description: string;
    price: number;
    imageUrl: string;
    isPrivate: boolean;
    tags: string;
}

export const getRoutes = () => {
    return request<TourRoute[]>({
        url: '/tourroutes',
        method: 'GET',
    });
};

export const getRouteDetails = (id: string) => {
    return request<TourRoute>({
        url: `/tourroutes/${id}`,
        method: 'GET',
    });
};
