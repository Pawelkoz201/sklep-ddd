import React, { useState } from 'react';
import CategoryList from "../components/CategoryList";
import Product from "../components/Products"; 
import "../styles/Product.css";
import axios from 'axios';
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

export default function ProductPage() {
  const [selectedCategoryId, setSelectedCategoryId] = useState<number | null>(null);
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');

  const handleCategorySelect = async (categoryId: number) => {
    setSelectedCategoryId(categoryId);
    setLoading(true);
    setError('');

    try {
      const response = await axios.get(`http://localhost:5000/api/products/category/${categoryId}`);
      setProducts(response.data);
      setLoading(false);
    } catch (error) {
      setError('Error fetching products');
      setLoading(false);
    }
  };

    return (
        <div className="product-page-container">
            <CategoryList onCategorySelect={handleCategorySelect} />
            <div className="product-display">
                {loading ? (
                    <div>Loading products...</div>
                ) : error ? (
                    <div>{error}</div>
                ) : (
                    <Product products={products} />
                )}
            </div>
        </div>
    );
}