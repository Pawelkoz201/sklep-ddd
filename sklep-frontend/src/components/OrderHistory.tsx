import React, { useEffect, useState } from 'react';
import api from '../api/api';
import { useUser } from '../services/userContext';
import { toast } from 'react-toastify';
import '../styles/orderhistory.css';

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

export default function OrderHistory() {
    const { user } = useUser();
    const [orders, setOrders] = useState<Order[]>([]);
    const [error, setError] = useState<string | null>(null);

    const userId = user.id;

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const data = await api.getOrders(userId);
                setOrders(data);
            } catch (error) {
                setError('Failed to fetch order data.');
                toast.error('Nie udało się pobrać danych.');
            }
        };

        fetchOrders();
    }, [userId]);

    if (error) return <p>{error}</p>;

    return (
        <div className="order-history-container">
            <h2 className="order-history-title">Historia zamówień</h2>
            {orders.length === 0 ? (
                <p className="order-history-empty">Brak zamówień.</p>
            ) : (
                <ul className="order-list">
                    {orders.map((order) => (
                        <li key={order.id} className="order-card">
                            <h3>Zamówienie nr:{order.id}</h3>
                            <p><strong>Data:</strong> {new Date(order.orderDate).toLocaleDateString()}</p>
                            <p><strong>Status:</strong> {order.deliveryStatus}</p>
                            <p><strong>Cena razem:</strong> ${order.totalPrice.toFixed(2)}</p>
                            <div className="order-items">
                                <h4>Produkty:</h4>
                                <ul>
                                    {order.items.map((item) => (
                                        <li key={item.productId}>
                                            {item.name} - {item.quantity} x ${item.price.toFixed(2)} = ${item.totalPrice.toFixed(2)}
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}