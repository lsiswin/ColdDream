import api, { type ApiResponse } from './index';

export interface CustomTour {
  id: string;
  destination: string;
  startDate: string;
  days: number;
  peopleCount: number;
  budget: string;
  requirements: string;
  contactName: string;
  contactPhone: string;
  status: string;
  createdAt: string;
}

export interface CreateCustomTourDto {
  destination: string;
  startDate: string;
  days: number;
  peopleCount: number;
  budget: string;
  requirements: string;
  contactName: string;
  contactPhone: string;
}

export const createCustomTour = (data: CreateCustomTourDto) => {
  return api.post<any, ApiResponse<CustomTour>>('/customtour', data);
};

export const getMyCustomTours = () => {
  return api.get<any, ApiResponse<CustomTour[]>>('/customtour/my');
};

export const getCustomTourById = (id: string) => {
  return api.get<any, ApiResponse<CustomTour>>(`/customtour/${id}`);
};

export const cancelCustomTour = (id: string) => {
  return api.post<any, ApiResponse<any>>(`/customtour/${id}/cancel`); // Assuming backend supports this
};
