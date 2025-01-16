import { useState } from 'react';
import Welcome from './pages/Welcome.jsx';
import App from './App.jsx';

function FirstPage() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const handleLogin = () => {
        setIsLoggedIn(true);
    };

    return (
        isLoggedIn ? <App /> : <Welcome onLogin={handleLogin} />
    );
}

export default FirstPage;