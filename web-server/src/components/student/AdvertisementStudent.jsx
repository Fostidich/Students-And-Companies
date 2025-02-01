import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import Cookies from "js-cookie";
import { getErrorMessage } from '../../utils/errorUtils';
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function AdvertisementStudent({ advertisement }) {
    const [showDetails, setShowDetails] = useState(false);
    const [profileCompany, setProfileCompany] = useState({
        username: '',
        email: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: '',
        advertisementId: ''
    });
    const [questionnaireAnswer, setQuestionnaireAnswer] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const authData = JSON.parse(Cookies.get('authData'));

    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };

    useEffect(() => {
        getDetails();
    }, []);

    const getDetails = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/company/${advertisement.companyId}`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const data = await response.json();
                setProfileCompany(data);
            }
        } catch (error) {
            console.error('Error checking advertisement details:', error);
        }
    }


    const handleSubmit = async (e) => {
        e.preventDefault()

        const response = await fetch(`${API_SERVER_URL}/api/enrollment/applications/${advertisement.advertisementId}`, {
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
        setQuestionnaireAnswer('');

    }

    return (
        <div className="bg-white rounded-lg border p-4 mb-4">
            <div className="flex justify-between items-start">
                <div>
                    <h2 className="text-2xl font-bold">{profileCompany.username}</h2>
                    <p className="text-xl font-semibold mb-2">{advertisement.description}</p>
                    <div className="flex gap-4">
                        <p>Duration: {advertisement.duration} months</p>
                        <p>Spots: {advertisement.spots}</p>
                        <p>Available: {advertisement.available}</p>
                    </div>
                </div>
                <button
                    onClick={() => setShowDetails(true)}
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                >
                    View Details
                </button>
            </div>

            {showDetails && (
                <div className=" fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center pt-40">
                    <div className="bg-white p-6 rounded-lg w-2/3 max-h-[90vh] overflow-y-scroll relative pb-16">
                        <button
                            onClick={() => setShowDetails(false)}
                            className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
                        >
                            ✕
                        </button>
                        <h2 className="text-xl font-bold mb-4">Advertisement Details</h2>
                        <div className=" border p-4 rounded-md space-y-2">
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
                                <p className="text-xs text-gray-600"><strong>Stutus:</strong> {advertisement.open ? 'Open' : 'Closed'}</p>
                                <p className="text-xs text-gray-600"><strong>Created:</strong> {formatDate(advertisement.createdAt)}</p>
                            </div>
                        </div>
                        <div className="pt-4 flex items-center justify-center">
                            <form onSubmit={handleSubmit} className={"w-full"}>
                                {errorMessage && <div className="text-red-500 mb-4">{errorMessage}</div>}
                                <p className={"flex"}><strong>Questionnaire: </strong> </p>
                                <p className={"flex"}>{advertisement.questionnaire}</p>
                                <textarea
                                    value={questionnaireAnswer}
                                    onChange={(e) => setQuestionnaireAnswer(e.target.value)}
                                    className="border rounded p-2 w-full"
                                    required
                                    rows="1"
                                    style={{ resize: 'none', overflow: 'hidden' }}
                                    onInput={(e) => {
                                        e.target.style.height = 'auto';
                                        e.target.style.height = `${e.target.scrollHeight}px`;
                                    }}
                                />
                                <button className="flex items-center justify-center w-full gap-2 p-2 border rounded-md text-white bg-green-500 hover:bg-green-600" type="submit">Apply</button>
                            </form>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

AdvertisementStudent.propTypes = {
    advertisement: PropTypes.shape({
        advertisementId: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired,
        createdAt: PropTypes.string.isRequired,
        companyId: PropTypes.number.isRequired,
        description: PropTypes.string.isRequired,
        duration: PropTypes.number.isRequired,
        spots: PropTypes.number.isRequired,
        available: PropTypes.number.isRequired,
        open: PropTypes.bool.isRequired,
        questionnaire: PropTypes.string.isRequired
    }).isRequired
};

export default AdvertisementStudent;
