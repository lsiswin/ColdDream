import { request, type ApiResponse } from '@/utils/request';

export interface BookingRequest {
    tourRouteId: string;
    bookingDate: string;
    travelers: number;
    contactName: string;
    contactPhone: string;
    timeSlot?: number;
    butlerId?: string;
}

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
    orderNumber?: string;
    peopleCount?: number;
    totalAmount?: number;
    travelDate?: string;
    butler?: any;
}

const mapRequestToBackend = (data: any) => {
    return {
        tourRouteId: data.tourRouteId,
        bookingDate: data.travelDate,
        travelers: data.peopleCount,
        contactName: data.contactName,
        contactPhone: data.contactPhone,
        butlerId: data.butlerId,
        couponId: data.couponId
    };
};

export const createBooking = (data: any) => {
    return request<ApiResponse<Booking>>({
        url: '/booking',
        method: 'POST',
        data: mapRequestToBackend(data)
    });
};

export const getMyBookings = () => {
    return request<ApiResponse<Booking[]>>({
        url: '/booking/my',
        method: 'GET'
    }).then(res => {
        if (res.success && res.data) {
            res.data = res.data.map(b => ({
                ...b,
                peopleCount: b.travelers,
                travelDate: b.bookingDate,
                totalAmount: b.totalPrice,
                orderNumber: b.id.substring(0, 8).toUpperCase()
            }));
        }
        return res;
    });
};

export const getBookingById = (id: string) => {
    return request<ApiResponse<Booking>>({
        url: `/booking/${id}`,
        method: 'GET'
    }).then(res => {
        if (res.success && res.data) {
            const data = res.data;
            res.data = {
                ...data,
                peopleCount: data.travelers,
                travelDate: data.bookingDate,
                totalAmount: data.totalPrice,
                orderNumber: data.id.substring(0, 8).toUpperCase()
            };
        }
        return res;
    });
};

export const cancelBooking = (id: string) => {
    return request<ApiResponse<any>>({
        url: `/booking/${id}/cancel`,
        method: 'POST'
    });
};

export const deleteBooking = (id: string) => {
    return request<ApiResponse<any>>({
        url: `/booking/${id}`,
        method: 'DELETE'
    });
};
