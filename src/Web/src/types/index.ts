export interface ServiceEntry {
  id: string;
  licensePlate: string;
  brandName: string;
  modelName: string;
  kilometers: number;
  modelYear?: number;
  serviceDate: string;
  hasWarranty?: boolean;
  serviceCity?: string;
  serviceNote?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateServiceEntryDto {
  licensePlate: string;
  brandName: string;
  modelName: string;
  kilometers: number;
  modelYear?: number;
  serviceDate: string;
  hasWarranty?: boolean;
  serviceCity?: string;
  serviceNote?: string;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}