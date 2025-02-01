import PropTypes from "prop-types";
import { useState } from "react";
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

    const getAdvertisement = async () => {
        const authData = JSON.parse(Cookies.get('authData'));
        const response = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements/${notification.advertisementId}`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            },
        });
        if (response.status === 200) {
            const data = await response.json();
            setAdvertisement(data);
        } else {
            const message = await getErrorMessage(response);
            console.error('Error fetching advertisement details:', message);
        }
    }

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
    }
    const handleSubmit = async (e) => {
        e.preventDefault()
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

    }
    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };

    return(
        <div>
            <div className="bg-white p-4 rounded-lg shadow-md mb-4">
                <div className="space-y-2">
                    <button onClick={deleteNotification} className="text-red-500 float-right">Delete</button>
                    <h3 className="text-lg font-semibold">
                        {notification.type}
                    </h3>
                    <p className="text-sm"><strong>Student ID:</strong> {notification.studentId}</p>
                    <p className="text-sm"><strong>Advertisement ID:</strong> {notification.advertisementId}</p>
                    <button
                        onClick={() => setShowDetails(true) || getAdvertisement()}
                        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    >
                        View Details
                    </button>
                </div>
            </div>
            {showDetails && (
                <div className=" fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <div className="bg-white p-6 rounded-lg w-2/3 h-auto relative">
                        <button
                            onClick={() => setShowDetails(false)}
                            className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
                        >
                            ✕
                        </button>
                        <h2 className="text-xl font-bold mb-4">Advertisement Details</h2>
                        <div className="space-y-2">
                            <p><strong>ADV ID:</strong> {advertisement.advertisementId}</p>
                            <p><strong>Created:</strong> {formatDate(advertisement.createdAt)}</p>
                            <p><strong>Company ID:</strong> {advertisement.companyId}</p>
                            <p><strong>Description:</strong> {advertisement.description}</p>
                            <p><strong>Duration:</strong> {advertisement.duration} months</p>
                            <p><strong>Total Spots:</strong> {advertisement.spots}</p>
                            <p><strong>Available Spots:</strong> {advertisement.available}</p>
                            <p><strong>Status:</strong> {advertisement.open ? 'Open' : 'Closed'}</p>
                        </div>
                        <div className=" items-center justify-center">
                            <form onSubmit={handleSubmit}>
                                {errorMessage && <div className="text-red-500 mb-4">{errorMessage}</div>}
                                <p><strong>Questionnaire: </strong> {advertisement.questionnaire}</p>
                                <input
                                    type="text"
                                    value={questionnaireAnswer}
                                    onChange={(e) => setQuestionnaireAnswer(e.target.value)}
                                    className="border rounded p-2 w-full"
                                    required
                                />
                                <button className=" gap-2  p-2 border rounded-md text-white bg-green-500 hover:bg-green-600" type="submit">Apply</button>
                            </form>
                        </div>
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