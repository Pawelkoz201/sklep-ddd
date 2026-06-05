import React, { useState } from 'react';
import { api } from '../../api/api';
import '../../styles/LoginRegister.css'; // Import stylów
import { useUser } from "../../services/userContext";

export default function LoginForm() {
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [success, setSuccess] = useState<string>('');
    const [error, setError] = useState<string>('');
    const { user, setUser } = useUser();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setSuccess('');
        setError('');
        try {
            const data = await api.loginUser(email, password);
            setSuccess('Zalogowano się pomyślnie!');
            localStorage.setItem('token', data.token);
            setUser((prev) => ({
                ...prev,isLoggedIn:true,
                 cartItems: [{ id: 1, name: "Plant A", quantity: 1 }],
                 token: data.token,
                 name: email,
                 id: data.userId,
            }));
       } catch (err: any) {
            setError(err.response?.data?.message || 'Nie udało się zalogować!');
        }
    };

    const handleRegisterRedirect = () => {
        window.location.href = '/register'; // Przekierowanie na stronę rejestracji
    };

    return (
        <div className="login-container">
                <div className="background-panel">
                    <div className="login-panel">
                        <div className="text">Zaloguj się</div>
                        <div className="underline"></div>
                        <form onSubmit={handleSubmit}>
                            <div className="form-group">
                                <div className="form-row">
                                    <img src="/mail.png" alt="Email Icon" className="input-icon" />
                                    <input
                                        type="email"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                        placeholder="Adres email"
                                        required
                                    />
                                </div>
                            </div>
                            <div className="form-group">
                                <div className="form-row">
                                    <img src="/padlock.png" alt="Password Icon" className="input-icon" />
                                    <input
                                        type="password"
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        placeholder="Hasło"
                                        required
                                    />
                                </div>
                            </div>
                            {success && <p className="success">{success}</p>}
                            {error && <p className="error">{error}</p>}
                            <button type="submit">Zaloguj się</button>
                        </form>                   
                    </div>
                    
                </div>
        </div>
    );
}



