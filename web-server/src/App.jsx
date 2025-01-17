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
            // Se hai bisogno di comunicare con il server per il logout
            // const response = await fetch(`${API_SERVER_URL}/api/authentication/logout`, {
            //     method: 'POST',
            //     headers: {
            //         'Authorization': `Bearer ${localStorage.getItem('token')}`
            //     }
            // });

            // if (response.ok) {
            //     onLogout();
            // }

            // Se non hai bisogno di comunicare con il server, chiama direttamente onLogout
            onLogout();
        } catch (error) {
            console.error('Errore durante il logout:', error);
        }
    };
    const jobs = [
        {
            id: 0,
            title: "Google",
            imgURL: "https://play-lh.googleusercontent.com/1-hPxafOxdYpYZEOKzNIkSP43HXCNftVJVttoo4ucl7rsMASXW3Xr6GlXURCubE1tA=w3840-h2160-rw",
            jobPosition: "Web Design",
        },
        {
            id: 1,
            title: "Microsoft",
            imgURL: "https://webcrew.it/wp-content/uploads/2016/03/microsoft-logo-850x351.jpg",
            jobPosition: "Web Developer",
        },
        {
            id: 2,
            title: "Informatica SRL",
            imgURL: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTK0sFaSmmEo27EzXqDbes0GKJrUc1nZyaj5A&s",
            jobPosition: "Data Analyst",
        },
        {
            id: 3,
            title: "Amazon",
            imgURL: "https://img.economyup.it/wp-content/uploads/2020/08/amazon.jpg",
            jobPosition: "Data Analyst",
        },
        {
            id: 4,
            title: "Accenture",
            imgURL: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQqX52yBHK1YX3u1f9Z1WJA5EMzZZAw6Nukig&s",
            jobPosition: "Data Analyst",
        },
        {
            id: 5,
            title: "Reply",
            imgURL: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtQj-kQQjDK0xlBlghU2wg-uM51X7tndS5og&s",
            jobPosition: "Data Analyst",
        }
    ]

    return (
        <Router>
            <div className="gap-8">
                <Navbar />
                <div className="pt-24">
                    <Routes>
                        <Route path="/" element={<Home jobs={jobs} />} />
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
