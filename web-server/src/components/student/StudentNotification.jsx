import PropTypes from "prop-types";
import {useEffect, useState} from "react";
import Cookies from "js-cookie";
import {getErrorMessage} from "../../utils/errorUtils.js";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function StudentNotification({notification, onDelete}) {
    const [showDetails, setShowDetails] = useState(false);
    const [questionnaireAnswer, setQuestionnaireAnswer] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [advertisement, setAdvertisement] = useState({
        advertisementId: 0,
        name: '',
        createdAt: '',
        companyId: 0,
        description: '',
        duration: 0,
        spots: 0,
        available: 0,
        open: false,
        questionnaire: '',
    });
    const [profileCompany, setProfileCompany] = useState({
        username: '',
        email: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: '',
        advertisementId: ''
    });

    const getAdvertisement = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements/${notification.advertisementId}`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });
            if (response.status === 200) {
                const data = await response.json();
                setAdvertisement(data);
                return data; // Return the data for chaining
            } else {
                const message = await getErrorMessage(response);
                console.error('Error fetching advertisement details:', message);
                return null;
            }
        } catch (error) {
            console.error('Error fetching advertisement:', error);
            return null;
        }
    };

    const getProfileCompanyDetails = async (companyId) => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/company/${companyId}`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const data = await response.json();
                setProfileCompany(data);
            }
        } catch (error) {
            console.error('Error checking company details:', error);
        }
    };

    useEffect(() => {
        const fetchData = async () => {
            const advData = await getAdvertisement();
            if (advData && advData.companyId) {
                await getProfileCompanyDetails(advData.companyId);
            }
        };
        fetchData();
    }, [notification.advertisementId]);

    const deleteNotification = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/notification/delete/${notification.studentNotificationId}`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                }
            });

            if (response.ok) {
                onDelete(notification.studentNotificationId);
            } else {
                const message = await getErrorMessage(response);
                setErrorMessage(message);
            }
        } catch {
            setErrorMessage('Failed to delete notification');
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const authData = JSON.parse(Cookies.get('authData'));

        const response = await fetch(`${API_SERVER_URL}/api/enrollment/applications/${notification.advertisementId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${authData.token}`,
            },
            body: JSON.stringify({questionnaireAnswer}),
        });

        if (response.status === 200) {
            console.log('Applied');
            setShowDetails(false);
        } else {
            setErrorMessage('');
            setErrorMessage(await getErrorMessage(response));
        }
    };

    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };

    return(
        <div>
            <div className="bg-white p-4 rounded-lg shadow-xl mb-4">
                {
                    notification.type === 'INVITED' ? (
                        <p className="text-sm text-blue-500">You have been invited from the company to apply for this advertisement</p>
                    ):(<div></div>)
                }
                {
                    notification.type === 'RECOMMENDED' ? (
                        <p className="text-sm  text-purple-500">Let me recommend you this internship, you fit well</p>
                    ):(<div></div>)
                }
                {
                    notification.type === 'ACCEPTED' ? (
                        <p className="text-sm text-green-500">You have been accepted for this advertisement</p>
                    ):(<div></div>)
                }
                {
                    notification.type === 'REJECTED' ? (
                        <p className="text-sm text-red-500">You have been rejected for this advertisement</p>
                    ):(<div></div>)
                }
                <div className="space-y-2">
                    <button onClick={deleteNotification} className="text-red-500 float-right">Delete</button>
                    <h1 className="text-sm"><strong>Company:</strong> {profileCompany.username}</h1>
                    <h2 className="text-sm"><strong>Name:</strong> {advertisement.name}</h2>
                    <p className="text-sm"><strong>Description:</strong> {advertisement.description}</p>

                    <button
                        onClick={() => setShowDetails(true)}
                        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    >
                        View Details
                    </button>
                </div>
            </div>
            {showDetails && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <div className="bg-white p-6 rounded-lg w-2/3 h-auto relative">
                        <button
                            onClick={() => setShowDetails(false)}
                            className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
                        >
                            ✕
                        </button>
                        <h2 className="text-xl font-bold mb-4">Advertisement Details</h2>
                        <div className="border p-4 rounded-md space-y-2">
                            <h3 className="text-lg font-semibold">
                                {advertisement.name}
                            </h3>
                            <p className="text-sm"><strong>Description:</strong> {advertisement.description}</p>
                            <div className="mt-2 border-t pt-2">
                                <p className="text-xs text-gray-600"><strong>ADV ID:</strong> {advertisement.advertisementId}</p>
                                <p className="text-xs text-gray-600"><strong>Company ID:</strong> {advertisement.companyId}</p>
                                <p className="text-xs text-gray-600"><strong>Duration:</strong> {advertisement.duration}</p>
                                <p className="text-xs text-gray-600"><strong>Total spots:</strong> {advertisement.spots}</p>
                                <p className="text-xs text-gray-600"><strong>Available spots:</strong> {advertisement.available}</p>
                                <p className="text-xs text-gray-600"><strong>Status:</strong> {advertisement.open ? 'Open' : 'Closed'}</p>
                                <p className="text-xs text-gray-600"><strong>Created:</strong> {formatDate(advertisement.createdAt)}</p>
                            </div>
                        </div>
                        {
                            notification.type === 'ACCEPTED' || notification.type === 'REJECTED' ? (
                                <div></div>
                            ):(
                                <div className="items-center justify-center">
                                    <form onSubmit={handleSubmit}>
                                        {errorMessage && <div className="text-red-500 mb-4">{errorMessage}</div>}
                                        <p><strong>Questionnaire: </strong></p>
                                        <p className="flex">{advertisement.questionnaire}</p>
                                        <input
                                            type="text"
                                            value={questionnaireAnswer}
                                            onChange={(e) => setQuestionnaireAnswer(e.target.value)}
                                            className="border rounded p-2 w-full"
                                            required
                                        />
                                        <button className="gap-2 p-2 border rounded-md text-white bg-green-500 hover:bg-green-600" type="submit">Apply</button>
                                    </form>
                                </div>
                            )
                        }
                    </div>
                </div>
            )}
        </div>
    );
}

StudentNotification.propTypes = {
    notification: PropTypes.shape({
        studentNotificationId: PropTypes.number,
        studentId: PropTypes.number,
        advertisementId: PropTypes.number,
        type: PropTypes.string,
    }),
    onDelete: PropTypes.func.isRequired
};

export default StudentNotification;