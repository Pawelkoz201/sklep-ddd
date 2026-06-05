import React from 'react';
import { useNavigate } from 'react-router-dom'; // Importujemy useNavigate do nawigacji
import '../styles/home.css';

export default function Home() {
    const navigate = useNavigate(); // Hook do obs�ugi nawigacji

    const handleExploreClick = () => {
        navigate('/shop'); // Przekierowanie do widoku /shop
        console.log("Explore button clicked");
    };

    return (
        <div className="home">
            <div className="home-overlay">
                <h1 className="home-title">Witamy w Sklepie z Roślinami</h1>
                <p className="home-description">
                Odkryj naszą bogatą kolekcję roślin do wnętrz i ogrodów.
                </p>
                <button className="home-button" onClick={handleExploreClick}>
                Odkryj teraz
                </button>
            </div>
        </div>
    );
}