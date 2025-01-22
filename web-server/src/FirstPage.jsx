import { useState, useEffect } from 'react';
import Welcome from './pages/Welcome.jsx';
import App from './App.jsx';
import Cookies from 'js-cookie';
const API_SERVER_URL = import.meta.env.VITE_API_SERVER_URL;

function FirstPage() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const checkAuthStatus = async () => {
            try {
                const data = Cookies.get('authData');
                if(data === undefined){
                    setIsLoggedIn(false);
                    setIsLoading(false);
                    return;
                }
                const authData = JSON.parse(data) ;

                const response = await fetch(`${API_SERVER_URL}/api/authentication`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${authData.token}`, // Invia il token nell'header Authorization
                    },
                });

                if (!response.status === 200) {
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

    const handleLogin = async () => {
        setIsLoggedIn(true);
    };

    const handleLogout = () => {
        Cookies.remove('authData');
        setIsLoggedIn(false);
    };

    if (isLoading) {
        return <div className="animate-ping ">Caricamento...</div>;
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