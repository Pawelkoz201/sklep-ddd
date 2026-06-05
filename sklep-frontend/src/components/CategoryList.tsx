import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../styles/CategoryList.css'; 

interface Category {
  id: number;
  name: string;
}

interface CategoryListProps {
  onCategorySelect: (categoryId: number) => void;
}

export default function CategoryList({ onCategorySelect }: CategoryListProps) {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/categories'); 
        setCategories(response.data);
        setLoading(false);
      } catch (error) {
        setError('Error fetching categories');
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="category-list-container">
      <h1>Kategorie</h1>
      <ul className="category-list">
          {categories.map((category) => (
          <li 
            key={category.id} 
            className="category-item"
            onClick={() => onCategorySelect(category.id)} 
            role="button" 
          >
            {category.name}
          </li>
        ))}
      </ul>
    </div>
  );
}