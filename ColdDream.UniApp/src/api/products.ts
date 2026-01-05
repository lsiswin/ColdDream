import { request } from '@/utils/request';

export interface Product {
    id: string;
    name: string;
    description: string;
    imageUrl: string;
    pointsPrice: number;
    stock: number;
}

export const getProducts = () => {
    return request<Product[]>({
        url: '/products',
        method: 'GET',
    });
};

export const buyProduct = (id: string) => {
    return request<void>({
        url: `/products/${id}/buy`,
        method: 'POST',
    });
};
