import { useState, useEffect } from 'react';
import Welcome from './pages/Welcome.jsx';
import App from './App.jsx';

function FirstPage() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const checkAuthStatus = async () => {
            try {
                const token = localStorage.getItem('token');

                if (!token) {
                    setIsLoggedIn(false);
                    setIsLoading(false);
                    return;
                }

                // Aggiungere una chiamata al server per validare il token se necessario

                setIsLoggedIn(true);
                setIsLoading(false);
            } catch (error) {
                console.error('Error checking authentication status:', error);
                localStorage.removeItem('token');
                setIsLoggedIn(false);
                setIsLoading(false);
            }
        };

        checkAuthStatus();
    }, []);

    const handleLogin = () => {
        setIsLoggedIn(true);
    };

    const handleLogout = () => {
        localStorage.removeItem('token');
        setIsLoggedIn(false);
    };

    if (isLoading) {
        return <div>Caricamento...</div>;
    }

    return (
        isLoggedIn ? (
            <App onLogout={handleLogout} />
        ) : (
            <Welcome onLogin={handleLogin} />
        )
    );
}

export default FirstPage;