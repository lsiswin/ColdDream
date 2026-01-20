import api, { type ApiResponse } from './index';

export interface Booking {
  id: string;
  tourRouteId: string;
  tourRoute?: any;
  bookingDate: string;
  travelers: number;
  contactName: string;
  contactPhone: string;
  totalPrice: number;
  discountAmount?: number;
  status: string;
  createdAt: string;
}

export interface CreateBookingDto {
  tourRouteId: string;
  bookingDate: string;
  travelers: number;
  contactName: string;
  contactPhone: string;
  couponId?: string;
}

export const createBooking = (data: CreateBookingDto) => {
  return api.post<any, ApiResponse<Booking>>('/booking', data);
};

export const getMyBookings = () => {
  return api.get<any, ApiResponse<Booking[]>>('/booking/my');
};

export const getBookingById = (id: string) => {
  return api.get<any, ApiResponse<Booking>>(`/booking/${id}`);
};

export const cancelBooking = (id: string) => {
  return api.post<any, ApiResponse<any>>(`/booking/${id}/cancel`);
};
