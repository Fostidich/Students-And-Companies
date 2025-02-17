import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import PropTypes from "prop-types";
import Navbar from "./components/NavBar.jsx";
import Home from "./pages/Home.jsx";
import Profile from "./pages/Profile.jsx";
import Notification from "./pages/Notification.jsx";
import ApplicationsNotifications from "./pages/ApplicationsNotifications.jsx";
import Cookies from "js-cookie";

function App({ onLogout }) {
  const authData = JSON.parse(Cookies.get("authData"));
  const userType = authData.userType;

  const handleLogoutClick = async () => {
    try {
      onLogout();
    } catch (error) {
      console.error("Errore durante il logout:", error);
    }
  };

  return (
    <Router>
      <div className="gap-8">
        <Navbar />
        <div className="pt-24">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route
              path="/profile"
              element={<Profile onLogout={handleLogoutClick} />}
            />
            {userType.toLowerCase() === "student" ? (
              <Route path="/notification" element={<Notification />} />
            ) : (
              <Route
                path="/applications"
                element={<ApplicationsNotifications />}
              />
            )}
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
