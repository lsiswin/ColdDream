import api, { type ApiResponse } from './index';

export interface Coupon {
  id: string;
  name: string;
  amount: number;
  minSpend: number;
  expiryDate: string;
  isUsed: boolean;
}

export const getMyCoupons = () => {
  return api.get<any, ApiResponse<Coupon[]>>('/coupon/my');
};

export const issueTestCoupon = () => {
  return api.post<any, ApiResponse<any>>('/coupon/test-issue');
};
