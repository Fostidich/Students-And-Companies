import PropTypes from "prop-types";

function Profile({onLogout}) {

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

    return (
        <div className="p-8">
            <h1 className="text-2xl font-bold mb-4">Profile Page</h1>
            <p>Welcome to the Profile page.</p>
            <button onClick={handleLogoutClick}>Logout</button>
        </div>
    );
}


Profile.propTypes = {
    onLogout: PropTypes.func.isRequired,
};

export default Profile;