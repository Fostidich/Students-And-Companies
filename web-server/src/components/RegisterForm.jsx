import PropTypes from 'prop-types';
import { useState } from 'react';
const API_SERVER_URL = import.meta.env.VITE_API_SERVER_URL;
function RegisterForm({ onRegister }) {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [userType, setUserType] = useState('Student');
    const [errorMessage, setErrorMessage] = useState('');
    const handleSubmit = async (e) => {
        e.preventDefault();
        const response = await fetch(`${API_SERVER_URL}/api/authentication/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password, userType, email }),
        });


        if (response.ok) {
            onRegister();
            console.log('Registration successful');
        } else {
            setErrorMessage('Invalid username or password error: '+ response.status);
            console.error('Registration failed: ' + response.status);
        }
    }
    return (
        <form onSubmit={handleSubmit} className="bg-white flex flex-col rounded-md p-6 border border-[#d9d9d9]">
            {errorMessage && <div className="text-red-500 mb-4">{errorMessage}</div>}
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="mb-4 px-4 py-2 border rounded-md"/>
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="mb-4 px-4 py-2 border rounded-md"/>
            <select
                value={userType}
                onChange={(e) => setUserType(e.target.value)}
                className="mb-4 px-4 py-2 border rounded-md">
                <option value="Student" className="text-black">Student</option>
                <option value="Company" className="text-black">Company</option>
            </select>
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="mb-4 px-4 py-2 border rounded-md"/>
            <button
                type="submit"
                className="px-24 py-3 text-lg font-light text-white bg-[#2c2c2c] rounded-md hover:bg-[#1e1e1e]"
            >
                Submit
            </button>

        </form>
    );
}

RegisterForm.propTypes = {
    onRegister: PropTypes.func.isRequired,
};

export default RegisterForm;