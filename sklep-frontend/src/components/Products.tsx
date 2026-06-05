import React from 'react';
import '../styles/Product.css'; // Import styles for the product page
import { useUser } from '../services/userContext';
import api from '../api/api';
import { toast } from 'react-toastify';
interface Category {
  id: number;
  name: string;
}
interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  imageUrl: string;
  stockQuantity: number;
  categoryId: number;
  category: Category | null; 
}

interface ProductListProps {
  products: Product[]; 
}

export default function Product({ products }: ProductListProps) {
  const { user, setUser } = useUser();
  const quantity = 1;

  const handleAddToCart = async (product: Product) => {
      try {
      const data = await api.addToCart(user.id, product.id, quantity);
          toast.success(`${product.name} zostało dodane do koszyka!`);
      } catch (err: any) {
          toast.error("Nie udało się dodać do koszyka");
          console.error("Error adding to cart");
      }
  };

  return (
    <div className="product-list-container">
      <ul className="product-list">
        {products.map((product) => (
          <li key={product.id} className="product-item">
            <img src={product.imageUrl} alt={product.name} className="product-image" loading="lazy" />
            <div className="product-info">
              <h2 className="product-name">{product.name}</h2>
              <p className="product-description">{product.description}</p>
              <p className="product-price">{product.price.toFixed(2)} zł</p>
              <p className="product-quantity">W magazynie: {product.stockQuantity}</p>
              <button onClick={() => handleAddToCart(product)} className="add-to-cart-button">Dodaj do koszyka</button>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}