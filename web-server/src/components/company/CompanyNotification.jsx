import {useState,useEffect} from "react";
import Cookies from "js-cookie";
import {getErrorMessage} from "../../utils/errorUtils";
import PropTypes from "prop-types";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';
function CompanyNotification({pendingApplication}){
    const [errorMessage, setErrorMessage] = useState('');
    const authData = JSON.parse(Cookies.get('authData'));
    const [showDetails, setShowDetails] = useState(false);
    const [profileStudent, setProfileStudent] = useState({
        studentId: '',
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
    const [advertisement, setAdvertisement] = useState({
        advertisementId: '',
        name: '',
        createdAt: '',
        companyId: '',
        description: '',
        duration: '',
        spots: '',
        available: '',
        open: '',
        questionnaire: ''
    });

    useEffect(() => {
        const fetchData = async () => {
            await getStudentDetails();
            await getAdvertisementDetails();
        }
        fetchData();
    }, []);

    const getStudentDetails = async () => {
        const response = await fetch(`${API_SERVER_URL}/api/profile/student/${pendingApplication.studentId}`, {
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
    const getAdvertisementDetails = async () => {
        const response = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements/${pendingApplication.advertisementId}`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            },
        });
        if (response.status === 200) {
            const data = await response.json();
            setAdvertisement(data);
        }else {
            const message = await getErrorMessage(response);
            setErrorMessage(message);
            console.error('Error checking advertisement details:', errorMessage);
        }

    }
    const acceptApplication = async () => {
        const dateTime = new Date().toISOString();
        console.log(dateTime);
        const response = await fetch(`${API_SERVER_URL}/api/enrollment/accept/${pendingApplication.applicationId}`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authData.token}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({dateTime}),
        });
        if (response.status === 200) {
            console.log('Application accepted');
            setShowDetails(false);
        } else {
            setErrorMessage('');
            setErrorMessage(await getErrorMessage(response));
        }

    }
    const rejectApplication = async () => {
        const response = await fetch(`${API_SERVER_URL}/api/enrollment/reject/${pendingApplication.applicationId}`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authData.token}`,
                'Content-Type': 'application/json',
            },
        });
        if (response.status === 200) {
            console.log('Application accepted');
            setShowDetails(false);
        } else {
            setErrorMessage('');
            setErrorMessage(await getErrorMessage(response));
        }

    }

    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };

    return (
        <div className="bg-white rounded-lg border p-4 mb-4">
            <div className="flex justify-between items-start">
                <div>
                    <h1 className="text-xl font-semibold mb-2">{advertisement.name}</h1>
                    <h2 className="text-lg font-semibold mb-2">{profileStudent.name + ' '+ profileStudent.surname}</h2>
                    <p className="text-xl font-light mb-2">{advertisement.description}</p>

                </div>
                <button
                    onClick={() => setShowDetails(true) }
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                >
                    View Details
                </button>
            </div>

            {showDetails && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <div className="bg-white p-6 rounded-lg w-2/3 h-auto relative">
                        <button
                            onClick={() => setShowDetails(false) || setErrorMessage('')}
                            className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
                        >
                            ✕
                        </button>
                        <h2 className="text-xl font-bold mb-4">Student profile details</h2>
                        <div className="space-y-2">
                            <p><strong>Name:</strong> {profileStudent.name}</p>
                            <p><strong>Surname:</strong> {profileStudent.surname}</p>
                            <p><strong>University:</strong> {profileStudent.university +' '+ profileStudent.courseOfStudy}</p>
                            <p><strong>Questionnaire response:</strong> {pendingApplication.questionnaire}</p>
                            <p><strong>Created:</strong> {formatDate(pendingApplication.createdAt)}</p>
                            <p><strong>Status:</strong> {pendingApplication.status}</p>
                        </div>
                        <div className="flex justify-between grid grid-cols-2 gap-4">
                            <button
                                onClick={() => acceptApplication()}
                                className="bg-green-500 text-white p-2 rounded-md hover:bg-green-600 ">Accept</button>
                            <button
                                onClick={() => rejectApplication()}
                                className="bg-red-500 text-white p-2 rounded-md hover:bg-red-600 ">
                                Reject

                            </button>

                        </div>
                        {errorMessage && <div className="text-red-500 mb-4">{errorMessage}</div>}

                    </div>
                </div>
            )}
        </div>
    );
}
CompanyNotification.propTypes = {
    pendingApplication: PropTypes.shape({
        advertisementId: PropTypes.number.isRequired,
        applicationId: PropTypes.number.isRequired,
        createdAt: PropTypes.string.isRequired,
        questionnaire: PropTypes.string.isRequired,
        status: PropTypes.string.isRequired,
        studentId: PropTypes.number.isRequired,
    })
};
export default CompanyNotification;