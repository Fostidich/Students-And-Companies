import PropTypes from 'prop-types';
import { useState } from 'react';
import LoginForm from '../components/LoginForm.jsx';
import RegisterFormStudent from '../components/student/RegisterFormStudent.jsx';
import RegisterFormCompany from "../components/company/RegisterFormCompany.jsx";

function Welcome({ onLogin }) {
    const [showLogin, setShowLogin] = useState(true);
    const [student, setStudent] = useState(false);

    const handleRegister = async () => {
        setShowLogin(true);
    }

    return (
        <div className="flex h-screen w-screen bg-white">
            {/* Left Column */}
            <div className="flex flex-col items-center justify-center bg-white text-black w-3/4">
                <img src="/SC-logo-welcome.png" alt="Site Logo" className="w-full" />
            </div>

            {/* Right Column */}
            <div className="flex flex-col items-center justify-center w-1/2 bg-white">
                {showLogin ? (
                    <LoginForm onLogin={onLogin}/>
                ) : (
                    student ? (<RegisterFormStudent onRegister={handleRegister}/>) : (<RegisterFormCompany onRegister={handleRegister}/>)
                )}
                <div className="mt-4">
                    <button
                        onClick={() => setShowLogin(!showLogin)}
                        className={`px-6 py-2 mr-2 ${showLogin ? 'bg-blue-500 text-white' : 'bg-gray-200 text-black'} rounded-md`}
                    >
                        {showLogin ? 'Register' : 'Login'}
                    </button>
                    {!showLogin && (
                        <button
                            onClick={() => setStudent(!student)}
                            className={`px-6 py-2 ${student ? 'bg-green-500 text-white' : 'bg-purple-500 text-white'} rounded-md`}
                        >
                            {student ? 'Company' : 'Student'}
                        </button>
                    )}
                </div>
            </div>
        </div>
    );
}

Welcome.propTypes = {
    onLogin: PropTypes.func.isRequired,
};

export default Welcome;
