import React from "react";
import { useUser } from "../services/userContext";
import { useNavigate } from "react-router-dom";
import api from "../api/api";

interface CartItem {
  id: number;
  name: string;
  quantity?: number;
}

interface OrderItem {
  productId: number;
  name: string;
  quantity: number;
  price: number;
  totalPrice: number;
}

interface Order {
  id: number;
  orderDate: string;
  deliveryStatus: string;
  totalPrice: number;
  items: OrderItem[];
}

interface Cart {
  id: number;
  cartItems: CartItem[];
}

interface User {
  id: number;
  username: string;
  email: string;
  passwordHash: string;
  cart?: Cart; // Optional, as it can be null
  orders: Order[];
}

export default function ProfileC() {
  const { user, setUser } = useUser();
  const navigate = useNavigate();
  const userId = user.id;

  const handleLogout = () => {
    setUser({ id: 0, name: "", isLoggedIn: false, cartItems: [], token: undefined });
    localStorage.clear();
    navigate("/");
  };



  return (
    <div className="profile-container">
      <h1>Welcome, {user.name}</h1>
      <div className="profile-info">
        <p><strong>Username:</strong> {user.name}</p>
        <p><strong>Email:</strong> {user.email || "No email provided"}</p>
        <p><strong>Status:</strong> {user.isLoggedIn ? "Logged In" : "Logged Out"}</p>
      </div>

      <div className="profile-actions">
        <button className="logout-button" onClick={handleLogout}>
          Log Out
        </button>
      </div>
    </div>
  );
}