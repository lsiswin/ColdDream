import api, { type ApiResponse } from './index';

export interface Inspiration {
  id: string;
  imageUrl: string;
  description: string;
  userId: string;
  userName: string;
  userAvatar: string;
  likes: number;
  collects: number;
  isLiked: boolean;
  isCollected: boolean;
}

export const getInspirations = () => {
  return api.get<any, ApiResponse<Inspiration[]>>('/inspiration');
};

export const getCollectedInspirations = () => {
  return api.get<any, ApiResponse<Inspiration[]>>('/inspiration/collected');
};

export const likeInspiration = (id: string) => {
  return api.post<any, ApiResponse<any>>(`/inspiration/${id}/like`);
};

export const collectInspiration = (id: string) => {
  return api.post<any, ApiResponse<any>>(`/inspiration/${id}/collect`);
};

export const createInspiration = (data: { description: string; imageUrl: string }) => {
  return api.post<any, ApiResponse<Inspiration>>('/inspiration', data);
};
