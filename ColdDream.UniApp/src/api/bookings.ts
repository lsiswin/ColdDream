import { request } from '@/utils/request';

export interface BookingRequest {
    tourRouteId: string;
    butlerId?: string;
    peopleCount: number;
    timeSlot: number; // 0: Morning, 1: Afternoon
    contactName: string;
    contactPhone: string;
    travelDate: string;
}

export const createBooking = (data: BookingRequest) => {
    return request<any>({
        url: '/bookings',
        method: 'POST',
        data,
    });
};

export const getMyBookings = () => {
    return request<any[]>({
        url: '/bookings/my',
        method: 'GET',
    });
};
