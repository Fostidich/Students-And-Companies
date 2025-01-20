import PropTypes from "prop-types";
import CardProfile from "../components/CardProfile.jsx";

function Profile({onLogout}) {

    const handleLogoutClick = async () => {
        try {
            onLogout();
        } catch (error) {
            console.error('Errore durante il logout:', error);
        }
    };

    return (
        <div className="flex flex-wrap p-8">

            <div className="w-full md:w-1/2 bg-blue-100 p-4 items-center ">
                <CardProfile/>

            </div>
            <div className="w-full md:w-1/2 bg-green-100 p-4">
                <h1 className="text-2xl font-bold mb-4">Your internship</h1>

            </div>
            <button className="border p-4 bg-red-600 text-white rounded-xl" onClick={handleLogoutClick}>Logout</button>

        </div>
    );
}


Profile.propTypes = {
    onLogout: PropTypes.func.isRequired,
};

export default Profile;