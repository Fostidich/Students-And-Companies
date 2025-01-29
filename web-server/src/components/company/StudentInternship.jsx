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




    return (
        <div>
           <div className="bg-white rounded-lg border p-4 mb-4">
               <h2 className="text-xl font-bold mb-4">Internship and Student Profile Details</h2>
               <div className="space-y-2">
                   <p><strong>Student Name:</strong> {profileStudent.name} {profileStudent.surname}</p>
                   <p><strong>University:</strong> {profileStudent.university}</p>
                   <p><strong>Course of Study:</strong> {profileStudent.courseOfStudy}</p>
                   <p><strong>Email:</strong> {profileStudent.email}</p>
                   <p><strong>Internship Start Date:</strong> {internship.startDate}</p>
                   <p><strong>Internship End Date:</strong> {internship.endDate}</p>
                   <p><strong>Internship Created At:</strong> {internship.createdAt}</p>
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