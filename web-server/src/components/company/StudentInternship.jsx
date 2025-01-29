import {useState,useEffect} from "react";
import Cookies from "js-cookie";
import {getErrorMessage} from "../../utils/errorUtils";
import PropTypes from "prop-types";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function StudentInternship({internship}) {
    const authData = JSON.parse(Cookies.get('authData'));
    const [errorMessage, setErrorMessage] = useState('');
    const [profileStudent, setProfileStudent] = useState({
        studentId: 0,
        username: '',
        email: '',
        bio: '',
        name: '',
        surname: '',
        university: '',
        courseOfStudy: '',
        gender: '',
        birthDate: '',
    });
    useEffect(() => {
        const fetchData = async () => {
            await getInformation();
        }
        fetchData();
    },[]);


    const getInformation = async () =>{
        const response = await fetch(`${API_SERVER_URL}/api/profile/student/${internship.studentId}`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            },
        });
        if (response.status === 200) {
            const data = await response.json();
            setProfileStudent(data);
        }else {
            const message = await getErrorMessage(response);
            setErrorMessage(message);
            console.error('Error checking advertisement details:', errorMessage);
        }
    }
    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };




    return (
        <div>
           <div className="bg-white p-4 rounded-lg shadow-md mb-4">
               <div className="space-y-2">
                   <h3 className="text-lg font-semibold">
                       {profileStudent.name} {profileStudent.surname}
                   </h3>
                   <p className="text-sm"><strong>University:</strong> {profileStudent.university}</p>
                   <p className="text-sm"><strong>Course of Study:</strong> {profileStudent.courseOfStudy}</p>
                   <p className="text-sm"><strong>Bio:</strong> {profileStudent.bio}</p>
                   <div className="mt-2 border-t pt-2">
                       <p className="text-xs text-gray-600"><strong>Email:</strong> {profileStudent.email}</p>
                       <p className="text-xs text-gray-600"><strong>Internship start date:</strong> {formatDate(internship.startDate)}</p>
                       <p className="text-xs text-gray-600"><strong>Internship end date:</strong> {formatDate(internship.endDate)}</p>
                       <p className="text-xs text-gray-600"><strong>Internship created at:</strong> {formatDate(internship.createdAt)}</p>
                   </div>
               </div>
               {errorMessage && <div className="text-red-500 mt-4">{errorMessage}</div>}
           </div>
        </div>
    );
}
StudentInternship.propTypes = {
    internship: PropTypes.shape({
        internshipId: PropTypes.number.isRequired,
        createdAt: PropTypes.string.isRequired,
        studentId: PropTypes.number.isRequired,
        companyId: PropTypes.number.isRequired,
        advertisementId: PropTypes.number.isRequired,
        startDate: PropTypes.string.isRequired,
        endDate: PropTypes.string.isRequired
    }).isRequired
};
export default StudentInternship;