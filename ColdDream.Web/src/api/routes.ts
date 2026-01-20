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

export const getTopRoutes = (count: number = 5) => {
  return api.get<any, ApiResponse<TourRoute[]>>(`/tourroutes/top?count=${count}`);
};

export const getRouteDetails = (id: string) => {
  return api.get<any, ApiResponse<TourRoute>>(`/tourroutes/${id}`);
};
