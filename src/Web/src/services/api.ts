import axios from 'axios';
import type { ServiceEntry, CreateServiceEntryDto, PaginatedResult, LoginRequest, LoginResponse } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5221/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authApi = {
  login: async (credentials: LoginRequest): Promise<LoginResponse> => {
    const { data } = await api.post<LoginResponse>('/auth/login', credentials);
    return data;
  },
};

export const serviceEntriesApi = {
  getAll: async (pageNumber: number = 1, pageSize: number = 10): Promise<PaginatedResult<ServiceEntry>> => {
    const { data } = await api.get<PaginatedResult<ServiceEntry>>('/serviceentries', {
      params: { pageNumber, pageSize },
    });
    return data;
  },

  create: async (dto: CreateServiceEntryDto): Promise<ServiceEntry> => {
    const { data } = await api.post<ServiceEntry>('/serviceentries', dto);
    return data;
  },
};

export default api;