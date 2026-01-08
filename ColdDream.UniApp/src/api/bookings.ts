import { request } from '@/utils/request';

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
    return request<Booking>({
        url: '/booking',
        method: 'POST',
        data: mapRequestToBackend(data)
    });
};

export const getMyBookings = () => {
    return request<Booking[]>({
        url: '/booking/my',
        method: 'GET'
    }).then(res => {
        return res.map(b => ({
            ...b,
            peopleCount: b.travelers,
            travelDate: b.bookingDate,
            totalAmount: b.totalPrice,
            orderNumber: b.id.substring(0, 8).toUpperCase()
        }));
    });
};

export const getBookingById = (id: string) => {
    return request<Booking>({
        url: `/booking/${id}`,
        method: 'GET'
    }).then(res => ({
        ...res,
        peopleCount: res.travelers,
        travelDate: res.bookingDate,
        totalAmount: res.totalPrice,
        orderNumber: res.id.substring(0, 8).toUpperCase()
    }));
};

export const cancelBooking = (id: string) => {
    return request({
        url: `/booking/${id}/cancel`,
        method: 'POST'
    });
};

export const deleteBooking = (id: string) => {
    return request({
        url: `/booking/${id}`,
        method: 'DELETE'
    });
};
