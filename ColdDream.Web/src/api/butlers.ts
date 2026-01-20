import api, { type ApiResponse } from './index';

export interface Butler {
  id: string;
  name: string;
  avatarUrl: string;
  price: number;
  tags: string;
  rating: number;
}

export const getButlersByRoute = (routeId: string) => {
  return api.get<any, ApiResponse<Butler[]>>(`/butlers/route/${routeId}`);
};
