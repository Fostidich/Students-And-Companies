import PropTypes from 'prop-types';
import { useState } from 'react';
import Cookies from 'js-cookie';
const API_SERVER_URL = import.meta.env.VITE_API_SERVER_URL;

function LoginForm({ onLogin }) {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const saveTokenToCookie = (token) => {
        // Salva il token in un cookie con un TTL di 24 ore
        Cookies.set('token', token, {
            expires: 1, // TTL: 1 giorno
            secure: true, // Richiede HTTPS
            sameSite: 'strict', // Previene attacchi CSRF
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const response = await fetch(`${API_SERVER_URL}/api/authentication/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),

        });

        if (response.ok) {
            const data = await response.json();
            saveTokenToCookie(data.token);
            onLogin();
            console.log('Login successful');
        } else {
            console.error('Login failed: '+response.status);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="bg-white flex flex-col rounded-md p-6 border border-[#d9d9d9]">
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="mb-4 px-4 py-2 border rounded-md"
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="mb-4 px-4 py-2 border rounded-md"
            />
            <button
                type="submit"
                className="border px-24 py-3 text-lg font-light text-black bg-white rounded-md hover:bg-[#f5f5f5]"
            >
                Submit
            </button>
        </form>
    );
}

LoginForm.propTypes = {
    onLogin: PropTypes.func.isRequired,
};

export default LoginForm;