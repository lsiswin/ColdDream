import { request, type ApiResponse } from '@/utils/request';

export interface Product {
    id: string;
    name: string;
    description: string;
    imageUrl: string;
    pointsPrice: number;
    stock: number;
}

export const getProducts = () => {
    return request<ApiResponse<Product[]>>({
        url: '/products',
        method: 'GET',
    });
};

export const buyProduct = (id: string) => {
    return request<ApiResponse<any>>({
        url: `/products/${id}/buy`,
        method: 'POST',
    });
};
