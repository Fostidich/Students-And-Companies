import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PropTypes from 'prop-types';
import Navbar from "./components/NavBar.jsx";
import Home from "./pages/Home.jsx";
import Profile from "./pages/Profile.jsx";
import Notification from "./pages/Notification.jsx";
import Help from "./pages/Help.jsx";
function App({ onLogout }) {

    const handleLogoutClick = async () => {
        try {
            onLogout();
        } catch (error) {
            console.error('Errore durante il logout:', error);
        }
    };

    return (
        <Router>
            <div className="gap-8">
                <Navbar />
                <div className="pt-24">
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/profile" element={<Profile onLogout={handleLogoutClick} />} />
                        <Route path="/notification" element={<Notification />} />
                        <Route path="/help" element={<Help />} />
                    </Routes>
                </div>
            </div>
        </Router>
    );
}

App.propTypes = {
    onLogout: PropTypes.func.isRequired,
};

export default App;
