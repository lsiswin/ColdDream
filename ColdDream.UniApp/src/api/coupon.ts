import { request } from '@/utils/request';

export interface Coupon {
    id: string;
    name: string;
    amount: number;
    minSpend: number;
    expiryDate: string;
    isUsed: boolean;
}

export const getMyCoupons = () => {
    return request<Coupon[]>({
        url: '/coupon/my',
        method: 'GET'
    });
};

export const issueTestCoupon = () => {
    return request({
        url: '/coupon/test-issue',
        method: 'POST'
    });
};
