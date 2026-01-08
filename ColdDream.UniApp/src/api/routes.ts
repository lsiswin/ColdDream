import { request } from '@/utils/request';

export interface TourRoute {
    id: string;
    title: string;
    description: string;
    price: number;
    imageUrl: string;
    isPrivate: boolean;
    tags: string;
    sales?: number;
    routeMapUrl?: string;
    itinerary?: string;
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

export const getTopRoutes = (count: number = 5) => {
    return request<TourRoute[]>({
        url: `/tourroutes/top?count=${count}`,
        method: 'GET'
    });
};
