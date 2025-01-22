import PropTypes from "prop-types";
import { useNavigate } from "react-router-dom";
import CardProfileStudent from "../components/student/CardProfileStudent.jsx";
import Cookies from 'js-cookie';
import CardProfileCompany from "../components/company/CardProfileCompany.jsx";
import CvBox from "../components/student/CvBox.jsx";
import SkillsBox from "../components/student/SkillsBox.jsx";

function Profile({onLogout}) {
    const navigate = useNavigate();

    const handleLogoutClick = async () => {
        try {
            navigate('/');
            onLogout();
        } catch (error) {
            console.error('Errore durante il logout:', error);
        }
    };
    const authData = JSON.parse(Cookies.get('authData'));
    const userType = authData.userType;

    return (
        <div className="flex flex-wrap p-8">
            <div className="w-full md:w-1/2 bg-blue-100 p-4 items-center">
                {userType.toLowerCase() === 'student' ? (<CardProfileStudent/>) : (<CardProfileCompany/>)}
                {userType.toLowerCase() === 'student' ?
                    <div className="bg-white border items-center p-10 rounded-xl flex flex-wrap space-x-10">
                        <CvBox/>
                        <SkillsBox/>
                    </div>: <div></div>
                }
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