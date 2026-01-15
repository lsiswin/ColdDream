import { request, type ApiResponse } from '@/utils/request';

export interface Coupon {
    id: string;
    name: string;
    amount: number;
    minSpend: number;
    expiryDate: string;
    isUsed: boolean;
}

export const getMyCoupons = () => {
    return request<ApiResponse<Coupon[]>>({
        url: '/coupon/my',
        method: 'GET'
    });
};

export const issueTestCoupon = () => {
    return request<ApiResponse<any>>({
        url: '/coupon/test-issue',
        method: 'POST'
    });
};
