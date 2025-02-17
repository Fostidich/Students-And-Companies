import PropTypes from "prop-types";
import { useState } from "react";
import LoginForm from "../components/LoginForm.jsx";
import RegisterFormStudent from "../components/student/RegisterFormStudent.jsx";
import RegisterFormCompany from "../components/company/RegisterFormCompany.jsx";

function Welcome({ onLogin }) {
  const [showLogin, setShowLogin] = useState(true);
  const [student, setStudent] = useState(false);

  const handleRegister = async () => {
    setShowLogin(true);
  };

  return (
    <div className="flex-col md:flex-row flex h-screen w-screen bg-white items-center justify-center gap-4">
      {/* Left Column */}
      <div className="flex flex-col items-center justify-center bg-white text-black w-3/4">
        <img src="/SC-logo-welcome.png" alt="Site Logo" className="w-full" />
      </div>

      {/* Right Column */}
      <div className="flex flex-col items-center justify-center w-1/2 bg-white">
        {showLogin ? (
          <LoginForm onLogin={onLogin} />
        ) : student ? (
          <RegisterFormStudent onRegister={handleRegister} />
        ) : (
          <RegisterFormCompany onRegister={handleRegister} />
        )}
        <div className="mt-4">
          <button
            onClick={() => setShowLogin(!showLogin)}
            className={`px-6 py-2 mr-2 ${showLogin ? "text-white bg-[#3e3e3e] hover:bg-[#1e1e1e]" : "bg-gray-200 hover:bg-gray-300 text-black"} rounded-md`}
          >
            {showLogin ? "Register" : "Login"}
          </button>
          {!showLogin && (
            <button
              onClick={() => setStudent(!student)}
              className={`px-6 py-2 ${student ? "bg-green-500 hover:bg-green-600 text-white" : "bg-purple-500 hover:bg-purple-600 text-white"} rounded-md`}
            >
              {student ? "Company" : "Student"}
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
