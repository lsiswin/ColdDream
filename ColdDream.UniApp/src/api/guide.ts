import { request } from '@/utils/request';

export interface Guide {
    id: string;
    userId: string;
    title: string;
    description: string;
    imageUrl?: string;
    itinerary?: string;
    status: number; // 0: Pending, 1: Published, 2: Rejected
    tags?: string; // JSON array string
    duration?: string;
    budget?: string;
    createdAt: string;
    updatedAt: string;
}

export const getMyGuides = () => {
    return request<Guide[]>({
        url: '/guide/my',
        method: 'GET'
    });
};

export const getGuideById = (id: string) => {
    return request<Guide>({
        url: `/guide/${id}`,
        method: 'GET'
    });
};

export const createGuide = (data: Partial<Guide>) => {
    return request<Guide>({
        url: '/guide',
        method: 'POST',
        data
    });
};

export const updateGuide = (id: string, data: Partial<Guide>) => {
    return request({
        url: `/guide/${id}`,
        method: 'PUT',
        data
    });
};

export const deleteGuide = (id: string) => {
    return request({
        url: `/guide/${id}`,
        method: 'DELETE'
    });
};
