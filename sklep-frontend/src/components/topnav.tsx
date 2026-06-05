import React from 'react';
import { Link } from 'react-router-dom';  // Importujemy Link do nawigacji
import '../styles/topnav.css'
import { useUser } from '../services/userContext';
import { useNavigate } from 'react-router-dom';

export default function TopNavbar() {
  const { user, setUser } = useUser();
  const navigate = useNavigate();

  const handleLogout = (): void => {
      localStorage.clear();
      setUser({ id: 0, name: "", isLoggedIn: false, cartItems: [], token: undefined });

      navigate('/'); 
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
          <div className="navbar-logo">
          <img src="/icon.png" className="logo" />
          <Link to="/">PlantShop</Link>
        </div>

        <ul className="navbar-links">
          <li>
            <Link to="/">Strona główna</Link>
          </li>
          <li>
            <Link to="/shop">Produkty</Link>
          </li>
          <li>
            <Link to="/contact">Kontakt</Link>
          </li>
        </ul>
        <div className="navbar-actions">
          {/* Dynamically show login or profile link */}
          {user.isLoggedIn ? (
            <Link to="/profile" className="navbar-button">
              Profil
            </Link>
          ) : (
            <Link to="/login" className="navbar-button">
              Zaloguj się
            </Link>
          )}
          <Link to="/cart" className="navbar-button">
            Koszyk
          </Link>
          {user.isLoggedIn ? (
          <Link onClick={handleLogout} to="/" className="navbar-button">
            Wyloguj się
          </Link>
          ) : (
              <></>
          )}
        </div>
      </div>
    </nav>
  );
}