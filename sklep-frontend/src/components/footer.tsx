import React from 'react';
import { Link } from 'react-router-dom';  // Importujemy Link do nawigacji
import '../styles/footer.css'


export default function Footer() {
    return (     
        <div className="footer">
            <img src="/icon.png" className="logo" />
            PlantShop
            <div className="copyrights">
                <br/>
                All rights reserved 2024.   
                
                <a href="/terms" className="link">  Regulamin oraz Polityka Prywatnosci</a>
            </div>
        </div>        
    )
}