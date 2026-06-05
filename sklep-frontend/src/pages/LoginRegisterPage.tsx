import React, { useState, useEffect } from 'react';
import LoginForm from '../components/Login/LoginForm'; // Your Login component
import RegisterForm from '../components/Login/RegistrationForm'; // Your Register component
import '../styles/LoginRegister.css';  // Import the updated CSS file
import { useUser } from '../services/userContext';

export default function LoginRegister() {
    const [isLogin, setIsLogin] = useState<boolean>(true);
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
    const { user } = useUser();

    // Check if the user is logged in (token exists in localStorage)
    useEffect(() => {
        const logged = user.isLoggedIn;
        if (logged) {
            setIsLoggedIn(true);  // User is logged in
        } else {
            setIsLoggedIn(false); // User is not logged in
        }
    }, []); // Runs once when component mounts

    if (isLoggedIn) {
        return (
            <div className="container">
                <h2>Jesteś już zalogowany!</h2>
            </div>
        );
    }

    return (
        <div className="login-register-container">
            <div className="background-image"></div> {/* Tło z obrazem */}
            <div className="login-register-content">
                <div className="login-register-panel">
                    {isLogin ? (
                        <>
                            <LoginForm />
                        </>
                    ) : (
                        <>
                            <RegisterForm />
                        </>
                    )}

                    <div className="switch-section">
                        {isLogin ? (
                            <div className="text-2">
                                Nie posiadasz jeszcze konta?
                                <button className="button" onClick={() => setIsLogin(false)}>Zarejestruj się</button>
                            </div>
                        ) : (
                            <div className="text-2">
                                Posiadasz już konto?
                                <button className="button" onClick={() => setIsLogin(true)}>Zaloguj się</button>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

