import React, { useState, useEffect } from "react";
import "../styles/ProfilePage.css";
import api from "../api/api";
import { useUser } from "../services/userContext";

interface User {
    name: string;
    email: string;
    address: string;
    phone: string;
}

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

interface User2 {
  id: number;
  username: string;
  email: string;
  passwordHash: string;
  cart?: Cart; 
  orders: Order[];
}

export default function Profile() {
    const [isEditing, setIsEditing] = useState<boolean>(false);
    const [formData, setFormData] = useState<User | null>(null);
    const { user, setUser } = useUser();

    useEffect(() => {
    const fetchUser = async () => {
        try {
            const data = await api.getUser(user.id);
        } catch (err : any) {
            console.error('Failed to fetch data');
        }
    };

    fetchUser();
}, []);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (formData) {
            setFormData({
                ...formData,
                [e.target.name]: e.target.value,
            });
        }
    };

    const handleSave = () => {
        if (formData) {
            setUser(formData);
            setIsEditing(false);
            alert("Zapisano zmiany!");
        }
    };

    return (
        <div className="profile-container">
            <h1>Twoj Profil</h1>
            {user ? (
                <div className="profile-details">
                    <div className="profile-field">
                        <label>Imie i nazwisko:</label>
                        {isEditing ? (
                            <input
                                type="text"
                                name="name"
                                value={formData?.name}
                                onChange={handleInputChange}
                            />
                        ) : (
                            <p>Jan Kowalski</p>
                        )}
                    </div>
                    <div className="profile-field">
                        <label>Email:</label>
                        {isEditing ? (
                            <input
                                type="email"
                                name="email"
                                value={formData?.email}
                                onChange={handleInputChange}
                            />
                        ) : (
                            <p>testowymail@example.com</p>
                        )}
                    </div>
                    <div className="profile-field">
                        <label>Adres:</label>
                        {isEditing ? (
                            <input
                                type="text"
                                name="address"
                                value={formData?.address}
                                onChange={handleInputChange}
                            />
                        ) : (
                            <p>Wiejska 45A</p>
                        )}
                    </div>
                    <div className="profile-field">
                        <label>Telefon:</label>
                        {isEditing ? (
                            <input
                                type="text"
                                name="phone"
                                value={formData?.phone}
                                onChange={handleInputChange}
                            />
                        ) : (
                            <p>+48 123 456 789</p>
                        )}
                    </div>

                    {/* <div className="profile-actions">
                        {isEditing ? (
                            <button onClick={handleSave} className="save-button">
                                Zapisz
                            </button>
                        ) : (
                            <button
                                onClick={() => setIsEditing(true)}
                                className="edit-button"
                            >
                                Edytuj
                            </button>
                        )}
                    </div> */}
                </div>
            ) : (
                <p>Ładowanie danych użytkownika...</p>
            )}
        </div>
    );
}
