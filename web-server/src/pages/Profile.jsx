import PropTypes from "prop-types";

function Profile({onLogout}) {

    const handleLogoutClick = async () => {
        try {
            onLogout();
        } catch (error) {
            console.error('Errore durante il logout:', error);
        }
    };

    return (
        <div className="flex flex-wrap p-8 ">

            <div className="w-full md:w-1/2 bg-blue-100 p-4">
                <h1 className="text-2xl font-bold mb-4">Profile Page</h1>
                <p>Welcome to the Profile page.</p>
                <button className="border" onClick={handleLogoutClick}>Logout</button>
            </div>
            <div className="w-full md:w-1/2 bg-green-100 p-4">
                <h1 className="text-2xl font-bold mb-4">Your internship</h1>

            </div>

        </div>
    );
}


Profile.propTypes = {
    onLogout: PropTypes.func.isRequired,
};

export default Profile;