import api, { type ApiResponse } from './index';

export interface TourRoute {
  id: string;
  title: string;
  description: string;
  price: number;
  imageUrl: string;
  isPrivate: boolean;
  tags: string;
  sales?: number;
  routeMapUrl?: string;
  itinerary?: string;
}

export const getRoutes = () => {
  return api.get<any, ApiResponse<TourRoute[]>>('/tourroutes');
};

export const getTopRoutes = (count = 10) => {
  return api.get<any, ApiResponse<TourRoute[]>>(`/tourroutes/top?count=${count}`);
};

export const getMyRoutes = () => {
  return api.get<any, ApiResponse<TourRoute[]>>('/tourroutes/my');
};

export const createRoute = (data: Partial<TourRoute>) => {
  return api.post<any, ApiResponse<TourRoute>>('/tourroutes', data);
};

export const updateRoute = (id: string, data: Partial<TourRoute>) => {
  return api.put<any, ApiResponse<TourRoute>>(`/tourroutes/${id}`, data);
};

export const getRouteDetails = (id: string) => {
  return api.get<any, ApiResponse<TourRoute>>(`/tourroutes/${id}`);
};
