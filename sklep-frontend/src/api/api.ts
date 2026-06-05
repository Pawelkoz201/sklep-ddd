import axios, { AxiosInstance, AxiosResponse } from 'axios';

const API_URL = 'http://localhost:5000/api'; 

const axiosInstance: AxiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

const getToken = (): string | null => {
  return localStorage.getItem('token');
};

const setAuthHeader = (): void => {
  const token = getToken();
  if (token) {
    axiosInstance.defaults.headers['Authorization'] = `Bearer ${token}`;
  } else {
    delete axiosInstance.defaults.headers['Authorization']; 
  }
};

interface User {
  id: number;
  username: string;
  email: string;
}

export const api = {
  getUser: async (userId: number) => {
    try {
      const response = await axiosInstance.get(`/users/${userId}`);
      return response.data; 
    } catch (error: any) {
      console.error('Error getting user:', error.response?.data || error.message);
      throw error;
    }
  },

  registerUser: async (username: string, email: string, password: string): Promise<User> => {
    try {
      const response: AxiosResponse<User> = await axiosInstance.post('/users/register', {
        username,
        email,
        password,
      });
      return response.data; 
    } catch (error: any) {
      console.error('Error registering user:', error.response?.data || error.message);
      throw error;
    }
  },


  loginUser: async (email: string, password: string) => {
  try {
    const response = await axiosInstance.post('/users/login', {
      email,
      password,
    });
    return response.data; 
  } catch (error: any) {
    console.error('Error logging in user:', error.response?.data || error.message);
    throw error;
  }
  },
  addToCart: async (userId: number, productId: number, quantity: number) => {
      try {
        const response = await axiosInstance.post(`/users/${userId}/addToCart`, {
          productId,
          quantity
      });
        return response.data;
      } catch (error: any) {
        console.error('Error adding to cart', error.response?.data || error.message);
        throw error;
      }
  },
  fetchUserCart: async (userId: number) => {
        try {
            const response = await axiosInstance.get(`/users/${userId}/cart`);
            return response.data;
        } catch (error: any) {
            console.error('Error fetching cart', error.response?.data || error.message);
            throw error;
        }
    },
    updateCartItem: async (userId: number, productId: number, quantity: number) => {
    try {
        const response = await axiosInstance.put(`/users/${userId}/cart/${productId}`, quantity);
        
        console.log('Cart item updated:', response.data);
        
        return response.data;
    } catch (error: any) {
        console.error('Error updating cart', error.response?.data || error.message);
        
        throw error;
    }
    },
    removeCartItem: async (userId: number, productId: number) => {
        try {
            await axiosInstance.delete(`/users/${userId}/cart/${productId}`);
        } catch (error: any) {
            console.error('Error removing item from cart', error.response?.data || error.message);
            throw error;
        }
    },
    getOrders: async (userId: number) => {
        try {
            const response = await axiosInstance.get(`/orders/${userId}/orders`);
            return response.data;
        } catch (error: any) {
            console.error('Error getting orders item from cart', error.response?.data || error.message);
            throw error;
        }
    },
  placeOrder: async (userId: number) => {
    try {
        const response = await axiosInstance.post(`/orders/${userId}/placeorder`, {
      });
      return response;
    } catch (error: any) {
        console.error('Error placing order:', error.response?.data || error.message);
        throw error; 
    }
  },

  logoutUser: (): void => {
    localStorage.removeItem('token');
    setAuthHeader(); 
  },
};

export default api;
