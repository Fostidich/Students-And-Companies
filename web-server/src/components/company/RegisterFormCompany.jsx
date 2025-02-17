import PropTypes from "prop-types";
import { useState } from "react";
import { getErrorMessage } from "../../utils/errorUtils";
const API_SERVER_URL =
  window.env?.VITE_API_SERVER_URL || "http://localhost:5000";

function RegisterFormStudent({ onRegister }) {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [bio, setBio] = useState("");
  const [headquarter, setHeadquarter] = useState("");
  const [fiscalCode, setFiscalCode] = useState("");
  const [vatNumber, setVatNumber] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    const response = await fetch(
      `${API_SERVER_URL}/api/authentication/register/company`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          username,
          email,
          password,
          bio,
          headquarter,
          fiscalCode,
          vatNumber,
        }),
      },
    );

    if (response.ok) {
      onRegister();
      console.log("Registration successful");
    } else {
      setErrorMessage("");
      setErrorMessage(await getErrorMessage(response));
    }
  };
  return (
    <form
      onSubmit={handleSubmit}
      className="bg-white flex flex-col rounded-md p-6 border border-[#d9d9d9]"
    >
      {errorMessage && <div className="text-red-500 mb-4">{errorMessage}</div>}
      <p className="text-center font-medium pb-2">Register as company</p>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        className="mb-4 px-4 py-2 border rounded-md"
      />
      <input
        type="email"
        placeholder="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        className="mb-4 px-4 py-2 border rounded-md"
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        className="mb-4 px-4 py-2 border rounded-md"
      />
      <input
        type="text"
        placeholder="Bio"
        value={bio}
        onChange={(e) => setBio(e.target.value)}
        className="mb-4 px-4 py-2 border rounded-md"
      />
      <input
        type="text"
        placeholder="Headquarter"
        value={headquarter}
        onChange={(e) => setHeadquarter(e.target.value)}
        className="mb-4 px-4 py-2 border rounded-md"
      />
      <input
        type="text"
        placeholder="FiscalCode"
        value={fiscalCode}
        onChange={(e) => setFiscalCode(e.target.value)}
        className="mb-4 px-4 py-2 border rounded-md"
      />
      <input
        type="text"
        placeholder="VatNumber"
        value={vatNumber}
        onChange={(e) => setVatNumber(e.target.value)}
        className="mb-4 px-4 py-2 border rounded-md"
      />
      <button
        type="submit"
        className="px-24 py-3 text-lg font-light text-white bg-[#2c2c2c] rounded-md hover:bg-[#1e1e1e]"
      >
        Submit
      </button>
    </form>
  );
}

RegisterFormStudent.propTypes = {
  onRegister: PropTypes.func.isRequired,
};

export default RegisterFormStudent;
