import { request } from '@/utils/request';

export interface CustomTourRequest {
    destination: string;
    startDate: string;
    days: number;
    peopleCount: number;
    budget: string;
    requirements: string;
    contactName: string;
    contactPhone: string;
}

export interface CustomTour {
    id: string;
    destination: string;
    startDate: string;
    days: number;
    peopleCount: number;
    budget: string;
    requirements: string;
    contactName: string;
    contactPhone: string;
    status: string;
    createdAt: string;
}

export const createCustomTour = (data: CustomTourRequest) => {
    return request<CustomTour>({
        url: '/customtour',
        method: 'POST',
        data
    });
};

export const getMyCustomTours = () => {
    return request<CustomTour[]>({
        url: '/customtour/my',
        method: 'GET'
    });
};

export const getCustomTourById = (id: string) => {
    return request<CustomTour>({
        url: `/customtour/${id}`,
        method: 'GET'
    });
};

export const cancelCustomTour = (id: string) => {
    return request({
        url: `/customtour/${id}/cancel`,
        method: 'POST'
    });
};

export const deleteCustomTour = (id: string) => {
    return request({
        url: `/customtour/${id}`,
        method: 'DELETE'
    });
};
