import api, { type ApiResponse } from './index';

export interface Guide {
  id: string;
  title: string;
  description: string;
  imageUrl: string;
  tags: string; // JSON string in backend usually
  userId: string;
  createdAt: string;
}

export interface CreateGuideDto {
  title: string;
  description: string;
  imageUrl: string;
  tags: string;
}

export const getGuides = () => {
  return api.get<any, ApiResponse<Guide[]>>('/guide');
};

export const getGuideById = (id: string) => {
  return api.get<any, ApiResponse<Guide>>(`/guide/${id}`);
};

export const getMyGuides = () => {
  return api.get<any, ApiResponse<Guide[]>>('/guide/my');
};

export const createGuide = (data: CreateGuideDto) => {
  return api.post<any, ApiResponse<Guide>>('/guide', data);
};
