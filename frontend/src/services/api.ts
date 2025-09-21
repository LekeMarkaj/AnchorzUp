import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:52053/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export interface CreateShortUrlRequest {
  originalUrl: string;
  expiresAt?: string;
}

export interface ShortUrlResponse {
  id: string;
  originalUrl: string;
  shortUrl: string;
  shortCode: string;
  createdAt: string;
  expiresAt?: string;
  clickCount: number;
  lastAccessedAt?: string;
  qrCodeBase64: string;
}

export const shortUrlApi = {
  createShortUrl: async (data: CreateShortUrlRequest): Promise<ShortUrlResponse> => {
    const response = await api.post('/shorturl/create', data);
    return response.data as ShortUrlResponse;
  },

  getAllShortUrls: async (): Promise<ShortUrlResponse[]> => {
    const response = await api.get('/shorturl');
    return response.data as ShortUrlResponse[];
  },


  deleteShortUrl: async (id: string): Promise<void> => {
    await api.delete(`/shorturl/${id}`);
  },

  getQrCode: async (shortCode: string): Promise<Blob> => {
    const response = await api.get(`/shorturl/qr/${shortCode}`, {
      responseType: 'blob',
    });
    return response.data as Blob;
  },
};

export default api;
