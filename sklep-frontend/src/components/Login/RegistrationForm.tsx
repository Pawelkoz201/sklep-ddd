import React, { useState } from 'react';
import { api } from '../../api/api';
import '../../styles/LoginRegister.css'; // Import stylów

export default function RegisterForm() {
    const [username, setUsername] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [success, setSuccess] = useState<string>('');
    const [error, setError] = useState<string>('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setSuccess('');
        setError('');

        try {
            const data = await api.registerUser(username, email, password);
            setSuccess(`User registered`);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Registration failed');
        }
    };

    const handleLoginRedirect = () => {
        window.location.href = '/login'; // Przekierowanie na stronę logowania
    };

    return (
        <div className="register-container">
            <div className="background-panel">
                <div className="login-panel">
                    <div className="text">Zarejestruj się</div>
                    <div className="underline"></div>
                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <div className="form-row">
                                <img src="/user.png" alt="Username Icon" className="input-icon" />
                                <input
                                    type="text"
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                    placeholder="Nazwa użytkownika"
                                    required
                                />
                            </div>
                        </div>
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
                        <button type="submit">Zarejestruj się</button>
                    </form>                   
                </div>
                
            </div>
        </div>
    );
}


